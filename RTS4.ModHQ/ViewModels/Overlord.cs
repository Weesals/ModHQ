using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using RTS4.Data;
using RTS4.Data.Lists;
using RTS4.Data.Resources;
using RTS4.Data.Serialization;
using RTS4.ModHQ.UI;

namespace RTS4.ModHQ.ViewModels {
    public class Overlord : PropertyModel {

        public class LogCapture : ILogService {
            public List<string> errors = new List<string>();
            public List<string> warnings = new List<string>();
            public List<string> infos = new List<string>();
            public void Error(string error) { errors.Add(error); }
            public void Warning(string warning) { warnings.Add(warning); }
            public void Info(string info) { infos.Add(info); }

            public string ErrorsAsString {
                get { return (errors.Count > 0 ? errors.Aggregate((e1, e2) => e1 + "\n" + e2) : ""); }
            }
            public string WarningsAsString {
                get { return (warnings.Count > 0 ? warnings.Aggregate((e1, e2) => e1 + "\n" + e2) : ""); }
            }
            public string InfosAsString {
                get { return (infos.Count > 0 ? infos.Aggregate((e1, e2) => e1 + "\n" + e2) : ""); }
            }

            public void Clear() { errors.Clear(); warnings.Clear(); infos.Clear(); }
        }

        private AOMDirectory directory;
        public AOMDirectory Directory {
            get { return directory; }
            private set { if (directory == value) return; directory = value; NotifyPropertyChanged("Directory"); }
        }
        public SerializationContext SerializationContext { get; private set; }

        public BARFile DataFile;
        public BARFile Data2File;
        public BARFile TextureFile;
        public BARFile Texture2File;
        public BARFile SoundsFile;
        public BARFile Sounds2File;

        public TextureRegistry TextureRegistry;

        private PrototypeList prototypes;
        public PrototypeList Prototypes {
            get { return prototypes; }
            private set { if (prototypes == value) return; prototypes = value; NotifyPropertyChanged("Prototypes"); }
        }

        public bool IsLoaded { get { return loaded; } }
        public bool IsNotLoaded { get { return !IsLoaded; } }

        private Action OnLoad;
        private bool loaded = false;

        public Overlord() {
            SerializationContext = new SerializationContext();
            SerializationContext.AddService(new FlagRegistry());
            SerializationContext.AddService(new ResourceRegistry("Food", "Wood", "Stone", "Gold", "Favor"));
            var logger = new Overlord.LogCapture();
            SerializationContext.AddService(logger);
        }

        public void FromAOMDirectory(AOMDirectory directory, Action<LogCapture> onLoad) {
            if (loaded != false) {
                loaded = false;
                NotifyPropertyChanged("IsLoaded", "IsNotLoaded");
            }
            TextureFile = null;
            Texture2File = null;
            DataFile = null;
            Data2File = null;
            Directory = directory;
            int toLoad = 0;
            Action CheckLoad = delegate {
                ++toLoad;
                if (toLoad == 6) Load(onLoad);
            };
            LoadBar(AOMDirectory.TexturesBAR, (bar) => { TextureFile = bar; CheckLoad(); });
            LoadBar(AOMDirectory.Textures2BAR, (bar) => { Texture2File = bar; CheckLoad(); });
            LoadBar(AOMDirectory.DataBAR, (bar) => { DataFile = bar; CheckLoad(); });
            LoadBar(AOMDirectory.Data2BAR, (bar) => { Data2File = bar; CheckLoad(); });
            LoadBar(AOMDirectory.SoundsBAR, (bar) => { SoundsFile = bar; CheckLoad(); });
            LoadBar(AOMDirectory.Sounds2BAR, (bar) => { Sounds2File = bar; CheckLoad(); });
        }

        private void LoadBar(string path, Action<BARFile> dest) {
            ThreadPool.QueueUserWorkItem(delegate {
                var barFile = Directory.GetFile(path);
                App.Current.Dispatcher.BeginInvoke((Action)delegate {
                    dest(barFile);
                });
            });
        }

        public void Load(Action<LogCapture> onLoad) {
            TextureRegistry = new TextureRegistry();
            TextureRegistry.Initialize(Texture2File, TextureFile);
            PrototypeList protoList = null;
            Action completeLoad = () => {
                Prototypes = protoList;
                var logger = SerializationContext.GetService<LogCapture>();
                if (logger != null) {
                    onLoad(logger);
                    /*string errors = logger.ErrorsAsString, warnings = logger.WarningsAsString, infos = logger.InfosAsString;
                    if (!string.IsNullOrWhiteSpace(errors) || !string.IsNullOrWhiteSpace(warnings) || !string.IsNullOrWhiteSpace(infos)) {
                        MessageBox.Show("Load complete." +
                            (!string.IsNullOrWhiteSpace(errors) ? "\n\nErrors:\n" + errors : "") +
                            (!string.IsNullOrWhiteSpace(warnings) ? "\n\nWarnings:\n" + warnings : "") +
                            (!string.IsNullOrWhiteSpace(infos) ? "\n\nInfo:\n" + infos : "")
                        );
                    }*/
                    logger.Clear();
                }
                if (OnLoad != null) OnLoad();
                loaded = true;
                NotifyPropertyChanged("IsLoaded", "IsNotLoaded");
            };
            ThreadPool.QueueUserWorkItem(delegate {
                /*using (var outFile = File.Create(@"E:\Games\AOM - Other shit\extracted\protox_uncomp.xmb")) {
                    XMBFile.DecompressAndSkipHeader(DataFile.GetFileStream("protox.xmb")).CopyTo(outFile);
                }*/
                Data.XMBFile protoFile = null;
                if (protoFile == null) try { LoadXMB(Data2File ?? DataFile, "protox.xmb"); } catch { }
                if (protoFile == null) try { protoFile = LoadXMB(Data2File ?? DataFile, "proto.xmb"); } catch { }
                if (protoFile == null) try { protoFile = LoadXMB(Data2File ?? DataFile, "proto.xml"); } catch { }
                /*string fileName = @"E:\Games\AOM - Other shit\extracted\protox_my_uncomp.xmb";
                using (var protoFileOut = File.Create(fileName)) { protoFile.Save(protoFileOut, false); }
                XMBFile protoFile2 = null;
                using (var protoFileOut = File.OpenRead(fileName)) { protoFile2 = XMBFile.Load(protoFileOut); }
                using (var protoFileOut = File.Create(fileName + ".xml")) { protoFile2.GetAsXDocument().Save(protoFileOut); }*/
                if (protoFile != null) {
                    using (new XElementExt.Case(true)) {
                        var xml = protoFile.GetAsXDocument();
                        protoList = new ProtoXML().Deserialize(
                            SerializationContext,
                            xml.Root
                        );
                    }
                }
                App.Current.Dispatcher.BeginInvoke(completeLoad);
            });
        }

        public XMBFile LoadXMB(BARFile barFile, string fileName) {
            return Directory.LoadXMB(barFile.SourceFilename, fileName);
        }

        public void AddLoadedCallback(Action callback, bool callNow) {
            OnLoad += callback;
            if (callNow && loaded) callback();
        }
        public void RemoveLoadedCallback(Action callback) {
            OnLoad -= callback;
        }

    }
}
