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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RTS4.ModHQ.Controls;
using RTS4.ModHQ.UI;
using RTS4.ModHQ.UI.Views;
using RTS4.ModHQ.ViewModels;

namespace RTS4.ModHQ.Pages {
    /// <summary>
    /// Interaction logic for UIEditor.xaml
    /// </summary>
    public partial class UIEditor : UserControl {
        public static string[] AllUiFiles = new[] {
            "uilaunchscreen", "uimain", "uipregamenew", "uipostgamescreen", "uioptions", "uieditormenu", "uitriggereditor", "uitriggroupeditor", "uicampaign", "uicampaigneditor", "uicampaignloading", "uieula", "uigamelist", "uihostgamedialog",
            "uipregame", "uipregame_alpha",
        };

        public class GadgetViewModel : PropertyModel {

            public GadgetView View { get; set; }
            public Gadget Gadget { get { return View.Gadget; } }
            public bool Visible { get { return !Gadget.Hidden; } set { Gadget.Hidden = !value; } }

            public GadgetViewModel[] Children { get; set; }

            public GadgetViewModel(GadgetView view) {
                View = view;
            }

        }
        public class MainViewModel : PropertyModel {

            public readonly UIEditor Editor;

            private Size resolution;
            public Size Resolution { get { return resolution; } set { ChangeProperty("Resolution", ref resolution, value); } }

            private Size[] resolutions = new[] { new Size(1024, 768), new Size(1280, 768), new Size(1920, 1080), };
            public Size[] Resolutions { get { return resolutions; } }

            private string uiFile = "uimain";
            public string UIFile { get { return uiFile; } set { ChangeProperty("UIFile", ref uiFile, value); } }

            private string[] uiFiles;
            public string[] UIFiles { get { return uiFiles; } }

            public GadgetViewModel[] AllGadgets { get; set; }
            public GadgetViewModel[] ToggleableGadgets { get; set; }
            public GadgetViewModel[] TopGadgets { get; set; }

            private GadgetViewModel selectedGadget;
            public GadgetViewModel SelectedGadget {
                get { return selectedGadget; }
                set { ChangeProperty("SelectedGadget", ref selectedGadget, value); }
            }

            public MainViewModel(UIEditor editor) {
                Editor = editor;
            }
            public void NotifyDataChanged() {
                AllGadgets = Editor.Views.Select(g => new GadgetViewModel(g)).ToArray();
                foreach (var gadget in AllGadgets) {
                    gadget.Children = AllGadgets.Where(g => g.Gadget.Parent == gadget.Gadget).ToArray();
                }
                ToggleableGadgets = AllGadgets.Where(v => v.Gadget.Children != null).ToArray();
                TopGadgets = AllGadgets.Where(v => v.Gadget.Parent == null).ToArray();
                NotifyPropertyChanged("AllGadgets", "ToggleableGadgets", "TopGadgets");
            }

            public void LoadUIFileList() {
                var sources = new[] {
                    Editor.Directory.GetFile(AOMDirectory.Data2BAR),
                    Editor.Directory.GetFile(AOMDirectory.DataBAR)
                };
                uiFiles = sources.
                    Where(s => s != null).
                    SelectMany(s => s.FilesNames.Where(f =>
                        f.StartsWith("ui", StringComparison.OrdinalIgnoreCase) &&
                        (f.EndsWith("xmb", StringComparison.OrdinalIgnoreCase) || f.EndsWith("xml", StringComparison.OrdinalIgnoreCase))
                    )).
                    Select(f => System.IO.Path.GetFileNameWithoutExtension(f)).
                    Distinct().
                    ToArray();
                /*uiFiles = AllUiFiles.Where(f =>
                    Editor.Directory.XMBExists(AOMDirectory.Data2BAR, f + ".xml") ||
                    Editor.Directory.XMBExists(AOMDirectory.Data2BAR, f + ".xmb") ||
                    Editor.Directory.XMBExists(AOMDirectory.DataBAR, f + ".xml") ||
                    Editor.Directory.XMBExists(AOMDirectory.DataBAR, f + ".xmb")
                ).ToArray();*/
                NotifyPropertyChanged("UIFiles");
            }
        }

