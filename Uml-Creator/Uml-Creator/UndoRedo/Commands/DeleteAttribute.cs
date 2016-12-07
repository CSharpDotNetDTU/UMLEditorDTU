using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model;
using Uml_Creator.Model.Interfaces;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class DeleteAttribute : IUndoCommand
    {
        private FigureViewModel box;
        private AttributeModel attribute;

        public DeleteAttribute(FigureViewModel box, AttributeModel attribute)
        {
            this.box = box;
            this.attribute = attribute;
        }

        public void Execute()
        {
            box.AttributeCollection.Remove(attribute);
        }

        public void Unexecute()
        {
            box.AttributeCollection.Add(attribute);
        }
    }
}
