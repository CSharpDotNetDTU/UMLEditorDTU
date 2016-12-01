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
    class AddAttributeCommand : IUndoCommand
    {
        private FigureViewModel box;
        private AttributeViewModel attribute;

        public AddAttributeCommand(FigureViewModel box, AttributeViewModel attribute)
        {
            this.box = box;
            this.attribute = attribute;
        }

        public void Execute()
        {
            box.attributes.Add(attribute);
        }

        public void Unexecute()
        {
            box.attributes.Remove(attribute);
        }
    }
}
