using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using Uml_Creator.Model;
using Uml_Creator.Model.ENUM;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using Model.Model;
using Uml_Creator.UndoRedo;
using Uml_Creator.UndoRedo.Commands;


namespace Uml_Creator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region copy members

        public ObservableCollection<FigureViewModel> copyFigures { get; private set; }

        #endregion

        #region data members

        private string _textBarText = "1234567890";
       
        public ObservableCollection<FigureViewModel> FiguresViewModels { get; private set; }
        
        public ObservableCollection<Line> Lines { get; private set; }

        private string filename;
        private double _x;
        private object _content;
       
        #endregion data members

        public object Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        private string _statusText = "Welcome to UML Editor";

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                if (value != _statusText)
                {
                    _statusText = value;
                    OnPropertyChanged("StatusText");
                }
            }
        }

        public ICommand BtnCopy { get; }
        public ICommand BtnPaste { get; }
        public ICommand BtnLoadCommand { get; }
        public ICommand BtnGemCommand { get; }
        public ICommand BtnExportCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand BtnAddClass { get; }
        public ICommand DeleteCommand { get; }
        public ICommand BtnCut { get; }
        UndoRedoController undoRedoController = UndoRedoController.Instance;
        public ObservableCollection<LineViewModel> lines { get; }
        public bool isAddingLineBtnPressed;
        public FigureViewModel _firstShapeToConnect;
        public string FileName;
        public string FileNamePic;
        BackgroundWorker worker = new BackgroundWorker();
        public Grid canvas;

        public MainViewModel()
        {
             Content = new Gem_Load();

            copyFigures = new ObservableCollection<FigureViewModel>();
          
            FiguresViewModels = new ObservableCollection<FigureViewModel>
            {
                //new FigureViewModel() {20.0,20.0,50.0,60.0,"lars",EFigure.ClassSquare},
               
             //    new FigureViewModel(0.0,0.0,50.0,20.0,"Dette er en klasse her skriver jeg min tekst!",EFigure.ClassSquare,false),

               //  new FigureViewModel(30.0,80.0,20.0,20.0,"Dette er en anden klasse, skriv noget andet tekst her!",EFigure.ClassSquare,false)
            };

          
          //  lines = new ObservableCollection<LineViewModel>
            //{
            //    new LineViewModel(new Line(), FiguresViewModels[0], FiguresViewModels[1], ELine.Solid)
            //};


            BtnLoadCommand = new RelayCommand(Load_Click);
            BtnGemCommand = new RelayCommand(Save_Click);
            BtnExportCommand = new RelayCommand<Grid>(Export_Click);
            BtnCopy = new RelayCommand(Copy_Click);
            BtnPaste = new RelayCommand(Paste_Click);
            BtnCut = new RelayCommand(Cut_click);
            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.canUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.canRedo);
            BtnAddClass = new RelayCommand(AddClass);
            DeleteCommand = new RelayCommand(Delete_Click);
        }

     
        private void ExecuteRemoveMethod()
        {
        //   MethodsCollection.Remove(SelectedMethod);
        }
      

        public bool IsAddingLineBtnPressed
        {
            get { return isAddingLineBtnPressed; }
            set
            {
                isAddingLineBtnPressed = value;
                if (!value)
                    _firstShapeToConnect = null;
                OnPropertyChanged(nameof(isAddingLineBtnPressed));
                AddLineBetweenShapes.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand<FigureViewModel> AddLineBetweenShapes
            => new RelayCommand<FigureViewModel>(OnAddLineBetweenShapes, lit => IsAddingLineBtnPressed);

        public void OnAddLineBetweenShapes(FigureViewModel fig)
        {
            if (_firstShapeToConnect == null)
                _firstShapeToConnect = fig;
            else
            {
                if (fig != _firstShapeToConnect)
                {
                    undoRedoController.DoExecute(new AddLineCommand(lines,
                        new LineViewModel(new Line(), _firstShapeToConnect, fig, ELine.Solid)));
                    IsAddingLineBtnPressed = false;
                }
            }
        }


        /// <summary>
        /// This method is used to take a copy of the selected objects on the canvas, that can be both lines and figures.
        /// 22-11 -2016 its only for figures at the moment, lines are not in system.
        /// </summary>
        private void Copy_Click()
        {
            copyFigures.Clear();
            int nrOfObjectsCopied = 0;
            foreach (FigureViewModel Figure in FiguresViewModels)
            {
                if (Figure.IsSelected)
                {
                    //We create a new instance to make sure we get a new object, and so it dosent have the same FigureNr, also its added a little ofset from the original models
                    nrOfObjectsCopied++;
                    copyFigures.Add(Figure);
                }
            }
            StatusText = "You have copied:" + nrOfObjectsCopied + "Objects";
        }

        /// <summary>
        /// This method copies/adds the selected objects in screen to the canvas, if no objects are in the copy list, send message to the statusbar
        /// 22-11-2016 works only for figures atm, lines not in system.
        /// </summary>
        private void Paste_Click()
        {
            if (copyFigures.Count > 0)
            {
                int nrOfCopied = 0;

            
                foreach (FigureViewModel figure in copyFigures)
                {


                    double offset = 20.0;
                    FigureViewModel newfigure = new FigureViewModel(figure.X + offset, figure.Y + offset, figure.Width,
                        figure.Height, figure.Data, figure.Type, false, figure.Name, figure.MethodCollection,figure.AttributeCollection);
                    undoRedoController.DoExecute(new AddBoxCommand(FiguresViewModels, newfigure));
                
                    nrOfCopied++;
                }
                StatusText = "you have copied " + nrOfCopied + " objects";
               // Debug.Print("Antal Objecter copieret:" + nrOfCopied);
               // Debug.Print("nr of objects total ---->"+FiguresViewModels.Count);
                //Write to status bar that x objects were copied to the canvas
            }
            else
            {
                string text = "Nothing to copy in the copy list";
                StatusText = text;
                //Debug.WriteLine(text);
                //throw new NotImplementedException();
                //No objects in copy list write to statusbar
            }
        }
        private void Cut_click()
        {
            copyFigures.Clear();

            //Vi går igennem listen bagfra for at undgå enumeration error
            for (int i = FiguresViewModels.Count -1; i >=0; i--)
            {
                FigureViewModel Figure = FiguresViewModels[i];
                    if (Figure.IsSelected)
                    {

                    copyFigures.Add(Figure);
                    undoRedoController.DoExecute(new DeleteFigureCommand(FiguresViewModels, Figure));
                }
                }
            /*
            foreach (FigureViewModel Figure in FiguresViewModels)
            {
                if (Figure.IsSelected)
                {
                    //We create a new instance to make sure we get a new object, and so it dosent have the same FigureNr, also its added a little ofset from the original models

                    copyFigures.Add(Figure);
                    undoRedoController.DoExecute(new DeleteFigureCommand(FiguresViewModels, Figure));
                }
            }*/
        }



        private void Delete_Click()
        {
            foreach (FigureViewModel Figure in FiguresViewModels.Reverse())
            {
                if (Figure.IsSelected)
                {
                    StatusText = "Deleted Objekt: " + Figure.Name;
                    undoRedoController.DoExecute(new DeleteFigureCommand(FiguresViewModels, Figure));
                   // FiguresViewModels.Remove(Figure);
                  
                }
            }
        }

          private void AddClass()
        {
            //FigureViewModel newFigure = new FigureViewModel(0, 0, 10, 20, "data", EFigure.ClassSquare, false,"testClass");
            
            undoRedoController.DoExecute(new AddBoxCommand(FiguresViewModels, new FigureViewModel()));
            
            //TextBar = "abekat";
        }

        private void Load_Click()
        {
            OpenFileDialog loadfildialog = new OpenFileDialog();
            loadfildialog.Filter = "XML files (*.xml)|*.xml";
            if (loadfildialog.ShowDialog() != DialogResult.OK) return;
            FileName = loadfildialog.FileName;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(FileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    ObservableCollection<FigureViewModel> temp;
                    Type outType = typeof(ObservableCollection<FigureViewModel>);
                        //skal være samme slags objekter som diagrammet
                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        temp = (ObservableCollection<FigureViewModel>) serializer.Deserialize(reader);
                        FiguresViewModels.Clear();
                        for (int i = 0; i < temp.Count; i++)
                        {
                          //  FiguresViewModels.Add(new FigureViewModel(temp[i].X, temp[i].Y, temp[i].Width, temp[i].Height, temp[i].Data, temp[i].Type,false,temp[i].Name));
                            FiguresViewModels.Add(new FigureViewModel(temp[i]));
                        }
                        
                        reader.Close();
                    }

                    read.Close();
                    StatusText = "The file has been loaded";
                }
            }
            catch (Exception ex)
            {
                StatusText = ex.ToString();
                Console.WriteLine(ex.ToString());
                //Log exception here
            }
        }

        private void Save_Click()
        {
            StatusText = "Saving your work now";

            SaveFileDialog gemfildialog = new SaveFileDialog();
            gemfildialog.Filter = "XML files (*.xml)|*.xml";
            if (gemfildialog.ShowDialog() != DialogResult.OK) return;
            FileName = gemfildialog.FileName;
            worker.DoWork += worker_Save;
            worker.RunWorkerAsync();

        }

        private void worker_Save(object sender, DoWorkEventArgs e)
        {
            var serialObject = FiguresViewModels; //skal importere diagrammets data

            if (serialObject == null) return;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serialObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serialObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(FileName);
                    stream.Close();
                    StatusText = "The document has been saved.";
                }
            }
            catch (Exception ex)
            {
                    StatusText = ex.ToString();
                Console.WriteLine(ex.ToString());
                //Log exception here
            }
        }

        private void Export_Click(Grid canvas)
        {
            this.canvas = canvas;
            SaveFileDialog exportfildialog = new SaveFileDialog();
            exportfildialog.Filter = "PNG files (*.png)|*.png";

            if (!(exportfildialog.ShowDialog() == DialogResult.OK)) return;
            FileNamePic = exportfildialog.FileName;

            RenderTargetBitmap rtb = new RenderTargetBitmap((int) canvas.RenderSize.Width,
                (int) canvas.RenderSize.Height, 96d, 96d, PixelFormats.Default);
                rtb.Render(canvas);

                //var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, 1000, 1000));

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
            using (var fs = File.OpenWrite(FileNamePic))
                {
                    pngEncoder.Save(fs);
                }
            //worker.DoWork += worker_Export;
            //worker.RunWorkerAsync();
            }

       /* private void worker_Export(object sender, DoWorkEventArgs e)
        {
            Grid canvas = this.canvas;
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas.RenderSize.Width,
                                           (int)canvas.RenderSize.Height, 96d, 96d, PixelFormats.Default);
            rtb.Render(canvas);

            //var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, 1000, 1000));

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
            using (var fs = File.OpenWrite(FileNamePic))
        {
                pngEncoder.Save(fs);
        }
        }*/


        /*
        public string StatusBarTextProperty
        {
            get { return (string) GetValue(StatusBarTextPropertyProperty); }
            set { SetValue(StatusBarTextPropertyProperty, value); }
        }
        
        private void SetValue(DependencyProperty statusBarTextPropertyProperty, string value)
        {
            statusBarTextPropertyProperty.Name = value;
        }
        */

        private string GetValue(DependencyProperty statusBarTextPropertyProperty)
        {
            return statusBarTextPropertyProperty.Name;
            //throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
