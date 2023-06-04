using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Model
{
    internal class BallInModel : IBall
    {
        public int Diameter { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        // Konstruktor klasy BallInModel
        public BallInModel(double top, double left, int radius)
        {
            Top = top;
            Left = left;
            Diameter = radius * 2;
        }

        private double top;

        // Właściwość Top z mechanizmem Property Change Notification
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

        // Właściwość Left z mechanizmem Property Change Notification
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

 


        // Metoda RaisePropertyChanged do wywołania zdarzenia Property Change Notification
        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Metoda Move do aktualizacji pozycji piłki na podstawie wektora pozycji
        public void Move(Vector2 position)
        {
            Left = position.X;
            Top = position.Y;
        }

    }
}
