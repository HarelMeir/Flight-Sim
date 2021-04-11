/*
 * CircleAnomalyDetector.h
 *
 *  Created on: 7/4/2021
 *      Author: Ariel Drellich 328925275
 */

#ifndef CIRCLEANOMALYDETECTOR_H_
#define CIRCLEANOMALYDETECTOR_H_

#include "anomaly_detection_util.h"
#include "AnomalyDetector.h"
#include <vector>
#include <algorithm>
#include <string.h>
#include <math.h>
#include "minCircle.h"

struct correlatedFeatures {
	string feature1, feature2;  // names of the correlated features
	float corrlation;
	Line lin_reg;
	float threshold;
	Circle circleThreshold;
};

class CircleAnomalyDetector :public TimeSeriesAnomalyDetector {
	float defaultThreshold = 0.9;
protected:
	vector<correlatedFeatures> cf;
	float correlationThreshold = defaultThreshold;
public:
	CircleAnomalyDetector();
	virtual ~CircleAnomalyDetector();

	virtual void learnNormal(const TimeSeries& ts);
	virtual vector<AnomalyReport> detect(const TimeSeries& ts);

	vector<correlatedFeatures> getNormalModel() {
		return cf;
	}

	void setNormalModel(vector<correlatedFeatures> correlatedFeatures);

	float getCorrelationThreshold();

	void setCorrelationThreshold(float newThreshold);

private:
	float highestDev(float* x, float* y, Line line, int size);

};

#endif /* CIRCLEANOMALYDETECTOR_H_ */
