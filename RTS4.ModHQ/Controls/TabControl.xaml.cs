using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF = System.Windows.Controls;
using AttachTo = RTS4.ModHQ.Controls;

namespace RTS4.ModHQ.Controls {
    public partial class TabControl : WPF.TabControl {

        public static readonly DependencyProperty LeftContentProperty =
            DependencyProperty.RegisterAttached("LeftContent", typeof(UIElement), typeof(AttachTo.TabControl));
        //public UIElement LeftContent { get { return (UIElement)GetValue(LeftContentProperty); } set { SetValue(LeftContentProperty, value); } }
        public static UIElement GetLeftContent(DependencyObject source) { return (UIElement)source.GetValue(LeftContentProperty); }
        public static void SetLeftContent(DependencyObject source, UIElement value) { source.SetValue(LeftContentProperty, value); }

        public static readonly DependencyProperty RightContentProperty =
            DependencyProperty.RegisterAttached("RightContent", typeof(UIElement), typeof(AttachTo.TabControl));
        //public UIElement RightContent { get { return (UIElement)GetValue(RightContentProperty); } set { SetValue(RightContentProperty, value); } }
        public static UIElement GetRightContent(DependencyObject source) { return (UIElement)source.GetValue(RightContentProperty); }
        public static void SetRightContent(DependencyObject source, UIElement value) { source.SetValue(RightContentProperty, value); }

        public static readonly DependencyProperty TabBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("TabBackgroundBrush", typeof(Brush), typeof(AttachTo.TabControl));
        //public Brush TabBackgroundBrush { get { return (Brush)GetValue(TabBackgroundBrushProperty); } set { SetValue(TabBackgroundBrushProperty, value); } }
        public static Brush GetTabBackgroundBrush(DependencyObject source) { return (Brush)source.GetValue(TabBackgroundBrushProperty); }
        public static void SetTabBackgroundBrush(DependencyObject source, Brush value) { source.SetValue(TabBackgroundBrushProperty, value); }

        public static readonly DependencyProperty TabInactiveBrushProperty =
            DependencyProperty.RegisterAttached("TabInactiveBrush", typeof(Brush), typeof(AttachTo.TabControl));
        //public Brush TabInactiveBrush { get { return (Brush)GetValue(TabInactiveBrushProperty); } set { SetValue(TabInactiveBrushProperty, value); } }
        public static Brush GetTabInactiveBrush(DependencyObject source) { return (Brush)source.GetValue(TabInactiveBrushProperty); }
        public static void SetTabInactiveBrush(DependencyObject source, Brush value) { source.SetValue(TabInactiveBrushProperty, value); }

        public static readonly DependencyProperty TabActiveBrushProperty =
            DependencyProperty.RegisterAttached("TabActiveBrush", typeof(Brush), typeof(AttachTo.TabControl));
        //public Brush TabActiveBrush { get { return (Brush)GetValue(TabActiveBrushProperty); } set { SetValue(TabActiveBrushProperty, value); } }
        public static Brush GetTabActiveBrush(DependencyObject source) { return (Brush)source.GetValue(TabActiveBrushProperty); }
        public static void SetTabActiveBrush(DependencyObject source, Brush value) { source.SetValue(TabActiveBrushProperty, value); }

        public static readonly DependencyProperty TabHoverBrushProperty =
            DependencyProperty.RegisterAttached("TabHoverBrush", typeof(Brush), typeof(AttachTo.TabControl));
        //public Brush TabHoverBrush { get { return (Brush)GetValue(TabHoverBrushProperty); } set { SetValue(TabHoverBrushProperty, value); } }
        public static Brush GetTabHoverBrush(DependencyObject source) { return (Brush)source.GetValue(TabHoverBrushProperty); }
        public static void SetTabHoverBrush(DependencyObject source, Brush value) { source.SetValue(TabHoverBrushProperty, value); }

        /*public static readonly DependencyProperty TabStripPlacementProperty =
            DependencyProperty.Register("TabStripPlacement", typeof(Dock), typeof(TabControl));
        [Bindable(true)]
        [Category("Behavior")]
        public Dock TabStripPlacement { get { return (Dock)GetValue(TabStripPlacementProperty); } set { SetValue(TabStripPlacementProperty, value); } }*/

        /*public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(TabControl), new PropertyMetadata((o, e) => {
                ((TabControl)o).ActualTabs.ItemsSource = (IEnumerable)e.NewValue;
            }));
        public IEnumerable ItemsSource { get { return (IEnumerable)GetValue(ItemsSourceProperty); } set { SetValue(ItemsSourceProperty, value); } }

        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ItemCollection Items { get; private set; }*/

        public TabControl() {
            Resources.Add("Self", this);
            InitializeComponent();
            //Items = ActualTabs.Items;
        }
    }
}
