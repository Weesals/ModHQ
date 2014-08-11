using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
/*using Microsoft.Xna.Framework.Graphics;

namespace RTS4.ModHQ {
    public class DDSToBMP {
        public static ImageSource Convert(Stream stream, int width, int height) {
            // Create the graphics device
            using (var graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.Reach, new PresentationParameters() {
                DeviceWindowHandle = new WindowInteropHelper(App.Current.MainWindow).Handle,
                IsFullScreen = false,
                PresentationInterval = PresentInterval.One,
                BackBufferWidth = 4,
                BackBufferHeight = 4,
                BackBufferFormat = SurfaceFormat.Color,
                RenderTargetUsage = RenderTargetUsage.PlatformContents,
            })) {
                // Load the texture
                using (var texture = Texture2D.FromStream(graphicsDevice, stream, width, height, false)) {
                    // Get the pixel data
                    var pixelColors = new Color[texture.Width * texture.Height];
                    texture.GetData(pixelColors);

                    // Copy the pixel colors into a byte array
                    var bytesPerPixel = 3;
                    var stride = texture.Width * bytesPerPixel;

                    var pixelData = new byte[pixelColors.Length * bytesPerPixel];
                    for (var i = 0; i < pixelColors.Length; i++) {
                        pixelData[i * bytesPerPixel + 0] = pixelColors[i].R;
                        pixelData[i * bytesPerPixel + 1] = pixelColors[i].G;
                        pixelData[i * bytesPerPixel + 2] = pixelColors[i].B;
                    }

                    // Create a bitmap source
                    return BitmapSource.Create(texture.Width, texture.Height, 96, 96, PixelFormats.Rgb24, null, pixelData, stride);
                }
            }
        }
    }
}
*/