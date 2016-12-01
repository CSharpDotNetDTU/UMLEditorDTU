using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class DeleteLineCommand : IUndoCommand
    {
        private ObservableCollection<LineViewModel> lines;
        private LineViewModel line;

        public DeleteLineCommand(ObservableCollection<LineViewModel> lines, LineViewModel line)
        {
            this.lines = lines;
            this.line = line;
        }

        public void Execute()
        {
            lines.Remove(line);
        }

        public void Unexecute()
        {
            lines.Add(line);
        }
    }
}
