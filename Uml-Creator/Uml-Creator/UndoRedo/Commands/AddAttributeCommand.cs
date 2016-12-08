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
        private ClassContent attribute;

        public AddAttributeCommand(FigureViewModel box, ClassContent attribute)
        {
            this.box = box;
            this.attribute = attribute;
        }

        public void Execute()
        {
            box.AttributeCollection.Add(attribute);
        }

        public void Unexecute()
        {
            box.AttributeCollection.Remove(attribute);
        }
    }
}
