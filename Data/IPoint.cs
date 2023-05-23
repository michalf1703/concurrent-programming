using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public abstract class IPoint
    {
        public abstract double XCoordinate { get; }
        public abstract double YCoordinate { get; }

        public static IPoint CreatePosition(double xCoordinate, double yCoordinate)
        {
            return new Point(xCoordinate, yCoordinate);
        }

        private class Point : IPoint
        {
            public override double XCoordinate { get; }
            public override double YCoordinate { get; }

            internal Point(double xCoordinate, double yCoordinate)
            {
                this.XCoordinate = xCoordinate;
                this.YCoordinate = yCoordinate;
            }
        }
    }
}
