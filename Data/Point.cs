using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    // Struktura reprezentująca punkt na płaszczyźnie.
    public struct Point
    {
        // Współrzędna X punktu.
        public double X;
        // Współrzędna Y punktu.
        public double Y;

        // Konstruktor tworzący punkt o podanych współrzędnych.
        public Point(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }

}
