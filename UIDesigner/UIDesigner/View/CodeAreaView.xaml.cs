using System;
using System.Collections.Generic;
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
using JsonToDML;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace UIDesigner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DMLUIElementList ui;        //List of objects i.e. UI Elements
        DMLUIElement ue;            //Temporary object of UI Element that will be initialized for each row in  json file
        public MainWindow()
        {
            InitializeComponent();
            codeArea.Text = "test";
            CreateFileWatcher();
        }
        public void CreateFileWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = "c:\\temp\\";
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.json";

            watcher.Changed += new FileSystemEventHandler(OnChanged);
            //watcher.Created += new FileSystemEventHandler(OnChanged);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

            watcher.EnableRaisingEvents = true;
        }
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            writeDML();
        }
        public void writeDML()
        {
            string json;
            try
            {
                using (StreamReader r = new StreamReader("c:\\temp\\UIDesign.json"))
                {
                    json = r.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return;
            }

            ui = new DMLUIElementList();
            //delete the previous list and create a new one for any json file change
            //can be optimized

            dynamic dynObj = JsonConvert.DeserializeObject(json);

            foreach (var data in dynObj.UIElements)
            {       
                string dmlKeyword = data.DMLKeyword;
                switch (dmlKeyword)
                {
                    case DMLUIElementSyntax.InputBlock: ue = new InputBlock();
                        break;
                    case DMLUIElementSyntax.OutputBlock: ue = new OutputBlock();
                        break;
                    case DMLUIElementSyntax.Text: ue = new JsonToDML.Text();
                        break;
                    case DMLUIElementSyntax.Line: ue = new JsonToDML.Line();
                        break;
                    case DMLUIElementSyntax.ItemBlock: ue = new ItemBlock();
                        break;
                    case DMLUIElementSyntax.MenuBlock: ue = new MenuBlock();
                        break;
                    case DMLUIElementSyntax.PauseBlock: ue = new PauseBlock();
                        break;
                    case DMLUIElementSyntax.YesNoBlock: ue = new YesNoBlock();
                        break;
                }

                ue.setMembers(data);
                ue.qualifiers.setMembers(data);
                
                ui.addUIElementToUIElementList(ue);
            }
            refreshDisplayedDMLCode();
        }

        public void refreshDisplayedDMLCode()
        {
            GenerateDMLCode genDC;
            Dispatcher.BeginInvoke(new Action(delegate
            {
                codeArea.Text = "";
                foreach (var ob in ui.UIL)
                {
                    genDC = new GenerateDMLCode(ob);
                    codeArea.Text += genDC.serializeDML();
                }
            }));   
        }
    }
}
