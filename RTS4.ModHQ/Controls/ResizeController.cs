using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using RTS4.ModHQ.UI.Views;

namespace RTS4.ModHQ.Controls {
    public class ResizeController : ContentControl {

        public readonly GadgetView View;
        public readonly UndoManager UndoManager;

        public new Grid Content { get { return base.Content as Grid; } set { base.Content = value; } }

        public ResizeController(UndoManager undoManager, GadgetView view) {
            UndoManager = undoManager;
            View = view;
            PositionChanged(null);

            Content = new Grid() {
                ClipToBounds = false,
            };
            var border = new Border() {
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush(Colors.Transparent),
                ClipToBounds = false,
            };
            Content.Children.Add(border);
            Content.Children.Add(GenerateNub(1, 0, 5, 5));
            Content.Children.Add(GenerateNub(0, 1, 5, 5));
            Content.Children.Add(GenerateNub(1, 0.5f, 5, 10));
            Content.Children.Add(GenerateNub(0.5f, 1, 10, 5));
            Content.Children.Add(GenerateNub(1, 1, 20, 20));
            RegisterDragEvents(border, 1, 1, 0, 0);
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Loaded += ResizeController_Loaded;
            Unloaded += ResizeController_Unloaded;
        }

        private Rectangle GenerateNub(float x, float y, float w, float h) {
            var rect = new Rectangle() {
                Fill = new SolidColorBrush(Color.FromArgb(255, 128, 32, 32)),
                Width = w,
                Height = h,
                Margin = new Thickness(-w * x, -h * y, 0, 0),
            };
            if (x < 0.25f) rect.HorizontalAlignment = HorizontalAlignment.Left;
            else if (x > 0.75f) rect.HorizontalAlignment = HorizontalAlignment.Right;
            else rect.HorizontalAlignment = HorizontalAlignment.Center;
            if (y < 0.25f) rect.VerticalAlignment = VerticalAlignment.Top;
            else if (y > 0.75f) rect.VerticalAlignment = VerticalAlignment.Bottom;
            else rect.VerticalAlignment = VerticalAlignment.Center;
            RegisterDragEvents(rect,
                x == 0 ? 1 : 0, y == 0 ? 1 : 0,
                x * 2 - 1, y * 2 - 1);
            return rect;
        }

        private void RegisterDragEvents(FrameworkElement nub, float xweight, float yweight, float widthWeight, float heightWeight) {
            bool hasUndoEvent = false;
            Point downPos = new Point();
            Rect originalRect = Rect.Empty;

            MouseEventHandler onMove = null;
            MouseButtonEventHandler onUp = null;
            nub.MouseDown += (o, e) => {
                if (e.Handled) return;
                nub.MouseMove += onMove;
                nub.MouseUp += onUp;
                hasUndoEvent = false;
                nub.CaptureMouse();
                downPos = new Point();
                e.Handled = true;
            };
            onMove += (o, e) => {
                if (!hasUndoEvent) {
                    hasUndoEvent = true;
                    var origRect = View.Gadget.Rectangle1024;
                    UndoManager.PushRestore(() => {
                        var dx = origRect.X - View.Gadget.Rectangle1024.X;
                        var dy = origRect.Y - View.Gadget.Rectangle1024.Y;
                        OffsetChildren(View.Gadget.Children, dx, dy);
                        View.Gadget.Rectangle1024 = origRect;
                    });
                    originalRect = origRect;
                }
                var pos = e.GetPosition(nub);
                if (downPos == new Point()) downPos = pos;
                var rect = View.Gadget.Rectangle1024;
                var space = View.Space;
                var x = rect.X + xweight * (pos.X - downPos.X) / space.WidthRatio;
                var y = rect.Y + yweight * (pos.Y - downPos.Y) / space.HeightRatio;
                var w = rect.Width + widthWeight * (pos.X - downPos.X) / space.WidthRatio;
                var h = rect.Height + heightWeight * (pos.Y - downPos.Y) / space.HeightRatio;
                const float Rounding = 10;
                if ((Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.Alt) {
                    x = originalRect.X + Math.Round((x - originalRect.X) / Rounding) * Rounding;
                    y = originalRect.Y + Math.Round((y - originalRect.Y) / Rounding) * Rounding;
                    w = originalRect.Width + Math.Round((w - originalRect.Width) / Rounding) * Rounding;
                    h = originalRect.Height + Math.Round((h - originalRect.Height) / Rounding) * Rounding;
                }
                if (rect.X != x || rect.Y != y) {
                    OffsetChildren(View.Gadget.Children, x - rect.X, y - rect.Y);
                }
                rect.X = x;
                rect.Y = y;
                rect.Width = Math.Max(w, 0);
                rect.Height = Math.Max(h, 0);
                View.Gadget.Rectangle1024 = rect;
            };
            onUp += (o, e) => {
                nub.ReleaseMouseCapture();
                nub.MouseMove -= onMove;
                nub.MouseUp -= onUp;
            };
        }

        private void OffsetChildren(UI.Gadget[] gadgets, double dx, double dy) {
            if (gadgets == null) return;
            foreach (var child in gadgets) {
                var childRect = child.Rectangle1024;
                childRect.X += dx;
                childRect.Y += dy;
                child.Rectangle1024 = childRect;
                OffsetChildren(child.Children, dx, dy);
            }
        }

        void ResizeController_Loaded(object sender, RoutedEventArgs e) {
            View.Gadget.OnPropertyChanged(PositionChanged, "Rectangle1024");
        }
        void ResizeController_Unloaded(object sender, RoutedEventArgs e) {
            View.Gadget.RemoveOnPropertyChanged(PositionChanged, "Rectangle1024");
        }

        private void PositionChanged(object obj) {
            var rect = View.TransformedRect;
            Margin = new Thickness(rect.X, rect.Y, 0, 0);
            Width = rect.Width; Height = rect.Height;
        }

    }
}
