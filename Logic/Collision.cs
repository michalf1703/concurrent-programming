using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Numerics;

namespace Logic
{

    // Klasa reprezentująca zderzenie dwóch piłek w przestrzeni 2D
    public class Collision
    {
        // Masa obiektu
        private int mass;
        // Promień obiektu
        private int radious;
        // Aktualna pozycja obiektu
        private Vector2 position;
        // Aktualna prędkość obiektu
        private Vector2 velocity;

        // Konstruktor służący do utworzenia obiektu Collision z początkową pozycją, prędkością, promieniem i masą.
        public Collision(double positionX, double positionY, double speedX, double speedY, int radius, int mass)
        {
            this.velocity = new Vector2(speedX, speedY); // tworzy wektor prędkości z podanych wartości X i Y
            this.position = new Vector2(positionX, positionY); // tworzy wektor pozycji z podanych wartości X i Y
            this.radious = radius; // ustawienie promienia
            this.mass = mass; // ustawienie masy
        }

        // Sprawdza, czy ten obiekt koliduje z innym obiektem w danej pozycji i promieniu. 
        // Jeśli tryb jest true, używa następnej przewidywanej pozycji na podstawie aktualnej prędkości.
        public bool IsCollision(double otherX, double otherY, double otherRadius, bool mode)
        {
            double currentX;
            double currentY;
            // Jeśli tryb jest true, przewiduj następną pozycję na podstawie aktualnej prędkości.
            if (mode)
            {
                currentX = position.X + velocity.X;
                currentY = position.Y + velocity.Y;
            }
            else
            {
                currentX = position.X;
                currentY = position.Y;
            }
            // Oblicza odległość między tym obiektem a innym obiektem.
            double distance = Math.Sqrt(Math.Pow(currentX - otherX, 2) + Math.Pow(currentY - otherY, 2));

            // Jeśli odległość jest mniejsza lub równa sumie dwóch promieni, występuje kolizja.
            if (Math.Abs(distance) <= radious + otherRadius)
            {
                return true;
            }
            // W przeciwnym razie nie występuje kolizja.
            return false;
        }
        // Sprawdza, czy ta kolizja dotyka granic osi X planszy o podanym rozmiarze.
        public bool IsTouchingBoundariesX(int boardSize)
        {
            double newX = position.X + velocity.X;

            if ((newX > boardSize && velocity.X > 0) || (newX < 0 && velocity.X < 0))
            {
                return true;
            }
            return false;
        }
        // Sprawdza, czy ta kolizja dotyka granic osi Y planszy o podanym rozmiarze.
        public bool IsTouchingBoundariesY(int boardSize)
        {
            double newY = position.Y + velocity.Y;
            if ((newY > boardSize && velocity.Y > 0) || (newY < 0 && velocity.Y < 0))
            {
                return true;
            }
            return false;
        }
        // Oblicza nowe prędkości dla dwóch obiektów, które się zderzają, mając podane pozycje, prędkości i masy.
        // Zwraca tablicę dwóch obiektów Vector2 reprezentujących nowe prędkości dla każdego z obiektów.
        public Vector2[] ImpulseSpeed(double otherX, double otherY, double speedX, double speedY, double otherMass)
        {
            Vector2 velocityOther = new Vector2(speedX, speedY);
            Vector2 positionOther = new Vector2(otherX, otherY);

            double fDistance = Math.Sqrt((position.X - positionOther.X) * (position.X - positionOther.X) + (position.Y - positionOther.Y) * (position.Y - positionOther.Y));

            // Obliczanie normalnej i stycznej do powierzchni zderzenia
            double nx = (positionOther.X - position.X) / fDistance;
            double ny = (positionOther.Y - position.Y) / fDistance;
            double tx = -ny;
            double ty = nx;

            // Obliczanie iloczynu skalarnego dla wektorów stycznych i normalnych dla obu obiektów
            double dpTan1 = velocity.X * tx + velocity.Y * ty;
            double dpTan2 = velocityOther.X * tx + velocityOther.Y * ty;
            double dpNorm1 = velocity.X * nx + velocity.Y * ny;
            double dpNorm2 = velocityOther.X * nx + velocityOther.Y * ny;

            // Obliczanie nowych prędkości dla obu obiektów
            double m1 = (dpNorm1 * (mass - otherMass) + 2.0f * otherMass * dpNorm2) / (mass + otherMass);
            double m2 = (dpNorm2 * (otherMass - mass) + 2.0f * mass * dpNorm1) / (mass + otherMass);

            // Zwracanie nowych prędkości dla obu obiektów w postaci tablicy Vector2
            return new Vector2[2] { new Vector2(tx * dpTan1 + nx * m1, ty * dpTan1 + ny * m1), new Vector2(tx * dpTan2 + nx * m2, ty * dpTan2 + ny * m2) };

        }
    }
}
