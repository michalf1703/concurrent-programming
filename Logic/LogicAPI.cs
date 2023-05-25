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
    public abstract class LogicAPI : IObserver<IBall>, IObservable<IBall>
    {
        // Dodaj komentarze w języku polskim do przestrzeni nazw i używanych bibliotek
        // ...

        public abstract void AddBallsAndStart(int BallsAmount);
        public abstract IDisposable Subscribe(IObserver<IBall> observer);
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(IBall ball);

        // Metoda fabrykująca do tworzenia instancji klasy LogicAPI
        public static LogicAPI CreateLayer(DataAbstractAPI data = default(DataAbstractAPI))
        {
            return new BusinessLogic(data == null ? DataAbstractAPI.CreateDataApi() : data);
        }

        // Klasa BallChaneEventArgs dziedzicząca po EventArgs
        public class BallChaneEventArgs : EventArgs
        {
            public IBall ball { get; set; }
        }

        // Klasa BusinessLogic dziedzicząca po LogicAPI i implementująca IObservable<IBall>
        private class BusinessLogic : LogicAPI, IObservable<IBall>
        {
            private readonly DataAbstractAPI dataAPI;
            private IDisposable unsubscriber;
            static object _lock = new object();
            private IObservable<EventPattern<BallChaneEventArgs>> eventObservable = null;
            public event EventHandler<BallChaneEventArgs> BallChanged;
            Dictionary<int, IBall> ballTree;
            Barrier barrier;

            // Konstruktor klasy BusinessLogic
            public BusinessLogic(DataAbstractAPI dataAPI)
            {
                eventObservable = Observable.FromEventPattern<BallChaneEventArgs>(this, "BallChanged");
                this.dataAPI = dataAPI;
                Subscribe(dataAPI);
                ballTree = new Dictionary<int, IBall>();
            }

            // Metoda do dodawania piłek i rozpoczęcia symulacji
            public override void AddBallsAndStart(int BallsAmount)
            {
                dataAPI.createBalls(BallsAmount);
                barrier = new Barrier(BallsAmount);
            }

            #region observer

            // Subskrybuj jako obserwator dostarczyciela
            public virtual void Subscribe(IObservable<IBall> provider)
            {
                if (provider != null)
                    unsubscriber = provider.Subscribe(this);
            }

            // Obsługa zdarzenia OnNext
            public override void OnNext(IBall ball)
            {
                Monitor.Enter(_lock);   // Wchodzimy w sekcję monitorowaną za pomocą obiektu _lock
                try
                {
                    if (!ballTree.ContainsKey(ball.Id))    // Jeśli piłka o danym Id nie istnieje w ballTree, dodaj ją
                    {
                        ballTree.Add(ball.Id, ball);
                    }

                    // Sprawdzamy kolizje piłki z innymi piłkami w ballTree
                    foreach (var item in ballTree)
                    {
                        if (item.Key != ball.Id)    // Jeśli klucz item.Key nie odpowiada Id badanej piłki (ball), sprawdzamy kolizję
                        {
                            if (Collision.IsCollision(ball, item.Value))    // Jeśli wystąpiła kolizja pomiędzy piłkami, stosujemy impuls prędkości
                            {
                                Collision.ImpulseSpeed(ball, item.Value);
                            }
                        }
                    }

                    Collision.IsTouchingBoundaries(ball, dataAPI.getBoardSize());    // Sprawdzamy, czy piłka dotyka granic planszy na podstawie rozmiaru pobranego z dataAPI

                    BallChanged?.Invoke(this, new BallChaneEventArgs() { ball = ball });    // Wywołujemy zdarzenie BallChanged i przekazujemy aktualną piłkę jako argument
                }
                catch (SynchronizationLockException exception)
                {
                    throw new Exception("Checking collision synchronization lock not working", exception);
                }
                finally
                {
                    Monitor.Exit(_lock);    // Opuszczamy sekcję monitorowaną
                }
            }

            // Obsługa zdarzenia OnCompleted
            public override void OnCompleted()
            {
                Unsubscribe();
            }

            // Obsługa zdarzenia OnError
            public override void OnError(Exception error)
            {
                throw error;
            }

            // Anuluj subskrypcję
            public virtual void Unsubscribe()
            {
                unsubscriber.Dispose();
            }

            #endregion

            #region observable

            // Subskrybuj jako obserwator zdarzeń
            public override IDisposable Subscribe(IObserver<IBall> observer)
            {
                return eventObservable.Subscribe(x => observer.OnNext(x.EventArgs.ball), ex => observer.OnError(ex), () => observer.OnCompleted());
            }

            #endregion

        }
    }
}
