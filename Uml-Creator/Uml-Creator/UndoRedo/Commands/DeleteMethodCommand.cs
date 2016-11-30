using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class DeleteMethodCommand : IUndoCommand
    {
        private FigureViewModel box;
        private MethodViewModel method;

        public DeleteMethodCommand(FigureViewModel box, MethodViewModel method)
        {
            this.box = box;
            this.method = method;
        }

        public void Execute()
        {
            box.methods.Remove(method);
        }

        public void Unexecute()
        {
            box.methods.Add(method);
        }
    }
}
