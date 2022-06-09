using DataClassModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;


namespace Main
{
    /// <summary>
    /// Логика взаимодействия для AdminEditWindow.xaml
    /// </summary>
    public partial class AdminEditWindow : Window
    {
        public HRWorkEntities Context { get; set; } = new HRWorkEntities();

        public AdminEditWindow()
        {
            InitializeComponent();
        }


        public AdminEditWindow(string tablename)
        {
            InitializeComponent();
            BindItems(tablename);
        }

        void BindItems(string tablename)
        {
            switch (tablename)
            {
                case "Appointments":
                    {
                        Context.Appointments.Load();
                        DG_View.ItemsSource = Context.Appointments.Local.ToBindingList();
                        break;
                    }
                case "CompanyInfo":
                    {
                        Context.CompanyInfo.Load();
                        DG_View.ItemsSource = Context.CompanyInfo.Local.ToBindingList();
                        break;
                    }
                case "Divisions":
                    {
                        Context.Divisions.Load();
                        DG_View.ItemsSource = Context.Divisions.Local.ToBindingList();
                        break;
                    }
                case "EduLevels":
                    {
                        Context.EduLevels.Load();
                        DG_View.ItemsSource = Context.EduLevels.Local.ToBindingList();
                        break;
                    }
                case "Users":
                    {
                        Context.Users.Load();
                        DG_View.ItemsSource = Context.Users.Local.ToBindingList();
                        break;
                    }
                case "FireCauses":
                    {
                        Context.FireCauses.Load();
                        DG_View.ItemsSource = Context.FireCauses.Local.ToBindingList();
                        break;
                    }
                case "Positions":
                    {
                        Context.Positions.Load();
                        DG_View.ItemsSource = Context.Positions.Local.ToBindingList();
                        break;
                    }
                case "Salaries":
                    {
                        Context.Salaries.Load();
                        DG_View.ItemsSource = Context.Salaries.Local.ToBindingList();
                        break;
                    }
                case "Vacations":
                    {
                        Context.Vacations.Load();
                        DG_View.ItemsSource = Context.Vacations.Local.ToBindingList();
                        break;
                    }
                default:
                    {
                        Close();
                    }
                    break;
            }
        }

        private void BTN_Save_Click(object sender, RoutedEventArgs e)
        {
            Context.SaveChanges();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in DG_View.Columns)
            {
                if (ForbiddenList.ForbidList.Any(x => x == item.Header.ToString()))
                {
                    item.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
