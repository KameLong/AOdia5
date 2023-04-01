using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AOdiaData
{
    public class UndoCommand
    {
        public string comment = "";
        public Action Invoke;
        public Action Undo;
        public Action Redo;
    }

    public class UndoStack: INotifyPropertyChanged
    {

        private readonly Stack<UndoCommand> _undoStack = new Stack<UndoCommand>();
        private readonly Stack<UndoCommand> _redoStack = new Stack<UndoCommand>();
        public readonly Stack<string> _urlStack = new Stack<string>();

        public event PropertyChangedEventHandler? PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
          => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        public static UndoStack Instance { get; } = new UndoStack();

        public UndoStack()
        {
            _urlStack.Push("");
        }

        public void PushURL(string url)
        {
            _urlStack.Push(url);
        }
        public string RecentUrl()
        {
            return _urlStack.First();
        }
        public void Push(UndoCommand command)
        {
            this._redoStack.Clear();
            this._undoStack.Push(command);
            command.Invoke();
            OnPropertyChanged();
        }
        public bool CanBack() => this._urlStack.Count>=3;

        public bool CanUndo() => this._undoStack.Any();
        public bool CanRedo() => this._redoStack.Any();

        public void Undo()
        {
            if (!this.CanUndo()) return;
            var command = this._undoStack.Pop();
            command.Undo();
            this._redoStack.Push(command);
            OnPropertyChanged();

        }

        public void Redo()
        {
            if (!this.CanRedo()) return;
            var command = this._redoStack.Pop();
            command.Redo();
            this._undoStack.Push(command);
            OnPropertyChanged();
        }
        public string PopUrl()
        {
            OnPropertyChanged();
            if (this._urlStack.Count > 1) { 

            return _urlStack.Pop();
            }
            return "";
        }
    }
}
