using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// Логика взаимодействия для AdminEditSelection.xaml
    /// </summary>
    public partial class AdminEditSelection : Window
    {
        public AdminEditSelection()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (RB_Div.IsChecked.HasValue && RB_Div.IsChecked.Value)
            {
                AdminEditWindow aw = new AdminEditWindow("Divisions");
                aw.Owner = this;
                aw.ShowDialog();
            }
            else if (RB_Pos.IsChecked.HasValue && RB_Pos.IsChecked.Value)
            {
                AdminEditWindow aw = new AdminEditWindow("Positions");
                aw.Owner = this;
                aw.ShowDialog();
            }
            else if (RB_FCs.IsChecked.HasValue && RB_FCs.IsChecked.Value)
            {
                AdminEditWindow aw = new AdminEditWindow("FireCauses");
                aw.Owner = this;
                aw.ShowDialog();
            }
            else if (RB_EduLevel.IsChecked.HasValue && RB_EduLevel.IsChecked.Value)
            {
                AdminEditWindow aw = new AdminEditWindow("EduLevels");
                aw.Owner = this;
                aw.ShowDialog();
            }
            else
            {
                MessageBox.Show("Нчиего не выбрано");
            }
        }
    }
}
