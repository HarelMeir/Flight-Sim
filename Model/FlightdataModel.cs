using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using Microsoft.VisualBasic.FileIO;



namespace Flight_Sim
{
   public class FlightdataModel : IFlightSimVM
    {
        //Flightcontrols. (6)
        private float aileron, elevator, rudder, flaps, slats, speedbrake;

        //Engines (2)
        private float throttle, throttle2;

        //Gear - Hydraulic (4)
        private float engine_pump, engine_pump2, electric_pump, electric_pump2;

        //Gear - Electric (2)
        private float external_power, APU_generator;

        //Position (3)
        private float latitude_deg, longitude_deg;
        private float altitude_ft;

        //Orientation (4)
        private float roll_deg, pitch_deg, heading_deg, side_slip_deg;

        //Velocities (3)
        private float airspeed_kt, glideslope, vertical_speed_fps;

        //Accelerations etc.. (17)
        private float airspeed_indicated_speed, altimeter_indicated_altitude, altimeter_pressure, attitude_indicated_pitch_deg, attitude_indicted_roll_deg,
            attitude_internal_pitch_deg, attitude_internal_roll_deg, encoder_indicated_altitude, encoder_pressure, gps_altitude, gps_ground_speed, gps_vertical_speed,
            indicated_heading, magnetic_compass_heading, slip_skid_balls, indicated_turn_rate, vertical_speed_speed_fpm, engine_rpm;

        private int currentLine;

