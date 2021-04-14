# Advanced-Programming-2 - Flight Inspection App

## Main files in the project
The App is made for and flight researchers that want to explore flights.
The project is divided into four main parts : 
1. Model - contains the latest information at any given moment.
2. View Model - the part that connects the model to visibility.
3. View - the visibility of each of the different controllers that control the simulator.
4. DLL - calculates the anomalies and creates a file with the results.




- Here you can control your flight video, go back and re-watch important sections and also quickly run non-important sections:
![pj3](https://github.com/HarelMeir/Flight-Sim/blob/master/controllers/images/3.png)

- Data display:
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






#### Video
Link to an explanation video:
