using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ViewModel
{
    // Klasa UserCommand pozwala na reakcję na akcje użytkownika w interfejsie użytkownika.
    internal class UserCommand : ICommand
    {
        // Pole execute przechowuje metodę, która zostanie wykonana po wywołaniu komendy.
        private readonly Action execute;
        // Pole canExecute to funkcja boolowska, która określa, czy można wywołać tę komendę.
        private readonly Func<bool> canExecute;

        // Zdarzenie CanExecuteChanged pozwala na odświeżenie widoku, gdy zmienia się możliwość wywołania komendy.
        public event EventHandler CanExecuteChanged;

        public UserCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public UserCommand(Action execute) : this(execute, null) { }

        // Implementacja metody Execute interfejsu ICommand, która wywołuje metodę execute.
        public virtual void Execute(object obj)
        {
            this.execute();
        }

        // Implementacja metody CanExecute interfejsu ICommand, która sprawdza, czy można wywołać komendę.
        public bool CanExecute(object obj)
        {
            if (this.canExecute == null)
            {
                // Jeśli canExecute jest puste, zwraca true.
                return true;
            }
            if (obj == null)
            {
                // Jeśli obj jest puste, zwraca wartość canExecute.
                return this.canExecute();
            }
            // Zwraca wartość canExecute.
            return this.canExecute();
        }

        // Metoda RaiseCanExecuteChanged służy do wywołania zdarzenia CanExecuteChanged.
        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
