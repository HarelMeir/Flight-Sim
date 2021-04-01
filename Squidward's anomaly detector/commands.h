/*
 * commands.h
 *
 *  Created on: 1/1/2021
 *      Author: Ariel Drellich 328925275
 */

#ifndef COMMANDS_H_
#define COMMANDS_H_

#include<iostream>
#include <string.h>

#include <fstream>
#include <vector>
#include "HybridAnomalyDetector.h"

using namespace std;

class DefaultIO{
public:
	virtual string read()=0;
	virtual void write(string text)=0;
	virtual void write(float f)=0;
	virtual void read(float* f)=0;
	virtual ~DefaultIO(){}
	//recieves line-by-line from dio and saves to received filename
	virtual void download(string fileName) {
		ofstream file(fileName);
		string tempLine = this->read();
		if (file.is_open()) {
			while (tempLine.compare("done") != 0) {
				file << tempLine << endl;
				tempLine = this->read();
			}
			file.close();
		}
	}
};

// shared info to be sent to the commands
typedef struct{
	HybridAnomalyDetector had;
	vector<AnomalyReport> anomalyReport;
	int numOfLines;
} AnomalyData;

// a way to save the range of anomalies
class ContinuousAnomalies {
public:
	const string description;
	const long firstTimeStep;
	const long lastTimeStep;
	ContinuousAnomalies(string description, long firstTimeStep, long lastTimeStep):
			description(description),firstTimeStep(firstTimeStep),lastTimeStep(lastTimeStep){}

	// recieves vector of AnomalyReports and if we have sequential anomalies, we save the first and last position.
	static vector<ContinuousAnomalies> findContinuousAnomalies(vector<AnomalyReport> ars) {
		vector<ContinuousAnomalies> continuousAnomalies;
		bool first = true;
		string currentDescription;
		long firstTS;
		long lastTS;
		for (AnomalyReport ar: ars) {
			//first time we go in we initialize
			if (first) {
				currentDescription = ar.description;
				firstTS = ar.timeStep;
				lastTS = firstTS;
				first = false;
			} else {
				//if we're in the same cf, check if the time step is continuous
				if (ar.description == currentDescription) {
					if (ar.timeStep != lastTS + 1) {
						continuousAnomalies.push_back(ContinuousAnomalies(currentDescription, firstTS, lastTS));
						firstTS = ar.timeStep;
					}
					lastTS = ar.timeStep;
				} else {
					//if the descriptions don't match
					continuousAnomalies.push_back(ContinuousAnomalies(currentDescription, firstTS, lastTS));
					firstTS = ar.timeStep;
					lastTS = firstTS;
					currentDescription = ar.description;
				}
			}
		}
		//pushes the final anomaly
		continuousAnomalies.push_back(ContinuousAnomalies(currentDescription, firstTS, lastTS));
		return continuousAnomalies;
	}
};


class Command {
	protected:
		DefaultIO* dio;
		// pointer to the data we'll be needing
		AnomalyData* anomalyData;
	public:
		string description;
		Command(DefaultIO* dio, AnomalyData* anomalyData):dio(dio),anomalyData(anomalyData){}
		virtual void execute()=0;
		virtual ~Command(){}
};

// recieves 2 files from the DIO and saves them using the download method
class UploadCSV: public Command {
	public:
		UploadCSV(DefaultIO* dio, AnomalyData* anomalyData) : Command(dio, anomalyData) {
			this->description = "upload a time series csv file\n";
		}
		virtual void execute() {
			dio->write("Please upload your local train CSV file.\n");
			dio->download("anomalyTrain.csv");
			dio->write("Upload complete.\nPlease upload your local test CSV file.\n");
			dio->download("anomalyTest.csv");
			dio->write("Upload complete.\n");
		}
};

// prints the current correlation threshold and then gives the option to change it
class AlgorithmSettings: public Command {
	public:
		AlgorithmSettings(DefaultIO* dio, AnomalyData* anomalyData) : Command(dio, anomalyData) {
			this->description = "algorithm settings\n";
		}
		virtual void execute() {
			dio->write("The current correlation threshold is ");
			dio->write(anomalyData->had.getCorrelationThreshold());
			dio->write("\nType a new threshold\n");
			// gets input from dio, if it's not in the proper range, ask again until it is.
			float newThresh = stof(dio->read());
			while (newThresh < 0 || newThresh > 1) {
				dio->write("please choose a value between 0 and 1.\n");
				newThresh = stof(dio->read());
			}
			// set the new threshold
			anomalyData->had.setCorrelationThreshold(newThresh);
		}
};

