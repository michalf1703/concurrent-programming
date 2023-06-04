using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Threading;
namespace Data
{
    internal class Ball : IBall
    {
        public override int Id { get; }
        public override int Radius { get; } = 10;
        public override double Mass { get; } = 10;
        private bool isRunning = true;
        private Vector2 _position;
        public override Vector2 Position
        {
            get => _position;
        }

        internal int board_size { get;} = 515;
        public override Vector2 Speed { get; set; }
        private static object _lock = new object();         // Tworzymy statyczny obiekt lock, który będzie współdzielony przez wszystkie instancje tej klasy
        internal readonly IList<IObserver<IBall>> observers;
        Stopwatch stopwatch;
        private Task BallThread;
        internal DAO dao { get; set; }
        internal Ball(int id)
        {
            this.Id = id;
            stopwatch = new Stopwatch();
            observers = new List<IObserver<IBall>>();

            createAndStart();  // Tworzymy i uruchamiamy piłkę
        }

        public void StartMoving()
        {
            
            this.BallThread = new Task(MoveBall);  // Tworzymy nowe zadanie do poruszania piłką
            BallThread.Start();  // Uruchamiamy zadanie
        }

        private async void MoveBall()
        {
            while (isRunning)
            {
                long time = stopwatch.ElapsedMilliseconds;

                // Zresetowanie i rozpoczęcie odliczania stopera
                stopwatch.Restart();
                stopwatch.Start();

                // Zmiana pozycji piłki na podstawie upływu czasu
                ChangeBallPosition(time);

                // Dodanie informacji o piłce do bufora
                dao.addToBuffer(this);

                // Pobranie prędkości piłki
                Vector2 _speed = Speed;

                // Obliczenie czasu oczekiwania w oparciu o prędkość piłki
                int sleepTime = (int)(1 / Math.Sqrt(Math.Pow(_speed.X, 2) + Math.Pow(_speed.Y, 2)));

                // Oczekiwanie asynchroniczne na określony czas
                await Task.Delay(sleepTime);

                // Zatrzymanie stopera
                stopwatch.Stop();
            }
        }

        private void ChangeBallPosition(long time)
        {
       
            Vector2 Move = default;
            Monitor.Enter(_lock);  // Zajmujemy blokadę, aby zapewnić wyłączny dostęp do współdzielonych zasobów, jeśli monitor jest zajęty to blokuje wątek
            try
            {
                // Obliczamy nową pozycję piłki na podstawie jej prędkości i upływu czasu

                    Move += Speed * time;
                    _position += Move;

            }
            catch (SynchronizationLockException exception)
            {
                throw new Exception("Synchronization lock not working", exception);
            }
            finally
            {
                Monitor.Exit(_lock);  // Zwolniamy blokadę
            }


            foreach (var observer in observers.ToList())
            {
                if (observer != null)
                {
                    observer.OnNext(this);
                }
            }

        }

        #region provider

        // Wewnętrzna klasa do zarządzania subskrypcjami
        private class Unsubscriber : IDisposable
        {
            private IList<IObserver<IBall>> _observers;
            private IObserver<IBall> _observer;

            public Unsubscriber(IList<IObserver<IBall>> observers, IObserver<IBall> observer)
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

        public override IDisposable Subscribe(IObserver<IBall> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private void createAndStart()
        {
            Random random = new Random();
            this._position = new Vector2(random.Next(1, 500), random.Next(1, 500));
            this.Speed = new Vector2(
                (float)(random.NextDouble() * (0.2 - 0) + 0),
                (float)(random.NextDouble() * (0.2 - 0) + 0)
            );
        }
        public override object getLock()
        {
            return _lock; 
        }

        #endregion
    }
}