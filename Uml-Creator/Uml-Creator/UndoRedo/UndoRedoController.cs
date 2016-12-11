using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Uml_Creator.UndoRedo
{
    internal class UndoRedoController
    {
        private Stack<IUndoCommand> undoStack = new Stack<IUndoCommand>();
        private Stack<IUndoCommand> redoStack = new Stack<IUndoCommand>();

        public int UndoStackSize { get { return undoStack.Count; } }
        public int RedoStackSize { get { return redoStack.Count; } }

        public static UndoRedoController Instance { get; } = new UndoRedoController();

        private UndoRedoController()
        {
        }

        public void reset()
        {
            undoStack.Clear();
            redoStack.Clear();
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

        public void ResetUndoRedoStacks()
        {
            undoStack.Clear();
            redoStack.Clear();
        }
    }
}