// runs learnNormal and Detect from HybridAnomalyDetector, then saves the results.
class DetectAnomalies: public Command {
	public:
		DetectAnomalies(DefaultIO* dio, AnomalyData* anomalyData) : Command(dio, anomalyData) {
			this->description = "detect anomalies\n";
		}
		virtual void execute() {
			// gets timeseries for each file
			TimeSeries train("anomalyTrain.csv");
			TimeSeries test("anomalyTest.csv");
			anomalyData->numOfLines = (test.getDetails()).size() - 1;
			anomalyData->had.learnNormal(train);
			// saves the anomalies we detected
			anomalyData->anomalyReport = anomalyData->had.detect(test);
			dio->write("anomaly detection complete.\n");
		}
};

// simply prints each anomaly we found
class DisplayResults: public Command {
	public:
		DisplayResults(DefaultIO* dio, AnomalyData* anomalyData) : Command(dio, anomalyData) {
			this->description = "display results\n";
		}
		virtual void execute() {
			for (AnomalyReport ar : anomalyData->anomalyReport) {
				dio->write(ar.timeStep);
				dio->write("\t");
				dio->write(ar.description);
				dio->write("\n");
			}
			dio->write("Done.\n");
		}
};

// recieves and saves file of expected anomalies and checks them against what we found, then returns the results.
class UploadAnomalies: public Command {
	private:
		const char* fileName = "expected anomalies.csv";
	public:
		UploadAnomalies(DefaultIO* dio, AnomalyData* anomalyData) : Command(dio, anomalyData) {
			this->description = "upload anomalies and analyze results\n";
		}

		// iterates over each expected anomaly range and sums them up
		float calculateNumOfAnomalies (vector<vector<string>> expectedAnomalies) {
			float n = 0;
			for (vector<string> ea : expectedAnomalies) {
				n += stoi(ea[1]) - stoi(ea[0]) + 1;
			}
			return n;
		}

		//checks if the two ranges overlap at any point
		bool containsOverlap(float a, float b, float x, float y) {
			//if a-b is before/in x-y
			if (a <= x && b <= y && b >= x)
				return true;
			//if x-y is before/in a-b
			if (x <= a && y <= b && y >= a)
				return true;
			//if one is fully inside the other
			if ((x <= a && y >= b) || (a <= x && b >= y))
				return true;
			return false;
		}

		virtual void execute() {
			dio->write("Please upload your local anomalies file.\n");
			dio->download(fileName);
			dio->write("Upload complete.\n");
			// saves the 2 lists of anomalies, expected and detected, for checking overlap
			vector<vector<string>> expectedAnomalies = (TimeSeries(fileName)).getDetails();
			vector<ContinuousAnomalies> continuousAnomalies = ContinuousAnomalies::findContinuousAnomalies(anomalyData->anomalyReport);
			// initializes counters for each data type needed
			float P = expectedAnomalies.size();
			float N = anomalyData->numOfLines - calculateNumOfAnomalies(expectedAnomalies);
			float TP = 0, FP = 0, FN = 0;

			// checks every expected anomaly vs detected anomaly for overlap, and anomalies that we didn't detect
			bool containsFN;
			for (vector<string> ea : expectedAnomalies) {
				containsFN = true;
				for (ContinuousAnomalies ca : continuousAnomalies) {
					if (containsOverlap(stof(ea[0]), stof(ea[1]), ca.firstTimeStep, ca.lastTimeStep)) {
						TP++;
						containsFN = false;
					}
				}
				if (containsFN)
					FN++;
			}
			// checks every detected anomaly vs expected anomaly for false positives
			bool containsFP;
			for (ContinuousAnomalies ca : continuousAnomalies) {
				containsFP = true;
				for (vector<string> ea : expectedAnomalies) {
					if (containsOverlap(stof(ea[0]), stof(ea[1]), ca.firstTimeStep, ca.lastTimeStep))
						containsFP = false;
				}
				if (containsFP)
					FP++;
			}
			// rounds the True Positve Rate and False Positive Rate to 3 numbers after the decimal
			float TPR = (float)((int)((TP/P)*1000))/1000;
			float FPR = (float)((int)((FP/N)*1000))/1000;
			dio->write("True Positive Rate: ");
			dio->write(TPR);
			dio->write("\n");
			dio->write("False Positive Rate: ");
			dio->write(FPR);
			dio->write("\n");
			
		}
};

#endif /* COMMANDS_H_ */
