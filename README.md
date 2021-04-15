# Advanced-Programming-2 - Flight Inspection App

https://www.youtube.com/watch?v=_tP9fRXuNbk

## Main files in the project
The App is made for and flight researchers that want to explore flights.
The project is divided into four main parts : 
  Model - Composed by The connection + data Layer.
         Conntection: this class called FlightSimM. It holds all the FG connection part from the user.
                      Its jobs is to parse the xml for the colNames, "learn normal" for finding anomalies from the "csv train", and detect with the "csv
                      Test". It contain another Model class - FlightSim data, and save some cauculations reasults in it.
         FlightSimData: This class holds all 42 properties of the flight. Morover, it contains the data structure Dictionary - that hold all the
                        anomaly csv data as string and float List.
                          All controler's views contains this class.
                         
 View Model -  This layer performs connection between the View and the Model layers. 
              Every controller has a vm of its own. this layer connect bewtween the controller's view to its model.
              it uses the interface called NotifyPropertyChanged, according the MVVM architecture.
              Part of its properties are binded to the view's ones, to execute real time changes.
              
              
 View - The visibility Layer. every Controller has a view of its own, and contains its VM.
        Composed by 2 parts:
        Xaml: The desgin part, that sets the overall look for each controller.
        Cs part: This is the "code behined". sets the logic behined its diff conponenets, like button clicks, binding to VM propertychanges, etc.
           
 The Views: - 1.MainWindeow - the connection screen.
              2.  FlightSimApp - the main app screen.
              3. GraphMain - the graph screen.

                    While finishing with the connection part - windows 2 and 3 opens.
                     
 The Controllers: mediaPlayer - the player controller.
                  Joistick - the joistick userstory.
                  UserStroy5 - the story that represent the plane's location properties.
                  Graphs - all the graphs part.
                  UserStory9 - the anomalies algorith part.
       
 DLL - the user receives a dll from us and decides which dll to let us use, the dll is an anomaly detector written in C++ and used dynamically by the c# program to detect anomalies, the API holds a single function that starts the whole process and creates a file with the detected anomalies that holds their description and timestep, the c# program use the file to save the anomalies locally in a list of AnomalyReports.




- Here you can control your flight video, go back and re-watch important sections and also quickly run non-important sections:


![pj3](https://github.com/HarelMeir/Flight-Sim/blob/master/controllers/images/3.png)

- Data display: allows the user to choose one of the attributes of the flight. 


![pj2](https://github.com/HarelMeir/Flight-Sim/blob/master/controllers/images/2.png)


## Installation:

1. Download "FlightGear" simulator from link: https://www.flightgear.org/
2.Enter to settings -> Additional Settings and add the information:
"--generic=socket,in,10,127.0.0.1,5400,tcp,playback_small
--fdm=null"
3. set configuration in the project to "x86"
4. Run the project and add reg_flight.csv, playback_small.xml, and SimpleAnomalyDetectorDLL.dll

 ![pj1](https://github.com/HarelMeir/Flight-Sim/blob/master/controllers/images/1.png)
 
5.Now, everything is ready, enjoy!

 ![pj5](https://github.com/HarelMeir/Flight-Sim/blob/master/controllers/images/5.png)






