using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public class Ball : IObservable<int>
    {
        // Unikalny identyfikator kuli.
        public int Id { get; }

        // Pozycja kuli na osiach X i Y.
        public double PositionX { get; private set; }
        public double PositionY { get; private set; }

        // Promień i masa kuli.
        public int Radius { get; } = 15;
        public double Mass { get; } = 10;

        // Ilość o jaką kula się porusza na osiach X i Y.
        public double MoveX { get; set; }
        public double MoveY { get; set; }

        // Kolekcja obserwatorów, którzy zasubskrybowali ruchy tej kuli.
        internal readonly IList<IObserver<int>> observers;

        // Task, który uruchamia logikę ruchu kuli.
        private Task BallThread;

        // Konstruktor dla kuli.
        public Ball(int id)
        {
            this.Id = id;

            // Ustaw losową pozycję startową i kierunek ruchu.
            Random random = new Random();
            this.PositionX = Convert.ToDouble(random.Next(1, 500));
            this.PositionY = Convert.ToDouble(random.Next(1, 500));
            this.MoveX = random.NextDouble() * (3 - 2) + 2;
            this.MoveY = random.NextDouble() * (3 - 2) + 2;

            // Zainicjuj kolekcję obserwatorów.
            observers = new List<IObserver<int>>();
        }

        // Rozpocznij logikę ruchu kuli na osobnym wątku.
        public void StartMoving()
        {
            this.BallThread = new Task(MoveBall);
            BallThread.Start();
        }

        // Logika przesuwania kuli.
        public void MoveBall()
        {
            while (true)
            {
                // Zaktualizuj pozycję kuli i powiadom wszystkich obserwatorów.
                ChangeBallPosition();
                foreach (var observer in observers.ToList())
                {
                    if (observer != null)
                    {
                        observer.OnNext(Id);
                    }
                }

                // Poczekaj na krótką chwilę, aby spowolnić ruch kuli.
                System.Threading.Thread.Sleep(1);
            }
        }

        // Zaktualizuj pozycję kuli na podstawie jej aktualnego kierunku ruchu.
        public void ChangeBallPosition()
        {
            PositionX += MoveX;
            PositionY += MoveY;
        }

        #region provider

        // Zasubskrybuj obserwatora na ruchy tej kuli.
        public IDisposable Subscribe(IObserver<int> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        // Klasa, która anuluje subskrypcję obserwatora na ruchy tej piłki.
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
