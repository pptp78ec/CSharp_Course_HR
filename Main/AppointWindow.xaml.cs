using DataClassModel;
using System;
using System.Linq;
using System.Windows;

namespace Main
{
    /// <summary>
    /// Логика взаимодействия для AppointWindow.xaml
    /// </summary>
    public partial class AppointWindow : Window
    {
        public int SelUserId { get; set; } = default;
        public int SelPosId { get; set; } = default;
        public int SelDivId { get; set; } = default;

        private HRWorkEntities Context { get; set; }
        public AppointWindow()
        {
            InitializeComponent();
        }
        public AppointWindow(int _selUsrId, HRWorkEntities context)
        {
            InitializeComponent();
            SelUserId = _selUsrId;
            Context = context;
        }

        private void BTN_SelDivision_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectionWindow selDivW = new SelectionWindow(Context.Divisions.Where(x => x.DeletionFlag == false).ToList(), "SelDivId");
                selDivW.Owner = this;
                selDivW.ShowDialog();
                TB_Division.Text = Context.Divisions.FirstOrDefault(x => x.Id == SelDivId).DivisionName;
            }
            catch (Exception) { MessageBox.Show("Возникла ошибка!"); }

        }

        private void BTN_SelPosition_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectionWindow selDivW = new SelectionWindow(Context.Positions.Where(x => x.DeletionFlag == false).ToList(), "SelPosId");
                selDivW.Owner = this;
                selDivW.ShowDialog();
                TB_Position.Text = Context.Positions.FirstOrDefault(x => x.Id == SelPosId).PositionName;
            }
            catch (Exception) { MessageBox.Show("Возникла ошибка!"); }
        }

        private void BTN_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var appointment = new Appointments();
                appointment.EmployeeId = SelUserId;
                appointment.PositionId = SelPosId;
                appointment.DivisionId = SelDivId;
                appointment.AppDate = DateTime.Now.Date;
                appointment.AppOrderNum = TB_AppOrder.Text;
                Context.Appointments.Add(appointment);
                Context.Employes.FirstOrDefault(x => x.Id == SelUserId).PositionId = appointment.PositionId;
                Context.Employes.FirstOrDefault(x => x.Id == SelUserId).DivisionId = appointment.DivisionId;
                Context.SaveChanges();
            }
            catch (Exception) { }
            Close();
        }
    }
}
