/*
 * CLI.cpp
 *
 *  Created on: 1/1/2021
 *      Author: Ariel Drellich 328925275
 */
#include "CLI.h"

CLI::CLI(DefaultIO* dio):dio(dio){
}

void CLI::start(){
    AnomalyData anomalyData;
    vector<Command*> commandList;
    UploadCSV uploadCSV(dio, &anomalyData);
    AlgorithmSettings algorithmSettings(dio, &anomalyData);
    DetectAnomalies detectAnomalies(dio, &anomalyData);
    DisplayResults displayResults(dio, &anomalyData);
    UploadAnomalies uploadResults(dio, &anomalyData);
    commandList.push_back(&uploadCSV);
    commandList.push_back(&algorithmSettings);
    commandList.push_back(&detectAnomalies);
    commandList.push_back(&displayResults);
    commandList.push_back(&uploadResults);

    while (true) {  
        dio->write("Welcome to the Anomaly Detection Server.\nPlease choose an option:\n");
        int cliSize = commandList.size();
        //writes the CLI according to each command's description
        for (int i = 0; i < cliSize; i++) {
            dio->write(i + 1);
            dio->write(".");
            dio->write(commandList[i]->description);
        }
        //writes the exit command to list
        dio->write(cliSize + 1);
        dio->write(".exit\n");

        int choice = stoi(dio->read());
        //intitiallizes exit if recieved exit command
        if (choice == cliSize + 1)
            break;

        commandList[choice - 1]->execute();
    }
}


CLI::~CLI() {
}

