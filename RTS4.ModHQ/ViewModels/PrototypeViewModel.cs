using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RTS4.Common;
using RTS4.Data;

namespace RTS4.ModHQ.ViewModels {
    public class PrototypeViewModel : PropertyModel {

        public string Name {
            get { return Source.Name; }
            //set { Source.Name = value; NotifyPropertyChanged("Name"); }
        }
        public int Id {
            get { return Source.Id; }
            //set { Source.Id = value; NotifyPropertyChanged("Id"); }
        }

        public int InitialHitPoints {
            get { return Source.HitPoints.Initial; }
            set { var hp = Source.HitPoints; hp.Initial = value; Source.HitPoints = hp; NotifyPropertyChanged("InitialHitPoints"); }
        }
        public int MaximumHitPoints {
            get { return Source.HitPoints.Maximum; }
            set { var hp = Source.HitPoints; hp.Maximum = value; Source.HitPoints = hp; NotifyPropertyChanged("MaximumHitPoints"); }
        }

        public float LOS {
            get { return Source.LineOfSight.ToFloat; }
            set { Source.LineOfSight = new XReal(value); }
        }

        bool hasTriedLoad = false;
        DDTImage image;
        ImageSource bmp;
        public ImageSource Icon {
            get {
                if (!hasTriedLoad && source.PortraitIcon != null) {
                    hasTriedLoad = true;
                    var entry = Overlord.TextureRegistry.SearchEntry(source.PortraitIcon);
                    if (entry != null) {
                        //@"icons\" + source.PortraitIcon
                        Overlord.TextureRegistry.GetWPFTexture(entry, (tex) => {
                            if (bmp != tex) {
                                bmp = tex;
                                NotifyPropertyChanged("Icon");
                            }
                        });
                    }
                }
                return bmp;
            }
        }

        public ObservableCollection<UnitFlag> Flags { get; private set; }
        public ObservableCollection<ActionViewModel> Actions { get; private set; }

        public Overlord Overlord { get; private set; }

        private UnitPrototype source;

        public PrototypeViewModel(Overlord Overlord) {
            this.Overlord = Overlord;
        }

        public UnitPrototype Source {
            get { return source; }
            set {
                if (source == value) return;
                source = value;
                if (Flags != null) Flags.Clear();
                else {
                    Flags = new ObservableCollection<UnitFlag>();
                    NotifyPropertyChanged("Flags");
                }
                if (Actions != null) Actions.Clear();
                else {
                    Actions = new ObservableCollection<ActionViewModel>();
                    NotifyPropertyChanged("Actions");
                }
                if (source != null) {
                    if (source.Flags != null) {
                        foreach (var flag in source.Flags) Flags.Add(flag);
                    }
                    if (source.Actions != null) {
                        foreach (var action in source.Actions) Actions.Add(new ActionViewModel() { Source = action });
                    }
                }
                bmp = null;
                image = null;
                hasTriedLoad = false;
                NotifyPropertyChanged("Name", "Id", "InitialHitPoints", "MaximumHitPoints", "LOS",  "Icon");
            }
        }

    }
}
