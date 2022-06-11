using DataClassModel;
using System;
using System.Collections;
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

        private List<GeneralOrders> GetGeneralOrders()
        {

            using (var entities = new HRWorkEntities())
            {
                try
                {
                    return entities.GeneralOrders.ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        private IList GetFireOrders()
        {
            using (var entities = new HRWorkEntities())
            {
                try
                {

                    var allfireOrders = from fo in entities.Fires
                                        join fc in entities.FireCauses on fo.CauseID equals fc.Id
                                        join em in entities.Employes on fo.EmployeeID equals em.Id
                                        join go in entities.GeneralOrders on fo.GeneralFireOrderId equals go.Id
                                        where fo.DeletionFlag != true
                                        select new
                                        {
                                            Id = fo.Id,
                                            Employee = em.LastName + " " + em.FirstName + " " + em.MiddleName,
                                            GenFireOrderName = go.GeneralOrderName,
                                            FireOrderName = fo.FireOrderName,
                                            Cause = fc.CauseName,
                                            AddInfo = fo.CauseAddInfo,
                                            FireDate = fo.FireDate,
                                        };

                    return allfireOrders.ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
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

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            GenOrdersWindow gw = new GenOrdersWindow();
            gw.Owner = this;
            gw.ShowDialog();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            var genOrders = GetGeneralOrders();
            if (genOrders != null && genOrders.Count > 0)
            {
                SelectionWindow sw = new SelectionWindow(genOrders, "SelectedItemId");
                sw.Owner = this;
                sw.btn_Select.IsEnabled = false;
                sw.btn_Select.Visibility = Visibility.Collapsed;
                sw.ShowDialog();
            }
            else
            {
                MessageBox.Show("Не найдено приказов по компании");
            }
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            var fireOrders = GetFireOrders();
            if (fireOrders != null && fireOrders.Count > 0)
            {
                SelectionWindow sw = new SelectionWindow(fireOrders, "SelectedItemId");
                sw.Owner = this;
                sw.btn_Select.IsEnabled = false;
                sw.btn_Select.Visibility = Visibility.Collapsed;
                sw.ShowDialog();
            }
            else
            {
                MessageBox.Show("Не найдено приказов на увольнение");
            }
        }
    }
}
