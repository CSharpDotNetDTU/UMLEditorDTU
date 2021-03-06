﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model;
using Uml_Creator.ViewModel;

namespace Uml_Creator.UndoRedo.Commands
{
    class DeleteMethodCommand : IUndoCommand
    {
        private FigureViewModel box;
        private ClassContent method;

        public DeleteMethodCommand(FigureViewModel box, ClassContent method)
        {
            this.box = box;
            this.method = method;
        }

        public void Execute()
        {
            box.MethodCollection.Remove(method);
        }

        public void Unexecute()
        {
            box.MethodCollection.Add(method);
        }
    }
}
