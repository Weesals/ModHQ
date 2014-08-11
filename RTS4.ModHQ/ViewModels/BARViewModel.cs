using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using Etier.IconHelper;
using RTS4.Data;

namespace RTS4.ModHQ.ViewModels {
    public class BARViewModel : PropertyModel {

        public class NamedItem : PropertyModel {
            public string Name { get; protected set; }
            public BARViewModel Owner { get; private set; }
            public virtual IEnumerable<NamedItem> Items { get { return null; } }
            public virtual ImageSource Icon { get { return null; } }

            public NamedItem(BARViewModel owner) { Owner = owner; }
        }

        public class FileViewModel : NamedItem {
            public BARFile.Entry Entry { get; private set; }
            public string Extension { get { return Path.GetExtension(Entry.Name); } }
            ImageSource img;
            private static ImageSource _xmlIcon;
            private static ImageSource _txtIcon;
            private static Dictionary<string, ImageSource> extensionIcons = new Dictionary<string, ImageSource>();
            public override ImageSource Icon {
                get {
                    if (img == null) {
                        switch (Extension.ToLower()) {
                            case ".btx": {
                                BTXImage btxImg = null;
                                var file = Owner.GetFileStream(Entry);
                                try {
                                    if (file != null) btxImg = BTXImage.Load(file);
                                } catch { }
                                if (btxImg != null) {
                                    var pixData = btxImg.Get32BitUncompressed();
                                    if (pixData != null)
                                        img = BitmapSource.Create(btxImg.Width, btxImg.Height, 96, 96, PixelFormats.Bgra32, null, pixData, btxImg.Width * 4);
                                }
                            } break;
                            case ".ddt": {
                                DDTImage ddtImg = null;
                                var file = Owner.GetFileStream(Entry);
                                try {
                                    if (file != null) ddtImg = DDTImage.Load(file);
                                } catch { }
                                if (ddtImg != null) {
                                    var pixData = ddtImg.Get32BitUncompressed();
                                    if (pixData != null)
                                        img = BitmapSource.Create(ddtImg.Width, ddtImg.Height, 96, 96, PixelFormats.Bgra32, null, pixData, ddtImg.Width * 4);
                                }
                            } break;
                            case ".xml":
                            case ".xmb": {
                                if (_xmlIcon == null) _xmlIcon = new BitmapImage(new Uri("/Images/Icons/XML80.png", UriKind.Relative));
                                img = _xmlIcon;
                            } break;
                            case ".txt": {
                                if (_txtIcon == null) _txtIcon = new BitmapImage(new Uri("/Images/Icons/TXT80.png", UriKind.Relative));
                                img = _txtIcon;
                            } break;
                            default: {
                                if (!extensionIcons.ContainsKey(Extension)) {
                                    var icon = IconReader.GetFileIcon(Name, IconReader.IconSize.Large, false);
                                    BitmapImage bmp = null;
                                    MemoryStream ms = new MemoryStream();
                                    icon.ToBitmap().Save(ms, ImageFormat.Png);
                                    ms.Position = 0;
                                    bmp = new BitmapImage();
                                    bmp.BeginInit();
                                    bmp.StreamSource = ms;
                                    bmp.EndInit();
                                    extensionIcons.Add(Extension, bmp);
                                }
                                img = extensionIcons[Extension];
                            } break;
                        }
                    }
                    return img;
                }
            }

            public FileViewModel(BARFile.Entry entry, BARViewModel owner) : base(owner) {
                Entry = entry;
                Name = Path.GetFileName(Entry.Name);
            }
        }
        public class FolderViewModel : NamedItem {
            public ObservableCollection<FolderViewModel> Folders { get; private set; }
            public ObservableCollection<FileViewModel> Files { get; private set; }
            private static ImageSource _folderIcon;
            public override ImageSource Icon {
                get {
                    if (_folderIcon == null) _folderIcon = new BitmapImage(new Uri("/Images/Icons/Folder80.png", UriKind.Relative));
                    return _folderIcon;
                }
            }

            public override IEnumerable<NamedItem> Items {
                get { return Folders.Select(f => (NamedItem)f).Concat(Files); }
            }

            public FolderViewModel(string name, BARViewModel owner) : base(owner) {
                Name = name;
                Folders = new ObservableCollection<FolderViewModel>();
                Files = new ObservableCollection<FileViewModel>();
            }
            public void AddFolder(FolderViewModel folder) {
                Folders.Add(folder);
            }
            public void AddFile(FileViewModel file) {
                Files.Add(file);
            }
        }

        public string Name { get; private set; }
        public FolderViewModel Root { get; private set; }

        public BARFile Source { get; private set; }

        private Dictionary<string, FolderViewModel> folders = new Dictionary<string, FolderViewModel>();

