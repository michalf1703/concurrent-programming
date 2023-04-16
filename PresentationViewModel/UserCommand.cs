using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ViewModel
{
    //UserCommand class is a class that allows to react to user actions in the user interface.
    internal class UserCommand : ICommand
    {
        //action, which be performed when the user invokes the command
        private readonly Action execute;
        //boolean function that determines whether this action can be performed
        private readonly Func<bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public UserCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public UserCommand(Action execute) : this(execute, null) { }

        ////Execute - method of ICommand interface
        public virtual void Execute(object obj)
        {
            this.execute();
        }
        //CanExecute - method of ICommand interface
        public bool CanExecute(object obj)
        {
            if (this.canExecute == null)
            {
                return true;
            }
            if (obj == null)
            {
                return this.canExecute();
            }
            return this.canExecute();
        }
        //The RaiseCanExecuteChanged method is used to fire the CanExecuteChanged event,
        //which allows the view to be refreshed when the ability to perform an action changes.
        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
