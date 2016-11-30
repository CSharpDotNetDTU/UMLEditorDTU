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

        public void doExecute(IUndoCommand undoRedo)
        {
            
        }
    }
}
