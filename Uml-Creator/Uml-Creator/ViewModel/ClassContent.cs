using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Uml_Creator.Model;
using Uml_Creator.Model.ENUM;
using Uml_Creator.Model.Interfaces;
using Uml_Creator.UndoRedo;
using Uml_Creator.UndoRedo.Commands;

namespace Uml_Creator.ViewModel
{
    public class ClassContent : ISerializable, INotifyPropertyChanged, IClassMethod, IClassAttribute
    {
        public ClassContent() { }

        private string _name;
        private AttributeModel _attribute;
        private MethodModel _method;
        private readonly UndoRedoController undoRedo = UndoRedoController.Instance;

        public string ContextName
        {
            get { return _name; }
            set
            {
                undoRedo.DoExecute(new EditAttributeName(this,_name,value));
            }
        }

        public void SetNewName(string newName)
        {
            _name = newName;
            OnPropertyChanged(nameof(ContextName));
        }



        public ClassContent(string type)
        {
            _name = "Empty";

            if (type.Equals("method"))
            {
                _method = new MethodModel();
            }
            else
            {
                _attribute = new AttributeModel();
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;

         string IClassMethod.Name
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        EVisibility IClassAttribute.Visibility
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        string IClassAttribute.Type
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        string IClassAttribute.Name
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        EVisibility IClassMethod.Visibility
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        string IClassMethod.Type
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public List<string> Parameters { get; set; }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new System.NotImplementedException();
        }

        private void OnPropertyChanged(String name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}