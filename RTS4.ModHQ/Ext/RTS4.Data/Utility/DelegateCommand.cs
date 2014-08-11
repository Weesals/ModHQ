using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RTS4 {
    public class DelegateCommand : ICommand {

        public Action<object> Action;

        public DelegateCommand(Action<object> action) {
            Action = action;
        }

        public bool CanExecute(object parameter) { return true; }

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter) {
            Action(parameter);
        }
    }
}
