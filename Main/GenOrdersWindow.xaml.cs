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
using System.Text.RegularExpressions;
using DataClassModel;

namespace Main
{
    /// <summary>
    /// Логика взаимодействия для GenOrdersWindow.xaml
    /// </summary>
    public partial class GenOrdersWindow : Window
    {
        public GenOrdersWindow()
        {
            InitializeComponent();
        }

        private HRWorkEntities Context = new HRWorkEntities();
        private Regex rgx_maxsize20 = new Regex(@"^[\d\D\w\D\s\S]{0,19}$");
        private Regex rgx_maxsize100 = new Regex(@"^[\d\D\w\D\s\S]{0,99}$");

        void SaveOrder()
        {
            using(Context = new HRWorkEntities())
            {
                try
                {
                    GeneralOrders newOrder = new GeneralOrders();
                    newOrder.GeneralOrderDate = DP_OrderDate.SelectedDate.GetValueOrDefault(DateTime.Now);
                    newOrder.GeneralOrderName = TB_Order.Text;
                    newOrder.GeneralOrderInfo = TB_AddInfo.Text;
                    Context.GeneralOrders.Add(newOrder);
                    Context.SaveChanges();
                }
                catch (Exception) { }
            }
        }

        private void BTN_Save_Click(object sender, RoutedEventArgs e)
        {
            if(TB_Order.Text.Length > 0)
            {
                SaveOrder();
                MessageBox.Show("Приказ сохранен");
            }
            else
            {
                MessageBox.Show("Введите название приказа");
            }
        }

        private void TB_Order_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = rgx_maxsize20.IsMatch(TB_Order.Text) ? false : true;
        }

        private void TB_AddInfo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = rgx_maxsize100.IsMatch(TB_AddInfo.Text) ? false : true;
        }
    }
}
