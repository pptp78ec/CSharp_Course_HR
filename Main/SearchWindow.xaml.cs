using DataClassModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Main
{
    /// <summary>
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
        }
        private int SelectedEmpId { get; set; } = default;

        private int EduLevelId { get; set; } = default;
        private int PositionId { get; set; } = default;
        private int DivisionId { get; set; } = default;
        private HRWorkEntities Context { get; set; }


        private void BTN_SelectEduLevel_Click(object sender, RoutedEventArgs e)
        {
            using (Context = new HRWorkEntities())
            {
                SelectionWindow selEduW = new SelectionWindow(Context.EduLevels.ToList(), "EduLevelId");
                selEduW.Owner = this;
                selEduW.ShowDialog();
                if (EduLevelId != default)
                {
                    TB_EduLevel.Text = Context.EduLevels.FirstOrDefault(x => x.Id == EduLevelId).EduLevelName;
                }
            }
        }


        private void BTN_SelectDivision_Click(object sender, RoutedEventArgs e)
        {
            using (Context = new HRWorkEntities())
            {
                SelectionWindow selDivW = new SelectionWindow(Context.Divisions.ToList(), "DivisionId");
                selDivW.Owner = this;
                selDivW.ShowDialog();
                if (DivisionId != default)
                {
                    TB_Divison.Text = Context.Divisions.FirstOrDefault(x => x.Id == DivisionId).DivisionName;
                }
            }
        }

        private void BTN_SelectPosition_Click(object sender, RoutedEventArgs e)
        {
            using (Context = new HRWorkEntities())
            {
                SelectionWindow selPosW = new SelectionWindow(Context.Positions.ToList(), "PositionId");
                selPosW.Owner = this;
                selPosW.ShowDialog();
                if (PositionId != default)
                {
                    TB_Position.Text = Context.Positions.FirstOrDefault(x => x.Id == PositionId).PositionName;
                }
            }
        }

        private List<Employes> SearchUser()
        {
            List<Employes> result = Context.Employes.ToList();

            if (TB_Name.Text.Count() > 0)
            {
                result = result.Where(x => x.FirstName.Contains(TB_Name.Text)).ToList();
            }
            if (TB_LastName.Text.Count() > 0)
            {
                result = result.Where(x => x.LastName.Contains(TB_Name.Text)).ToList();
            }
            if (TB_MiddleName.Text.Count() > 0)
            {
                result = result.Where(x => x.MiddleName.Contains(TB_MiddleName.Text)).ToList();
            }
            if (DP_Bday.SelectedDate != null)
            {
                result = result.Where(x => x.BirthDate.Equals(DP_Bday.SelectedDate)).ToList();
            }
            if (TB_Experience.Text.Count() > 0)
            {
                result = result.Where(x => x.PriorWorkYears.Value <= Convert.ToInt32(TB_Experience.Text)).ToList();
            }
            if (EduLevelId > 0)
            {
                result = result.Where(x => x.EduLevelId.Equals(EduLevelId)).ToList();
            }
            if (TB_Speciality.Text.Count() > 0)
            {
                result = result.Where(x => x.Speciality.Contains(TB_Speciality.Text)).ToList();
            }
            if (DivisionId > 0)
            {
                result = result.Where(x => x.DivisionId.Equals(DivisionId)).ToList();
            }
            if (PositionId > 0)
            {
                result = result.Where(x => x.PositionId.Equals(DivisionId)).ToList();
            }
            if (DP_HireDate.SelectedDate != null)
            {
                result = result.Where(x => x.HireDate.Equals(DP_HireDate.SelectedDate)).ToList();
            }
            if (TB_Salary.Text.Count() > 0)
            {
                result = result.Where(x => x.SalaryLevel.Value.ToString().Equals(TB_Salary.Text)).ToList();
            }
            if (CB_Fired.IsChecked.Value)
            {
                result = result.Where(x => x.FiredFlag.Equals(true)).ToList();
            }
            if (DP_FireDate.SelectedDate != null)
            {
                result = result.Where(x => x.FiredDate.Equals(DP_FireDate.SelectedDate)).ToList();
            }
            return result;
        }


        private void BTN_Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Context = new HRWorkEntities())
                {

                    var result = SearchUser();
                    if (result != null && result.Count > 0)
                    {
                        SelectionWindow selectUser = new SelectionWindow(result, "SelectedEmpId");
                        selectUser.Owner = this;
                        selectUser.ShowDialog();
                        if (SelectedEmpId != 0)
                        {
                            EmpCardViewEdit userCard = new EmpCardViewEdit(false, SelectedEmpId);
                            userCard.Owner = this;
                            userCard.ShowDialog();
                            SelectedEmpId = 0;
                        }
                        else { MessageBox.Show("Ничего не выбрано!"); }
                    }
                    else { MessageBox.Show("Ничего не найдено!"); }
                }
            }
            catch (Exception) { }
        }
    }
}
