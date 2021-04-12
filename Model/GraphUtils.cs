using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Sim.Model
{
    class Point
    {
        private float x;
        private float y;

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float X
        {
            get
            {
                return this.x;
            }
            set
            {
                if (this.x != value)
                {
                    this.x = value;
                }
            }
        }

        public float Y
        {
            get
            {
                return this.y;
            }
            set
            {
                if (this.y != value)
                {
                    this.y = value;
                }
            }
        }
    }
    class Line
    {
        private float a;
        private float b;

        public Line(float a, float b)
        {
            this.a = a;
            this.b = b;
        }

        public float A
        {
            get
            {
                return this.a;
            }

            set
            {
                if (this.a != value)
                {
                    this.a = value;
                }
            }
        }

        public float B
        {
            get
            {
                return this.b;
            }

            set
            {
                if (this.b != value)
                {
                    this.b = value;
                }
            }
        }

        //methods
        public float LineEx(float x)
        {
            return a * x + b;
        }
    }


    class AnomalyUtils
    {
        public AnomalyUtils() { }

        //****methods******//

        //avg
        public static float Avg(List<float> list)
        {
            float sum = 0;
            foreach (float val in list)
            {
                sum += val;
            }
            // return value changed from sum to sum / list.count
            return sum / list.Count;
        }

        //variance
        public static float Var(List<float> x)
        {
            float ex = Avg(x);
            for (int i = 0; i < x.Count; i++)
            {
                x[i] *= x[i];
            }
            float ex2 = Avg(x);
            return ex2 - (ex * ex);
        }

        //cov
        public static float Cov(List<float> x, List<float> y)
        {
            float ex = Avg(x);
            float ey = Avg(y);
            for (int i = 0; i < x.Count; i++)
            {
                x[i] *= y[i];
            }

            return Avg(x) - (ex * ey);
        }

        //pearson
        public static float Pearson(List<float> x, List<float> y)
        {
            return Cov(x, y) / (float)Math.Sqrt(Var(x) * Var(y));
        }

        //linear reg
        public static Line LinearReg(List<Point> points)
        {
            var x = new List<float>();
            var y = new List<float>();
            foreach (Point p in points)
            {
                x.Add(p.X);
                y.Add(p.Y);
            }
            float a = Cov(x, y) / Var(x);
            float b = Avg(x) - (a * Avg(x));

            return new Line(a, b);
        }
        public static Line LinearReg(List<float> x, List<float> y)
        {
            float a = Cov(x, y) / Var(x);
            float b = Avg(x) - (a * Avg(x));

            return new Line(a, b);
        }
        //1st dev
        public static float Dev(Point p, List<Point> points)
        {
            return Dev(p, LinearReg(points)); 
        }

        //2st dev
        public static float Dev(Point p, Line l) {
            return Math.Abs(p.Y - l.LineEx(p.X));
        }
    }
}

