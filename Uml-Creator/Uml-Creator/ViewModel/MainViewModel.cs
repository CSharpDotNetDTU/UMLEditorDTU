using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

namespace Uml_Creator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private double _x;
        private object _content;



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

        public double Y { get; set; }

        public MainViewModel()
        {
            Content = new Gem_Load();
             BtnLoadCommand = new RelayCommand(Load_Click);
             BtnGemCommand = new RelayCommand(Save_Click);
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

        public event
            PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
