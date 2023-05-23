using System;
using Data;

namespace Logic
{
    public class Collision
    {
        public static bool IsCollision(IBall current, IBall other)
        {
            Vector2 currentPosition = current.Position + current.Move;
            Vector2 otherPosition = other.Position + other.Move;

            double distance = Math.Sqrt(Math.Pow(currentPosition.X - otherPosition.X, 2) + Math.Pow(currentPosition.Y - otherPosition.Y, 2));


            if (Math.Abs(distance) <= current.Radius + other.Radius)
            {
                return true;
            }

            return false;
        }

        public static void IsTouchingBoundaries(IBall ball, int boardSize)
        {
            Vector2 newPos = ball.Position + ball.Move;

            if ((newPos.X > boardSize && ball.Position.X > 0) || (newPos.X < 0 && ball.Position.X < 0))
            {
                ball.Move = new Vector2(-ball.Move.X, ball.Move.Y);
            }

            if ((newPos.Y > boardSize && ball.Position.Y > 0) || (newPos.Y < 0 && ball.Position.Y < 0))
            {
                ball.Move = new Vector2(ball.Move.X, -ball.Move.Y);
            }
        }



        public static void ImpulseSpeed(IBall current, IBall other)
        {
            Vector2 currentVelocity = current.Speed;
            Vector2 currentPosition = current.Position;
            double currentMass = current.Mass;

            Vector2 otherVelocity = other.Speed;
            Vector2 otherPosition = other.Position;
            double otherMass = other.Mass;

            double fDistance = Math.Sqrt((currentPosition.X - otherPosition.X) * (currentPosition.X - otherPosition.X) + (currentPosition.Y - otherPosition.Y) * (currentPosition.Y - otherPosition.Y));

            double nx = (otherPosition.X - currentPosition.X) / fDistance;
            double ny = (otherPosition.Y - currentPosition.Y) / fDistance;

            double tx = -ny;
            double ty = nx;

            // Dot Product Tangent
            double dpTan1 = currentVelocity.X * tx + currentVelocity.Y * ty;
            double dpTan2 = otherVelocity.X * tx + otherVelocity.Y * ty;

            // Dot Product Normal
            double dpNorm1 = currentVelocity.X * nx + currentVelocity.Y * ny;
            double dpNorm2 = otherVelocity.X * nx + otherVelocity.Y * ny;

            // Conservation of momentum in 1D
            double m1 = (dpNorm1 * (currentMass - otherMass) + 2.0f * otherMass * dpNorm2) / (currentMass + otherMass);
            double m2 = (dpNorm2 * (otherMass - currentMass) + 2.0f * currentMass * dpNorm1) / (currentMass + otherMass);

            Vector2 currentNewVelocity = new Vector2(tx * dpTan1 + nx * m1, ty * dpTan1 + ny * m1);
            Vector2 otherNewVelocity = new Vector2(tx * dpTan2 + nx * m2, ty * dpTan2 + ny * m2);

            current.Speed = currentNewVelocity;
            other.Speed = otherNewVelocity;
        }
    }
}
