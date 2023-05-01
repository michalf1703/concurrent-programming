using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using Data;

namespace Logic
{
    // The LogicAPI abstract class that implements the IObserver and IObservable interfaces.
    public abstract class LogicAPI : IObserver<int>, IObservable<int>
    {
        // Abstract methods that must be implemented in derived classes.
        public abstract void AddBallsAndStart(int BallsAmount);
        public abstract double getBallPositionX(int ballId);
        public abstract double getBallPositionY(int ballId);
        public abstract int getBallRadius(int ballId);

        // Abstract methods from IObserver interface.
        public abstract IDisposable Subscribe(IObserver<int> observer);
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(int value);

        // A static factory method to create a new BusinessLogic instance with a DataAbstractAPI instance.
        public static LogicAPI CreateLayer(DataAbstractAPI data = default(DataAbstractAPI))
        {
            return new BusinessLogic(data == null ? DataAbstractAPI.CreateDataApi() : data);
        }

        // A class that represents the event arguments for the BallChanged event.
        public class BallChaneEventArgs : EventArgs
        {
            public int ballId { get; set; }
        }

        // The BusinessLogic class that derives from the LogicAPI class and implements the IObservable interface.
        private class BusinessLogic : LogicAPI, IObservable<int>
        {
            private readonly DataAbstractAPI dataAPI;
            private IDisposable unsubscriber;
            static object _lock = new object();
            private IObservable<EventPattern<BallChaneEventArgs>> eventObservable = null;
            public event EventHandler<BallChaneEventArgs> BallChanged;

            public BusinessLogic(DataAbstractAPI dataAPI)
            {
                eventObservable = Observable.FromEventPattern<BallChaneEventArgs>(this, "BallChanged");
                this.dataAPI = dataAPI;
                Subscribe(dataAPI);
            }

            // Implementation of abstract methods from LogicAPI.
            public override double getBallPositionX(int ballId)
            {
                return this.dataAPI.getBallPositionX(ballId);
            }

            public override double getBallPositionY(int ballId)
            {
                return this.dataAPI.getBallPositionY(ballId);
            }

            public override int getBallRadius(int ballId)
            {
                return this.dataAPI.getBallRadius(ballId);
            }

            // Adds the given amount of balls and starts the simulation.
            public override void AddBallsAndStart(int BallsAmount)
            {
                dataAPI.createBalls(BallsAmount);
            }

            #region observer

            // Subscribes to an IObservable instance.
            public virtual void Subscribe(IObservable<int> provider)
            {
                if (provider != null)
                    unsubscriber = provider.Subscribe(this);
            }

            // Notifies the observer of a new ball index.
            public override void OnNext(int value)
            {
                // Enter the lock to prevent race conditions while accessing shared resources
                Monitor.Enter(_lock);
                try
                {
                    // Create a Collision instance to check for collisions with other balls and boundaries
                    Collision collisionControler = new Collision(dataAPI.getBallPositionX(value), dataAPI.getBallPositionY(value), dataAPI.getBallSpeedX(value), dataAPI.getBallSpeedY(value), dataAPI.getBallRadius(value), 10);

                    // Check for collisions with other balls
                    for (int i = 1; i <= dataAPI.getBallsAmount(); i++)
                    {
                        if (value != i)
                        {
                            // Get the properties of the other ball
                            double otherBallX = dataAPI.getBallPositionX(i);
                            double otherBallY = dataAPI.getBallPositionY(i);
                            double otherBallSpeedX = dataAPI.getBallSpeedX(i);
                            double otherBallSpeedY = dataAPI.getBallSpeedY(i);
                            int otherBallRadius = dataAPI.getBallRadius(i);
                            double otherBallMass = dataAPI.getBallMass(i);

                            // Check if there is a collision with the other ball
                            if (collisionControler.IsCollision(otherBallX + otherBallSpeedX, otherBallY + otherBallSpeedY, otherBallRadius, true))
                            {
                                // Check if the balls are already colliding to prevent multiple collisions
                                if (!collisionControler.IsCollision(otherBallX, otherBallY, otherBallRadius, false))
                                {
                                    // If a collision occurs, calculate the new velocities of the two balls
                                    System.Diagnostics.Trace.WriteLine("Ball " + value + " hit ball " + i);

                                    Vector2[] newVelocity = collisionControler.ImpulseSpeed(otherBallX, otherBallY, otherBallSpeedX, otherBallSpeedY, otherBallMass);

                                    // Set the new velocities of the two balls
                                    dataAPI.setBallSpeed(value, newVelocity[0].X, newVelocity[0].Y);
                                    dataAPI.setBallSpeed(i, newVelocity[1].Y, newVelocity[1].Y);
                                }
                            }
                        }
                    }

                    // Check for collisions with boundaries
                    int boardSize = dataAPI.getBoardSize();

                    if (collisionControler.IsTouchingBoundariesX(boardSize))
                    {
                        // If the ball collides with the horizontal boundaries, reverse its horizontal velocity
                        dataAPI.setBallSpeed(value, -dataAPI.getBallSpeedX(value), dataAPI.getBallSpeedY(value));
                    }

                    if (collisionControler.IsTouchingBoundariesY(boardSize))
                    {
                        // If the ball collides with the vertical boundaries, reverse its vertical velocity
                        dataAPI.setBallSpeed(value, dataAPI.getBallSpeedX(value), -dataAPI.getBallSpeedY(value));
                    }

                    // Notify subscribers that a ball has changed
                    BallChanged?.Invoke(this, new BallChaneEventArgs() { ballId = value });
                }
                catch (SynchronizationLockException exception)
                {
                    // If an exception occurs while trying to enter the lock, throw a new exception with additional information
                    throw new Exception("Checking collision synchronization lock not working", exception);
                }
                finally
                {
                    // Release the lock to allow other threads to access shared resources
                    Monitor.Exit(_lock);
                }
            }

            public override void OnCompleted()
            {
                // Unsubscribe from the observable
                Unsubscribe();
            }

            public override void OnError(Exception error)
            {
                // Throw the received exception
                throw error;
            }


            public virtual void Unsubscribe()
            {
                // Dispose of the unsubscriber object.
                unsubscriber.Dispose();
            }

            #endregion

            #region observable

            public override IDisposable Subscribe(IObserver<int> observer)
            {
                // Subscribe the observer to the event observable.
                return eventObservable.Subscribe(x => observer.OnNext(x.EventArgs.ballId), ex => observer.OnError(ex), () => observer.OnCompleted());
            }
            #endregion

        }
    }
}