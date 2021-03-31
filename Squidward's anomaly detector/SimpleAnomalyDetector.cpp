/*
 * SimpleAnomalyDetector.cpp
 *
 *  Created on: 19/11/2020
 *  Updated on: 4/1/2020
 *      Author: Ariel Drellich 328925275
 */
#include "SimpleAnomalyDetector.h"

float SimpleAnomalyDetector::getCorrelationThreshold() {
	return correlationThreshold;
}

void SimpleAnomalyDetector::setCorrelationThreshold(float newThreshold) {
	correlationThreshold = newThreshold;
}

SimpleAnomalyDetector::SimpleAnomalyDetector() {
	// TODO Auto-generated constructor stub

}

SimpleAnomalyDetector::~SimpleAnomalyDetector() {
	// TODO Auto-generated destructor stub
}

void SimpleAnomalyDetector::setNormalModel(vector<correlatedFeatures> correlatedFeatures) {
	cf = correlatedFeatures;
}

void SimpleAnomalyDetector::learnNormal(const TimeSeries& ts) {
	TimeSeries series = ts;
	vector<vector<string>> normalSeries = series.getDetails();
	int columnSize = normalSeries.size();
	int rowSize = normalSeries[0].size();
	// k will be the marker for our first column
	for(int k = 0; k < rowSize; k++) {
		correlatedFeatures cfs;
		//set to -1 (which is the minimum possible) so that we can find the largest by comparison
		cfs.corrlation = -1;
		//will be used to store the data in the columns
		float* x = new float[columnSize - 1];
		float* y = new float[columnSize - 1];
		//sets the first column
		for(int j = 1; j < columnSize; j++) {
			x[j - 1] = stof(normalSeries[j][k]);
		}
		//runs through secondary columns
		int secondColumnIndex;
		for(int i = 0; i < rowSize; i++) {
			//doesn't check if first column is the same as the second column
			if(i == k) {
				continue;
			}
			//saves data from second column
			for(int j = 1; j < columnSize; j++) {
				y[j - 1] = stof(normalSeries[j][i]);
			}
			//finds correlation
			float correlation = pearson(x, y, columnSize - 1);
			//updates the struct if we found the highest correlation so far
			if(correlation > cfs.corrlation) {
				cfs.corrlation = correlation;
				// if (k < i) {	/////instructions unclear so I have the following code in comments if it's what you wanted
				cfs.feature1 = normalSeries[0][k];
				cfs.feature2 = normalSeries[0][i];
				cfs.lin_reg = linear_reg(x, y, columnSize - 1);
				cfs.threshold = highestDev(x, y, cfs.lin_reg, columnSize -1) * 1.15;
				// }
				// else {
				// 	cfs.lin_reg = linear_reg(y, x, columnSize - 1);
				// 	cfs.feature1 = normalSeries[0][i];
				// 	cfs.feature2 = normalSeries[k][0];
				// 	cfs.threshold = highestDev(y, x, cfs.lin_reg, columnSize -1) * 1.1;
				// }
				secondColumnIndex = i;
			}
		}

		//checks to see if the correlation we found is already in the vector
		bool isDuplicate = false;
		for (int l = 0; l < cf.size(); l++) {
			if (((cf[l].feature1 == cfs.feature1) && (cf[l].feature2 == cfs.feature2))
			 || ((cf[l].feature1 == cfs.feature2) && (cf[l].feature2 == cfs.feature1))) {
				 isDuplicate = true;
			}
		}
		if (!isDuplicate && cfs.corrlation > 0.5) {
			if (cfs.corrlation < correlationThreshold) {
				vector<Point> pointsVector;
				for (int i = 1; i < columnSize; i++ ) {
					Point p(stof(normalSeries[i][k]), stof(normalSeries[i][secondColumnIndex]));
					pointsVector.push_back(p);
				}
				cfs.circleThreshold = findMinCircle(pointsVector);
				cfs.circleThreshold.radius *= 1.1;
			}
			cf.push_back(cfs);
		}
		//frees memory that was dynamically allocated
		delete [] x;
		delete [] y;
	}
}

//sends each x y pair to dev and returns the highest
float SimpleAnomalyDetector::highestDev(float* x, float* y, Line line, int size) {
	float maxDev = 0;
	for(int i = 0; i < size; i++) {
		Point p(x[i],y[i]);
		float newDev = dev(p, line);
		if(newDev > maxDev) {
			maxDev = newDev;
		}
	}
	return maxDev;
}

vector<AnomalyReport> SimpleAnomalyDetector::detect(const TimeSeries& ts) {
	vector<AnomalyReport> anomalyReport;
	TimeSeries timeseries = ts;
	vector<vector<string>> data = timeseries.getDetails();
	//iterates through the correlated features list
	for(int i = 0; i < cf.size(); i++) {
		string f1 = cf.at(i).feature1;
		string f2 = cf.at(i).feature2;
		int f1Index, f2Index;
		//finds the indexes for the features we found in cf in the timeseries
		for(int j = 0; j < data.at(0).size(); j++) {
			if (data.at(0).at(j) == f1) {
				f1Index = j;
			}
			if (data.at(0).at(j) == f2) {
				f2Index = j;
			}
		}
		//iterates over the columns in sends each x y pair to dev() to see if they exceed the threshold
		for(int k = 1; k < data.size(); k++) {
			Point p(stof(data.at(k).at(f1Index)), stof(data.at(k).at(f2Index)));
			if (dev(p, cf.at(i).lin_reg) > cf.at(i).threshold) {
				//if the point is too far away from the line, adds a report of the
				// problem to the list of anomaly reports
				AnomalyReport ar(f1 + "-" + f2, k);
				anomalyReport.push_back(ar);
			}
		}
	}
	return anomalyReport;
}