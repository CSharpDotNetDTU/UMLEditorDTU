using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uml_Creator.UndoRedo
{
    class UndoRedoController
    {
        private Stack<IUndoCommand> undoStack = new Stack<IUndoCommand>();
        private Stack<IUndoCommand> redoStack = new Stack<IUndoCommand>();
        public static UndoRedoController Instance { get; } = new UndoRedoController();

        private UndoRedoController()
        {
        }

        public void DoExecute(IUndoCommand command)
        {
            undoStack.Push(command);
            command.Execute();
        }

        public void Undo()
        {
            IUndoCommand command = undoStack.Pop();
            command.Unexecute();
            redoStack.Push(command);
        }

        public void Redo()
        {
            IUndoCommand command = redoStack.Pop();
            command.Execute();
            undoStack.Push(command);
        }

        public bool canUndo()
        {
            return undoStack.Any();
        }

        public bool canRedo()
        {
            return redoStack.Any();
        }
    }
}
