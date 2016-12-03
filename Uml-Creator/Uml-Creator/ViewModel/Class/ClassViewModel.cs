using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using PropertyChanged;
using Uml_Creator.Annotations;


namespace Uml_Creator.ViewModel.Class
{
    [ImplementPropertyChanged]
    public class ClassViewModel: UmlControl
    {
        #region Properties

        public string ClassName { get; set; }

        public ClassViewModel ParentClass { get; set; }

        public ObservableCollection<MethodViewModel> MethodsCollection { get; set; } = new ObservableCollection<MethodViewModel>();

        public ObservableCollection<string> PropertiesCollection { get; set; } = new ObservableCollection<string>();

        public MethodViewModel SelectedMethod { get; set; }

        #endregion

        #region RelayCommands
        public RelayCommand AddMethod { get; set; }
        public RelayCommand RemoveMethod { get; set; }

        #endregion

        public ClassViewModel()
        {

            AddMethod = new RelayCommand(ExecuteAddMethod);
            //RemoveMethod = new RelayCommand(ExecuteRemoveMethod, CanRemoveMethod);
            RemoveMethod = new RelayCommand(ExecuteRemoveMethod);
        }

       

        #region Commands

        public void ExecuteAddMethod()
        {
            MethodsCollection.Add(new MethodViewModel() { Name = "Method" });
        }

        public void ExecuteRemoveMethod()
        {
            MethodsCollection.Remove(SelectedMethod);
        }

        public bool CanRemoveMethod()
        {
            return SelectedMethod != null && MethodsCollection.Contains(SelectedMethod);
        }

        #endregion
    }
}
