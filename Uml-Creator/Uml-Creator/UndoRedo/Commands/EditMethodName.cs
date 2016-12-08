using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class EditMethodName : IUndoCommand
    {
        private ClassContent method;
        private string oldName;
        private string newName;

        public EditMethodName(ClassContent method, string oldName, string newName)
        {
            this.method = method;
            this.oldName = oldName;
            this.newName = newName;
        }

        public void Execute()
        {
            method.setNewName(newName);
        }

        public void Unexecute()
        {
            method.setNewName(oldName);
        }
    }
}
