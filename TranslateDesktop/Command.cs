using System;
using System.Windows.Input;

namespace TranslateDesktop
{
    public class Command : ICommand
    {
        private Action WhattoExecute;
        public Command(Action What)
        {
            WhattoExecute = What;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            WhattoExecute();
        }
    }
}
