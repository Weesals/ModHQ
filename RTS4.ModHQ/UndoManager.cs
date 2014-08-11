using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace RTS4.ModHQ {
    public class UndoManager {

        Stack<Action> undoEvents = new Stack<Action>();

        public void PushRestore(Action action) {
            undoEvents.Push(action);
        }

        public void Undo() {
            if (undoEvents.Count > 0) {
                undoEvents.Pop()();
            }
        }

    }
}
