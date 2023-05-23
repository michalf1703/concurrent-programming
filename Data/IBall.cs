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
        public abstract Vector2 Position { get; set; }
        public abstract Vector2 Speed { get; set; }
        public abstract Vector2 Move { get; set; }
        public abstract int Radius { get; }
        public abstract double Mass { get; }
        //public 
        public abstract IDisposable Subscribe(IObserver<IBall> observer);

    }

}
