using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DogFightApp
{
    public class DummyCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private void OnCanExecuteChanged(EventArgs e)
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null) handler(this, e);
        }

        public void Execute(object parameter)
        {
            MessageBox.Show("Execute!");
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}