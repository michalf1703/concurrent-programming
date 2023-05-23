using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Data

{
    public abstract class IBall : IObservable<IBall>
    {
        public abstract int Id { get; }
        public abstract double PositionX { get; set; }
        public abstract double PositionY { get; set; }
        public abstract double SpeedX { get; set; }
        public abstract double SpeedY { get; set; }

        public abstract double MoveX { get; set; }
        public abstract double MoveY { get; set; }
        public abstract int Radius { get; }
        public abstract double Mass { get; }
        //public 
        public abstract IDisposable Subscribe(IObserver<IBall> observer);

    }

}
