/*
 * CircleAnomalyDetector.h
 *
 *  Created on: 7/4/2021
 *      Author: Ariel Drellich 328925275
 */

#ifndef CIRCLEANOMALYDETECTOR_H_
#define CIRCLEANOMALYDETECTOR_H_

#include "SimpleAnomalyDetector.h"
#include "minCircle.h"

class CircleAnomalyDetector :public SimpleAnomalyDetector {
public:
	CircleAnomalyDetector();
	virtual ~CircleAnomalyDetector();
	virtual vector<AnomalyReport> detect(const TimeSeries& ts);

};

#endif /* CIRCLEANOMALYDETECTOR_H_ */
