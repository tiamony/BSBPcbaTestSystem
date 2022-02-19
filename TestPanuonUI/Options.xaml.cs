using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Panuon.UI.Silver;
using System.IO;
using XmlDeserializeSpace;

namespace BsbPcbaTestSpace
{
    /// <summary>
    /// Options.xaml 的交互逻辑
    /// </summary>
    public partial class Options : WindowX
    {
        public OptionsDataModel ODM = new OptionsDataModel();
        private string OptionsDataPath = Directory.GetCurrentDirectory() + @"\config.xml";
        List<ComPortCombo> comboxConent = new List<ComPortCombo>();
        public delegate void LoadConfigHandler();

        public Options(OptionsDataModel odm )
        {
            InitializeComponent();
            for (int i= 1; i<20; i++)
            comboxConent.Add(new ComPortCombo { ID = i, Name = "COM"+i });
            cmb_list.ItemsSource = comboxConent;
            this.ODM = odm;
            //this.OptionInit();
            foreach (ComPortCombo c in comboxConent)
            {
                if (c.Name == ODM.ComPort) this.cmb_list.SelectedIndex = c.ID - 1;
            }
            this.DataContext = this.ODM;
        }
        private void OptionInit()
        {
            try
            {
                if (File.Exists(OptionsDataPath))
                {
                    ODM = (XmlSerializa.Deserialize(typeof(OptionsDataModel), OptionsDataPath) as OptionsDataModel);
                }
                else XmlSerializa.CreateXML(ODM, OptionsDataPath);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public event LoadConfigHandler LoadConfigEvent;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            XmlSerializa.CreateXML(ODM, OptionsDataPath);
            LoadConfigEvent?.Invoke();
            this.Close();
        }

        private void Se_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cmb_list.SelectedIndex > -1)
            {
                ODM.ComPort = comboxConent[this.cmb_list.SelectedIndex].Name;
            }
        }

    }
}
