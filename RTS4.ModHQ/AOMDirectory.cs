using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTS4.Data;
using System.IO;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace RTS4.ModHQ {
    public class AOMDirectory {
        public static readonly string[] InstallDirectories = new[] {
            @"Program Files\Microsoft Games\Age of Mythology\",
            @"Program Files\Games\Age of Mythology\",
            @"Program Files\Age of Mythology\",
            @"Program Files (x86)\Microsoft Games\Age of Mythology\",
            @"Program Files (x86)\Games\Age of Mythology\",
            @"Program Files (x86)\Age of Mythology\",
            @"Games\Age of Mythology",
            @"Games\Age of Mythology - Voobly",
            // Add your directory here?
        };
        public static readonly string TexturesBAR = @"textures\textures.bar";
        public static readonly string Textures2BAR = @"textures\textures2.bar";
        public static readonly string DataBAR = @"data\data.bar";
        public static readonly string Data2BAR = @"data\data2.bar";
        public static readonly string SoundsBAR = @"sound\sounds.bar";
        public static readonly string Sounds2BAR = @"sound\sounds2.bar";

        public string InstallPath;

        public bool Loaded { get { return InstallPath != null; } }

        Dictionary<string, BARFile> barCache = new Dictionary<string, BARFile>();

        public ObservableCollection<string> InstallLocations { get; private set; }

        public AOMDirectory() {
            InstallLocations = new ObservableCollection<string>();
        }

        public void Enumerate() {
            InstallLocations.Clear();
            // Search through each drive on the PC
            foreach (var drive in DriveInfo.GetDrives()) {
                if (drive.DriveType == DriveType.Fixed && drive.IsReady) {
                    // Search through each possible directory for each drive
                    foreach (var tdir in InstallDirectories) {
                        var dir = Path.Combine(drive.RootDirectory.FullName, tdir);
                        if (Directory.Exists(dir))
                            InstallLocations.Add(dir);
                    }
                }
            }
        }

        public BARFile GetFile(string name) {
            BARFile file;
            lock (barCache) {
                if (!barCache.TryGetValue(name, out file)) barCache.Add(name, file = BARFile.Load(Path.Combine(InstallPath, name)));
            }
            return file;
        }

        public bool Load(string installDir) {
            if (Directory.Exists(installDir))
                InstallPath = installDir;

            return Loaded;
        }

        public XMBFile LoadXMB(string barPath, string fileName) {
            var extFile = Path.Combine(Path.GetDirectoryName(barPath), fileName);
            try {
                if (File.Exists(extFile)) {
                    using (var file = File.OpenRead(extFile)) {
                        return XMBFile.Load(file);
                    }
                }
            } catch { }
            {
                var barFile = GetFile(barPath);
                if (barFile == null) return null;
                var file = barFile.GetFileStream(fileName);
                if (file == null) return null;
                return XMBFile.Load(file);
            }
        }
        public bool XMBExists(string barPath, string fileName) {
            var extFile = Path.Combine(Path.GetDirectoryName(barPath), fileName);
            if (File.Exists(extFile)) return true;
            var barFile = GetFile(barPath);
            if (barFile == null) return false;
            var file = barFile.GetFileStream(fileName);
            return file != null;
        }

        public override string ToString() { return InstallPath; }

    }
}