        public BARViewModel(BARFile file, string name) {
            Name = name;
            Source = file;
            Root = new FolderViewModel(name, this);
            if (file != null) {
                foreach (var subFile in file.Files) {
                    string directory = Path.GetDirectoryName(subFile.Name);
                    var folder = Root;
                    if (!string.IsNullOrEmpty(directory)) {
                        if (folders.ContainsKey(directory)) folder = folders[directory];
                        else {
                            string[] folderNames = directory.Split(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                            for (int f = 0; f < folderNames.Length; ++f) {
                                var folderName = folderNames[f];
                                var subFolder = folder.Folders.FirstOrDefault(subF => subF.Name == folderName);
                                if (subFolder == null) {
                                    subFolder = new FolderViewModel(folderName, this);
                                    folder.AddFolder(subFolder);
                                }
                                folder = subFolder;
                            }
                            // Doesnt add parent directories created, but idk if safe the
                            // directory name might not match.. might have a slash after
                            // or use the wrong slashes
                            folders.Add(directory, folder);
                        }
                    }
                    folder.AddFile(new FileViewModel(subFile, this));
                }
            }
        }

        public static string[] GetConversionExtensions(FileViewModel file) {
            if (file == null) return null;
            switch (file.Extension.ToLower()) {
                case ".xmb": return new[] { ".xml" };
                case ".ddt": return new[] { ".png", ".jpg", ".bmp", ".gif" };
                case ".btx": return new[] { ".png", ".jpg", ".bmp", ".gif" };
            }
            return new[] { file.Extension };
        }

        public void ProcessFolderTo(Func<FileViewModel, string, string> withFiles, FolderViewModel folder, string path) {
            foreach (var subfolder in folder.Folders) {
                string subfolderPath = Path.Combine(path, subfolder.Name);
                Directory.CreateDirectory(subfolderPath);
                ProcessFolderTo(withFiles, subfolder, subfolderPath);
            }
            foreach (var subfile in folder.Files) {
                var conversion = GetConversionExtensions(subfile);
                string subfilePath = Path.Combine(path, Path.GetFileNameWithoutExtension(subfile.Name) + conversion);
                withFiles(subfile, subfilePath);
            }
        }

        public Stream GetFileStream(BARFile.Entry entry) {
            return Source.GetFileStream(entry);
        }

        public static string CreateFilter(string[] extensions) {
            string filter = "";
            for (int e = 0; e < extensions.Length; ++e) {
                var extension = extensions[e];
                if (filter.Length > 0) filter += "|";
                switch (extension.ToLower()) {
                    case ".png": filter += "PNG Image|*.png"; break;
                    case ".jpg": filter += "JPG Image|*.jpg"; break;
                    case ".gif": filter += "GIF Image|*.gif"; break;
                    case ".bmp": filter += "Bitmap Image|*.bmp"; break;
                    case ".xml": filter += "XML File|*.xml"; break;
                    case ".xmb": filter += "Compiled XML|*.xmb"; break;
                    case ".ddt": filter += "RTS3 Texture Format|*.ddt"; break;
                    case ".btx": filter += "RTS3 Alpha Texture Format|*.btx"; break;
                    case ".txt": filter += "Plain Text|*.txt"; break;
                    default: filter += "Unknown " + extension + "|*" + extension; break;
                }
            }
            return filter;
        }

        public void SetFileData(FileViewModel file, byte[] newData) {
            Source.SetFileData(file.Entry, newData);
        }

        public string SaveFileTo(FileViewModel file, string path) {
            var stream = Source.GetFileStream(file.Entry);
            using (var outFile = File.Create(path)) {
                stream.CopyTo(outFile);
            }
            return path;
        }
        public string ConvertFileTo(FileViewModel file, string path) {
            try {
                switch (file.Extension.ToLower()) {
                    case ".xmb": {
                        var xdoc = XMBFile.Load(GetFileStream(file.Entry)).GetAsXDocument();
                        xdoc.Save(path);
                    } break;
                    case ".btx":
                    case ".ddt": {
                        RTS3Image srcImg = null;
                        if (file.Extension.ToLower() == ".ddt") {
                            srcImg = DDTImage.Load(GetFileStream(file.Entry));
                        } else if (file.Extension.ToLower() == ".btx") {
                            srcImg = BTXImage.Load(GetFileStream(file.Entry));
                        }
                        byte[] pixData = srcImg.Get32BitUncompressed();
                        if (pixData == null) return null;
                        var bmp = BitmapSource.Create(srcImg.Width, srcImg.Height, 96, 96, PixelFormats.Bgra32, null, pixData, srcImg.Width * 4);
                        BitmapEncoder encoder = null;
                        if (Path.GetExtension(path).ToLower() == ".png") encoder = new PngBitmapEncoder();
                        else if (Path.GetExtension(path).ToLower() == ".jpg") encoder = new JpegBitmapEncoder();
                        else if (Path.GetExtension(path).ToLower() == ".gif") encoder = new GifBitmapEncoder();
                        else if (Path.GetExtension(path).ToLower() == ".bmp") encoder = new BmpBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bmp));
                        using (var stream = File.Create(path)) {
                            encoder.Save(stream);
                        }
                    } break;
                    default: return null;
                }
            } catch {
                return null;
            }
            return path;
        }
        public byte[] UnConvertData(FileViewModel file, byte[] data) {
            switch (file.Extension.ToLower()) {
                case ".xmb": {
                    var xdoc = XDocument.Load(new MemoryStream(data));
                    var xmb = XMBFile.FromDocument(xdoc);
                    using (var stream = new MemoryStream()) {
                        xmb.Save(stream, true);
                        return stream.GetBuffer().Take((int)stream.Length).ToArray();
                    }
                }
                default: return null;
            }
        }
    }
}
