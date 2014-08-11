using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using RTS4.Data;
using RTS4.ModHQ.ViewModels;

namespace RTS4.ModHQ {
    /// <summary>
    /// Interaction logic for UnitPrototypeSettings.xaml
    /// </summary>
    public partial class UnitPrototypeSettings : Window, INotifyPropertyChanged {

        public PrototypeViewModel prototype;
        public PrototypeViewModel Prototype {
            get { return prototype; }
            set {
                if (prototype == value) return;
                prototype = value;
                NotifyPropertyChanged("Prototype");
            }
        }

        public UnitPrototypeSettings() {
            Resources.Add("Self", this);
            InitializeComponent();
        }

        public void NotifyPropertyChanged(string propertyName) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
