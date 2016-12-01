using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model.Interfaces;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class AddMethod : IUndoCommand
    {
        private FigureViewModel box;
        private MethodViewModel method;

        public AddMethod(FigureViewModel box, MethodViewModel method)
        {
            this.box = box;
            this.method = method;
        }

        public void Execute()
        {
            box.methods.Add(method);
        }

        public void Unexecute()
        {
            box.methods.Remove(method);
        }
    }
}
