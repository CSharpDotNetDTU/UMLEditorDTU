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
using Uml_Creator.View.Resource;


namespace Uml_Creator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region copy members

        public ObservableCollection<FigureViewModel> copyFigures { get; private set; }

        #endregion

        #region data members

        public ObservableCollection<FigureViewModel> FiguresViewModels { get; private set; }
        
        #endregion data members

        private string _statusText = "Welcome to UML Editor";

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                    _statusText = value;
                    OnPropertyChanged("StatusText");
                }
            }

        public ICommand BtnNewCommand { get; }

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

        private bool _isAddingLineBtnPressed;
        private FigureViewModel _firstShapeToConnect;
        public string FileName;
        public string FileNamePic;
        private ELine _lineType;
        BackgroundWorker worker = new BackgroundWorker();
        public Grid canvas;

        public MainViewModel()
        {
             //Content = new Gem_Load();

            copyFigures = new ObservableCollection<FigureViewModel>();
          
            FiguresViewModels = new ObservableCollection<FigureViewModel>
            {

            };

          
            lines = new ObservableCollection<LineViewModel>
            {
            };


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
            BtnNewCommand = new RelayCommand(NewClassDiagram);
        }

        public static bool IsDragging;
        public ICommand OnMouseLeftBtnDownCommand => new RelayCommand<UIElement>(OnMouseLeftBtnDown);
        public ICommand OnMouseLeftBtnUpCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseLeftUp);

        public ICommand OnMouseMoveCommand => new RelayCommand<UIElement>(OnMouseMove);

        //public ICommand OnMouseLeaveCommand => new RelayCommand<UIElement>(OnMouseLeave);

        private void OnMouseLeave(UIElement obj)
        {
            if (obj == null) return;
            if (!IsDragging) return;
            foreach (FigureViewModel t in FiguresViewModels)
            {
                if (t.IsSelected && IsDragging)
                {
                    var pos = Mouse.GetPosition(VisualTreeHelper.GetParent(obj) as IInputElement);
                    t.X = t.X + pos.X;
                    t.Y = t.Y + pos.Y;

                }
            }
        }

        private void OnMouseMove(UIElement obj)
        {
            if (!IsDragging) return;
            if (obj == null) return;
            var pos = Mouse.GetPosition(obj);
            foreach (FigureViewModel t in FiguresViewModels)
            {
                if (t.IsSelected)
                {
                    t.X = pos.X - t.XOffset;
                    t.Y = pos.Y - t.YOffset;
                }
            }
        }

        private void OnMouseLeftBtnDown(UIElement obj)
        {
            if (obj == null) return;

            foreach (FigureViewModel t in FiguresViewModels)
            {
                if (t.IsSelected)
                {
                    OnAddLineBetweenShapes(t);
                    obj.Focus();
                }
                t.XOffset = Mouse.GetPosition(obj).X - t.X;
                t.YOffset = Mouse.GetPosition(obj).Y - t.Y;
            }
        }


        private void OnMouseLeftUp(MouseButtonEventArgs obj)
        {
            IsDragging = false;
            var visual = obj.Source as UIElement;
            if (visual == null) return;
            foreach (FigureViewModel t in FiguresViewModels)
            {
                if (IsDragging) return;

                if(t.IsSelected) UndoRedoController.Instance.DoExecute(new MoveBoxCommand(t, t.OrigShapePostion, new Point(t.X, t.Y)));
            }
            
            Mouse.Capture(null);
            obj.Handled = true;
        }

        private void NewClassDiagram()
        {
           ClearEverything();
        }

        private void ClearEverything()
        {
            copyFigures.Clear();
            undoRedoController.ResetUndoRedoStacks();
            FiguresViewModels.Clear();
            if (lines != null)
            {
                lines.Clear();
            }
        }

        public bool IsAddingSolidLineBtnPressed
        {
            get { return _isAddingLineBtnPressed; }
            set
            {
                foreach (var t in FiguresViewModels)
                {
                    t.IsSelected = false;
                }
                Debug.WriteLine("im on");
                _isAddingLineBtnPressed = value;
                if (!value)
                    _firstShapeToConnect = null;

                _lineType = ELine.Solid; 
                OnPropertyChanged(nameof(_isAddingLineBtnPressed));
            }
        }

        public bool IsAddingDashedLineBtnPressed
        {
            get { return _isAddingLineBtnPressed; }
            set
            {
                foreach (var t in FiguresViewModels)
                {
                    t.IsSelected = false;
                }
                Debug.WriteLine("im on");
                _isAddingLineBtnPressed = value;
                if (!value)
                    _firstShapeToConnect = null;

                _lineType = ELine.DashedLine;;
                OnPropertyChanged(nameof(_isAddingLineBtnPressed));
            }
        }


        public void OnAddLineBetweenShapes(FigureViewModel fig)
        {
            Debug.WriteLine("adding");
            if (_isAddingLineBtnPressed) { 
            if (_firstShapeToConnect == null)
                _firstShapeToConnect = fig;
            else
            {
                if (fig != _firstShapeToConnect)
                {
                    undoRedoController.DoExecute(new AddLineCommand(lines,
                        new LineViewModel(new Line(), _firstShapeToConnect, fig, _lineType)));
                        IsAddingSolidLineBtnPressed = false;
                    IsAddingDashedLineBtnPressed = false;
                    _firstShapeToConnect = null;
                }
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
            StatusText = nrOfObjectsCopied + " items copied";
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
                        figure.Height, figure.Data, figure.Type, false, figure.Name, figure.MethodCollection,
                        figure.AttributeCollection);
                    undoRedoController.DoExecute(new AddBoxCommand(FiguresViewModels, newfigure));
                
                    nrOfCopied++;
                }
                StatusText = nrOfCopied + " items pasted";
            }
            else
            {
                StatusText = "Nothing to paste";
            }
        }

        private void Cut_click()
        {
            copyFigures.Clear();

            //Vi går igennem listen bagfra for at undgå enumeration error
            for (int i = FiguresViewModels.Count - 1; i >= 0; i--)
            {
                FigureViewModel Figure = FiguresViewModels[i];
                if (Figure.IsSelected)
                {
                    copyFigures.Add(Figure);
                    undoRedoController.DoExecute(new DeleteFigureCommand(FiguresViewModels, Figure));
                }
            }
        }


        private void Delete_Click()
        {
            foreach (FigureViewModel Figure in FiguresViewModels.Reverse())
            {
                if (Figure.IsSelected)
                {
                    StatusText = "Deleted Object: " + Figure.Name;
                    undoRedoController.DoExecute(new DeleteFigureCommand(FiguresViewModels, Figure));
                   // FiguresViewModels.Remove(Figure);
                }
            }
        }

          private void AddClass()
        {
            //FigureViewModel newFigure = new FigureViewModel(0, 0, 10, 20, "data", EFigure.ClassSquare, false,"testClass");
            
            undoRedoController.DoExecute(new AddBoxCommand(FiguresViewModels, new FigureViewModel()));
            StatusText = "New class has been added";

            for (int i = 0; i < FiguresViewModels.Count; i++)
            {
                var tempfig = FiguresViewModels[i];

                
            }
            
        }

        private void Load_Click()
        {
            ClearEverything();

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
                    SaveClass temp;
                    Type outType = typeof(SaveClass);
                        //skal være samme slags objekter som diagrammet
                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        temp = (SaveClass) serializer.Deserialize(reader);
                        for (int i = 0; i < temp.Figures.Count; i++)
                        {
                          //  FiguresViewModels.Add(new FigureViewModel(temp[i].X, temp[i].Y, temp[i].Width, temp[i].Height, temp[i].Data, temp[i].Type,false,temp[i].Name));
                            FiguresViewModels.Add(new FigureViewModel(temp.Figures[i]));
                        }

                        for (int i = 0; i < temp.Lines.Count; i++)
                        {
                            //  FiguresViewModels.Add(new FigureViewModel(temp[i].X, temp[i].Y, temp[i].Width, temp[i].Height, temp[i].Data, temp[i].Type,false,temp[i].Name));
                            lines.Add(new LineViewModel(temp.Lines[i]));
                        }
                        undoRedoController.reset();
                        reader.Close();
                    }

                    read.Close();
                    StatusText = "File has been loaded";
                }
            }
            catch (Exception ex)
            {
                StatusText = "File could not be loaded";
                Console.WriteLine(ex.ToString());
                //Log exception here
            }
        }

        private void Save_Click()
        {
            StatusText = "Saving...";

            SaveFileDialog gemfildialog = new SaveFileDialog();
            gemfildialog.Filter = "XML files (*.xml)|*.xml";
            if (gemfildialog.ShowDialog() != DialogResult.OK) return;
            FileName = gemfildialog.FileName;
            worker.DoWork += worker_Save;
            worker.RunWorkerAsync();
        }

        private void worker_Save(object sender, DoWorkEventArgs e)
        {
            var serialObject = new SaveClass(FiguresViewModels, lines); //skal importere diagrammets data

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
                    StatusText = "Document has been saved";
                }
            }
            catch (Exception ex)
            {
                StatusText = "Document could not be saved";
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
