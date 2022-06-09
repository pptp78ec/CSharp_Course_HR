using DataClassModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Main
{

    public struct tstStruct
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Lastname { set; get; }
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public int SelectedItemId { set; get; } = default;
        private Users CurrentUser { set; get; }
        public MainWindow()
        {
            FillForbidList();
            InitializeComponent();
        }

        public MainWindow(Users loggedUser)
        {
            CurrentUser = loggedUser;
            InitializeComponent();
            FillForbidList();
            if (loggedUser.AccessLevel < 2)
            {
                MN_AdminTools.Visibility = Visibility.Hidden;
            }
        }

        private void FillForbidList()
        {
            ForbiddenList.ForbidList = new List<string>() { "Appointments", "Appointments1", "CompanyInfo", "CompanyInfo1",
            "CompanyInfo2","DataModel","Divisions","EduLevels","Employes","Employes1","Employes2","FireCauses","Fires", "Photos","Positions","Salaries", "Users","Vacations"};
        }

        private List<Employes> GetVictimList()
        {
            using (var entities = new HRWorkEntities())
            {
                try
                {
                    return entities.Employes.ToList().Where(x => (double)DateTime.Now.Subtract(x.BirthDate.Value).TotalDays / 365.25 > 58).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public int RetValue { set; get; }
       

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.Owner = this;
            sw.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.Owner = this;
            searchWindow.ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var victims = GetVictimList();
            if (victims != null && victims.Count > 0)
            {
                SelectionWindow sw = new SelectionWindow(victims, "SelectedItemId");
                sw.Owner = this;
                sw.ShowDialog();
                if (SelectedItemId > 0)
                {
                    EmpCardViewEdit userCard = new EmpCardViewEdit(false, SelectedItemId);
                    userCard.Owner = this;
                    userCard.ShowDialog();
                    SelectedItemId = 0;
                }
            }
            else
            {
                MessageBox.Show("Нет сотдрудников, которые подходят по сокращению по возрасту!");
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            EmpCardViewEdit addEmpWindow = new EmpCardViewEdit(true, 0);
            addEmpWindow.Owner = this;
            addEmpWindow.ShowDialog();
        }

        private void MN_AdminTools_Click(object sender, RoutedEventArgs e)
        {
            AdminEditSelection aec = new AdminEditSelection();
            aec.Owner = this;
            aec.ShowDialog();
        }
    }
}
