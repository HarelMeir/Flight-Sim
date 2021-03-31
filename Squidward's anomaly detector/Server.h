/*
 * Server.h
 *
 *  Created on: Jan 12, 2021
 *      Author: Ariel Drellich
 */

#ifndef SERVER_H_
#define SERVER_H_
#include "commands.h"
#include "CLI.h"
#include <pthread.h>
#include <thread>
#include <iostream>
#include <sys/socket.h>
#include <netinet/in.h>
#include <unistd.h>
#include <string.h> 
#include <sstream>


using namespace std;

// edit your ClientHandler interface here:
class ClientHandler{
    public:
    virtual void handle(int clientID)=0;
};

class SocketIO: public DefaultIO{
	int fd;
public:
	//constructor
	SocketIO(int fd) {
		this->fd = fd;
	}
	//reads string one byte at a time until new line from socket
	virtual string read() {
		string s;
		char buffer[1];
		buffer[0] = '0';
		while (true) {
			if(recv(fd, buffer, 1, 0) < 0)
				throw "Error reading from socket";
			if (buffer[0] == '\n')
				break;
			s += buffer[0];
		}
		return s;
	}
	//sends string through socket
	virtual void write(string text) {
		send(fd, text.data(), text.size(), 0);
	}
	//converts float to string and sends it
	virtual void write(float f) {
		stringstream ss;
		ss << f;
		string str = ss.str();
		send(fd, str.data(), str.size(), 0);
	}
	//reads float from socket
	virtual void read(float* f) {
		if(recv(fd, f, 1, 0) < 0)
			throw "Error reading from socket";
	}
};

// you can add helper classes here and implement on the cpp file


// edit your AnomalyDetectionHandler class here
class AnomalyDetectionHandler:public ClientHandler{
	public:
    virtual void handle(int clientID){
		//make a new socketio and send to cli.
		SocketIO socketIO(clientID);
		CLI cli(&socketIO);
		cli.start();
		//close this clients fd
		close(clientID);
    }
};


// implement on Server.cpp
class Server {
	thread* t; // the thread to run the start() method in

	// you may add data members
	int port;
	// struct used for opening socket
	struct sockaddr_in server;  
	int socketFD;
	// struct for connecting to client
	struct sockaddr_in client;
	socklen_t clientLen = sizeof(client);
	int clientID;
	// variables to stop loops
	volatile bool notStop = true;
public:
	Server(int port) throw (const char*);
	virtual ~Server();
	void start(ClientHandler& ch)throw(const char*);
	void stop();
	void initialize();
};

#endif /* SERVER_H_ */
