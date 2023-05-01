using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public class Ball : IObservable<int>
    {
        // Unique identifier for the ball.
        public int Id { get; }

        // The position of the ball on the X and Y axes.
        public double PositionX { get; private set; }
        public double PositionY { get; private set; }

        // The radius and mass of the ball.
        public int Radius { get; } = 15;
        public double Mass { get; } = 10;

        // The amount by which the ball moves on the X and Y axes.
        public double MoveX { get; set; }
        public double MoveY { get; set; }

        // The collection of observers who have subscribed to this ball's movements.
        internal readonly IList<IObserver<int>> observers;

        // The task that runs the ball's movement logic.
        private Task BallThread;

        // Constructor for a ball.
        public Ball(int id)
        {
            this.Id = id;

            // Set up a random starting position and movement direction.
            Random random = new Random();
            this.PositionX = Convert.ToDouble(random.Next(1, 500));
            this.PositionY = Convert.ToDouble(random.Next(1, 500));
            this.MoveX = random.NextDouble() * (3 - 2) + 2;
            this.MoveY = random.NextDouble() * (3 - 2) + 2;

            // Initialize the observer collection.
            observers = new List<IObserver<int>>();
        }

        // Start the ball's movement logic on a separate thread.
        public void StartMoving()
        {
            this.BallThread = new Task(MoveBall);
            BallThread.Start();
        }

        // The logic for moving the ball.
        public void MoveBall()
        {
            while (true)
            {
                // Update the ball's position and notify all observers.
                ChangeBallPosition();
                foreach (var observer in observers.ToList())
                {
                    if (observer != null)
                    {
                        observer.OnNext(Id);
                    }
                }

                // Pause for a short amount of time to slow down the ball's movement.
                System.Threading.Thread.Sleep(1);
            }
        }

        // Update the ball's position based on its current movement direction.
        public void ChangeBallPosition()
        {
            PositionX += MoveX;
            PositionY += MoveY;
        }

        #region provider

        // Subscribe an observer to this ball's movements.
        public IDisposable Subscribe(IObserver<int> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        // Class that unsubscribes an observer from this ball's movements.
        private class Unsubscriber : IDisposable
        {
            private IList<IObserver<int>> _observers;
            private IObserver<int> _observer;

            public Unsubscriber(IList<IObserver<int>> observers, IObserver<int> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        #endregion
    }
}
