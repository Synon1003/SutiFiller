﻿using System;
using System.Windows.Input;

namespace SutiFiller.Admin.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<Object> _execute; // a tevékenységet végrehajtó lambda-kifejezés
        private readonly Func<Object, Boolean> _canExecute; // a tevékenység feltételét ellenőző lambda-kifejezés

        // Végrehajthatóság változásának eseménye.
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(Action<Object> execute) : this(null, execute) { }

        public DelegateCommand(Func<Object, Boolean> canExecute, Action<Object> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        public Boolean CanExecute(Object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(Object parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("Command execution is disabled.");
            }
            _execute(parameter);
        }
    }
}