        //default constructor
        public FlightdataModel() {
            this.currentLine = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /**
        * All Properties.
        * 
        */
        //#1
        public float aileron_p
        {
            get
            {
                return this.aileron;
            }

            set
            {
                if (this.aileron != value)
                {
                    this.aileron = value;
                    NotifyPropertyChanged("aileron_p");
                }
            }
        }
        //#2
        public float elevator_p
        {
            get
            {
                return this.elevator;
            }

            set
            {
                if (this.elevator != value)
                {
                    this.elevator = value;
                    NotifyPropertyChanged("elevator_p");
                }
            }
        }
        //#3
        public float rudder_p
        {
            get
            {
                return this.rudder;
            }

            set
            {
                if (this.rudder != value)
                {
                    this.rudder = value;
                    NotifyPropertyChanged("rudder_p");
                }
            }
        }
        //#4
        public float flaps_p
        {
            get
            {
                return this.flaps;
            }

            set
            {
                if (this.flaps != value)
                {
                    this.flaps = value;
                    NotifyPropertyChanged("flaps_p");
                }
            }
        }
        //#5
        public float slats_p
        {
            get
            {
                return this.slats;
            }

            set
            {
                if (this.slats != value)
                {
                    this.slats = value;
                    NotifyPropertyChanged("slats_p");
                }
            }
        }
        //#6
        public float speedbrake_p
        {
            get
            {
                return this.speedbrake;
            }

            set
            {
                if (this.speedbrake != value)
                {
                    this.speedbrake = value;
                    NotifyPropertyChanged("speedbrake_p");
                }
            }
        }
        //#7
        public float throttle_p
        {
            get
            {
                return this.throttle;
            }

            set
            {
                if (this.throttle != value)
                {
                    this.throttle = value;
                    NotifyPropertyChanged("throttle_p");
                }
            }
        }
        //#8
        public float throttle2_p
        {
            get
            {
                return this.throttle2;
            }

            set
            {
                if (this.throttle2 != value)
                {
                    this.throttle2 = value;
                    NotifyPropertyChanged("throttle2_p");
                }
            }
        }
        //#9
        public float engine_pump_p
        {
            get
            {
                return this.engine_pump;
            }

            set
            {
                if (this.engine_pump != value)
                {
                    this.engine_pump = value;
                    NotifyPropertyChanged("engine_pump_p");
                }
            }
        }
        //#10
        public float engine_pump2_p
        {
            get
            {
                return this.engine_pump2;
            }

            set
            {
                if (this.engine_pump2 != value)
                {
                    this.engine_pump2 = value;
                    NotifyPropertyChanged("engine_pump2_p");
                }
            }
        }
        //#11
        public float electric_pump_p
        {
            get
            {
                return this.electric_pump;
            }

            set
            {
                if (this.electric_pump != value)
                {
                    this.electric_pump = value;
                    NotifyPropertyChanged("electric_pump_p");
                }
            }
        }
        //#12
        public float electric_pump2_p
        {
            get
            {
                return this.electric_pump2;
            }

            set
            {
                if (this.electric_pump2 != value)
                {
                    this.electric_pump2 = value;
                    NotifyPropertyChanged("electric_pump2_p");
                }
            }
        }
        //#13
        public float external_power_p
        {
            get
            {
                return this.external_power;
            }

            set
            {
                if (this.external_power != value)
                {
                    this.external_power = value;
                    NotifyPropertyChanged("external_power_p");
                }
            }
        }
        //#14
        public float APU_generator_p
        {
            get
            {
                return this.APU_generator;
            }

            set
            {
                if (this.APU_generator != value)
                {
                    this.APU_generator = value;
                    NotifyPropertyChanged("APU_generator_p");
                }
            }
        }
        //#15
        public float latitude_deg_p
        {
            get
            {
                return this.latitude_deg;
            }

            set
            {
                if (this.latitude_deg != value)
                {
                    this.latitude_deg = value;
                    NotifyPropertyChanged("latitude_deg_p");
                }
            }
        }
        //#16
        public float longitude_deg_p
        {
            get
            {
                return this.longitude_deg;
            }

            set
            {
                if (this.longitude_deg != value)
                {
                    this.longitude_deg = value;
                    NotifyPropertyChanged("longitude_deg_p");
                }
            }
        }
        //#17
        public float altitude_ft_p
        {
            get
            {
                return this.altitude_ft;
            }
            set
            {
                if (this.altitude_ft != value)
                {
                    this.altitude_ft = value;
                    NotifyPropertyChanged("altitude_ft_p");
                }
            }
        }
        //#18
        public float roll_deg_p
        {
            get
            {
                return this.roll_deg;
            }
            set
            {
                if (this.roll_deg != value)
                {
                    this.roll_deg = value;
                    NotifyPropertyChanged("roll_deg_p");
                }
            }
        }
        //#19
        public float pitch_deg_p
        {
            get
            {
                return this.pitch_deg;
            }
            set
            {
                if (this.pitch_deg != value)
                {
                    this.pitch_deg = value;
                    NotifyPropertyChanged("pitch_deg_p");
                }
            }
        }
        //#20
        public float heading_deg_p
        {
            get
            {
                return this.heading_deg;
            }
            set
            {
                if (this.heading_deg != value)
                {
                    this.heading_deg = value;
                    NotifyPropertyChanged("heading_deg_p");
                }
            }
        }
        //#21
        public float side_slip_deg_p
        {
            get
            {
                return this.side_slip_deg;
            }
            set
            {
                if (this.side_slip_deg != value)
                {
                    this.side_slip_deg = value;
                    NotifyPropertyChanged("side_slip_deg_p");
                }
            }
        }
        //#22
        public float airspeed_kt_p
        {
            get
            {
                return this.airspeed_kt;
            }
            set
            {
                if (this.airspeed_kt != value)
                {
                    this.airspeed_kt = value;
                    NotifyPropertyChanged("airspeed_kt_p");
                }
            }
        }
        //#23
        public float glideslope_p
        {
            get
            {
                return this.glideslope;
            }
            set
            {
                if (this.glideslope != value)
                {
                    this.glideslope = value;
                    NotifyPropertyChanged("glideslope_p");
                }
            }
        }
        //#24
        public float vertical_speed_fps_p
        {
            get
            {
                return this.vertical_speed_fps;
            }
            set
            {
                if (this.vertical_speed_fps != value)
                {
                    this.vertical_speed_fps = value;
                    NotifyPropertyChanged("vertical_speed_fps_p");
                }
            }
        }
        //#25
        public float airspeed_indicator_indicated_speed_kt_p
        {
            get
            {
                return this.airspeed_indicated_speed;
            }
            set
            {
                if (this.airspeed_indicated_speed != value)
                {
                    this.airspeed_indicated_speed = value;
                    NotifyPropertyChanged("airspeed_indicator_indicated_speed_kt_p");
                }
            }
        }
        //#26
        public float altimeter_indicated_altitude_ft_p
        {
            get
            {
                return this.altimeter_indicated_altitude;
            }
            set
            {
                if (this.altimeter_indicated_altitude != value)
                {
                    this.altimeter_indicated_altitude = value;
                    NotifyPropertyChanged("altimeter_indicated_altitude_ft_p");
                }
            }
        }
        //#27
        public float altimeter_pressure_alt_ft_p
        {
            get
            {
                return this.altimeter_pressure;
            }
            set
            {
                if (this.altimeter_pressure != value)
                {
                    this.altimeter_pressure = value;
                    NotifyPropertyChanged("altimeter_pressure_alt_ft_p");
                }
            }
        }
        //#28
        public float attitude_indicator_indicated_pitch_deg_p
        {
            get
            {
                return this.attitude_indicated_pitch_deg;
            }
            set
            {
                if (this.attitude_indicated_pitch_deg != value)
                {
                    this.attitude_indicated_pitch_deg = value;
                    NotifyPropertyChanged("attitude_indicator_indicated_pitch_deg_p");
                }
            }
        }
        //#29
        public float attitude_indicator_indicated_roll_deg_p
        {
            get
            {
                return this.attitude_indicted_roll_deg;
            }
            set
            {
                if (this.attitude_indicted_roll_deg != value)
                {
                    this.attitude_indicted_roll_deg = value;
                    NotifyPropertyChanged("attitude_indicator_indicated_roll_deg_p");
                }
            }
        }
        //#30
        public float attitude_indicator_internal_pitch_deg_p
        {
            get
            {
                return this.attitude_internal_pitch_deg;
            }
            set
            {
                if (this.attitude_internal_pitch_deg != value)
                {
                    this.attitude_internal_pitch_deg = value;
                    NotifyPropertyChanged("attitude_indicator_internal_pitch_deg_p");
                }
            }
        }
        //#31
        public float attitude_indicator_internal_roll_deg_p
        {
            get
            {
                return this.attitude_internal_roll_deg;
            }
            set
            {
                if (this.attitude_internal_roll_deg != value)
                {
                    this.attitude_internal_roll_deg = value;
                    NotifyPropertyChanged("attitude_indicator_internal_roll_deg_p");
                }
            }
        }
        //#32
        public float encoder_indicated_altitude_ft_p
        {
            get
            {
                return this.encoder_indicated_altitude;
            }
            set
            {
                if (this.encoder_indicated_altitude != value)
                {
                    this.encoder_indicated_altitude = value;
                    NotifyPropertyChanged("encoder_indicated_altitude_ft_p");
                }
            }
        }
        //#33
        public float encoder_pressure_alt_ft_p
        {
            get
            {
                return this.encoder_pressure;
            }
            set
            {
                if (this.encoder_pressure != value)
                {
                    this.encoder_pressure = value;
                    NotifyPropertyChanged("encoder_pressure_alt_ft_p");
                }
            }
        }
        //#34
        public float gps_indicated_altitude_ft_p
        {
            get
            {
                return this.gps_altitude;
            }
            set
            {
                if (this.gps_altitude != value)
                {
                    this.gps_altitude = value;
                    NotifyPropertyChanged("gps_indicated_altitude_ft_p");
                }
            }
        }
        //#35
        public float gps_indicated_ground_speed_kt_p
        {
            get
            {
                return this.gps_ground_speed;
            }
            set
            {
                if (this.gps_ground_speed != value)
                {
                    this.gps_ground_speed = value;
                    NotifyPropertyChanged("gps_indicated_ground_speed_kt_p");
                }
            }
        }
        //#36
        public float gps_indicated_vertical_speed_p
        {
            get
            {
                return this.gps_vertical_speed;
            }
            set
            {
                if (this.gps_vertical_speed != value)
                {
                    this.gps_vertical_speed = value;
                    NotifyPropertyChanged("gps_indicated_vertical_speed_p");
                }
            }
        }
        //#37
        public float indicated_heading_deg_p
        {
            get
            {
                return this.indicated_heading;
            }
            set
            {
                if (this.indicated_heading != value)
                {
                    this.indicated_heading = value;
                    NotifyPropertyChanged("indicated_heading_deg_p");
                }
            }
        }
        //#38
        public float magnetic_compass_indicated_heading_deg_p
        {
            get
            {
                return this.magnetic_compass_heading;
            }
            set
            {
                if (this.magnetic_compass_heading != value)
                {
                    this.magnetic_compass_heading = value;
                    NotifyPropertyChanged("magnetic_compass_indicated_heading_deg_p");
                }
            }
        }
        //#39
        public float slip_skid_ball_indicated_slip_skid_p
        {
            get
            {
                return this.slip_skid_balls;
            }
            set
            {
                if (this.slip_skid_balls != value)
                {
                    this.slip_skid_balls = value;
                    NotifyPropertyChanged("slip_skid_ball_indicated_slip_skid_p");
                }
            }
        }
        //#40
        public float turn_indicator_indicated_turn_rate_p
        {
            get
            {
                return this.indicated_turn_rate;
            }
            set
            {
                if (this.indicated_turn_rate != value)
                {
                    this.indicated_turn_rate = value;
                    NotifyPropertyChanged("turn_indicator_indicated_turn_rate_p");
                }
            }
        }
        //#41
        public float vertical_speed_indicator_indicated_speed_fpm_p
        {
            get
            {
                return this.vertical_speed_speed_fpm;
            }
            set
            {
                if (this.vertical_speed_speed_fpm != value)
                {
                    this.vertical_speed_speed_fpm = value;
                    NotifyPropertyChanged("vertical_speed_indicator_indicated_speed_fpm_p");
                }
            }
        }
        //#42
        public float engine_rpm_p
        {
            get
            {
                return this.engine_rpm;
            }
            set
            {
                if (this.engine_rpm != value)
                {
                    this.engine_rpm = value;
                    NotifyPropertyChanged("vengine_rpm_p");
                }
            }
        }
        
        
        public int CurrentLine
        {
            get
            {
                return currentLine;
            }
            set
            {
                currentLine = value;
                NotifyPropertyChanged("CurrentLine");
            }
        }

        //INotifyHelperApp
        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
