using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Data;
using Logic;

namespace Model
{
    // Klasa implementująca interfejs IBall

    public class BallInModel : IBall
    {
        // Właściwości
        public int Diameter { get; } // Właściwość tylko do odczytu dla średnicy piłki
        public event PropertyChangedEventHandler PropertyChanged; // Zdarzenie wywoływane, gdy zmieni się wartość właściwości

        // Konstruktor
        public BallInModel(double top, double left, int radius)
        {
            // Inicjalizacja obiektu z wartościami top, left i radius
            Top = top;
            Left = left;
            Diameter = radius * 2;
        }

        // Pola
        private double top; // Pozycja top piłki
        private double left; // Pozycja left piłki

        // Właściwości z powiadomieniem o zmianie
        public double Top
        {
            get { return top; }
            set
            {
                // Sprawdzenie, czy nowa wartość jest taka sama jak bieżąca wartość
                if (top == value)
                    return;

                // Jeśli nie, zaktualizuj wartość i wywołaj zdarzenie PropertyChanged
                top = value;
                RaisePropertyChanged();
            }
        }

        public double Left
        {
            get { return left; }
            set
            {
                // Sprawdzenie, czy nowa wartość jest taka sama jak bieżąca wartość
                if (left == value)
                    return;

                // Jeśli nie, zaktualizuj wartość i wywołaj zdarzenie PropertyChanged
                left = value;
                RaisePropertyChanged();
            }
        }

        // Metoda służąca do przenoszenia piłki do nowej pozycji
        public void Move(double poitionX, double positionY)
        {
            // Zaktualizuj pozycję piłki
            Left = poitionX;
            Top = positionY;
        }

        // Metoda wywołująca zdarzenie PropertyChanged
        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
