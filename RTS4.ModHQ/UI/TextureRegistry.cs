using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTS4.Data;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Threading;
using System.Globalization;

namespace RTS4.ModHQ.UI {
    public class TextureRegistry {

        public BARFile[] Sources { get; private set; }
        private Dictionary<string, WriteableBitmap> img_cache = new Dictionary<string, WriteableBitmap>();

        public void Initialize(params BARFile[] bars) {
            Sources = bars;
        }

        internal string SearchEntry(string name) {
            foreach (var source in Sources) {
                if (source == null) continue;
                foreach (var entry in source.FilesNames.Where(e => CultureInfo.InvariantCulture.CompareInfo.IndexOf(e, name, CompareOptions.IgnoreCase) >= 0)) {
                    if (entry != null &&
                        (entry.EndsWith("ddt", StringComparison.OrdinalIgnoreCase) || entry.EndsWith("btx", StringComparison.OrdinalIgnoreCase))
                    ) return entry;
                }
            }
            return null;
        }
        internal Stream GetStream(string name) {
            foreach (var source in Sources) {
                if (source == null) continue;
                var stream = source.GetFileStream(name);
                if (stream != null) return stream;
            }
            return null;
        }
        public WriteableBitmap GetTexture(string name, bool transparent) {
            if (img_cache.ContainsKey(name)) return img_cache[name];
            Stream stream = GetStream(name + ".ddt");
            if (stream == null) return null;
            var img = DDTImage.Load(stream);
            WriteableBitmap tex = new WriteableBitmap(img.Width, img.Height, 75, 75, PixelFormats.Bgra32, null);
            var data = img.Get32BitUncompressed();
            /*for (int d = 0; d < data.Length; d += 4) {
                byte r = data[d + 2];
                data[d + 2] = data[d + 0];
                data[d + 0] = r;
            }*/
            /*if (transparent)
            {
                for (int d = 0; d < data.Length; d += 4) {
                    if (data[d + 0] == 0 && data[d + 1] == 0 && data[d + 2] == 0 && data[d + 3] == 255) {
                        data[d + 3] = 0;
                    }
                }
            }*/
            tex.WritePixels(new Int32Rect(0, 0, img.Width, img.Height), data, img.Width * 4, 0);
            //tex.SetData(data);
            img_cache.Add(name, tex);
            return tex;
        }

        private string DropExt(string path) {
            if (path.EndsWith(".ddt") || path.EndsWith(".btx")) return path.Substring(0, path.Length - 4);
            return path;
        }
        private ImageSource FindInCache(string name) {
            lock (imageCache) {
                ImageSource cache = null;
                if (imageCache.TryGetValue(name, out cache)) return cache;
                var noext = DropExt(name);
                if (noext != name && imageCache.TryGetValue(noext, out cache)) return cache;
            }
            return null;
        }
        private Dictionary<string, ImageSource> imageCache = new Dictionary<string, ImageSource>();
        public void GetWPFTexture(string name, Action<ImageSource> callback) {
            if (string.IsNullOrWhiteSpace(name)) return;
            var cache = FindInCache(name);
            if (cache != null) { callback(cache); return; }
            ThreadPool.QueueUserWorkItem(delegate {
                RTS3Image image = null;
                lock (imageCache) {
                    cache = FindInCache(name);
                    if (cache != null) { callback(cache); return; }
                    var file = GetStream(name);
                    if (file == null) {
                        var noext = DropExt(name);
                        if (file == null) file = GetStream(noext + ".ddt");
                        if (file == null) file = GetStream(noext + ".btx");
                    }
                    if (file != null) {
                        var magic = file.ReadByte();
                        file.Position -= 1;
                        if (magic == 'b') {
                            try { image = BTXImage.Load(file); } catch { }
                        } else {
                            try { image = DDTImage.Load(file); } catch { }
                        }
                    }
                }
                if (image != null) {
                    var pixData = image.Get32BitUncompressed();
                    App.Current.Dispatcher.BeginInvoke((Action)delegate {
                        var bmp = BitmapSource.Create(image.Width, image.Height, 96, 96, PixelFormats.Bgra32, null, pixData, image.Width * 4);
                        lock (imageCache) {
                            if (!imageCache.ContainsKey(name)) imageCache.Add(name, bmp);
                        }
                        callback(bmp);
                    });
                } else {
                    App.Current.Dispatcher.BeginInvoke((Action)delegate {
                        callback(null);
                    });
                }
            });
        }

    }
}
