/*
 * CircleAnomalyDetector.cpp
 *
 *  Created on: 7/4/2021
 *      Author: Ariel Drellich 328925275
 */
#include "CircleAnomalyDetector.h"

CircleAnomalyDetector::CircleAnomalyDetector() {
	// TODO Auto-generated constructor stub

}

CircleAnomalyDetector::~CircleAnomalyDetector() {
	// TODO Auto-generated destructor stub
}

vector<AnomalyReport> CircleAnomalyDetector::detect(const TimeSeries& ts) {

	vector<AnomalyReport> circleAnomalyReport;
	//checks the points outside of the min circle
	TimeSeries timeseries = ts;
	vector<vector<string>> data = timeseries.getDetails();

	for (correlatedFeatures cFeature : cf) {
		//iterates through the correlated features list
		string f1 = cFeature.feature1;
		string f2 = cFeature.feature2;
		int f1Index, f2Index;
		//finds the indexes for the features we found in cf in the timeseries
		for (int j = 0; j < data.at(0).size(); j++) {
			if (data.at(0).at(j) == f1) {
				f1Index = j;
			}
			if (data.at(0).at(j) == f2) {
				f2Index = j;
			}
		}

		//iterates over the columns in sends each x y pair to dev() to see if they exceed the threshold
		for (int k = 1; k < data.size(); k++) {
			Point p(std::stof(data.at(k).at(f1Index)), std::stof(data.at(k).at(f2Index)));
			// if the point is out of the min circle, its an anomaly
			if (!isPointInside(cFeature.circleThreshold, p)) {
				AnomalyReport ar(f1 + "-" + f2, k);
				circleAnomalyReport.push_back(ar);
			}
		}
	}

	return circleAnomalyReport;
}

