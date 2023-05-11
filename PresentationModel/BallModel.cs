using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Data;
using Logic;

namespace Model
{
    // Class implementing the IBall interface

    public class BallInModel : IBall
    {
        // Properties
        public int Diameter { get; } // Read-only property for the ball's diameter
        public event PropertyChangedEventHandler PropertyChanged; // Event that is raised when a property value changes

        // Constructor
        public BallInModel(double top, double left, int radius)
        {
            // Initialize the object with the given top, left, and radius values
            Top = top;
            Left = left;
            Diameter = radius * 2;
        }

        // Fields
        private double top; // The top position of the ball
        private double left; // The left position of the ball

        // Properties with change notification
        public double Top
        {
            get { return top; }
            set
            {
                // Check if the new value is the same as the current value
                if (top == value)
                    return;

                // If not, update the value and raise the PropertyChanged event
                top = value;
                RaisePropertyChanged();
            }
        }

        public double Left
        {
            get { return left; }
            set
            {
                // Check if the new value is the same as the current value
                if (left == value)
                    return;

                // If not, update the value and raise the PropertyChanged event
                left = value;
                RaisePropertyChanged();
            }
        }

        // Method for moving the ball to a new position
        public void Move(double poitionX, double positionY)
        {
            // Update the ball's position
            Left = poitionX;
            Top = positionY;
        }

        // Method for raising the PropertyChanged event
        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
