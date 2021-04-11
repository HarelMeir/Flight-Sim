using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flight_Sim.Model;

namespace Flight_Sim.cppToCSharp
{
    class Circle
    {
    Point center;
        float radius;
        public Circle()
        {
            center = new Point(0, 0);
            radius = 0;
        }
        Circle(Point c, float r)
        {
            center = c;
            radius = r;
        }
    };
    class MinCircle
    {/*
    public float dist(Point a, Point b);

    public bool isPointInside(Circle c, Point p);

    Circle twoPointsCircle(Point p1, Point p2);

    Circle threePointCircle(Point p1, Point p2, Point p3);

    Circle welzl(List<Point> P, List<Point> R, int sizeP);

    Circle findMinCircle(Point** points[], int size);

    Circle findMinCircle(List<Point> pointsList);
        */
    }
}
