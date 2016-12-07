﻿using System;
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
        private MethodModel method;
        private string oldName;
        private string newName;

        public EditMethodName(MethodModel method, string oldName, string newName)
        {
            this.method = method;
            this.oldName = oldName;
            this.newName = newName;
        }

        public void Execute()
        {
            method.Name = newName;
        }

        public void Unexecute()
        {
            method.Name = oldName;
        }
    }
}
