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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RTS4.ModHQ.ViewModels;

namespace RTS4.ModHQ.Pages {
    /// <summary>
    /// Interaction logic for PrototypesPage.xaml
    /// </summary>
    public partial class PrototypesPage : UserControl {

        public static readonly DependencyProperty OverlordProperty =
            DependencyProperty.Register("Overlord", typeof(Overlord), typeof(PrototypesPage), new PropertyMetadata(_OverlordChanged));
        public Overlord Overlord { get { return (Overlord)GetValue(OverlordProperty); } set { SetValue(OverlordProperty, value); } }

        public static readonly DependencyProperty PrototypesProperty = DependencyProperty.Register("Prototypes", typeof(PrototypesViewModel), typeof(PrototypesPage));
        public PrototypesViewModel Prototypes { get { return (PrototypesViewModel)GetValue(PrototypesProperty); } set { SetValue(PrototypesProperty, value); } }

        public PrototypesPage() {
            Resources.Add("Self", this);
            Resources.Add("OpenCommand", new DelegateCommand((sender) => {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                var result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK) {
                    var aomDir = new AOMDirectory();
                    aomDir.Load(dialog.SelectedPath);
                    Overlord.FromAOMDirectory(aomDir, (log) => { });
                }
            }));
            Resources.Add("SaveCommand", new DelegateCommand((sender) => {
                MessageBox.Show("Saving doesnt work yet");
                /*var dialog = new SaveFileDialog() { };
                dialog.ShowDialog();*/
            }));
            Resources.Add("CloseCommand", new DelegateCommand((sender) => { }));
            Resources.Add("PrototypeImageCommand", new DelegateCommand((prototype) => {
                var unitSettings = new UnitPrototypeSettings() {
                    Prototype = prototype as PrototypeViewModel,
                };
                unitSettings.Show();
            }));

            InitializeComponent();

            /*if (string.IsNullOrWhiteSpace(Overlord.Directory)) {
                ((Storyboard)FileSourceGrid.Resources["FileSourceGridFlash"]).Begin();
            }*/
        }

        private void OverlordChanged(DependencyPropertyChangedEventArgs e) {
            Prototypes = new PrototypesViewModel(Overlord);
        }
        private static void _OverlordChanged(object sender, DependencyPropertyChangedEventArgs e) {
            ((PrototypesPage)sender).OverlordChanged(e);
        }
    }
}
