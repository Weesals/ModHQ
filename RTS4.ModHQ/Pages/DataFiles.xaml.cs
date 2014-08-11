using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using RTS4.ModHQ.ViewModels;
using Forms = System.Windows.Forms;

namespace RTS4.ModHQ.Pages {
    /// <summary>
    /// Interaction logic for DataFiles.xaml
    /// </summary>
    public partial class DataFiles : UserControl {

        public static readonly DependencyProperty OverlordProperty =
            DependencyProperty.Register("Overlord", typeof(Overlord), typeof(DataFiles), new PropertyMetadata(_OverlordChanged));
        public Overlord Overlord { get { return (Overlord)GetValue(OverlordProperty); } set { SetValue(OverlordProperty, value); } }

        public static readonly DependencyProperty BARFilesProperty = DependencyProperty.Register("BARFiles", typeof(BARsViewModel), typeof(DataFiles));
        public BARsViewModel BARFiles { get { return (BARsViewModel)GetValue(BARFilesProperty); } set { SetValue(BARFilesProperty, value); } }

        private bool isDirty = false;

        public DataFiles() {
            Resources.Add("ExtractItem", new DelegateCommand((obj) => {
                var items = ((System.Collections.IList)obj).Cast<BARViewModel.NamedItem>().ToArray();
                string path = AskForPathForItems(items, false);
                if (path != null) DoAction(SaveItem, items, path);
            }));
            Resources.Add("ConvertItem", new DelegateCommand((obj) => {
                var items = ((System.Collections.IList)obj).Cast<BARViewModel.NamedItem>().ToArray();
                string path = AskForPathForItems(items, true);
                if (path != null) DoAction((file, outPath) => {
                    string res = ConvertItem(file, outPath);
                    if (res == null) {
                        MessageBox.Show("Unable to convert " + file.Name);
                    }
                    return res;
                }, items, path);
            }));
            Resources.Add("ReplaceItem", new DelegateCommand((obj) => {
            }));
            Resources.Add("AddItem", new DelegateCommand((obj) => {
            }));

            IsVisibleChanged += (o, e) => {
                if (isDirty) UpdateBars();
            };

            InitializeComponent();
        }

        private string AskForPathForItems(BARViewModel.NamedItem[] item, bool doConversion) {
            if (item.Length == 0) return null;
            if (item.Length > 1 || item[0] is BARViewModel.FolderViewModel) {
                var folderBrowser = new Forms.FolderBrowserDialog();
                if (folderBrowser.ShowDialog() != Forms.DialogResult.OK) return null;
                return folderBrowser.SelectedPath + Path.DirectorySeparatorChar;
            } else if (item[0] is BARViewModel.FileViewModel) {
                var file = item[0] as BARViewModel.FileViewModel;
                string[] exts = new[] { file.Extension };
                if (doConversion) exts = BARViewModel.GetConversionExtensions(file);
                string prefferedOutName = Path.GetFileNameWithoutExtension(file.Name) + exts[0];
                var folderBrowser = new Forms.SaveFileDialog() {
                    FileName = prefferedOutName,
                    DefaultExt = exts[0],
                    Filter = BARViewModel.CreateFilter(exts),
                };
                if (folderBrowser.ShowDialog() != Forms.DialogResult.OK) return null;
                return folderBrowser.FileName;
            }
            return null;
        }
        private void DoAction(Func<BARViewModel.FileViewModel, string, string> with, BARViewModel.NamedItem[] items, string path) {
            for (int i = 0; i < items.Length; ++i) {
                var item = items[i];
                if (item is BARViewModel.FolderViewModel) {
                    var folder = item as BARViewModel.FolderViewModel;
                    if (path != null) folder.Owner.ProcessFolderTo(with, folder, path);
                } else if (item is BARViewModel.FileViewModel) {
                    var file = item as BARViewModel.FileViewModel;
                    if (path != null) with(file, path);
                }
            }
        }
        private string EnsureFilename(string path, BARViewModel.FileViewModel file, string newExt) {
            var lastC = path.LastOrDefault();
            if (lastC == Path.DirectorySeparatorChar || lastC == Path.AltDirectorySeparatorChar) {
                if (newExt == null) {
                    path += file.Name;
                } else {
                    path += Path.GetFileNameWithoutExtension(file.Name) + newExt;
                }
            }
            return path;
        }
        private void OverlordChanged(DependencyPropertyChangedEventArgs e) {
            UpdateBars();
        }
        private static void _OverlordChanged(object sender, DependencyPropertyChangedEventArgs e) {
            ((DataFiles)sender).OverlordChanged(e);
        }

        private void UpdateBars() {
            if (!IsVisible) {
                isDirty = true;
                return;
            }
            BARFiles = new BARsViewModel(Overlord);
            isDirty = false;
        }

        private BARViewModel.NamedItem[] GetItemsFromObject(object obj, out ListBoxItem listBoxItem, out ListBox listBox) {
            var element = (DependencyObject)obj;
            while (element != null && !(element is ListBoxItem)) element = VisualTreeHelper.GetParent(element);
            listBoxItem = element as ListBoxItem;
            if (listBoxItem == null) element = (DependencyObject)obj;       // We could already be at the listbox
            while (element != null && !(element is ListBox)) element = VisualTreeHelper.GetParent(element);
            listBox = element as ListBox;
            return listBox.SelectedItems.Cast<BARViewModel.NamedItem>().ToArray();
        }

