using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class AddLineCommand : IUndoCommand
    {
        private ObservableCollection<LineViewModel> lines;
        private LineViewModel line;

        public AddLineCommand(ObservableCollection<LineViewModel> _lines, LineViewModel _line)
        {
            lines = _lines;
            line = _line;
        }

        public void Execute()
        {
            lines.Add(line);
        }

        public void Unexecute()
        {
            lines.Remove(line);
        }
    }
}
