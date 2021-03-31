/*
 * SimpleAnomalyDetector.h
 *
 *  Created on: 19/11/2020
 *      Author: Ariel Drellich 328925275
 */

#ifndef SIMPLEANOMALYDETECTOR_H_
#define SIMPLEANOMALYDETECTOR_H_

#include "anomaly_detection_util.h"
#include "AnomalyDetector.h"
#include <vector>
#include <algorithm>
#include <string.h>
#include <math.h>
#include "minCircle.h"

struct correlatedFeatures{
	string feature1,feature2;  // names of the correlated features
	float corrlation;
	Line lin_reg;
	float threshold;
	Circle circleThreshold;
};


class SimpleAnomalyDetector:public TimeSeriesAnomalyDetector{
		float defaultThreshold = 0.9;
	protected:
		vector<correlatedFeatures> cf;
		float correlationThreshold = defaultThreshold;
	public:
		SimpleAnomalyDetector();
		virtual ~SimpleAnomalyDetector();

		virtual void learnNormal(const TimeSeries& ts);
		virtual vector<AnomalyReport> detect(const TimeSeries& ts);

		vector<correlatedFeatures> getNormalModel(){
			return cf;
		}

		void setNormalModel(vector<correlatedFeatures> correlatedFeatures);

		float getCorrelationThreshold();
		
		void setCorrelationThreshold(float newThreshold);

	private:
		float highestDev(float* x, float* y, Line line, int size);
};



#endif /* SIMPLEANOMALYDETECTOR_H_ */
