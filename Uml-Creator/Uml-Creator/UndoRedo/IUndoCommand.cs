using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uml_Creator.UndoRedo
{
    interface IUndoCommand
    {
        void Execute();
        void Unexecute();
    }
}
