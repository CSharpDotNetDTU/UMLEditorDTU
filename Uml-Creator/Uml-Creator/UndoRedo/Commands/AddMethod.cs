using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model;
using Uml_Creator.Model.Interfaces;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class AddMethod : IUndoCommand
    {
        private FigureViewModel box;
        private MethodModel method;

        public AddMethod(FigureViewModel box, MethodModel method)
        {
            this.box = box;
            this.method = method;
        }

        public void Execute()
        {
            box.MethodCollection.Add(method);
        }

        public void Unexecute()
        {
            box.MethodCollection.Remove(method);
        }
    }
}
