using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class EditAttributeName : IUndoCommand
    {
        private ClassContent attribute;
        private string oldName;
        private string newName;

        public EditAttributeName(ClassContent attribute, string oldName, string newName)
        {
            this.attribute = attribute;
            this.oldName = oldName;
            this.newName = newName;
        }

        public void Execute()
        {
            attribute.SetNewName(newName); 
        }

        public void Unexecute()
        {
            attribute.SetNewName(oldName);
        }
    }
}
