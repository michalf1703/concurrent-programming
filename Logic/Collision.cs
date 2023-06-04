using Data;
using System;
using System.Numerics;

namespace Logic
{
    internal class Collision
    {
        // Sprawdza, czy wystąpiła kolizja między dwiema piłkami
        public static bool IsCollision(IBall current, IBall other)
        {
            Vector2 currentDelta = current.Position - other.Position;   // Oblicza różnicę wektorową pomiędzy pozycją bieżącej piłki a pozycją innej piłki
            float distance = currentDelta.Length();                     // Oblicza długość wektora różnicy

            if (Math.Abs(distance) <= current.Radius + other.Radius)     // Sprawdza, czy odległość między piłkami jest mniejsza lub równa sumie ich promieni
            {
                return true;                                            // Zwraca true, jeśli wystąpiła kolizja
            }

            return false;                                               // Zwraca false, jeśli nie wystąpiła kolizja
        }

        // Sprawdza, czy piłka dotyka granic planszy i odwraca jej prędkość w odpowiednim kierunku
public static void IsTouchingBoundaries(IBall ball, int boardSize)
{
    Vector2 newPosition = ball.Position;
    Vector2 speed = ball.Speed;

    if ((newPosition.X > boardSize && speed.X > 0) || (newPosition.X < 0 && speed.X < 0))
    {
        if (Math.Sign(speed.X) != Math.Sign(boardSize - newPosition.X))
        {
            ball.Speed *= new Vector2(-1, 1); // Odwraca prędkość piłki w osi X
        }
    }

    if ((newPosition.Y > boardSize && speed.Y > 0) || (newPosition.Y < 0 && speed.Y < 0))
    {
        if (Math.Sign(speed.Y) != Math.Sign(boardSize - newPosition.Y))
        {
            ball.Speed *= new Vector2(1, -1); // Odwraca prędkość piłki w osi Y
        }
    }
}


        // Wykonuje impulsowe zmiany prędkości dwóch piłek po kolizji
        public static void ImpulseSpeed(IBall current, IBall other)
        {
            Vector2 currentVelocity = current.Speed;                     // Pobiera prędkość bieżącej piłki
            Vector2 currentPosition = current.Position;                 // Pobiera pozycję bieżącej piłki
            float currentMass = (float)current.Mass;                     // Pobiera masę bieżącej piłki
            Vector2 otherVelocity = other.Speed;                         // Pobiera prędkość innej piłki
            Vector2 otherPosition = other.Position;                     // Pobiera pozycję innej piłki
            float otherMass = (float)other.Mass;                         // Pobiera masę innej piłki

            Vector2 deltaPosition = currentPosition - otherPosition;     // Oblicza różnicę wektorową pomiędzy pozycjami piłek
            float distance = deltaPosition.Length();                     // Oblicza długość wektora różnicy

            Vector2 normal = Vector2.Normalize(deltaPosition);           // Normalizuje wektor różnicy pozycji jako wektor normalny kolizji
            Vector2 tangent = new Vector2(-normal.Y, normal.X);          // Oblicza wektor styczny do kolizji

            // Mnożenie wektorowe styczne
            float dpTan1 = Vector2.Dot(currentVelocity, tangent);        // Oblicza iloczyn skalarny prędkości bieżącej piłki i wektora stycznego
            float dpTan2 = Vector2.Dot(otherVelocity, tangent);          // Oblicza iloczyn skalarny prędkości innej piłki i wektora stycznego

            // Mnożenie wektorowe normalne
            float dpNorm1 = Vector2.Dot(currentVelocity, normal);        // Oblicza iloczyn skalarny prędkości bieżącej piłki i wektora normalnego
            float dpNorm2 = Vector2.Dot(otherVelocity, normal);          // Oblicza iloczyn skalarny prędkości innej piłki i wektora normalnego

            // Zachowanie pędu w 1D
            float m1 = (dpNorm1 * (currentMass - otherMass) + 2.0f * otherMass * dpNorm2) / (currentMass + otherMass);
            float m2 = (dpNorm2 * (otherMass - currentMass) + 2.0f * currentMass * dpNorm1) / (currentMass + otherMass);

            Vector2 currentNewVelocity = tangent * dpTan1 + normal * m1; // Oblicza nową prędkość bieżącej piłki po kolizji
            Vector2 otherNewVelocity = tangent * dpTan2 + normal * m2;   // Oblicza nową prędkość innej piłki po kolizji

            current.Speed = currentNewVelocity;                          // Ustawia nową prędkość bieżącej piłki
            other.Speed = otherNewVelocity;                              // Ustawia nową prędkość innej piłki
        }
    }
}
