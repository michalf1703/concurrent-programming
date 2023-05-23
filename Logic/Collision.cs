using Data;
using System;
using System.Numerics;

namespace Logic
{
    public class Collision
    {
        public static bool IsCollision(IBall current, IBall other)
        {
            Vector2 currentDelta = current.Position + current.Move - (other.Position + other.Move);
            float distance = currentDelta.Length();

            if (Math.Abs(distance) <= current.Radius + other.Radius)
            {
                return true;
            }

            return false;
        }

        public static void IsTouchingBoundaries(IBall ball, int boardSize)
        {
            Vector2 newPosition = ball.Position + ball.Move;

            if ((newPosition.X > boardSize && ball.Position.X > 0) || (newPosition.X < 0 && ball.Position.X < 0))
            {
                ball.Speed *= new Vector2(-1, 1);
            }

            if ((newPosition.Y > boardSize && ball.Position.Y > 0) || (newPosition.Y < 0 && ball.Position.Y < 0))
            {
                ball.Speed *= new Vector2(1, -1);
            }
        }

        public static void ImpulseSpeed(IBall current, IBall other)
        {
            Vector2 currentVelocity = current.Speed;
            Vector2 currentPosition = current.Position;
            float currentMass = (float)current.Mass;

            Vector2 otherVelocity = other.Speed;
            Vector2 otherPosition = other.Position;
            float otherMass = (float)other.Mass;

            Vector2 deltaPosition = currentPosition - otherPosition;
            float distance = deltaPosition.Length();

            Vector2 normal = Vector2.Normalize(deltaPosition);
            Vector2 tangent = new Vector2(-normal.Y, normal.X);

            // Dot Product Tangent
            float dpTan1 = Vector2.Dot(currentVelocity, tangent);
            float dpTan2 = Vector2.Dot(otherVelocity, tangent);

            // Dot Product Normal
            float dpNorm1 = Vector2.Dot(currentVelocity, normal);
            float dpNorm2 = Vector2.Dot(otherVelocity, normal);

            // Conservation of momentum in 1D
            float m1 = (dpNorm1 * (currentMass - otherMass) + 2.0f * otherMass * dpNorm2) / (currentMass + otherMass);
            float m2 = (dpNorm2 * (otherMass - currentMass) + 2.0f * currentMass * dpNorm1) / (currentMass + otherMass);

            Vector2 currentNewVelocity = tangent * dpTan1 + normal * m1;
            Vector2 otherNewVelocity = tangent * dpTan2 + normal * m2;

            current.Speed = currentNewVelocity;
            other.Speed = otherNewVelocity;
        }
    }
}
