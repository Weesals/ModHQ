using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using RTS4.ModHQ.UI;
using System.Windows.Media;

namespace RTS4.ModHQ.UI.Views {
    public class GadgetView : Grid {

        public class BackgroundEntry {
            public Func<string> Getter;
            public Image Image;
            public BackgroundEntry(Func<string> getter) { Getter = getter; }
        }

        public class TextEntry {
            public Func<string> Getter;
            public Func<HorizontalAlignment> AlignX;
            public Func<VerticalAlignment> AlignY;
            public Func<float> Size;
            public TextBlock Text;
            public TextEntry(Func<string> getter) { Getter = getter; }
        }

        public readonly Gadget Gadget;
        public readonly WindowSpace Space;
        public Rect TransformedRect { get { return Space.Transform(Gadget.Rectangle1024); } }

        public BackgroundEntry[] Backgrounds;
        public TextEntry[] Texts;
        public GadgetView[] ChildViews;

        public TextureRegistry TextureRegistry;

        public GadgetView(TextureRegistry texRegistry, Gadget gadget, WindowSpace space) {
            TextureRegistry = texRegistry;
            Gadget = gadget;
            Space = space;

            ToolTip = gadget.Command;

            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            UseLayoutRounding = true;
            SnapsToDevicePixels = true;

            Arrange();

            IsVisibleChanged += GadgetView_IsVisibleChanged;
            MouseEnter += GadgetView_MouseEnter;
            MouseLeave += GadgetView_MouseLeave;

            Gadget.OnPropertyChanged(VisualChange, "Hidden", "Rectangle1024");
        }

        void GadgetView_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
            EnterState(0);
        }

        void GadgetView_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
            EnterState(1);
        }

        void GadgetView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if ((bool)e.NewValue && Backgrounds == null) {
                Backgrounds = new[] {
                    new BackgroundEntry(MakeGetter("background")),
                    new BackgroundEntry(MakeGetter("progressbarbackground")),
                    new BackgroundEntry(MakeGetter("foreground")),
                };
                foreach (var bg in Backgrounds) {
                    var value = bg.Getter();
                    if (!string.IsNullOrWhiteSpace(value)) {
                        //value = TextureRegistry.SearchEntry(value);
                        TextureRegistry.GetWPFTexture(value, (bmp) => {
                            SetBackground(bg, bmp);
                        });
                    }
                }
                EnterState(0);
                Texts = new[] {
                    new TextEntry(() => Gadget.Text) { AlignX = () => {
                        if (Gadget.Values.ContainsKey("textcenterhoriz")) return HorizontalAlignment.Center;
                        if (Gadget.Values.ContainsKey("textjustifyright")) return HorizontalAlignment.Right;
                        return HorizontalAlignment.Left;
                    }, AlignY = () => {
                        if (Gadget.Values.ContainsKey("textcentervert")) return VerticalAlignment.Center;
                        return VerticalAlignment.Top;
                    }, Size = () => {
                        if (Gadget.Values.ContainsKey("textfontsize")) return float.Parse(Gadget.Values["textfontsize"]);
                        return 12;
                    }, },
                    //new TextEntry(() => Gadget.Command),
                };
                foreach (var bg in Texts) {
                    var value = bg.Getter();
                    if (value != null) {
                        bg.Text = new TextBlock() { Text = value, Foreground = new SolidColorBrush(Colors.White), };
                        if (bg.AlignX != null) bg.Text.HorizontalAlignment = bg.AlignX();
                        if (bg.AlignY != null) bg.Text.VerticalAlignment = bg.AlignY();
                        if (bg.Size != null) bg.Text.FontSize = bg.Size();
                        Children.Add(bg.Text);
                    }
                }
            }
        }

        private void SetBackground(BackgroundEntry bg, ImageSource img) {
            if (bg.Image == null) {
                bg.Image = new Image() { Stretch = Stretch.Fill, };
                Children.Add(bg.Image);
            }
            bg.Image.Source = img;
        }
        private void EnterState(int state) {
            if (Gadget.StateEntries == null || Gadget.StateEntries.Length == 0) return;
            if (state >= Gadget.StateEntries.Length) state = Gadget.StateEntries.Length - 1;
            var bg = Gadget.StateEntries[state].Background;
            //bg = TextureRegistry.SearchEntry(bg);
            TextureRegistry.GetWPFTexture(bg, (bmp) => {
                SetBackground(Backgrounds[0], bmp);
            });
        }

        private Func<string> MakeGetter(string name) {
            return () => {
                if (Gadget.Values.ContainsKey(name)) return Gadget.Values[name];
                return null;
            };
        }

        private void VisualChange(object sender) {
            Arrange();
            if (ChildViews != null) foreach (var child in ChildViews) child.VisualChange(sender);
        }

        public void Arrange() {
            bool isHidden = Gadget.Hidden;
            var parent = Gadget.Parent;
            while (parent != null) {
                isHidden |= parent.Hidden;
                parent = parent.Parent;
            }

            var rect = TransformedRect;
            Margin = new Thickness(rect.X, rect.Y, 0, 0);
            Width = rect.Width; Height = rect.Height;
            Visibility = isHidden ? Visibility.Hidden : Visibility.Visible;
        }

    }
}
