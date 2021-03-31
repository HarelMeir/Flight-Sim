// Ariel Drellich 328935275
// Eyal Michon 314965989

#ifndef MINCIRCLE_H_
#define MINCIRCLE_H_

#include "anomaly_detection_util.h"
#include <math.h>
#include <iostream>
#include <random> // std::default_random_engine
#include <chrono> // std::chrono::system_clock
#include <vector>
#include <algorithm>

using namespace std;

class Circle
{
public:
    Point center;
    float radius;
    Circle():center(0,0),radius(0){};
    Circle(Point c, float r) : center(c), radius(r) {}
};

float dist(const Point &a, const Point &b);

bool isPointInside(const Circle &c, const Point &p);

Circle twoPointsCircle(const Point &p1, const Point &p2);

Circle threePointCircle(const Point &p1, const Point &p2, const Point &p3);

Circle welzl(const vector<Point> &P, vector<Point> R, int sizeP);

Circle findMinCircle(Point **points, size_t size);

Circle findMinCircle(vector<Point> &pointsVector);

#endif /* MINCIRCLE_H_ */
