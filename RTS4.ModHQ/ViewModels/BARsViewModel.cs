using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS4.ModHQ.ViewModels {
    public class BARsViewModel : PropertyModel {

        public ObservableCollection<BARViewModel> Files { get; set; }

        public Overlord Overlord { get; private set; }

        public BARsViewModel(Overlord overlord) {
            Overlord = overlord;
            Files = new ObservableCollection<BARViewModel>();
            Overlord.AddLoadedCallback(OnLoaded, true);
        }

        public void OnLoaded() {
            Files.Clear();
            Files.Add(new BARViewModel(Overlord.DataFile, "data.bar"));
            Files.Add(new BARViewModel(Overlord.Data2File, "data2.bar"));
            Files.Add(new BARViewModel(Overlord.TextureFile, "textures.bar"));
            Files.Add(new BARViewModel(Overlord.Texture2File, "textures2.bar"));
            Files.Add(new BARViewModel(Overlord.SoundsFile, "sounds.bar"));
            Files.Add(new BARViewModel(Overlord.Sounds2File, "sounds2.bar"));
        }

    }
}
