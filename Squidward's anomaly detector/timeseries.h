/*
 * timeseries.h
 *
 *  Created on: 19/11/2020
 *      Author: Ariel Drellich 328925275
 */

#ifndef TIMESERIES_H_
#define TIMESERIES_H_
#include <string>
#include <vector>
using namespace std;

class TimeSeries {
vector<vector<string>> timeseries;
public:
	TimeSeries(const char* CSVfileName);
	vector<vector<string>> getDetails();
};


#endif /* TIMESERIES_H_ */