using System.Collections.Generic;
using System;

namespace Data
{
    public abstract class DataAbstractAPI : IObserver<IBall>, IObservable<IBall>
    {
        public abstract int getBoardSize();                           // Abstrakcyjna metoda zwracająca rozmiar planszy
        public abstract void createBalls(int ballsAmount);            // Abstrakcyjna metoda tworząca piłki
        public abstract int getBallsAmount();                         // Abstrakcyjna metoda zwracająca liczbę piłek

        public abstract void OnCompleted();                           // Abstrakcyjna metoda wywoływana po zakończeniu przesyłania danych
        public abstract void OnError(Exception error);                // Abstrakcyjna metoda wywoływana w przypadku błędu
        public abstract void OnNext(IBall Ball);                      // Abstrakcyjna metoda wywoływana po otrzymaniu kolejnej piłki

        public abstract IDisposable Subscribe(IObserver<IBall> observer);   // Abstrakcyjna metoda subskrybująca obserwatora

        public static DataAbstractAPI CreateDataApi()                 // Statyczna metoda tworząca instancję klasy DataApi
        {
            return new DataApi();
        }

        public class BallChaneEventArgs : EventArgs                   // Klasa reprezentująca argumenty zdarzenia zmiany piłki
        {
            public IBall newBall { get; set; }                        // Nowa piłka
        }

        private class DataApi : DataAbstractAPI                        // Klasa implementująca interfejs DataAbstractAPI
        {
            private BallRepository ballRepository;                    // Repozytorium piłek
            private IDisposable unsubscriber;                          // Obiekt subskrypcji
            private IList<IObserver<IBall>> observers;                 // Lista obserwatorów

            public DataApi()
            {
                this.ballRepository = new BallRepository();            // Inicjalizacja repozytorium piłek

                observers = new List<IObserver<IBall>>();              // Inicjalizacja listy obserwatorów
            }

            public override int getBoardSize()
            {
                return ballRepository.BoardSize;                      // Zwraca rozmiar planszy z repozytorium piłek
            }

            public override int getBallsAmount()
            {
                return ballRepository.balls.Count;                    // Zwraca liczbę piłek z repozytorium piłek
            }

            public override void createBalls(int ballsAmount)
            {
                ballRepository.CreateBalls(ballsAmount);

                foreach (var ball in ballRepository.balls)               // Dla każdej piłki w repozytorium
                {
                    Subscribe(ball);                                    // Subskrybuj obserwatora dla piłki
                    ball.StartMoving();                                 // Rozpocznij poruszanie piłki
                }

            }

            #region observer

            public virtual void Subscribe(IObservable<IBall> provider)
            {
                if (provider != null)
                    unsubscriber = provider.Subscribe(this);             // Subskrybuj dostawcę obserwowalnego, przypisując subskrypcję do obiektu unsubscriber
            }

            public override void OnCompleted()
            {
                Unsubscribe();                                          // Wywołuje metodę Unsubscribe() w momencie zakończenia przesyłania danych
            }

            public override void OnError(Exception error)
            {
                throw error;                                            // Rzuca wyjątek w przypadku błędu
            }

            public override void OnNext(IBall ball)
            {
                foreach (var observer in observers)                      // Dla każdego obserwatora w liście obserwatorów
                {
                    observer.OnNext(ball);                               // Wywołaj metodę OnNext() obserwatora, przekazując aktualną piłkę
                }

            }

            public virtual void Unsubscribe()
            {
                unsubscriber.Dispose();                                 // Zwalnia subskrypcję
            }

            #endregion

            #region provider

            public override IDisposable Subscribe(IObserver<IBall> observer)
            {
                if (!observers.Contains(observer))
                    observers.Add(observer);                             // Dodaje obserwatora do listy obserwatorów, jeśli nie jest już zawarty
                return new Unsubscriber(observers, observer);            // Zwraca obiekt Unsubscriber, reprezentujący subskrypcję
            }

            private class Unsubscriber : IDisposable
            {
                private IList<IObserver<IBall>> _observers;                // Lista obserwatorów
                private IObserver<IBall> _observer;                       // Obserwator

                public Unsubscriber
                (IList<IObserver<IBall>> observers, IObserver<IBall> observer)
                {
                    _observers = observers;                              // Inicjalizacja listy obserwatorów
                    _observer = observer;                                // Inicjalizacja obserwatora
                }

                public void Dispose()
                {
                    if (_observer != null && _observers.Contains(_observer))
                        _observers.Remove(_observer);                     // Usuwa obserwatora z listy obserwatorów
                }
            }

            #endregion
        }
    }
}