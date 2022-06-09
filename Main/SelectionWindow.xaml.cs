using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Main
{
    
    /// <summary>
    /// Логика взаимодействия для SelectionWindow.xaml
    /// </summary>
    public partial class SelectionWindow : Window
    {
        private IEnumerable SelColl { set; get; }

        private string SelectionChoice { set; get; }

        public SelectionWindow()
        {
            InitializeComponent();
        }
        public SelectionWindow(IList selColl, string basePropertyName)
        {
            InitializeComponent();
            SelColl = selColl;
            SelectionChoice = basePropertyName;
            selColl.GetType();

            if (selColl != null)
            {
                LV_selectionList.ItemsSource = selColl;
                GridView gridView = new GridView();
                if (selColl.Count > 0)
                {
                    foreach (var item in selColl[0].GetType().GetProperties())
                    {
                        if (ForbiddenList.ForbidList != null && ForbiddenList.ForbidList.Count != 0)
                        {
                            if (!ForbiddenList.ForbidList.Any(x => x == item.Name))
                            {
                                GridViewColumn column = new GridViewColumn();
                                column.DisplayMemberBinding = new Binding(item.Name);
                                column.Header = item.Name;
                                gridView.Columns.Add(column);
                            }
                        }
                        else
                        {
                            GridViewColumn column = new GridViewColumn();
                            column.DisplayMemberBinding = new Binding(item.Name);
                            column.Header = item.Name;
                            gridView.Columns.Add(column);
                        }
                    }
                    LV_selectionList.View = gridView;
                }
            }
        }

        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LV_selectionList.SelectedItem != null)
                {
                    var ts = LV_selectionList.SelectedItem.GetType().GetProperty("Id");
                    var ownerProperty = Owner.GetType().GetProperty(SelectionChoice, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    ownerProperty.SetValue(Owner, Convert.ToInt32((ts.GetValue((LV_selectionList.SelectedItem)))));
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не выбран никакой элемент");
                }
            }
            catch (Exception) { }
        }
    }
}
