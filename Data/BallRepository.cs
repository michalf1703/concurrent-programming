using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class BallRepository
    {
        public List<Ball> balls { get; set; } // A list of all the balls created in the repository
        public int BoardSize { get; private set; } = 515; // The size of the game board, which is fixed at 515

        public BallRepository()
        {
            balls = new List<Ball>(); // Initialize the list of balls
        }

        // Creates a specified number of balls and adds them to the list
        public void CreateBalls(int ballsAmount)
        {
            for (int i = 0; i < ballsAmount; i++)
            {
                balls.Add(new Ball(i + 1));
            }
        }

        // Gets a ball by its unique identifier
        public Ball GetBall(int ballId)
        {
            return balls[ballId - 1];
        }
    }
}
