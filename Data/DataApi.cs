using System;
using System.Collections.Generic;
using System.Threading;

namespace Data
{
    public abstract class DataAbstractAPI : IObserver<int>, IObservable<int>
    {
        // Deklaracja zbioru metod abstrakcyjnych, które muszą być zaimplementowane przez klasy pochodne
        public abstract double getBallPositionX(int ballId); // zwraca pozycję X kuli o podanym id
        public abstract double getBallPositionY(int ballId); // zwraca pozycję Y kuli o podanym id
        public abstract int getBallRadius(int ballId); // zwraca promień kuli o podanym id
        public abstract double getBallSpeedX(int ballId); // zwraca prędkość X kuli o podanym id
        public abstract double getBallSpeedY(int ballId); // zwraca prędkość Y kuli o podanym id
        public abstract double getBallMass(int ballId); // zwraca masę kuli o podanym id
        public abstract int getBoardSize(); // zwraca rozmiar planszy
        public abstract void setBallSpeed(int ballId, double speedX, double speedY); // ustawia prędkość X i Y kuli o podanym id
        public abstract void createBalls(int ballsAmount); // tworzy określoną liczbę kul
        public abstract int getBallsAmount(); // zwraca liczbę kul na planszy

        // Deklaracja metod wymaganych przez interfejs IObserver, które muszą być zaimplementowane przez klasy pochodne
        public abstract void OnCompleted(); // sygnalizuje, że obserwator zakończył otrzymywanie wartości
        public abstract void OnError(Exception error); // obsługuje błędy związane z obserwowanym źródłem
        public abstract void OnNext(int value); // otrzymuje wartość od obserwowanego źródła

        // Deklaracja metody wymaganej przez interfejs IObservable, która musi być zaimplementowana przez klasy pochodne
        public abstract IDisposable Subscribe(IObserver<int> observer); // subskrybuje obserwatora do obserwowanego źródła

        // Zdefiniowanie statycznej metody fabrycznej, która tworzy instancję klasy DataApi
        public static DataAbstractAPI CreateDataApi()
        {
            return new DataApi();
        }

        // Zdefiniowanie zagnieżdżonej klasy DataApi dziedziczącej po klasie DataAbstractAPI
        private class DataApi : DataAbstractAPI
        {
            // Deklaracja prywatnych pól
            private BallRepository ballRepository; // repozytorium kul
            private IDisposable unsubscriber; // obiekt umożliwiający wyrejestrowanie z obserwowanego źródła
            static object _lock = new object(); // obiekt umożliwiający synchronizację wątków
            private IList<IObserver<int>> observers; // lista obserwatorów
            private Barrier barrier; // bariera synchronizująca


            // Definicja konstruktora, który inicjalizuje pola ballRepository i observers
            public DataApi()
            {
                // Tworzymy nowy obiekt BallRepository i przypisujemy go do pola ballRepository
                this.ballRepository = new BallRepository();
                // Tworzymy nową listę obserwatorów i przypisujemy ją do pola observers
                observers = new List<IObserver<int>>();

            }
            // Implementacja metod abstrakcyjnych zadeklarowanych w klasie DataAbstractAPI
            public override double getBallPositionX(int ballId)
            {
                // Zwracamy pozycję X piłki o podanym ID za pomocą metody GetBall z ballRepository
                return this.ballRepository.GetBall(ballId).PositionX;
            }

            public override double getBallPositionY(int ballId)
            {
                // Zwracamy pozycję Y piłki o podanym ID za pomocą metody GetBall z ballRepository
                return this.ballRepository.GetBall(ballId).PositionY;
            }

            public override int getBoardSize()
            {
                // Zwracamy rozmiar planszy (BoardSize) z ballRepository
                return ballRepository.BoardSize;
            }

            public override double getBallMass(int ballId)
            {
                // Zwracamy masę piłki o podanym ID za pomocą metody GetBall z ballRepository
                return this.ballRepository.GetBall(ballId).Mass;
            }

            public override int getBallRadius(int ballId)
            {
                // Zwracamy promień piłki o podanym ID za pomocą metody GetBall z ballRepository
                return this.ballRepository.GetBall(ballId).Radius;
            }

            public override double getBallSpeedX(int ballId)
            {
                // Zwracamy prędkość w osi X piłki o podanym ID za pomocą metody GetBall z ballRepository
                return this.ballRepository.GetBall(ballId).MoveX;
            }

            public override double getBallSpeedY(int ballId)
            {
                // Zwracamy prędkość w osi Y piłki o podanym ID za pomocą metody GetBall z ballRepository
                return this.ballRepository.GetBall(ballId).MoveY;
            }

            public override void setBallSpeed(int ballId, double speedX, double speedY)
            {
                // Ustawiamy prędkość w osi X i Y piłki o podanym ID za pomocą metody GetBall z ballRepository
                this.ballRepository.GetBall(ballId).MoveX = speedX;
                this.ballRepository.GetBall(ballId).MoveY = speedY;
            }

            public override int getBallsAmount()
            {
                // Zwracamy ilość piłek na planszy (liczbę elementów w liście balls z ballRepository)
                return ballRepository.balls.Count;
            }

            public override void createBalls(int ballsAmount)
            {
                // Tworzymy nową barierę z określoną liczbą piłek
                barrier = new Barrier(ballsAmount);
                // Tworzymy określoną liczbę piłek i dodajemy je do ballRepository
                ballRepository.CreateBalls(ballsAmount);

                // Subskrybujemy każdą piłkę i uruchamiamy ją
                foreach (var ball in ballRepository.balls)
                {
                    Subscribe(ball);
                    ball.StartMoving();
                }


            }

            #region observer

            // Subskrybuj do dostawcy obserwowalnego strumienia.
            public virtual void Subscribe(IObservable<int> provider)
            {
                if (provider != null)
                    unsubscriber = provider.Subscribe(this);
            }

            // Zrezygnuj z subskrypcji do dostawcy obserwowalnego strumienia.
            public override void OnCompleted()
            {
                Unsubscribe();
            }

            // Obsłuż błąd pochodzący od dostawcy obserwowalnego strumienia.
            public override void OnError(Exception error)
            {
                throw error;
            }

            // Otrzymaj wartość od dostawcy obserwowalnego strumienia.
            public override void OnNext(int value)
            {
                // Zasygnalizuj, że wątek osiągnął barierę i czekaj na osiągnięcie jej przez wszystkie pozostałe wątki.
                barrier.SignalAndWait();

                // Przekaż wartość do wszystkich zarejestrowanych obserwatorów.
                foreach (var observer in observers)
                {
                    observer.OnNext(value);
                }

            }

            // Zrezygnuj z subskrypcji do dostawcy obserwowalnego strumienia.
            public virtual void Unsubscribe()
            {
                unsubscriber.Dispose();
            }

            #endregion

            #region Dostawca

            // Subskrybuj do obserwatora.
            public override IDisposable Subscribe(IObserver<int> observer)
            {
                if (!observers.Contains(observer))
                    observers.Add(observer);

                // Zwróć obiekt, który można użyć do zrezygnowania z subskrypcji do obserwatora.
                return new Unsubscriber(observers, observer);
            }

            private class Unsubscriber : IDisposable
            {
                private IList<IObserver<int>> _observers;
                private IObserver<int> _observer;

                // Obiekt, który można użyć do zrezygnowania z subskrypcji do obserwatora.
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
}