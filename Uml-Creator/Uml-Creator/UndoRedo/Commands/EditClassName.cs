using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class EditClassName : IUndoCommand
    {
        private FigureViewModel box;
        private string oldName;
        private string newName;

        public EditClassName(FigureViewModel box, string oldName, string newName)
        {
            this.box = box;
            this.oldName = oldName;
            this.newName = newName;
        }

        public void Execute()
        {
            box.Data = newName;
        }

        public void Unexecute()
        {
            box.Data = oldName;
        }
    }
}
