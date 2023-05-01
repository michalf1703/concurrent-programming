using System;
using System.Collections.Generic;
using System.Threading;

namespace Data
{
    public abstract class DataAbstractAPI : IObserver<int>, IObservable<int>
    {
        // Declare a set of abstract methods that must be implemented by derived classes
        public abstract double getBallPositionX(int ballId);
        public abstract double getBallPositionY(int ballId);
        public abstract int getBallRadius(int ballId);
        public abstract double getBallSpeedX(int ballId);
        public abstract double getBallSpeedY(int ballId);
        public abstract double getBallMass(int ballId);
        public abstract int getBoardSize();
        public abstract void setBallSpeed(int ballId, double speedX, double speedY);
        public abstract void createBalls(int ballsAmount);
        public abstract int getBallsAmount();

        // Declare methods required by the IObserver interface that must be implemented by derived classes
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(int value);

        // Declare a method required by the IObservable interface that must be implemented by derived classes
        public abstract IDisposable Subscribe(IObserver<int> observer);

        // Define a static factory method that creates an instance of the DataApi class
        public static DataAbstractAPI CreateDataApi()
        {
            return new DataApi();
        }
        // Define a nested class called DataApi that inherits from the DataAbstractAPI class
        private class DataApi : DataAbstractAPI
        {
            // Declare private fields
            private BallRepository ballRepository;
            private IDisposable unsubscriber;
            static object _lock = new object();
            private IList<IObserver<int>> observers;
            private Barrier barrier;



            // Define a constructor that initializes the ballRepository and observers fields
            public DataApi()
            {
                this.ballRepository = new BallRepository();

                observers = new List<IObserver<int>>();
            }
            // Implement the abstract methods declared in the DataAbstractAPI class
            public override double getBallPositionX(int ballId)
            {
                return this.ballRepository.GetBall(ballId).PositionX;
            }

            public override double getBallPositionY(int ballId)
            {
                return this.ballRepository.GetBall(ballId).PositionY;
            }

            public override int getBoardSize()
            {
                return ballRepository.BoardSize;
            }

            public override double getBallMass(int ballId)
            {
                return this.ballRepository.GetBall(ballId).Mass;
            }

            public override int getBallRadius(int ballId)
            {
                return this.ballRepository.GetBall(ballId).Radius;
            }

            public override double getBallSpeedX(int ballId)
            {
                return this.ballRepository.GetBall(ballId).MoveX;
            }

            public override double getBallSpeedY(int ballId)
            {
                return this.ballRepository.GetBall(ballId).MoveY;
            }

            public override void setBallSpeed(int ballId, double speedX, double speedY)
            {
                this.ballRepository.GetBall(ballId).MoveX = speedX;
                this.ballRepository.GetBall(ballId).MoveY = speedY;
            }

            public override int getBallsAmount()
            {
                return ballRepository.balls.Count;
            }

            public override void createBalls(int ballsAmount)
            
            {
                // Create a new barrier with the specified number of balls.
                barrier = new Barrier(ballsAmount);
                // Create the specified number of balls and add them to the ball repository.
                ballRepository.CreateBalls(ballsAmount);
                // Subscribe to each ball and start it moving.
                foreach (var ball in ballRepository.balls)
                {
                    Subscribe(ball);
                    ball.StartMoving();
                }

            }

            #region observer

            // Subscribe to an observable provider.
            public virtual void Subscribe(IObservable<int> provider)
            {
                if (provider != null)
                    unsubscriber = provider.Subscribe(this);
            }
            // Unsubscribe from the observable provider.
            public override void OnCompleted()
            {
                Unsubscribe();
            }
            // Handle an error from the observable provider.
            public override void OnError(Exception error)
            {
                throw error;
            }
            // Receive a value from the observable provider.
            public override void OnNext(int value)
            {
                // Signal that this thread has reached the barrier and wait for all other threads to reach it.
                barrier.SignalAndWait();
                // Forward the value to all registered observers.
                foreach (var observer in observers)
                {
                    observer.OnNext(value);
                }

            }
            // Unsubscribe from the observable provider.
            public virtual void Unsubscribe()
            {
                unsubscriber.Dispose();
            }

            #endregion

            #region provider
            // Subscribe to an observer.
            public override IDisposable Subscribe(IObserver<int> observer)
            {
                if (!observers.Contains(observer))
                    observers.Add(observer);
                // Return an object that can be used to unsubscribe from the observer.
                return new Unsubscriber(observers, observer);
            }

            private class Unsubscriber : IDisposable
            {
                private IList<IObserver<int>> _observers;
                private IObserver<int> _observer;

                // An object that can be used to unsubscribe from the observer.
                public Unsubscriber
                (IList<IObserver<int>> observers, IObserver<int> observer)
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
}