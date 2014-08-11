using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Microsoft.Win32;
using RTS4.Data;
using RTS4.Data.Lists;
using RTS4.Data.Resources;
using RTS4.Data.Serialization;
using RTS4.ModHQ.ViewModels;
using WinForms = System.Windows.Forms;

namespace RTS4.ModHQ {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public DependencyProperty MessagesProperty = DependencyProperty.Register("Messages", typeof(string), typeof(MainWindow));

        public static readonly DependencyProperty OverlordProperty =
            DependencyProperty.Register("Overlord", typeof(Overlord), typeof(MainWindow));
        public Overlord Overlord { get { return (Overlord)GetValue(OverlordProperty); } set { SetValue(OverlordProperty, value); } }
        //public Overlord Overlord { get; set; }

        public AOMDirectory AOMDirectory { get; private set; }

        public MainWindow() {
            Overlord = new Overlord();
            AOMDirectory = new AOMDirectory();

            Resources.Add("Self", this);
            /*Resources.Add("OpenCommand", new DelegateCommand((sender) => {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                var result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK) {
                    LoadAOMDirectory(dialog.SelectedPath);
                }
            }));
            Resources.Add("SaveCommand", new DelegateCommand((sender) => {
                MessageBox.Show("Saving doesnt work yet");
                //var dialog = new SaveFileDialog() { };
                //dialog.ShowDialog();
            }));
            Resources.Add("PrototypeImageCommand", new DelegateCommand((prototype) => {
                var unitSettings = new UnitPrototypeSettings() {
                    Prototype = prototype as PrototypeViewModel,
                };
                unitSettings.Show();
            }));*/
            Resources.Add("CloseCommand", new DelegateCommand((sender) => { this.Close(); }));

            InitializeComponent();

            //SetValue(PrototypesProperty, protoXml);

            Loaded += delegate {
                AOMDirectory.Enumerate();
                //dir.Load();
                //LoadAOMDirectory(dir);
            };

            /*KeyDown += (s, e) => {
                if (e.Key >= Key.D0 && e.Key <= Key.D9) {
                    SetValue(SelectedItemProperty, protoXml.Prototypes[(int)e.Key - (int)Key.D0]);
                }
            };*/

            //DwmDropShadow.DropShadowToWindow(this);
        }

        private void LocateDir_Click(object sender, RoutedEventArgs e) {
            var fileDialog = new WinForms.OpenFileDialog() {
                CheckFileExists = false,
                FileName = "Folder",
                Filter = "Folder | (*.folder)",
                ValidateNames = false,
            };
            if (fileDialog.ShowDialog() == WinForms.DialogResult.OK) {
                var path = System.IO.Path.GetDirectoryName(fileDialog.FileName);
                if (AOMDirectory.Load(path)) LoadAOMDirectory(AOMDirectory);
            }
        }
        private void LoadDir_Click(object sender, RoutedEventArgs e) {
            var dir = ((Button)sender).DataContext as string;
            if (AOMDirectory.Load(dir)) LoadAOMDirectory(AOMDirectory);
        }

        private void LoadAOMDirectory(AOMDirectory dir) {
            //LoadBar.IsIndeterminate = true;
            //LoadBar.Visibility = Visibility.Visible;
            if (dir.Loaded) {
                TryingLoad.Visibility = Visibility.Visible;
                NotLoaded.Visibility = Visibility.Collapsed;
                Overlord.FromAOMDirectory(dir, (log) => {
                    string errors = log.ErrorsAsString, warnings = log.WarningsAsString, infos = log.InfosAsString;
                    if (!string.IsNullOrWhiteSpace(errors) || !string.IsNullOrWhiteSpace(warnings) || !string.IsNullOrWhiteSpace(infos)) {
                        string msg = "Load complete." +
                            (!string.IsNullOrWhiteSpace(errors) ? "\n\nErrors:\n" + errors : "") +
                            (!string.IsNullOrWhiteSpace(warnings) ? "\n\nWarnings:\n" + warnings : "") +
                            (!string.IsNullOrWhiteSpace(infos) ? "\n\nInfo:\n" + infos : "");
                        SetValue(MessagesProperty, msg);
                        Debug.WriteLine("Loaded AOM data from " + dir);
                    }
                    //LoadBar.IsIndeterminate = false;
                    //LoadBar.Visibility = Visibility.Hidden;
                    LoadError.Visibility = Visibility.Collapsed;
                    WindowContent.Visibility = Visibility.Visible;
                });
            }
            //((Storyboard)FileSourceGrid.Resources["FileSourceGridFlash"]).Stop();
        }

        private void StartDrag(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

    }
}
