using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace RTS4.ModHQ.Controls {
    public class NerfedWrapPanel : VirtualizingPanel, IScrollInfo {

        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(VirtualizingWrapPanel), new FrameworkPropertyMetadata(double.PositiveInfinity));
        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(VirtualizingWrapPanel), new FrameworkPropertyMetadata(double.PositiveInfinity));

        [TypeConverter(typeof(LengthConverter))]
        public double ItemHeight {
            get { return (double)base.GetValue(ItemHeightProperty); }
            set { base.SetValue(ItemHeightProperty, value); }
        }
        [TypeConverter(typeof(LengthConverter))]
        public double ItemWidth {
            get { return (double)base.GetValue(ItemWidthProperty); }
            set { base.SetValue(ItemWidthProperty, value); }
        }

        private double extentHeight = 0;
        private double verticalOffset = 0;
        private double viewportHeight = 0;

        UIElementCollection _children;
        ItemsControl _itemsControl;
        IItemContainerGenerator _generator;
        Dictionary<UIElement, Rect> _realizedChildLayout = new Dictionary<UIElement, Rect>();
        

        public bool CanHorizontallyScroll {
            get { return false; }
            set { }
        }

        public bool CanVerticallyScroll {
            get { return true; }
            set { }
        }

        public double ExtentWidth { get { return 0; } }
        public double ExtentHeight { get { return extentHeight; } }
        public double HorizontalOffset { get { return 0; } }
        public double VerticalOffset { get { return verticalOffset; } }
        public double ViewportWidth { get { return 0; } }
        public double ViewportHeight { get { return viewportHeight; } }

        public void LineDown() {
            SetVerticalOffset(VerticalOffset + 20);
        }
        public void LineLeft() { }
        public void LineRight() { }
        public void LineUp() {
            SetVerticalOffset(VerticalOffset - 20);
        }

        public Rect MakeVisible(Visual visual, Rect rectangle) {
            var gen = (ItemContainerGenerator)_generator.GetItemContainerGeneratorForPanel(this);
            var element = (UIElement)visual;
            int itemIndex = gen.IndexFromContainer(element);
            while (itemIndex == -1) {
                element = (UIElement)VisualTreeHelper.GetParent(element);
                itemIndex = gen.IndexFromContainer(element);
            }
            int section = _abstractPanel[itemIndex].Section;
            Rect elementRect = _realizedChildLayout[element];
            if (Orientation == Orientation.Horizontal) {
                double viewportHeight = _pixelMeasuredViewport.Height;
                if (elementRect.Bottom > viewportHeight)
                    _offset.Y += 1;
                else if (elementRect.Top < 0)
                    _offset.Y -= 1;
            } else {
                double viewportWidth = _pixelMeasuredViewport.Width;
                if (elementRect.Right > viewportWidth)
                    _offset.X += 1;
                else if (elementRect.Left < 0)
                    _offset.X -= 1;
            }
            InvalidateMeasure();
            return elementRect;
        }

        public void MouseWheelDown() { PageDown(); }
        public void MouseWheelLeft() { PageLeft(); }
        public void MouseWheelRight() { PageRight(); }
        public void MouseWheelUp() { PageUp(); }

        public void PageDown() {
            SetVerticalOffset(VerticalOffset + viewportHeight * 0.8);
        }
        public void PageLeft() { }
        public void PageRight() { }
        public void PageUp() {
            SetVerticalOffset(VerticalOffset - viewportHeight * 0.8);
        }

        public ScrollViewer ScrollOwner { get; set; }

        public void SetHorizontalOffset(double offset) { }
        public void SetVerticalOffset(double offset) {
            if (offset < 0 || viewportHeight >= extentHeight) {
                offset = 0;
            } else {
                if (offset + viewportHeight >= extentHeight) offset = extentHeight - viewportHeight;
            }

            verticalOffset = offset;

            if (ScrollOwner != null) ScrollOwner.InvalidateScrollInfo();

            InvalidateMeasure();
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            switch (e.Key) {
                case Key.Down: PageDown(); e.Handled = true; break;
                case Key.Left: PageLeft(); e.Handled = true; break;
                case Key.Right: PageRight(); e.Handled = true; break;
                case Key.Up: PageUp(); e.Handled = true; break;
                default: base.OnKeyDown(e); break;
            }
        }

        protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args) {
            base.OnItemsChanged(sender, args);
        }

        protected override void OnInitialized(EventArgs e) {
            _itemsControl = ItemsControl.GetItemsOwner(this);
            _children = InternalChildren;
            _generator = ItemContainerGenerator;
            this.SizeChanged += new SizeChangedEventHandler(this.Resizing);
            base.OnInitialized(e);
        }
        public void Resizing(object sender, EventArgs e) {
        }

        protected override Size MeasureOverride(Size availableSize) {
            _realizedChildLayout.Clear();
            int itemsPerRow = (int)(availableSize.Width / ItemWidth);
            int rowsPerScreen = (int)Math.Ceiling(availableSize.Height / ItemHeight) + 1;
            GeneratorPosition startPos = _generator.GeneratorPositionFromIndex(firstVisibleIndex);
            //using(_generator.StartAt(
        }
        protected override Size ArrangeOverride(Size finalSize) {
            if (_children != null) {
                foreach (UIElement child in _children) {
                    var layoutInfo = _realizedChildLayout[child];
                    child.Arrange(layoutInfo);
                }
            }
            return finalSize;
        }

    }
}
