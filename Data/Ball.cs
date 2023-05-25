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
        public override int Radius { get; } = 15;
        public override double Mass { get; } = 10;

        private bool isRunning = true;
        private Vector2 _position;
        public override Vector2 Position
        {
            get => _position;
        }
        public override Vector2 Speed { get; set; }

        private Vector2 Move { get; set; }

        private static object _lock = new object();         // Tworzymy statyczny obiekt lock, który będzie współdzielony przez wszystkie instancje tej klasy

        internal readonly IList<IObserver<IBall>> observers;
        Stopwatch stopwatch;
        private Task BallThread;


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
                Monitor.Enter(_lock);  // Zajmujemy blokadę, aby zapewnić wyłączny dostęp do współdzielonych zasobów, jeśli monitor jest zajęty to blokuje wątek
                try
                {
                    long time = stopwatch.ElapsedMilliseconds; //czy czas powinien znajdować się w sekcji krytycznej?
                    stopwatch.Restart();
                    stopwatch.Start();
                    ChangeBallPosition(time);
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
                stopwatch.Stop(); //skoro to pętla nieskończona to stoper chyba nigdy sie nie zatrzyma? czy .Stop() jest potrzebne?
            }
        }


        private void ChangeBallPosition(long time)
        {
            // Obliczamy nową pozycję piłki na podstawie jej prędkości i upływu czasu
            if (time > 0)
            {
                Move += Speed * time;
            }
            else
            {
                Move = Speed;
            }

            _position += Move;
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

        #endregion
    }
}
