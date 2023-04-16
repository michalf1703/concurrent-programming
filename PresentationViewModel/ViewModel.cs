using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using Prism.Mvvm;
using Model;
using System.Collections.ObjectModel;

namespace ViewModel
{
    //The ViewModel includes an implementation of the INotifyPropertyChanged interface,
    //which is used to notify the view of changes to the model
    public class ViewModel : INotifyPropertyChanged
    {
        //reference to the ModelAbstractApi 
        private ModelAbstractApi modelApi;
        //define the BallModel collection, which holds the balls models, and properties and methods for handling user interaction.
        public ObservableCollection<BallModel> balls { get; set; }
       //the command that handles clicking the START button
        public ICommand StartButtonClicked { get; set; }
        //the value entered by the user in the text box
        private string text;
        private Task task;
        //a boolean value that indicates whether the program is in active mode
        private bool active;
        //a boolean value that indicates whether the program is in inactive mode 
        private bool notActive = false;
        //information about the error of the entered data 
        private string errorMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel() : this(ModelAbstractApi.CreateApi())
        {

        }

        public ViewModel(ModelAbstractApi baseModel)
        {
            Active = true;
            this.modelApi = baseModel;
            StartButtonClicked = new UserCommand(() => StartButtonClickHandler());
            balls = new ObservableCollection<BallModel>();
        }

        //StartButtonClickHandler() - a method that handles clicking the Start button. Retrieves the value entered
        //by the user in the text box, calls the method
        private void StartButtonClickHandler()
        {
           // GenerateBalls() from ModelAbstractApi and runs the ChangePosition() method on a new thread(Task).
            modelApi.GenerateBalls(readFromBox());
            task = new Task(ChangePosition);
            task.Start();
        }

        //ChangePosition() is an asynchronous method that updates the positions of the balls in the balls
        //collection in an infinite loop.
        public void ChangePosition()
        {
            while (true)
            {
                ObservableCollection<BallModel> ballsList = new ObservableCollection<BallModel>();

                foreach (BallModel ball in modelApi.balls)
                {
                    ballsList.Add(ball);
                }
                balls = ballsList;
                RaisePropertyChanged(nameof(balls));
                Thread.Sleep(10);
            }
        }

        // RaisePropertyChanged() notifies the view of changes to the balls collection.
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //readFromBox() - a method that converts the value entered by the user in the text field to an integer.
        //The method returns that number or 0 if the value entered is invalid.
        //The method also sets the Active and NotActive properties to true or false, depending on the correctness of the input.
        public int readFromBox()
        {
            int result;
            if (Int32.TryParse(TextMethod, out result) && TextMethod != "0")
            {
                result = Int32.Parse(TextMethod);
                ErrorMessage = " ";
                Active = !Active;
                NotActive = !NotActive;
                return result;
            }
            ErrorMessage = "Zła liczba!";
            return 0;
        }

        public bool NotActive
        {
            get
            {
                return notActive;
            }
            set
            {
                notActive = value;
                RaisePropertyChanged(nameof(NotActive));
            }
        }

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
                RaisePropertyChanged(nameof(Active));
            }
        }

        public string TextMethod
        {
            get { return text; }
            set
            {
                text = value;
                RaisePropertyChanged(nameof(TextMethod));
            }
        }


        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
                RaisePropertyChanged(nameof(ErrorMessage));
            }
        }
    }
}
