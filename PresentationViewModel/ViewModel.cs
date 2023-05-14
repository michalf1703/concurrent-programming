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
        private ModelAPI modelApi;
        public ObservableCollection<IBall> Balls { get; set; }

        public ICommand StartButtonClick { get; set; }
        private string inputText;

        private bool state;

        //The State property is used to enable/disable the start button in the view
        public bool State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                RaisePropertyChanged(nameof(State));
            }
        }

        //The InputText property is bound to a text box in the view
        public string InputText
        {
            get
            {
                return inputText;
            }
            set
            {
                inputText = value;
                RaisePropertyChanged(nameof(InputText));
            }
        }

        private string errorMessage;

        //The ErrorMessage property is used to display validation errors in the view
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

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel() : this(ModelAPI.CreateApi())
        {

        }

        public ViewModel(ModelAPI baseModel)
        {
            State = true;
            this.modelApi = baseModel;
            StartButtonClick = new UserCommand(() => StartButtonClickHandler());
            Balls = new ObservableCollection<IBall>();
            //Subscribe to the model's IObservable<IBall> to get notified of changes
            IDisposable observer = modelApi.Subscribe(x => Balls.Add(x));
        }

        private void StartButtonClickHandler()
        {
            modelApi.AddBallsAndStart(readFromTextBox());
        }

        //Parse the user input from the text box and validate it
        public int readFromTextBox()
        {
            int number;
            if (Int32.TryParse(InputText, out number) && InputText != "0")
            {
                number = Int32.Parse(InputText);
                ErrorMessage = "";
                State = false;
                if (number > 25)
                {
                    return 25;
                }
                return number;
            }
            ErrorMessage = "Nieprawidłowa wartość";
            return 0;
        }

        //Raise the PropertyChanged event to notify the view of changes to the properties
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
