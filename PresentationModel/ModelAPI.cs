using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;
using Logic;

namespace Model
{
    public interface IBall : INotifyPropertyChanged
    {
        double Top { get; } // The top position of the ball
        double Left { get; } // The left position of the ball
        int Diameter { get; } // The diameter of the ball
    }

    public class BallChaneEventArgs : EventArgs  // Custom event arguments for the BallChanged event
    {
        public IBall Ball { get; set; } // The ball that triggered the event
    }

    public abstract class ModelAPI : IObservable<IBall> // Base API class that allows for observing balls
    {
        public static ModelAPI CreateApi() // Factory method for creating instances of ModelAPI
        {
            return new ModelBall();
        }

        public abstract void AddBallsAndStart(int ballsAmount); // Method for adding balls to the model and starting their movement

        #region IObservable

        public abstract IDisposable Subscribe(IObserver<IBall> observer); // Method for subscribing to ball movement events

        #endregion IObservable

        internal class ModelBall : ModelAPI  // Implementation of the ModelAPI class for the ball model
        {
            private LogicAPI logicApi;      // Instance of the logic API for communicating with the model's logic layer
            public event EventHandler<BallChaneEventArgs> BallChanged;  // The observable that is used to notify observers of ball movement events

            private IObservable<EventPattern<BallChaneEventArgs>> eventObservable = null; // The observable that is used to notify observers of ball movement events
            private List<BallInModel> Balls = new List<BallInModel>();  // List of balls currently in the model

            public ModelBall()
            {
                logicApi = logicApi ?? LogicAPI.CreateLayer(); // Create the logic API if it hasn't been created yet
                // Subscribe to the logic API to receive updates about ball movement
                IDisposable observer = logicApi.Subscribe<int>(x => Balls[x - 1].Move(logicApi.getBallPositionX(x), logicApi.getBallPositionY(x)));
                eventObservable = Observable.FromEventPattern<BallChaneEventArgs>(this, "BallChanged"); // Create the observable that will be used to notify observers of ball movement events
            }

            public override void AddBallsAndStart(int ballsAmount) // Add the specified number of balls to the model and start their movement
            {
                logicApi.AddBallsAndStart(ballsAmount);
                for (int i = 1; i <= ballsAmount; i++)
                {
                    // Create a new BallInModel instance for each ball in the model
                    BallInModel newBall = new BallInModel(logicApi.getBallPositionX(i), logicApi.getBallPositionY(i), logicApi.getBallRadius(i)); 
                    Balls.Add(newBall);  // Add the new ball to the list of balls
                }

                foreach (BallInModel ball in Balls)
                {
                    BallChanged?.Invoke(this, new BallChaneEventArgs() { Ball = ball }); // Raise the BallChanged event for each ball in the model
                }

            }

            public override IDisposable Subscribe(IObserver<IBall> observer)
            {
                // Subscribe the observer to the eventObservable and return the IDisposable object that can be used to unsubscribe later
                return eventObservable.Subscribe(x => observer.OnNext(x.EventArgs.Ball), ex => observer.OnError(ex), () => observer.OnCompleted());
            }
        }
    }
}