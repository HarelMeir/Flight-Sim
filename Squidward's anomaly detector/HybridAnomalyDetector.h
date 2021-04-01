/*
 * HybridAnomalyDetector.h
 *
 *  Created on: 1/1/2021
 *      Author: Ariel Drellich 328925275
 */

#ifndef HYBRIDANOMALYDETECTOR_H_
#define HYBRIDANOMALYDETECTOR_H_

#include "SimpleAnomalyDetector.h"
#include "minCircle.h"

class HybridAnomalyDetector:public SimpleAnomalyDetector {
public:
	HybridAnomalyDetector();
	virtual ~HybridAnomalyDetector();
	virtual vector<AnomalyReport> detect(const TimeSeries& ts);

};

#endif /* HYBRIDANOMALYDETECTOR_H_ */
