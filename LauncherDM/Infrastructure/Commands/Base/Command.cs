using System;
using System.Windows.Input;

namespace LauncherDM.Infastructure.Commands.Base
{
    public abstract class Command : ICommand
    {
        //   public event EventHandler? CanExecuteChanged; // вызывается при изменении условий (CanExecute), указывающий, может ли команда выполняться

        // передаем управление системе WPF
        // реализуем событие явно
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value; // передаем управления этого события классу CommandManager
            remove => CommandManager.RequerySuggested -= value;
        }

        public abstract bool CanExecute(object? parameter); // true|false Если false значит команду выполнить нельзя и наоборот 

        public abstract void Execute(object? parameter); // логика комманды, что она должна выполнять 

    }
}
