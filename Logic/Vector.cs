using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public struct Vector2
    {
        // Static readonly vectors that are commonly used
        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 One = new Vector2(1, 1);
        public static readonly Vector2 Up = new Vector2(0f, 1f);
        public static readonly Vector2 Down = new Vector2(0f, -1f);
        public static readonly Vector2 Left = new Vector2(-1f, 0f);
        public static readonly Vector2 Right = new Vector2(1f, 0f);

        // Public properties for X and Y values
        public float X { get; set; }
        public float Y { get; set; }

        // Constructor that takes in X and Y values as doubles
        public Vector2(double x, double y)
        {
            X = (float)x;
            Y = (float)y;
        }

        // Static method that calculates the distance between two Vector2 points
        public static float Distance(Vector2 point1, Vector2 point2)
        {
            return (float)Math.Sqrt(DistanceSquared(point1, point2));
        }

        // Static method that calculates the squared distance between two Vector2 points
        public static float DistanceSquared(Vector2 point1, Vector2 point2)
        {
            float xDifference = point1.X - point2.X;
            float yDifference = point1.Y - point2.Y;
            return xDifference * xDifference + yDifference * yDifference;
        }

        // Method that checks if the vector is zero
        public bool IsZero()
        {
            return Equals(Zero);
        }

        // Override of the ToString() method to display the vector's X and Y values
        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }

        // Method to deconstruct the Vector2 into its X and Y values
        public void Deconstruct(out float x, out float y)
        {
            x = X;
            y = Y;
        }

        // Method to check if two Vector2 objects are equal
        public bool Equals(Vector2 other)
        {
            float xDiff = X - other.X;
            float yDiff = Y - other.Y;
            return xDiff * xDiff + yDiff * yDiff < 9.99999944E-11f;
        }

        // Override of the GetHashCode() method to calculate a unique hash code for the vector
        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        // Operator overloads for adding, subtracting, multiplying, and dividing Vector2 objects
        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2
            {
                X = lhs.X + rhs.X,
                Y = lhs.Y + rhs.Y,
            };
        }

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2
            {
                X = lhs.X - rhs.X,
                Y = lhs.Y - rhs.Y,
            };
        }

        public static Vector2 operator *(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2
            {
                X = lhs.X * rhs.X,
                Y = lhs.Y * rhs.Y,
            };
        }

        public static Vector2 operator /(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2
            {
                X = lhs.X / rhs.X,
                Y = lhs.Y / rhs.Y,
            };
        }
        public static Vector2 operator -(Vector2 vector)
        {
            return new Vector2
            {
                X = -vector.X,
                Y = -vector.Y,
            };
        }

        public static Vector2 operator *(Vector2 lhs, float d)
        {
            return new Vector2
            {
                X = lhs.X * d,
                Y = lhs.Y * d,
            };
        }

        public static Vector2 operator /(Vector2 lhs, float d)
        {
            return new Vector2
            {
                X = lhs.X / d,
                Y = lhs.Y / d,
            };
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return !(lhs == rhs);
        }
    }
}
