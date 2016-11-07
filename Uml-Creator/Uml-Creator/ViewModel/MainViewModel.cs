using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using Uml_Creator.Model;
using Uml_Creator.Model.ENUM;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media;
using System.Windows;

namespace Uml_Creator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        #region data members

        public ObservableCollection<FigureViewModel> FiguresViewModels { get; private set; }
        public ObservableCollection<Line> Lines { get; private set; }

       

        private double _x;
        private object _content;
        #endregion data members
        private string filename;


        public object Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public ICommand BtnLoadCommand { get; }

        public ICommand BtnGemCommand { get; }

        public ICommand BtnExportCommand { get; }

        public double Y { get; set; }

        public MainViewModel()
        {
            Content = new Gem_Load();
             BtnLoadCommand = new RelayCommand(Load_Click);
             BtnGemCommand = new RelayCommand(Save_Click);


            FiguresViewModels = new ObservableCollection<FigureViewModel>
            {
               
                //new FigureViewModel() {20.0,20.0,50.0,60.0,"lars",EFigure.ClassSquare},
               
                 new FigureViewModel(50.0,120.0,50.0,60.0,"Peter",EFigure.ClassSquare),

                 new FigureViewModel(30.0,80.0,20.0,30.0,"Lars",EFigure.ClassSquare)
            };
            BtnLoadCommand = new RelayCommand(Load_Click);
            BtnGemCommand = new RelayCommand(Save_Click);
            BtnExportCommand = new RelayCommand<Canvas>(Export_Click);

        }


        private void Load_Click()
        {
            OpenFileDialog loadfildialog = new OpenFileDialog();
            loadfildialog.Filter = "XML files (*.xml)|*.xml";
            if (loadfildialog.ShowDialog() != DialogResult.OK) return;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(loadfildialog.FileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = 3.GetType(); //skal være samme slags objekter som diagrammet
                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //Log exception here
            }
            //convert fra xml til diagram via serialization
            //  textBox1.Text = File.ReadAllText(loadfildialog.FileName);
        }

        private void Save_Click()
        {
            SaveFileDialog gemfildialog = new SaveFileDialog();
            gemfildialog.Filter = "XML files (*.xml)|*.xml";
            if (gemfildialog.ShowDialog() != DialogResult.OK) return;
            var serialObject = 5; //skal importere diagrammets data
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
                    xmlDocument.Save(gemfildialog.FileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //Log exception here
            }

            // File.WriteAllText(gemfildialog.FileName, textBox1.Text);
        }

        private void Export_Click(Canvas canvas)
        {
            SaveFileDialog exportfildialog = new SaveFileDialog();
            exportfildialog.Filter = "PNG files (*.png)|*.png";

            if (exportfildialog.ShowDialog() == DialogResult.OK)
            {
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas.RenderSize.Width,
                                            (int)canvas.RenderSize.Height, 96d, 96d, PixelFormats.Default);
                rtb.Render(canvas);

                var crop = new CroppedBitmap(rtb, new Int32Rect(50, 50, 250, 250));

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(crop));

                using (var fs = File.OpenWrite(filename))
                {
                    pngEncoder.Save(fs);
                }
            }
            }

   

        public ObservableCollection<FigureViewModel> FigureViewModels
        {
            get
            {
                return FiguresViewModels;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
