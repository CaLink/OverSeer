using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DesktopPart.CUstoM
{
    public class CUstoMTreeView : TreeView
    {
        public object CUstoMSelectedItem
        {
            get { return GetValue(CUstoMSelectedItemProperty); }
            set { SetValue(CUstoMSelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CUstoMSelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CUstoMSelectedItemProperty =
            DependencyProperty.Register("CUstoMSelectedItem", typeof(object), typeof(CUstoMTreeView));

        public CUstoMTreeView()
        {
            SelectedItemChanged += CUstoMTreeView_SelectedItemChanged;
        }

        private void CUstoMTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            CUstoMSelectedItem = SelectedItem;
        }
    }
}
