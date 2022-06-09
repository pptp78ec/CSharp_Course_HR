using DataClassModel;
using ReportModel;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Data.Entity;

namespace Main
{
    /// <summary>
    /// Логика взаимодействия для EmpCardViewEdit.xaml
    /// </summary>
    public partial class EmpCardViewEdit : Window
    {
        public bool IsCreationWindow { set; get; }
        private bool editMode = false;
        private bool EditMode
        {
            get
            {
                return editMode;
            }
            set
            {
                editMode = value;
                EditVisibility(value);
            }
        }
        private int SelectedEmpId { get; set; }
        private int EduLevelId { get; set; }
        private int PositionId { get; set; }
        private int DivisionId { get; set; }
        private int ImageHash { get; set; }
        private string NewImageName { get; set; }
        public HRWorkEntities Context { get; set; }
        public EmpCardViewEdit()
        {
            InitializeComponent();
        }

        private void ReloadContext()
        {
            Context.Appointments.Load();
            Context.CompanyInfo.Load();
            Context.Divisions.Load();
            Context.EduLevels.Load();
            Context.Employes.Load();
            Context.FireCauses.Load();
            Context.Fires.Load();
            Context.GeneralOrders.Load();
            Context.Photos.Load();
            Context.Positions.Load();
            Context.Salaries.Load();
            Context.Users.Load();
            Context.Vacations.Load();
        }

        private bool FieldsFilled()
        {
            if (TB_Name.Text.Count() == 0)
            {
                return false;
            }
            if (TB_LastName.Text.Count() == 0)
            {
                return false;
            }
            if (TB_MiddleName.Text.Count() == 0)
            {
                return false;
            }
            if (DP_Bday.SelectedDate == null)
            {
                return false;
            }
            if (TB_Experience.Text.Count() == 0)
            {
                return false;
            }
            if (EduLevelId == 0)
            {
                return false;
            }
            if (TB_Speciality.Text.Count() == 0)
            {
                return false;
            }
            if (DivisionId == 0)
            {
                return false;
            }
            if (DivisionId == 0)
            {
                return false;
            }
            if (TB_Salary.Text.Count() == 0)
            {
                return false;
            }

            return true;

        }

