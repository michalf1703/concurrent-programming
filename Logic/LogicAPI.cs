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
    // Klasa abstrakcyjna LogicAPI, która implementuje interfejsy IObserver i IObservable.
    public abstract class LogicAPI : IObserver<int>, IObservable<int>
    {
        // Metody abstrakcyjne, które muszą zostać zaimplementowane w klasach pochodnych.
        public abstract void AddBallsAndStart(int BallsAmount);
        public abstract double getBallPositionX(int ballId);
        public abstract double getBallPositionY(int ballId);
        public abstract int getBallRadius(int ballId);

        // Metody abstrakcyjne z interfejsu IObserver.
        public abstract IDisposable Subscribe(IObserver<int> observer);
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(int value);

        // Metoda statyczna factory, która tworzy nową instancję BusinessLogic z instancją DataAbstractAPI.
        public static LogicAPI CreateLayer(DataAbstractAPI data = default(DataAbstractAPI))
        {
            return new BusinessLogic(data == null ? DataAbstractAPI.CreateDataApi() : data);
        }

        // Klasa reprezentująca argumenty zdarzenia BallChanged.
        public class BallChaneEventArgs : EventArgs
        {
            public int ballId { get; set; }
        }

        // Klasa BusinessLogic, która dziedziczy po klasie LogicAPI i implementuje interfejs IObservable.
        private class BusinessLogic : LogicAPI, IObservable<int>
        {
            private readonly DataAbstractAPI dataAPI;
            private IDisposable unsubscriber;
            static object _lock = new object();
            private IObservable<EventPattern<BallChaneEventArgs>> eventObservable = null;
            public event EventHandler<BallChaneEventArgs> BallChanged;

            public BusinessLogic(DataAbstractAPI dataAPI)
            {
                // Tworzenie obserwatora eventObservable.
                eventObservable = Observable.FromEventPattern<BallChaneEventArgs>(this, "BallChanged");
                this.dataAPI = dataAPI;
                Subscribe(dataAPI);
            }

            // Implementacja metod abstrakcyjnych z klasy LogicAPI.
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

            // Dodaje podaną liczbę kulek i rozpoczyna symulację.
            public override void AddBallsAndStart(int BallsAmount)
            {
                dataAPI.createBalls(BallsAmount);
            }

            #region observer

            // Subskrybuje do instancji IObservable.
            public virtual void Subscribe(IObservable<int> provider)
            {
                if (provider != null)
                    unsubscriber = provider.Subscribe(this);
            }

            // Informuje obserwatora o nowym indeksie kuli.
            public override void OnNext(int value)
            {
                // Wejście do blokady w celu zapobieżenia wyścigom przy dostępie do wspólnych zasobów.
                Monitor.Enter(_lock);
                try
                {
                    // Utwórz instancję klasy Collision, aby sprawdzić kolizje z innymi piłkami i granicami planszy
                    Collision collisionControler = new Collision(dataAPI.getBallPositionX(value), dataAPI.getBallPositionY(value), dataAPI.getBallSpeedX(value), dataAPI.getBallSpeedY(value), dataAPI.getBallRadius(value), 10);

                    // Sprawdź kolizje z innymi piłkami
                    for (int i = 1; i <= dataAPI.getBallsAmount(); i++)
                    {
                        if (value != i)
                        {
                            // Pobierz właściwości innej piłki
                            double otherBallX = dataAPI.getBallPositionX(i);
                            double otherBallY = dataAPI.getBallPositionY(i);
                            double otherBallSpeedX = dataAPI.getBallSpeedX(i);
                            double otherBallSpeedY = dataAPI.getBallSpeedY(i);
                            int otherBallRadius = dataAPI.getBallRadius(i);
                            double otherBallMass = dataAPI.getBallMass(i);

                            // Sprawdź, czy jest kolizja z inną piłką
                            if (collisionControler.IsCollision(otherBallX + otherBallSpeedX, otherBallY + otherBallSpeedY, otherBallRadius, true))
                            {
                                // Sprawdź, czy piłki już ze sobą kolidują, aby zapobiec wielokrotnym kolizjom
                                if (!collisionControler.IsCollision(otherBallX, otherBallY, otherBallRadius, false))
                                {
                                    // Jeśli wystąpi kolizja, oblicz nowe prędkości dwóch piłek
                                    System.Diagnostics.Trace.WriteLine("Piłka " + value + " uderzyła w piłkę " + i);

                                    Vector2[] newVelocity = collisionControler.ImpulseSpeed(otherBallX, otherBallY, otherBallSpeedX, otherBallSpeedY, otherBallMass);

                                    // Ustaw nowe prędkości dwóch piłek
                                    dataAPI.setBallSpeed(value, newVelocity[0].X, newVelocity[0].Y);
                                    dataAPI.setBallSpeed(i, newVelocity[1].Y, newVelocity[1].Y);
                                }
                            }
                        }
                    }

                    // Sprawdź kolizje z granicami planszy
                    int boardSize = dataAPI.getBoardSize();

                    if (collisionControler.IsTouchingBoundariesX(boardSize))
                    {
                        // Jeśli piłka zderza się z granicami poziomymi, odwróć jej prędkość poziomą
                        dataAPI.setBallSpeed(value, -dataAPI.getBallSpeedX(value), dataAPI.getBallSpeedY(value));
                    }

                    if (collisionControler.IsTouchingBoundariesY(boardSize))
                    {
                        // Jeśli piłka zderza się z granicami pionowymi, odwróć jej prędkość pionową
                        dataAPI.setBallSpeed(value, dataAPI.getBallSpeedX(value), -dataAPI.getBallSpeedY(value));
                    }
                    // Powiadom subskrybentów o zmianie piłki
                    BallChanged?.Invoke(this, new BallChaneEventArgs() { ballId = value });
                }
                catch (SynchronizationLockException exception)
                {
                    // Jeśli podczas próby wejścia do blokady wystąpi wyjątek, rzuć nowy wyjątek z dodatkowymi informacjami
                    throw new Exception("Nie działa blokada synchronizacji sprawdzania kolizji", exception);
                }
                finally
                {
                    // Zwolnij blokadę, aby inne wątki mogły uzyskać dostęp do współdzielonych zasobów
                    Monitor.Exit(_lock);
                }
            }

            public override void OnCompleted()
            {
                // Odsubskrybuj się od obserwowalnej
                Unsubscribe();
            }

            public override void OnError(Exception error)
            {
                // Rzuć otrzymanym wyjątkiem
                throw error;
            }

            public virtual void Unsubscribe()
            {
                // Usuń obiekt unsubscribera.
                unsubscriber.Dispose();
            }

            #endregion

            #region observable

            public override IDisposable Subscribe(IObserver<int> observer)
            {
                // Subskrybuj obserwatora do obserwowalnej zdarzeń.
                return eventObservable.Subscribe(x => observer.OnNext(x.EventArgs.ballId), ex => observer.OnError(ex), () => observer.OnCompleted());
            }
            #endregion


        }
    }
}