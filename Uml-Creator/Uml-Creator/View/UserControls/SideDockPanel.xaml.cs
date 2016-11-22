using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Uml_Creator.Model;

namespace Uml_Creator.View.UserControls
{
    /// <summary>
    /// Interaction logic for SideDockPanel.xaml
    /// </summary>
    public partial class SideDockPanel : UserControl
    {
        
        private readonly ObservableCollection<ToolboxSelection> _toolboxList;
        private string _selectedList;



        public SideDockPanel()
        {
            var list = new ObservableCollection<ToolboxSelection>();
            list.Add(new ToolboxSelection("test1"));
            list.Add(new ToolboxSelection("test2"));
            _toolboxList = list;
            
            InitializeComponent();
        }

        private void toolboxCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        public class Item : INotifyPropertyChanged
        {
            public string ItemName { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        ObservableCollection<Item> _ItemsList = new ObservableCollection<Item>();
        public ObservableCollection<Item> ItemsList
        {
            get
            {
                return _ItemsList;
            }
            set
            {
                _ItemsList = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged()
        {
            throw new NotImplementedException();
        }
    }
    /*
    class ToolboxViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ToolboxSelection> list;
        public ToolboxViewModel()
        {
            list = new ObservableCollection<ToolboxSelection>();
            list.Add(new ToolboxSelection("test1"));
            list.Add(new ToolboxSelection("test2"));
            _toolboxList = new CollectionView(list);
        }

        private readonly CollectionView _toolboxList;
        private string _selectedList;



        public CollectionView ToolboxSelections
        {
            get { return _toolboxList; }
        }


        public string SelectedList
        {
            get { return _selectedList; }
            set
            {
                if (_selectedList == value) return;
                _selectedList = value;
                OnPropertyChanged("SelectedList");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    */
}
