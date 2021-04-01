/*
 * Server.cpp
 *
 *  Created on: Jan 12, 2021
 *      Author: Ariel Drellich
 */
#include "Server.h"

Server::Server(int port)throw (const char*) {
	this->port = port;
}

void Server::start(ClientHandler& ch)throw(const char*){
	//initialize server by binding the socket 
	this->initialize();
	//new lambda expression that keeps listening for new connections.
	t = new thread([this, &ch](){
		//stops when stop function is called and changes notStop to false
		while (notStop) {
			clientID = accept(socketFD, (sockaddr*)&client, &clientLen);
			if (clientID < 0)
				throw "Failed to accept";

			ch.handle(clientID);
			sleep(2);
		}
	});
}

void Server::stop(){
	//tells the while loop in t to stop
	notStop = false;
	t->join(); // do not delete this!
	//frees memory
	delete t;
	//closes the socket
	if (close(socketFD) < 0) {
        throw "Error closing socket";
    }
}

Server::~Server() {
}

void Server::initialize() {
	server.sin_family = AF_INET;
	//set endianess
    server.sin_port = htons(port);
	//gets ip
	server.sin_addr.s_addr = INADDR_ANY;
	//gets socket
	socketFD = socket(AF_INET, SOCK_STREAM, 0);
	//makes sure we got socket
	if (socketFD < 0)
		throw "Error opening socket";
	//binds socket
	if (bind(socketFD, (const sockaddr*)&server, sizeof(server)) < 0)
        throw "Error binding socket";
	//sets that this socket will have a queue of 3 clients
	if (listen(socketFD, 3) < 0)
		throw "Listen failed";
}