        private void BeginDrag(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                ListBoxItem element;
                ListBox listBox;
                var items = GetItemsFromObject(e.OriginalSource, out element, out listBox);

                List<string> tmpFiles = new List<string>();
                foreach (var item in items) {
                    if (item is BARViewModel.FileViewModel) {
                        var file = item as BARViewModel.FileViewModel;
                        bool wasConverted;
                        var tmpFile = AttemptConvertOrSave(file, out wasConverted);
                        tmpFiles.Add(tmpFile);
                    }
                }
                if (tmpFiles.Count > 0) {
                    var dataObject = new DataObject(DataFormats.FileDrop, tmpFiles.ToArray());
                    DragDrop.DoDragDrop(element, dataObject, DragDropEffects.Move);
                }
                e.Handled = true;
            }
        }

        private void ListboxClicked(object sender, MouseButtonEventArgs e) {
            ListBoxItem element;
            ListBox listBox;
            var items = GetItemsFromObject(e.OriginalSource, out element, out listBox);

            if (element != null) {
                var oldSelection = (element.IsSelected ? items : null);
                MouseEventHandler beginDrag = null;
                MouseButtonEventHandler endClick = null;
                beginDrag = (o2, e2) => {
                    if (oldSelection != null) {
                        listBox.SelectedItems.Clear();
                        foreach (var obj in oldSelection) listBox.SelectedItems.Add(obj);
                        oldSelection = null;
                    }
                    BeginDrag(o2, e2);
                    endClick(o2, null);
                };
                endClick = (o2, e2) => {
                    listBox.MouseMove -= beginDrag;
                    listBox.MouseUp -= endClick;
                };
                listBox.MouseMove += beginDrag;
                listBox.MouseUp += endClick;
            }
        }

        private static Dictionary<Data.BARFile.Entry, FileSystemWatcher> watchers = new Dictionary<Data.BARFile.Entry, FileSystemWatcher>();
        private void OpenItem(object sender, MouseEventArgs e) {
            ListBoxItem element;
            ListBox listBox;
            var items = GetItemsFromObject(sender, out element, out listBox);
            foreach (var item in items) {
                if (item is BARViewModel.FileViewModel) {
                    var file = item as BARViewModel.FileViewModel;
                    bool wasConverted;
                    var tmpFile = AttemptConvertOrSave(file, out wasConverted);

                    Process.Start(tmpFile);
                    if (!watchers.ContainsKey(file.Entry)) {
                        FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(tmpFile));
                        watcher.NotifyFilter = NotifyFilters.LastWrite;
                        watcher.Filter = Path.GetFileName(tmpFile);
                        watcher.Changed += (o2, e2) => {
                            if (e2.ChangeType == WatcherChangeTypes.Changed) {
                                watcher.EnableRaisingEvents = false;
                                byte[] tmpdata = null;
                                while (true) {
                                    try {
                                        using (var stream = File.OpenRead(tmpFile)) {
                                            tmpdata = new byte[stream.Length];
                                            stream.Read(tmpdata, 0, tmpdata.Length);
                                            if (wasConverted) {
                                                tmpdata = UnconvertData(file, tmpdata);
                                            }
                                        }
                                        break;
                                    } catch (IOException) {
                                        Thread.Sleep(100);
                                        continue;
                                    }
                                }
                                if (tmpdata != null) {
                                    Dispatcher.BeginInvoke((Action)delegate {
                                        Thread.Sleep(100);
                                        file.Owner.SetFileData(file, tmpdata);
                                        watcher.EnableRaisingEvents = true;
                                    });
                                } else {
                                    MessageBox.Show("Unable to update file");
                                }
                            }
                        };
                        watcher.EnableRaisingEvents = true;
                        watchers.Add(file.Entry, watcher);
                    }
                } else {
                    MessageBox.Show("Unsupported :(. Please let me know if you want this feature!");
                }
            }
        }

        private string AttemptConvertOrSave(BARViewModel.FileViewModel file, out bool wasConverted) {
            wasConverted = true;
            string tmpFile = ConvertItem(file, Path.GetTempPath());
            if (tmpFile == null) {
                tmpFile = SaveItem(file, Path.GetTempPath());
                wasConverted = false;
            }
            return tmpFile;
        }


        private string SaveItem(BARViewModel.FileViewModel file, string path) {
            path = EnsureFilename(path, file, null);
            return file.Owner.SaveFileTo(file, path);
        }
        private string ConvertItem(BARViewModel.FileViewModel file, string path) {
            var conversions = BARViewModel.GetConversionExtensions(file);
            if (conversions == null) return null;
            path = EnsureFilename(path, file, conversions[0]);
            return file.Owner.ConvertFileTo(file, path);
        }
        private byte[] UnconvertData(BARViewModel.FileViewModel file, byte[] tmpdata) {
            return file.Owner.UnConvertData(file, tmpdata);
        }

    }
}
