using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flight_Sim.Model;

namespace Flight_Sim.cppToCSharp
{
    public class Circle
    {
        public Point center;
        public double radius;
        public Circle()
        {
            center = new Point(0, 0);
            radius = 0;
        }
        public Circle(Point c, double r)
        {
            center = c;
            radius = r;
        }
    };
    class MinCircle
    {
        public static double Dist( Point a, Point b) {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        public static bool IsPointInside(Circle c, Point p) {
            return Dist(c.center, p) <= c.radius;
        }

        public static Circle TwoPointsCircle(Point p1, Point p2) {
            return new Circle(new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2), Dist(p1, p2) / 2);
        }

        public static Circle ThreePointCircle(Point p1, Point p2, Point p3) {
            float xc = ((p1.X * p1.X - p2.X * p2.X + p1.Y * p1.Y - p2.Y * p2.Y) * (p1.Y - p3.Y) +
                (-p1.X * p1.X + p3.X * p3.X - p1.Y * p1.Y + p3.Y * p3.Y) * (p1.Y - p2.Y)) /
               (2 * ((p1.X - p2.X) * (p1.Y - p3.Y) - (p1.X - p3.X) * (p1.Y - p2.Y)));

            float yc = ((p1.X * p1.X - p3.X * p3.X - 2 * xc * (p1.X - p3.X) + p1.Y * p1.Y - p3.Y * p3.Y)) / (2 * (p1.Y - p3.Y));
            double radius = Math.Sqrt((p1.X - xc) * (p1.X - xc) + (p1.Y - yc) * (p1.Y - yc));
            return new Circle(new Point(xc, yc), radius);
        }

        public static Circle Welzl(List<Point> P, List<Point> R, int sizeP) {
            // trivial cases + case that we need to make a circle using 2 or 3 points (depending on how many we recieve).
            if (sizeP == 0 || R.Count == 3)
            {
                switch (R.Count)
                {
                case 0:
                    // trivial case, nothing to work off of
                    return new Circle(new Point(0, 0), 0);
                    //break;
                case 1:
                    // also trivial, one point can't have a radius
                    return new Circle(R.ElementAt(0), 0);
                   //break;
                case 2:
                    // here we can finally properly calculate a circle
                   return TwoPointsCircle(R.ElementAt(0), R.ElementAt(1));
                    //break;
               case 3:
                   // uses function wirtten earlier to calculate circle given 3 points
                    return ThreePointCircle(R.ElementAt(0), R.ElementAt(1), R.ElementAt(2));
                    //break;
                default:
                    // Program should never enter here.
                    return null;
                }
             }
                 Point p = P.ElementAt(sizeP - 1);
                 // A recursive call to get the minimum Circle using fewer points each time.
                 Circle minC = Welzl(P, R, sizeP - 1);
                 // Checks if the point p chosen at the current recursion level is inside the minimum circle we found.
                 if (IsPointInside(minC, p))
                 {
                    return minC;
                 }
                 // Adds another point to our circle and recursively checks if it's the minimum.
                 R.Add(p);
                 return Welzl(P, R, sizeP - 1);
        }


        public static Circle FindMinCircle(List<Point> pointsList)
        {
            // obtain a time-based seed using time since epoch for a proper random shuffle.
            //unsigned int seed = std::chrono::system_clock::now().time_since_epoch().count();
            // Randomly shuffles the points as is proposed by Welzl's algorithm.
            //shuffle(pointsVector.begin(), pointsVector.end(), default_random_engine(seed));
            // Calls the recursive function on the vector of points.
            return Welzl(pointsList, new List<Point>(), pointsList.Count);
        }
    }
}