        public static readonly DependencyProperty OverlordProperty =
            DependencyProperty.Register("Overlord", typeof(Overlord), typeof(UIEditor), new PropertyMetadata(_OverlordChanged));
        public Overlord Overlord { get { return (Overlord)GetValue(OverlordProperty); } set { SetValue(OverlordProperty, value); } }
        private static void _OverlordChanged(object sender, DependencyPropertyChangedEventArgs e) {
            ((UIEditor)sender).Load();
        }
        public AOMDirectory Directory { get { return Overlord != null ? Overlord.Directory : null; } }
        public Layout Layout;

        public WindowSpace Space;
        public GadgetView[] Views;

        public MainViewModel ViewModel { get; set; }

        public UndoManager UndoManager;
        public ResizeController ResizeControls;

        private bool dirty = false;

        public UIEditor() {
            Space = new WindowSpace();

            UndoManager = new UndoManager();

            ViewModel = new MainViewModel(this);
            ViewModel.Resolution = new Size(1024, 768);
            ViewModel.OnPropertyChanged(ViewModel_SelectedGadget, "SelectedGadget");
            ViewModel.OnPropertyChanged(ViewModel_ChangeFile, "UIFile");

            InitializeComponent();

            ViewsContainer.SizeChanged += ViewsContainer_SizeChanged;
            ViewsContainer.MouseDown += ViewsContainer_MouseDown;

            IsVisibleChanged += (o, e) => {
                if (dirty) Load();
            };

            KeyDown += MainWindow_KeyDown;
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Z & (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) {
                UndoManager.Undo();
            }
        }

        private void ViewModel_ChangeFile(object obj) {
            Load();
        }
        public void Load() {
            if (ViewModel == null) return;
            if (!IsVisible) {
                dirty = true;
                return;
            }
            Layout = null;
            ViewModel.LoadUIFileList();
            Data.XMBFile uiFile = null;
            if(Directory != null) {
                if (uiFile == null) try { uiFile = Directory.LoadXMB(AOMDirectory.Data2BAR, ViewModel.UIFile + ".xml"); } catch { }
                if (uiFile == null) try { uiFile = Directory.LoadXMB(AOMDirectory.Data2BAR, ViewModel.UIFile + ".xmb"); } catch { }
                if (uiFile == null) try { uiFile = Directory.LoadXMB(AOMDirectory.DataBAR, ViewModel.UIFile + ".xml"); } catch { }
                if (uiFile == null) try { uiFile = Directory.LoadXMB(AOMDirectory.DataBAR, ViewModel.UIFile + ".xmb"); } catch { }
            }
            var xdoc = uiFile != null ? uiFile.GetAsXDocument() : null;
            if (xdoc != null) Layout = GadgetSerializer.Parse(xdoc.Root);
            if (Layout == null) return;

            Views = Layout.Gadgets.
                SelectManyRecursive(g => g.Children).
                Select(g => new GadgetView(Overlord.TextureRegistry, g, Space)).
                ToArray();
            foreach (var view in Views) {
                view.ChildViews = Views.Where(v => v.Gadget.Parent == view.Gadget).ToArray();
            }
            ViewsContainer.Children.Clear();
            foreach (var view in Views) {
                ViewsContainer.Children.Add(view);
            }
            SidePanel.IsExpanded = true;
            LayoutRoot.Visibility = Visibility.Visible;
            ViewModel.NotifyDataChanged();
            dirty = false;
        }

        private void ViewModel_SelectedGadget(object obj) {
            var selected = ViewModel.SelectedGadget;
            if (ResizeControls != null) {
                ControlsContainer.Children.Remove(ResizeControls);
                ResizeControls = null;
            }
            if (selected != null) {
                ResizeControls = new ResizeController(UndoManager, selected.View);
                ControlsContainer.Children.Add(ResizeControls);
            }
        }

        void ViewsContainer_MouseDown(object sender, MouseButtonEventArgs e) {
            var source = e.OriginalSource as FrameworkElement;
            while (source != null && !(source is GadgetView)) {
                if (source is ResizeController) return;
                source = VisualTreeHelper.GetParent(source) as FrameworkElement;
            }
            var view = source as GadgetView;
            var viewModel = view != null ? ViewModel.AllGadgets.FirstOrDefault(v => v.View == view) : null;
            ViewModel.SelectedGadget = viewModel;
        }

        public void ViewsContainer_SizeChanged(object sender, SizeChangedEventArgs e) {
            Space.Width = (float)e.NewSize.Width;
            Space.Height = (float)e.NewSize.Height;
            if (Views != null) {
                foreach (var view in Views) view.Arrange();
            }
        }

    }
}
