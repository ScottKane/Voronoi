using UnityEngine;

public static class Geometry
{
    //Is a triangle in 2d space oriented clockwise or counter-clockwise
    //https://math.stackexchange.com/questions/1324179/how-to-tell-if-3-connected-points-are-connected-clockwise-or-counter-clockwise
    //https://en.wikipedia.org/wiki/Curve_orientation
    public static bool IsTriangleOrientedClockwise(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        bool isClockWise = true;

        float determinant = p1.x * p2.y + p3.x * p1.y + p2.x * p3.y - p1.x * p3.y - p3.x * p2.y - p2.x * p1.y;

        if (determinant > 0f)
        {
            isClockWise = false;
        }

        return isClockWise;
    }

    //Calculate the center of circle in 2d space given three coordinates
    //http://paulbourke.net/geometry/circlesphere/
    public static Vector2 CalculateCircleCenter(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        Vector2 center = new Vector2();

        float ma = (p2.y - p1.y) / (p2.x - p1.x);
        float mb = (p3.y - p2.y) / (p3.x - p2.x);

        center.x = (ma * mb * (p1.y - p3.y) + mb * (p1.x + p2.x) - ma * (p2.x + p3.x)) / (2 * (mb - ma));
        center.y = (-1 / ma) * (center.x - (p1.x + p2.x) / 2) + (p1.y + p2.y) / 2;

        return center;
    }

    public static Vector2 XZ(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    //Is a point d inside, outside or on the same circle as a, b, c
    //https://gamedev.stackexchange.com/questions/71328/how-can-i-add-and-subtract-convex-polygons
    //Returns positive if inside, negative if outside, and 0 if on the circle
    public static float IsPointInsideOutsideOrOnCircle(Vector2 aVec, Vector2 bVec, Vector2 cVec, Vector2 dVec)
    {
        //This first part will simplify how we calculate the determinant
        float a = aVec.x - dVec.x;
        float d = bVec.x - dVec.x;
        float g = cVec.x - dVec.x;

        float b = aVec.y - dVec.y;
        float e = bVec.y - dVec.y;
        float h = cVec.y - dVec.y;

        float c = a * a + b * b;
        float f = d * d + e * e;
        float i = g * g + h * h;

        //float c = Mathf.Abs(a) + Mathf.Abs(b);
        //float f = Mathf.Abs(d) + Mathf.Abs(e);
        //float i = Mathf.Abs(g) + Mathf.Abs(g);

        float determinant = (a * e * i) + (b * f * g) + (c * d * h) - (g * e * c) - (h * f * a) - (i * d * b);
        //float determinant = ((a * e) + i) + ((b * g) + f) + ((d * h) + c) - ((g * e) + c) - ((h * a) + f) - ((d * b) + i);

        return determinant;
    }

    //Is a quadrilateral convex? Assume no 3 points are colinear and the shape doesnt look like an hourglass
    public static bool IsQuadrilateralConvex(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        bool isConvex = false;

        bool abc = IsTriangleOrientedClockwise(a, b, c);
        bool abd = IsTriangleOrientedClockwise(a, b, d);
        bool bcd = IsTriangleOrientedClockwise(b, c, d);
        bool cad = IsTriangleOrientedClockwise(c, a, d);

        if (abc && abd && bcd & !cad)
        {
            isConvex = true;
        }
        else if (abc && abd && !bcd & cad)
        {
            isConvex = true;
        }
        else if (abc && !abd && bcd & cad)
        {
            isConvex = true;
        }
        //The opposite sign, which makes everything inverted
        else if (!abc && !abd && !bcd & cad)
        {
            isConvex = true;
        }
        else if (!abc && !abd && bcd & !cad)
        {
            isConvex = true;
        }
        else if (!abc && abd && !bcd & !cad)
        {
            isConvex = true;
        }


        return isConvex;
    }
}