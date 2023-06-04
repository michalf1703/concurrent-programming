using Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;

namespace Model
{
    public interface IBall : INotifyPropertyChanged
    {
        double Top { get; }
        double Left { get; }
        int Diameter { get; }
    }

    public class BallChaneEventArgs : EventArgs
    {
        public IBall Ball { get; set; }
    }

    public abstract class ModelAPI : IObservable<IBall>
    {
        // Metoda statyczna CreateApi do tworzenia instancji ModelAPI
        public static ModelAPI CreateApi()
        {
            return new ModelBall();
        }

        // Metoda abstrakcyjna AddBallsAndStart do dodawania piłek i rozpoczęcia symulacji
        public abstract void AddBallsAndStart(int ballsAmount);

        #region IObservable

        // Metoda abstrakcyjna Subscribe do subskrybowania obserwatora
        public abstract IDisposable Subscribe(IObserver<IBall> observer);

        #endregion IObservable

        // Wewnętrzna klasa ModelBall dziedzicząca po ModelAPI
        internal class ModelBall : ModelAPI
        {
            private LogicAPI logicApi;
            public event EventHandler<BallChaneEventArgs> BallChanged;

            private IObservable<EventPattern<BallChaneEventArgs>> eventObservable = null;
            private List<BallInModel> Balls = new List<BallInModel>();

            // Konstruktor klasy ModelBall
            public ModelBall()
            {
                // Tworzenie instancji LogicAPI
                logicApi = logicApi ?? LogicAPI.CreateLayer();

                // Subskrypcja logiki do aktualizacji pozycji piłek w modelu
                IDisposable observer = logicApi.Subscribe(x => Balls[x.Id - 1].Move(x.Position));

                // Tworzenie obserwowalnego zdarzenia BallChanged
                eventObservable = Observable.FromEventPattern<BallChaneEventArgs>(this, "BallChanged");
            }

            // Metoda AddBallsAndStart do dodawania piłek i rozpoczęcia symulacji
            public override void AddBallsAndStart(int ballsAmount)
            {
                for (int i = 1; i <= ballsAmount; i++)
                {
                    // Tworzenie nowej piłki w modelu
                    BallInModel newBall = new BallInModel(0, 0, 10);
                    Balls.Add(newBall);

                    // Wywołanie zdarzenia BallChanged i przekazanie nowej piłki jako argument
                    BallChanged?.Invoke(this, new BallChaneEventArgs() { Ball = newBall });
                }

                // Dodanie piłek do logiki i rozpoczęcie symulacji
                logicApi.AddBallsAndStart(ballsAmount);
            }

            // Metoda Subscribe do subskrybowania obserwatora
            public override IDisposable Subscribe(IObserver<IBall> observer)
            {
                return eventObservable.Subscribe(x => observer.OnNext(x.EventArgs.Ball), ex => observer.OnError(ex), () => observer.OnCompleted());
            }
        }
    }
}
