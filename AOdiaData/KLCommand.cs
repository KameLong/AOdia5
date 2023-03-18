using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOdiaData
{
    public class UndoCommand
    {
        public string commnet = "";
        public Action Invoke;
        public Action Undo;
        public Action Redo;
    }

    public class UndoStack
    {
        public List<string> undoStackStr { get { return _undoStack.Select(x => x.commnet).ToList(); } }
        public List<string> redoStackStr { get { return _redoStack.Select(x => x.commnet).ToList(); } }
        private readonly Stack<UndoCommand> _undoStack = new Stack<UndoCommand>();
        private readonly Stack<UndoCommand> _redoStack = new Stack<UndoCommand>();

        public static UndoStack Instance { get; } = new UndoStack();

        public void Push(UndoCommand command)
        {
            this._redoStack.Clear();
            this._undoStack.Push(command);
            command.Invoke();
        }

        public bool CanUndo() => this._undoStack.Any();
        public bool CanRedo() => this._redoStack.Any();

        public void Undo()
        {
            if (!this.CanUndo()) return;
            var command = this._undoStack.Pop();
            command.Undo();
            this._redoStack.Push(command);
        }

        public void Redo()
        {
            if (!this.CanRedo()) return;
            var command = this._redoStack.Pop();
            command.Redo();
            this._undoStack.Push(command);
        }
    }
}
