using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataClassModel;

namespace Main
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private HRWorkEntities Context { get; set; }

        void TryLogin()
        {
            using(Context = new HRWorkEntities())
            {
                try
                {
                    if(Context.Users.Any(x=>x.UsrLogin == TB_login.Text && x.UsrPassword == TB_Password.Password.ToString()))
                    {
                        MainWindow mainWindow = new MainWindow(Context.Users.FirstOrDefault(x => x.UsrLogin == TB_login.Text && x.UsrPassword == TB_Password.Password.ToString()));
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Введены неправильные логин и пароль!", "Ошибка");
                    }
                }
                catch (Exception) { }
            }
        }

        private void BTN_TryLogin_Click(object sender, RoutedEventArgs e)
        {
            TryLogin();
        }
    }
}
