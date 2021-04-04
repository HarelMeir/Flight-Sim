using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Flight_Sim
{
    class FlightdataModel : IFlightSimVM
    {
        //Flightcontrols. (6)
        private float aileron, elevtor, rudder, flaps, salts, speedbrake;

        //Engines (2)
        private float throttle1, throttle2;

        //Gear - Hydraulic (4)
        private float engine_pump1, engine_pump2, electric_pump1, electric_pump2;

        //Gear - Electric (2)
        private float external_power, APU_generator;

        //Position (3)
        private double latitude_deg, longitude_deg;
        private float altitude_ft;

        //Orientation (4)
        private float roll_deg, pitch_deg, heading_deg, side_slip_deg;

        //Velocities (3)
        private float airspeed_kt, glideslope, vertical_speed_fps;

        //Accelerations etc.. (17)
        private float airspeed_indicated_speed, altimeter_indicated_altitude, altimeter_pressure, attitude_indicated_pitch_deg, attitude_indicted_roll_deg,
            attitude_internal_pitch_deg, attitude_internal_roll_deg, encoder_indicated_altitude, encoder_pressure, gps_altitude, gps_ground_speed, gps_vertical_speed,
            indicated_heading, magnetic_compass_heading, slip_skid_balls, indicated_turn_rate, vertical_speed_speed_fpm, engine_rpm;

        //Properties
        public event PropertyChangedEventHandler PropertyChanged;

        public float Aileron { get; set; }
        public float Elevator { get; set; }
        public float Rudder { get; set; }
        public float Flaps { get; set; }
        public float Salts { get; set; }
        public float SpeedBrake { get; set; }


        public float Throttle1 { get; set; }
        public float Throttle2 { get; set; }

        public float Engine_pump1 { get; set; }
        public float Engine_pump2 { get; set; }
        public float Electric_pump1 { get; set; }
        public float Electric_pump2 { get; set; }


        public float External_power { get; set; }
        public float APU_enerator { get; set; }

        public float Latitude_deg { get; set; }
        public float Longitude_deg { get; set; }
        public float Altitude_ft { get; set; }


        public float Roll_deg { get; set; }
        public float Pitch_deg { get; set; }
        public float Heading_deg { get; set; }
        public float Side_slip_deg { get; set; }

        public float Airspeed { get; set; }
        public float Glidslope { get; set; }
        public float Vectical_speed { get; set; }


        public float Airspeed_indicated_speed { get; set; }
        public float Altimeter_indicated_altitude { get; set; }
        public float Altimeter_pressure { get; set; }
        public float Attitude_indicated_pitch_deg { get; set; }
        public float Attitude_indicated_roll_deg { get; set; }
        public float Attitude_internal_pitch_deg { get; set; }
        public float Attitude_internal_roll_deg { get; set; }
        public float Encoder_indicated_altitude { get; set; }
        public float Encoder_pressure_altitude { get; set; }
        public float Gps_indicated_altitude { get; set; }
        public float Gps_indicated_ground_speed { get; set; }


        public float Gps_indicated_vertucal_speed { get; set; }
        public float Indicated_heading_deg { get; set; }
        public float Magnetic_compass_heading_deg { get; set; }
        public float Slip_skid_ball_indicated_ss { get; set; }
        public float Turn_indicated_slip_skid { get; set; }
        public float Turn_indicated_turn_rate { get; set; }

        public float Veritcal_speed_indicated_speed_fpm { get; set; }
        public float Engine_rpm { get; set; }


        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
