using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    //description of the behavior of the ball
    public class Ball
    {
        //x and y - the current coordinates of the ball
        public double x { get; set; }
        public double y { get; set; }
        //xSpeed and ySpeed - ball speed
        public double xSpeed { get; set; }
        public double ySpeed { get; set; }
        public double r { get; set; }


        public Ball()
        {
            x = generateRandomDouble(21, 479);
            y = generateRandomDouble(21, 479);

            xSpeed = generateRandomDouble(1, 3);
            ySpeed = generateRandomDouble(1, 3);

            r = 10;
        }
        //ball position update 
        public void updatePosition(int axis)
        {
            double x2 = x + xSpeed;
            double y2 = y + ySpeed;
         //check if ball hit top, bottom, left or right wall 
            if (x2 > axis - 10 || x2 < 0)
            {
                xSpeed = -xSpeed;
            }
            if (y2 > axis - 10 || y2 < 0)
            {
                ySpeed = -ySpeed;
            }

            x = x2;
            y = y2;
        }


        //generate random numbers to determine the position and speed of the ball
        Random rng = new Random();
        public double generateRandomDouble(double min, double max)
        {
            return rng.NextDouble() * (max - min) + min;
        }
    }
}