        private ReportData FillDataForReport()
        {

            try
            {
                var reportData = new ReportData();
                var employes = Context.Employes.FirstOrDefault(x => x.Id == SelectedEmpId);
                var companyInfo = Context.CompanyInfo.FirstOrDefault();
                var dismissal = Context.Fires.FirstOrDefault(x => x.EmployeeID == SelectedEmpId);
                var appointment = Context.Appointments.Where(x => x.EmployeeId == SelectedEmpId)?.ToList().Last();
                var CEO = Context.Employes.FirstOrDefault(x => x.Id == companyInfo.CompanyHeadId);
                var hrHead = Context.Employes.FirstOrDefault(x => x.Id == companyInfo.CompanyHRHeadId);
                var finHead = Context.Employes.FirstOrDefault(x => x.Id == companyInfo.CompanyFinHeadId);
                var vacations = Context.Vacations.FirstOrDefault(x => x.EmployeeID == SelectedEmpId);
                reportData.EmpFirstName = employes.FirstName;
                reportData.EmpLastName = employes.LastName;
                reportData.EmpMiddleName = employes.MiddleName;
                reportData.EmpDate = employes.HireDate.Value;
                reportData.EmpDivision = Context.Divisions.FirstOrDefault(x => x.Id == employes.DivisionId).DivisionName;
                reportData.EmpPosition = Context.Positions.FirstOrDefault(x => x.Id == employes.PositionId).PositionName;
                if (appointment != null)
                {
                    reportData.AppOrderDate = appointment.AppDate.GetValueOrDefault();
                    reportData.AppOrderNumber = appointment.AppOrderNum;
                }
                if (CEO != null)
                {
                    reportData.CeoLastName = CEO.LastName;
                    reportData.CeoFirstName = CEO.FirstName;
                    reportData.CeoMiddleName = CEO.MiddleName;
                    reportData.CeoPosition = CEO.Positions.PositionName;
                }
                if (hrHead != null)
                {
                    reportData.HRHeadFirstName = hrHead.FirstName;
                    reportData.HRHeadLastName = hrHead.LastName;
                    reportData.HRHeadMiddleName = hrHead.MiddleName;
                    reportData.HRHeadPosition = hrHead.Positions.PositionName;
                }
                if (finHead != null)
                {
                    reportData.FinanceHeadLastName = finHead.LastName;
                    reportData.FinanceHeadFirstName = finHead.FirstName;
                    reportData.FinanceHeadMiddleName = finHead.MiddleName;
                    reportData.FinanceHeadPosition = finHead.Positions.PositionName;
                }
                if (dismissal != null)
                {
                    reportData.CurrentOrderDate = DateTime.Now.Date;
                    reportData.CurrentOrderNumber = dismissal.FireOrderName;
                    reportData.GeneralOrderDate = dismissal.GeneralOrders.GeneralOrderDate.GetValueOrDefault();
                    reportData.GeneralOrderNumber = dismissal.GeneralOrders.GeneralOrderName;
                    reportData.DismissalDate = dismissal.FireDate.GetValueOrDefault();
                }
                if (vacations != null)
                {
                    reportData.VacationDaysLeft = vacations.VacationDays.GetValueOrDefault();
                }
                if (companyInfo != null)
                {
                    reportData.CompanyAddr = companyInfo.CompanyAdress;
                    reportData.CompanyCity = companyInfo.CompanyCity;
                    reportData.CompanyName = companyInfo.CompanyName;
                    reportData.CompanyPhone = companyInfo.CompanyPhine;
                    reportData.CompanyEDRP = companyInfo.CompanyEDRP;
                }

                return reportData;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public EmpCardViewEdit(bool isCreationWindow, int selectedEmpId)
        {
            InitializeComponent();
            Context = new HRWorkEntities();
            Context.Database.CommandTimeout = 0;
            if (!isCreationWindow)
            {
                SelectedEmpId = selectedEmpId;
                IsCreationWindow = isCreationWindow;
                FillFields();
            }
            else
            {
                IsCreationWindow = isCreationWindow;
            }

        }
        private void ButtonVisibility(bool isVisible)
        {
            if (isVisible)
            {
                BTN_SelectDivision.Visibility = Visibility.Visible;
                BTN_SelectEduLevel.Visibility = Visibility.Visible;
                BTN_SelectPosition.Visibility = Visibility.Visible;
                BTN_SelectImage.Visibility = Visibility.Visible;
                BTN_SaveChanges.Visibility = Visibility.Visible;
            }
            else
            {
                BTN_SelectDivision.Visibility = Visibility.Hidden;
                BTN_SelectEduLevel.Visibility = Visibility.Hidden;
                BTN_SelectPosition.Visibility = Visibility.Hidden;
                BTN_SelectImage.Visibility = Visibility.Hidden;
                BTN_SaveChanges.Visibility = Visibility.Hidden;
            }
        }
        private void EditVisibility(bool isEditMode)
        {
            if (isEditMode)
            {
                MN_ActionsMenu.Visibility = Visibility.Hidden;
                ButtonVisibility(true);
                TB_LastName.IsEnabled = true;
                TB_Name.IsEnabled = true;
                TB_MiddleName.IsEnabled = true;
                DP_Bday.IsEnabled = true;
                DP_Bday.IsEnabled = true;
                TB_Experience.IsEnabled = true;
                TB_Speciality.IsEnabled = true;
                TB_Salary.IsEnabled = true;
            }
            else
            {
                MN_ActionsMenu.Visibility = Visibility.Visible;
                ButtonVisibility(false);
                TB_LastName.IsEnabled = false;
                TB_Name.IsEnabled = false;
                TB_MiddleName.IsEnabled = false;
                DP_Bday.IsEnabled = false;
                DP_Bday.IsEnabled = false;
                TB_Experience.IsEnabled = false;
                TB_Speciality.IsEnabled = false;
                TB_Salary.IsEnabled = false;
            }

        }
        private void FillFields()
        {
            try
            {
                Employes employee = Context.Employes.FirstOrDefault(x => x.Id == SelectedEmpId);
                EduLevelId = (int)employee.EduLevelId;
                PositionId = (int)employee.PositionId;
                DivisionId = (int)employee.DivisionId;

                TB_LastName.Text = employee.LastName;
                TB_Name.Text = employee.FirstName;
                TB_MiddleName.Text = employee.MiddleName;
                DP_Bday.SelectedDate = employee.BirthDate.GetValueOrDefault();
                DP_Bday.DisplayDate = employee.BirthDate.GetValueOrDefault();
                TB_Experience.Text = employee.PriorWorkYears.ToString();
                TB_EduLevel.Text = Context.EduLevels.FirstOrDefault(x => x.Id == employee.EduLevelId).EduLevelName;
                TB_Speciality.Text = employee.Speciality;
                TB_Divison.Text = Context.Divisions.FirstOrDefault(x => x.Id == DivisionId).DivisionName;
                TB_Position.Text = Context.Positions.FirstOrDefault(x => x.Id == PositionId).PositionName;
                DP_HireDate.DisplayDate = employee.HireDate.GetValueOrDefault();
                DP_HireDate.SelectedDate = employee.HireDate.GetValueOrDefault();
                TB_Salary.Text = employee.SalaryLevel.ToString();
                var appointments = Context.Appointments.Where(x => x.EmployeeId == SelectedEmpId);
                if (appointments != null && appointments.Count() > 0)
                {
                    DP_AppDate.SelectedDate = appointments.ToList().Last().AppDate;
                }
                if (employee.FiredFlag == true)
                {
                    CB_Fired.IsChecked = true;
                    DP_FireDate.Visibility = Visibility.Visible;
                    DP_FireDate.SelectedDate = employee.FiredDate.GetValueOrDefault();
                    DP_FireDate.DisplayDate = employee.FiredDate.GetValueOrDefault();
                }
                if (IMG_Photo.Source == null)
                {
                    Uri photofromDb = PhotoWorx.LoadPhotoFromDB(Context, SelectedEmpId);
                    IMG_Photo.Source = new BitmapImage(photofromDb ?? new Uri("pack://application:,,,/Resources/StockPhoto.jpg"));
                    ImageHash = IMG_Photo.Source.GetHashCode();
                }
                
            }
            catch (Exception) { }
        }
        private void SaveChangesAfterEdit()
        {
            try
            {
                Employes emp = Context.Employes.FirstOrDefault(x => x.Id == SelectedEmpId);
                emp.FirstName = TB_Name.Text;
                emp.LastName = TB_LastName.Text;
                emp.MiddleName = TB_MiddleName.Text;
                emp.BirthDate = DP_Bday.SelectedDate;
                emp.PriorWorkYears = Int32.Parse(TB_Experience.Text);
                if (emp.EduLevelId != EduLevelId)
                {
                    emp.EduLevelId = EduLevelId;
                }
                if (emp.PositionId != EduLevelId)
                {
                    emp.PositionId = PositionId;
                }
                if (emp.DivisionId != DivisionId)
                {
                    emp.DivisionId = DivisionId;
                }
                if (emp.SalaryLevel != Double.Parse(TB_Salary.Text))
                {
                    emp.SalaryLevel = Double.Parse(TB_Salary.Text);
                    Context.Salaries.Add(new Salaries() { SalaryChanged = DateTime.Now, EmployeeID = SelectedEmpId, SalaryLevel = Double.Parse(TB_Salary.Text) });
                }
                if (IMG_Photo.Source.GetHashCode() != ImageHash && NewImageName != null)
                {
                    PhotoWorx.SavePhotoToDB(NewImageName, Context, SelectedEmpId);
                }
                Context.SaveChanges();
            }
            catch (Exception) { MessageBox.Show("Возникла ошибка!"); }

        }
        private void SaveChangesNewEmp()
        {
            Employes emp = new Employes();
            try
            {
                emp.FirstName = TB_Name.Text;
                emp.LastName = TB_LastName.Text;
                emp.MiddleName = TB_MiddleName.Text;
                emp.BirthDate = DP_Bday.SelectedDate;
                emp.PriorWorkYears = Int32.Parse(TB_Experience.Text);
                if (emp.EduLevelId != EduLevelId)
                {
                    emp.EduLevelId = EduLevelId;
                }
                if (emp.PositionId != EduLevelId)
                {
                    emp.PositionId = PositionId;
                }
                if (emp.DivisionId != DivisionId)
                {
                    emp.DivisionId = DivisionId;
                }
                emp.Speciality = TB_Speciality.Text;
                emp.HireDate = DateTime.Now.Date;
                emp.SalaryLevel = Double.Parse(TB_Salary.Text);
                emp.FiredFlag = false;
                emp.DeletionFlag = false;
                Context.Salaries.Add(new Salaries() { SalaryChanged = DateTime.Now, EmployeeID = SelectedEmpId, SalaryLevel = Double.Parse(TB_Salary.Text) });
                Context.Employes.Add(emp);
                Context.SaveChanges();
            }
            catch (Exception) { MessageBox.Show("Возникла ошибка!"); }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditMode = true;
        }
        private void BTN_SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (IsCreationWindow == false)
            {
                EditMode = false;
                SaveChangesAfterEdit();
            }
            else
            {
                if (FieldsFilled())
                {
                    SaveChangesNewEmp();
                    MessageBox.Show("Струдник добавлен");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не все поля заполнены!");
                }
            }
        }
        private void BTN_SelectEduLevel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectionWindow selEduW = new SelectionWindow(Context.EduLevels.ToList(), "EduLevelId");
                selEduW.Owner = this;
                selEduW.ShowDialog();
                TB_EduLevel.Text = Context.EduLevels.FirstOrDefault(x => x.Id == EduLevelId).EduLevelName;
            }
            catch (Exception) { MessageBox.Show("Возникла ошибка!"); }

        }
        private void BTN_SelectDivision_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectionWindow selDivW = new SelectionWindow(Context.Divisions.ToList(), "DivisionId");
                selDivW.Owner = this;
                selDivW.ShowDialog();
                TB_Divison.Text = Context.Divisions.FirstOrDefault(x => x.Id == DivisionId).DivisionName;
            }
            catch (Exception) { MessageBox.Show("Возникла ошибка!"); }

        }
        private void BTN_SelectPosition_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectionWindow selPosW = new SelectionWindow(Context.Positions.ToList(), "PositionId");
                selPosW.Owner = this;
                selPosW.ShowDialog();

                TB_Position.Text = Context.Positions.FirstOrDefault(x => x.Id == PositionId).PositionName;
            }
            catch (Exception) { MessageBox.Show("Возникла ошибка!"); }

        }
        private void BTN_SelectImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP; *.PNG; *.JPG; *.GIF)| *.BMP; *.PNG; *.JPG; *.GIF";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != null && openFileDialog.FileName.Length > 0)
            {
                NewImageName = openFileDialog.FileName;
                IMG_Photo.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsCreationWindow)
            {
                EditVisibility(true);
                MN_ActionsMenu.Visibility = Visibility.Hidden;
                BRD_Border.Visibility = Visibility.Collapsed;
                IMG_Photo.Visibility = Visibility.Collapsed;
                BTN_SelectImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                MN_ActionsMenu.Visibility = Visibility.Visible;
            }
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            DismissalWindow dismissalWindow = new DismissalWindow(SelectedEmpId, Context);
            dismissalWindow.Owner = this;
            dismissalWindow.ShowDialog();
            FillFields();
        }
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var report = FillDataForReport();
            if (report != null)
            {
                HRReportGenerator.GenerateFireOrderCut(report);
            }
        }
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            var report = FillDataForReport();
            if (report != null)
            {
                HRReportGenerator.GenerateEmplStatusReportWorking(report);
            }
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            AppointWindow ap = new AppointWindow(SelectedEmpId, Context);
            ap.Owner = this;
            ap.ShowDialog();
            FillFields();
            //using (Context = new HRWorkEntities())
            //{
            //    try
            //    {
            //        Appointments appointments = Context.Appointments.LastOrDefault(x => x.EmployeeId == SelectedEmpId);
            //        DP_AppDate.SelectedDate = appointments.AppDate;
            //        Context.Employes.FirstOrDefault(x => x.Id == SelectedEmpId).LastAppDateID = appointments.Id;
            //    }
            //    catch (Exception) { }
            //}

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(Context != null)
            {
                Context.Dispose();
            }
        }
    }
}
