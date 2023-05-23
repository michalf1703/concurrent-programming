
using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Model
{
    public class BallInModel : IBall
    {
        public int Diameter { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public BallInModel(double top, double left, int radius)
        {
            Top = top;
            Left = left;
            Diameter = radius * 2;
        }

        private double top;

        public double Top
        {
            get { return top; }
            set
            {
                if (top == value)
                    return;
                top = value;
                RaisePropertyChanged();
            }
        }

        private double left;

        public double Left
        {
            get { return left; }
            set
            {
                if (left == value)
                    return;
                left = value;
                RaisePropertyChanged();
            }
        }
        public void Move1(Vector2 position)
        {
            // Zaktualizuj pozycję piłki
            Left = position.X;
            Top = position.Y;
        }

        public void Move(double poitionX, double positionY)
        {
            Left = poitionX;
            Top = positionY;
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal object Move(Vector2 position)
        {
            throw new NotImplementedException();
        }
    }
}