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
        public override double PositionX { get; set; }
        public override double PositionY { get; set; }
        public override double SpeedX { get; set; }
        public override double SpeedY { get; set; }

        public override double MoveX { get; set; }
        public override double MoveY { get; set; }

        public override int Radius { get; } = 15;
        public override double Mass { get; } = 10;

        public bool isRunning = true;

        public int counter { get; set; } = 1;


        internal readonly IList<IObserver<IBall>> observers;
        Stopwatch stopwatch;
        private Task BallThread;


       public Ball(int id)
        {
            this.Id = id;

            Random random = new Random();
            stopwatch = new Stopwatch();
            observers = new List<IObserver<IBall>>();

            this.PositionX = Convert.ToDouble(random.Next(1, 500));
            this.PositionY = Convert.ToDouble(random.Next(1, 500));

            this.SpeedX = random.NextDouble() * (0.2 - 0) + 0;
            this.SpeedY = random.NextDouble() * (0.2 - 0) + 0;
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
                MoveX = SpeedX / 5 * time;
                MoveY = SpeedY / 5 * time;
            }
            else
            {
                MoveX = SpeedX / 5;
                MoveY = SpeedY / 5;
            }

            PositionX += MoveX;
            PositionY += MoveY;
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
