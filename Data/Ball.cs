using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Data
{
    internal class Ball : IBall
    {
        public override int Id { get; }
        private Vector2 _position;
        public override Vector2 Position
        {
            get => _position;
        }

        public override int Radius { get; } = 15;
        public override double Mass { get; } = 10;

        public bool isRunning = true;

        public int counter { get; set; } = 1;

        public override Vector2 Speed { get; set; }
        public override Vector2 Move { get; set; }

        internal readonly IList<IObserver<IBall>> observers;
        Stopwatch stopwatch;
        private Task BallThread;


       public Ball(int id)
        {
            this.Id = id;
            Random random = new Random();
            stopwatch = new Stopwatch();
            observers = new List<IObserver<IBall>>();
            this._position = new Vector2(random.Next(1, 500), random.Next(1, 500));
            this.Speed = new Vector2(
                (float)(random.NextDouble() * (0.2 - 0) + 0),
                (float)(random.NextDouble() * (0.2 - 0) + 0)
            );

        }


        public void StartMoving()
        {
            this.BallThread = new Task(MoveBall);
            BallThread.Start();
        }

        public void MoveBall()
        {
            while (isRunning)
            {
                long time = stopwatch.ElapsedMilliseconds;
                counter++;
                stopwatch.Restart();
                stopwatch.Start();

                ChangeBallPosition(time);
                if (counter % 100 == 0)
                {
                    counter = 1;
                }

                foreach (var observer in observers.ToList())
                {
                    if (observer != null)
                    {
                        observer.OnNext(this);
                    }
                }
                stopwatch.Stop();
            }
        }


        public void ChangeBallPosition(long time)
        {

            if (time > 0)
            {
                Move += Speed / 12 * time;
            }
            else
            {
                Move = Speed / 12;

            }

            _position += Move;
        }


        #region provider

        public override IDisposable Subscribe(IObserver<IBall> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private IList<IObserver<IBall>> _observers;
            private IObserver<IBall> _observer;

            public Unsubscriber
            (IList<IObserver<IBall>> observers, IObserver<IBall> observer)
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
