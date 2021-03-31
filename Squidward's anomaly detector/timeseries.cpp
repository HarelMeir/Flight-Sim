/*
 * timeseries.cpp
 *
 *  Created on: 19/11/2020
 *      Author: Ariel Drellich 328925275
 */

#include "timeseries.h"
#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <sstream>

using namespace std;

string line;

TimeSeries::TimeSeries(const char* CSVfileName) {
	ifstream file(CSVfileName);
    //seperator
	char tok = ',';
	if (file.is_open()) {
        //read each line
		while(getline(file, line)) {
			stringstream streamLine(line);
			vector<string> row;
            //seperates line by ","
			while(getline(streamLine, line, tok))
				row.push_back(line);
            //saves the info we found to a vector that holds all the rows
			timeseries.push_back(row);
		}

	    file.close();
	} else cout << "Cannot open file.";
}

vector<vector<string>> TimeSeries::getDetails() {
    return timeseries;
}
