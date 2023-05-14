using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public struct Vector2
    {
        // Statyczne, tylko do odczytu wektory, które są powszechnie używane
        public static readonly Vector2 Zero = new Vector2(0, 0);  // Wektor o wartości X i Y równych 0
        public static readonly Vector2 One = new Vector2(1, 1);   // Wektor o wartości X i Y równych 1
        public static readonly Vector2 Up = new Vector2(0f, 1f);  // Wektor skierowany w górę (o wartości Y równym 1)
        public static readonly Vector2 Down = new Vector2(0f, -1f); // Wektor skierowany w dół (o wartości Y równym -1)
        public static readonly Vector2 Left = new Vector2(-1f, 0f); // Wektor skierowany w lewo (o wartości X równym -1)
        public static readonly Vector2 Right = new Vector2(1f, 0f); // Wektor skierowany w prawo (o wartości X równym 1)

        // Właściwości publiczne dla wartości X i Y
        public float X { get; set; }  // Wartość X wektora
        public float Y { get; set; }  // Wartość Y wektora

        // Konstruktor, który przyjmuje wartości X i Y jako double
        public Vector2(double x, double y)
        {
            X = (float)x;
            Y = (float)y;
        }

        // Metoda statyczna, która oblicza odległość między dwoma punktami Vector2
        public static float Distance(Vector2 punkt1, Vector2 punkt2)
        {
            return (float)Math.Sqrt(DistanceSquared(punkt1, punkt2));
        }

        // Metoda statyczna, która oblicza odległość między dwoma punktami Vector2 podniesioną do kwadratu
        public static float DistanceSquared(Vector2 punkt1, Vector2 punkt2)
        {
            float roznicaX = punkt1.X - punkt2.X;
            float roznicaY = punkt1.Y - punkt2.Y;
            return roznicaX * roznicaX + roznicaY * roznicaY;
        }

        // Metoda, która sprawdza, czy wektor jest zerowy
        public bool IsZero()
        {
            return Equals(Zero);
        }

        // Przeciążenie metody ToString(), aby wyświetlała wartości X i Y wektora
        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }

        // Metoda dekonstruująca Vector2 na jego wartości X i Y
        public void Deconstruct(out float x, out float y)
        {
            x = X;
            y = Y;
        }

        // Metoda sprawdzająca, czy dwa obiekty Vector2 są równe
        public bool Equals(Vector2 other)
        {
            float roznicaX = X - other.X;
            float roznicaY = Y - other.Y;
            return roznicaX * roznicaX + roznicaY * roznicaY < 9.99999944E-11f;
        }

        // Przeciążenie metody GetHashCode() w celu obliczenia unikalnego kodu skrótu dla wektora
        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        // Przeciążenie operatorów dodawania, odejmowania, mnożenia i dzielenia obiektów Vector2
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
