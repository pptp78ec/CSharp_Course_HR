﻿using DataClassModel;
using System;
using System.Linq;
using System.Windows;

namespace Main
{
    /// <summary>
    /// Логика взаимодействия для DismissalWindow.xaml
    /// </summary>
    public partial class DismissalWindow : Window
    {
        public DismissalWindow()
        {
            InitializeComponent();
        }
        public DismissalWindow(int selEmplId, HRWorkEntities context)
        {
            InitializeComponent();
            SelEmplId = selEmplId;
            Context = context;
        }
        public int SelEmplId { get; set; }

        public int FireCauseId { get; set; }
        public int GenOrderId { get; set; }
        private HRWorkEntities Context { get; set; }

        private void DoDismissal()
        {

            try
            {
                Fires dismissal = new Fires();
                dismissal.EmployeeID = SelEmplId;
                dismissal.FireOrderName = TB_Order.Text;
                dismissal.CauseID = FireCauseId;
                dismissal.CauseAddInfo = TB_AddInfo.Text;
                dismissal.GeneralFireOrderId = GenOrderId;
                dismissal.FireDate = DP_FireDate.SelectedDate.Value;
                dismissal.DeletionFlag = false;
                Context.Fires.Add(dismissal);
                var employee = Context.Employes.FirstOrDefault(x => x.Id.Equals(SelEmplId));
                employee.FiredDate = DP_FireDate.SelectedDate.Value;
                employee.FiredFlag = true;
                Context.SaveChanges();
            }
            catch (Exception) { }

        }

        private void BTN_Fire_Click(object sender, RoutedEventArgs e)
        {
            DoDismissal();
            Close();
        }

        private void BTN_SelFireCause_Click(object sender, RoutedEventArgs e)
        {
            SelectionWindow selFCW = new SelectionWindow(Context.FireCauses.ToList(), "FireCauseId");
            selFCW.Owner = this;
            selFCW.ShowDialog();
            TB_FireCause.Text = Context.FireCauses.FirstOrDefault(x => x.Id == FireCauseId).CauseName;

        }

        private void BTN_SelGenOrder_Click(object sender, RoutedEventArgs e)
        {
            SelectionWindow selFCW = new SelectionWindow(Context.GeneralOrders.ToList(), "GenOrderId");
            selFCW.Owner = this;
            selFCW.ShowDialog();
            TB_GenOrder.Text = Context.GeneralOrders.FirstOrDefault(x => x.Id == GenOrderId).GeneralOrderName;

        }
    }
}
