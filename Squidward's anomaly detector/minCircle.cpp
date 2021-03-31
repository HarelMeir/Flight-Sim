// Ariel Drellich 328935275
// Eyal Michon 314965989
#include "minCircle.h"
#include <iostream>

// Mathematical distance between two given points
float dist(const Point &a, const Point &b)
{
    return sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
}
// Check whether or not the point is within the radius of the circle.
bool isPointInside(const Circle &c, const Point &p)
{
    return dist(c.center, p) <= c.radius;
}
// Creates a circle from two points.
Circle twoPointsCircle(const Point &p1, const Point &p2)
{
    return {{(p1.x + p2.x) / 2, (p1.y + p2.y) / 2}, dist(p1, p2) / 2};
}
// Creates a circle from three points.
Circle threePointCircle(const Point &p1, const Point &p2, const Point &p3)
{
    float xc = ((p1.x * p1.x - p2.x * p2.x + p1.y * p1.y - p2.y * p2.y) * (p1.y - p3.y) +
                (-p1.x * p1.x + p3.x * p3.x - p1.y * p1.y + p3.y * p3.y) * (p1.y - p2.y)) /
               (2 * ((p1.x - p2.x) * (p1.y - p3.y) - (p1.x - p3.x) * (p1.y - p2.y)));

    float yc = ((p1.x * p1.x - p3.x * p3.x - 2 * xc * (p1.x - p3.x) + p1.y * p1.y - p3.y * p3.y)) / (2 * (p1.y - p3.y));
    float radius = sqrt((p1.x - xc) * (p1.x - xc) + (p1.y - yc) * (p1.y - yc));
    return {{xc, yc}, radius};
}
// Using Welzl's algorithm we created a recursive function that calculates the minimum circle around a group of points.
Circle welzl(const vector<Point> &P, vector<Point> R, int sizeP)
{
    // trivial cases + case that we need to make a circle using 2 or 3 points (depending on how many we recieve).
    if (sizeP == 0 || R.size() == 3)
    {
        switch (R.size())
        {
        case 0:
            // trivial case, nothing to work off of
            return Circle(Point(0, 0), 0);
            break;
        case 1:
            // also trivial, one point can't have a radius
            return Circle(R.at(0), 0);
            break;
        case 2:
            // here we can finally properly calculate a circle
            return twoPointsCircle(R.at(0), R.at(1));
            break;
        case 3:
            // uses function wirtten earlier to calculate circle given 3 points
            return threePointCircle(R.at(0), R.at(1), R.at(2));
            break;
        default:
            // Program should never enter here.
            exit(1);
            break;
        }
    }

    Point p = P.at(sizeP - 1);
    // A recursive call to get the minimum Circle using fewer points each time.
    Circle minC = welzl(P, R, sizeP - 1);
    // Checks if the point p chosen at the current recursion level is inside the minimum circle we found.
    if (isPointInside(minC, p))
    {
        return minC;
    }
    // Adds another point to our circle and recursively checks if it's the minimum.
    R.push_back(p);
    return welzl(P, R, sizeP - 1);
}

Circle findMinCircle(Point **points, size_t size)
{
    // a vector to store all of our points in instead of the given array for easier managment.
    // we decided to switch to vectors after seeing that it didn't affect run-time.
    vector<Point> pointsVector;
    for (size_t i = 0; i < size; i++)
    {
        pointsVector.push_back(*points[i]);
    }
    // obtain a time-based seed using time since epoch for a proper random shuffle.
    unsigned int seed = std::chrono::system_clock::now().time_since_epoch().count();
    // Randomly shuffles the points as is proposed by Welzl's algorithm.
    shuffle(pointsVector.begin(), pointsVector.end(), default_random_engine(seed));
    // Calls the recursive function on the vector of points.
    return welzl(pointsVector, {}, size);
}

Circle findMinCircle(vector<Point> &pointsVector)
{
    // obtain a time-based seed using time since epoch for a proper random shuffle.
    unsigned int seed = std::chrono::system_clock::now().time_since_epoch().count();
    // Randomly shuffles the points as is proposed by Welzl's algorithm.
    shuffle(pointsVector.begin(), pointsVector.end(), default_random_engine(seed));
    // Calls the recursive function on the vector of points.
    return welzl(pointsVector, {}, pointsVector.size());
}
