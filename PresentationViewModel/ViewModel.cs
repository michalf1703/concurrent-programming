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
            IDisposable observer = modelApi.Subscribe(x => Balls.Add(x));
        }

        private void StartButtonClickHandler()
        {
            modelApi.AddBallsAndStart(readFromTextBox());
        }

        public int readFromTextBox()
        {
            int number;
            if (Int32.TryParse(InputText, out number) && InputText != "0")
            {
                number = Int32.Parse(InputText);
                ErrorMessage = "";
                State = false;
                if (number > 10)
                {
                    return 10;
                }
                return number;
            }
            ErrorMessage = "Nieprawidłowa wartość";
            return 0;
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
