using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopPart.Model
{
    public class CustomCUMmand<T> : ICommand where T : class
    {
        Action<T> action;
        Func<bool> chech;

        public CustomCUMmand(Action<T> action, Func<bool> check = null)
        {
            this.action = action;
            this.chech = check;
        }


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (chech == null)
                return true;
            else
                return chech();

        }

        public void Execute(object parameter)
        {
            if (parameter != null)
                action((T)parameter);
            else
                action(null);
        }

    }
}
