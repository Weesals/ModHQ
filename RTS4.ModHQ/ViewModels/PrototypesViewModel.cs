using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Data;

namespace RTS4.ModHQ.ViewModels {
    public class PrototypesViewModel : PropertyModel {

        public class PrototypeFilter {
            public string Name { get; private set; }
            public Func<UnitPrototype, bool> Test { get; private set; }

            public PrototypeFilter(string name, Func<UnitPrototype, bool> test) {
                Name = name;
                Test = test;
            }
        }

        public Overlord Overlord { get; private set; }

        public PrototypesViewModel(Overlord overlord) {
            Overlord = overlord;
            Overlord.PropertyChanged += (o, e) => {
                if (e.PropertyName == "Prototypes") {
                    Source = Overlord.Prototypes;
                }
            };

            Prototypes = new ObservableCollection<PrototypeViewModel>();
            PrototypeFilters = new ObservableCollection<PrototypeFilter>();
            FilteredPrototypes = new ObservableCollection<PrototypeViewModel>();

            PrototypeFilters.Add(new PrototypeFilter("All", (prototype) => { return true; }));
            PrototypeFilters.Add(new PrototypeFilter("Buildings", (prototype) => { return prototype.Movement.MaxVelocity == 0; }));
            PrototypeFilters.Add(new PrototypeFilter("Units", (prototype) => { return prototype.Movement.MaxVelocity > 0; }));

            PropertyChanged += (sender, e) => { if (e.PropertyName == "SelectedFilter") UpdateFilteredPrototypes(); };
            Prototypes.CollectionChanged += delegate { UpdateFilteredPrototypes(); };
            SelectedFilter = PrototypeFilters.FirstOrDefault();

            Source = Overlord.Prototypes;
        }

        private void UpdateFilteredPrototypes() {
            int inF = 0;
            for (int p = 0; p < Prototypes.Count; ++p) {
                if (!SelectedFilter.Test(Prototypes[p].Source)) continue;
                if (inF < FilteredPrototypes.Count) {
                    if (FilteredPrototypes[inF] != Prototypes[p]) {
                        while (inF >= FilteredPrototypes.Count) FilteredPrototypes.RemoveAt(inF);
                    }
                }
                if (inF >= FilteredPrototypes.Count) FilteredPrototypes.Add(Prototypes[p]);
                ++inF;
            }
        }

        private PrototypeList source;
        public PrototypeList Source {
            get { return source; }
            set {
                if (source == value) return;
                source = value;
                Prototypes.Clear();
                if (source != null) {
                    foreach (var prototype in source.Prototypes) {
                        Prototypes.Add(new PrototypeViewModel(Overlord) {
                            Source = prototype as UnitPrototype,
                        });
                    }
                }
            }
        }

        public ObservableCollection<PrototypeViewModel> Prototypes { get; private set; }

        public ObservableCollection<PrototypeFilter> PrototypeFilters { get; private set; }
        public PrototypeFilter selectedFilter;
        public PrototypeFilter SelectedFilter {
            get { return selectedFilter; }
            set {
                if (selectedFilter == value) return;
                selectedFilter = value;
                NotifyPropertyChanged("SelectedFilter");
            }
        }
        public ObservableCollection<PrototypeViewModel> FilteredPrototypes { get; private set; }

        public override string ToString() {
            return "ASDFasdfasdfasdfasdfasdfasdf";
        }

    }
}
