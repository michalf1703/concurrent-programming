using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Model;
using System.Collections.ObjectModel;

namespace ViewModel
{
    // Klasa ViewModel implementuje interfejs INotifyPropertyChanged, który jest używany do powiadamiania widoku o zmianach w modelu.
    public class ViewModel : INotifyPropertyChanged
    {
        // Referencja do modelu
        private ModelAPI modelApi;
        // Kolekcja kulek
        public ObservableCollection<IBall> Balls { get; set; }

        // Komenda wywoływana po kliknięciu przycisku "Start"
        public ICommand StartButtonClick { get; set; }
        // Tekst wpisywany przez użytkownika w polu tekstowym
        private string inputText;

        // Stan przycisku "Start"
        private bool state;

        // Właściwość stanu przycisku "Start", używana do włączania/wyłączania przycisku w widoku.
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

        // Właściwość tekstu wpisywanego przez użytkownika w polu tekstowym
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

        // Właściwość wykorzystywana do wyświetlania komunikatów o błędach w widoku
        private string errorMessage;

        // Właściwość służąca do wyświetlania komunikatów o błędach w widoku
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

        // Zdarzenie PropertyChangedEventHandler wywoływane w momencie zmiany wartości właściwości.
        public event PropertyChangedEventHandler PropertyChanged;

        // Konstruktor domyślny ViewModel
        public ViewModel() : this(ModelAPI.CreateApi())
        {

        }

        // Konstruktor ViewModel z parametrem modelu
        public ViewModel(ModelAPI baseModel)
        {
            // Ustawienie początkowego stanu przycisku "Start"
            State = true;
            // Przypisanie referencji do modelu
            this.modelApi = baseModel;
            // Inicjalizacja kolekcji kulek
            Balls = new ObservableCollection<IBall>();
            // Subskrypcja do IObservable<IBall> modelu w celu otrzymywania powiadomień o zmianach.
            IDisposable observer = modelApi.Subscribe(x => Balls.Add(x));
            // Utworzenie nowej komendy i zdefiniowanie jej akcji wywołującej metodę StartButtonClickHandler()
            StartButtonClick = new UserCommand(() => StartButtonClickHandler());
        }

        // Ta metoda jest wywoływana po kliknięciu przycisku "Start"
        // Dodaje kulki do modelu i uruchamia symulację
        private void StartButtonClickHandler()
        {
            modelApi.AddBallsAndStart(readFromTextBox());
        }

        // Parsuje wprowadzone przez użytkownika dane z pola tekstowego i waliduje je
        public int readFromTextBox()
        {
            int number;
            if (Int32.TryParse(InputText, out number) && InputText != "0")
            {
                number = Int32.Parse(InputText);
                ErrorMessage = "";
                State = false;
                if (number > 15)
                {
                    return 15;
                }
                return number;
            }
            ErrorMessage = "Nieprawidłowa wartość";
            return 0;
        }

        // Wywołuje zdarzenie PropertyChanged, aby powiadomić widok o zmianach właściwości
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
