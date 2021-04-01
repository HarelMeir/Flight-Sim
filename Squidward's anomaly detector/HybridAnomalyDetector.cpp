/*
 * HybridAnomalyDetector.cpp
 *
 *  Created on: 1/1/2021
 *      Author: Ariel Drellich 328925275
 */
#include "HybridAnomalyDetector.h"

HybridAnomalyDetector::HybridAnomalyDetector() {
	// TODO Auto-generated constructor stub

}

HybridAnomalyDetector::~HybridAnomalyDetector() {
	// TODO Auto-generated destructor stub
}

vector<AnomalyReport> HybridAnomalyDetector::detect(const TimeSeries& ts) {
	//two vectors, one for each type of detect
	vector<correlatedFeatures> linearFeatures;
	vector<correlatedFeatures> circleFeatures;
	//seperates the correlated features into the appropriate vectors
	for (correlatedFeatures features: cf) {
		if (features.corrlation < correlationThreshold) {
			circleFeatures.push_back(features);
		} else {
			linearFeatures.push_back(features);
		}
	}

	//changes the model to one that we want to work with (don't worry, we'll change it back at the end)
	this->SimpleAnomalyDetector::setNormalModel(linearFeatures);
	vector<AnomalyReport> linearAnomalyReport = SimpleAnomalyDetector::detect(ts);
	vector<AnomalyReport> circleAnomalyReport;
	//checks the points outside of the min circle
	TimeSeries timeseries = ts;
	vector<vector<string>> data = timeseries.getDetails();
	
	for (correlatedFeatures cFeature: circleFeatures) {
		//iterates through the correlated features list
		string f1 = cFeature.feature1;
		string f2 = cFeature.feature2;
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
			Point p(std::stof(data.at(k).at(f1Index)), std::stof(data.at(k).at(f2Index)));
			// if the point is out of the min circle, its an anomaly
			if (!isPointInside(cFeature.circleThreshold, p)) {
				AnomalyReport ar(f1 + "-" + f2, k);
				circleAnomalyReport.push_back(ar);
			}
		}
	}


	//iterate over each anomaly report and push them into the final anomaly report vector in alphabetical order
	vector<AnomalyReport> anomalyReport;
	int y = 0, z = 0;
	int anomalySize = circleAnomalyReport.size() + linearAnomalyReport.size();
	for (int x = 0; x < anomalySize; x++) {
		//iterates over each list if we haven't gotten to the end of one of them
		//otherwise dump the one that's not finished into the vector
		if (y < circleAnomalyReport.size() && z < linearAnomalyReport.size()) {
			if (circleAnomalyReport[y].description[0] < linearAnomalyReport[z].description[0]) {
				anomalyReport.push_back(circleAnomalyReport[y]);
				y++;
			}
			else {
				anomalyReport.push_back(linearAnomalyReport[z]);
				z++;
			}
		} else if (y == circleAnomalyReport.size()) {
			for (; x < anomalySize; x++) {
				anomalyReport.push_back(linearAnomalyReport[z]);
				z++;
			}
		} else {
			for (; x < anomalySize; x++) {
				anomalyReport.push_back(circleAnomalyReport[y]);
				y++;
			}
		}
		
	}

	//returns model to the way it was
	this->SimpleAnomalyDetector::setNormalModel(cf);
	return anomalyReport;
}

