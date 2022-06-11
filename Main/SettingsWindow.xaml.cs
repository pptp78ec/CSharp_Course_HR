using DataClassModel;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Main
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            LoadSettings();
        }
        private HRWorkEntities Context { set; get; }

        public int SelCEOId { set; get; } = default;
        public int SelFinId { set; get; } = default;
        public int SelHRId { set; get; } = default;

        private Regex rgx_edrp_input = new Regex(@"[0-9]");
        private Regex rgx_edrp_total = new Regex(@"^[0-9]{0,11}$");
        private Regex rgx_phone_input = new Regex(@"[0-9-+()]");
        private Regex rgx_phone_total = new Regex(@"^[0-9-+()]{0,19}$");
        private Regex rgx_maxsize100 = new Regex(@"^[\d\D\w\D\s\S]{0,99}$");
        private void LoadSettings()
        {
            using (Context = new HRWorkEntities())
            {
                try
                {
                    var settings = Context.CompanyInfo.First();

                    TB_CompanyName.Text = settings.CompanyName;
                    TB_Addr.Text = settings.CompanyAdress;
                    TB_City.Text = settings.CompanyCity;
                    TB_EDRP.Text = settings.CompanyEDRP;
                    TB_Phone.Text = settings.CompanyPhine;
                    SelCEOId = settings.CompanyHeadId.Value;
                    SelFinId = settings.CompanyFinHeadId.Value;
                    SelHRId = settings.CompanyHRHeadId.Value;
                    var CEO = Context.Employes.FirstOrDefault(x => x.Id == SelCEOId);
                    TB_CEO.Text = CEO.LastName + " " + CEO.FirstName + " " + CEO.MiddleName;
                    var HR = Context.Employes.FirstOrDefault(x => x.Id == SelHRId);
                    TB_HRHead.Text = HR.LastName + " " + HR.FirstName + " " + HR.MiddleName;
                    var Fin = Context.Employes.FirstOrDefault(x => x.Id == SelFinId);
                    TB_FinHead.Text = Fin.LastName + " " + Fin.FirstName + " " + Fin.MiddleName;
                }
                catch (Exception) { }
            }
        }

        private void SaveSettings()
        {
            using (Context = new HRWorkEntities())
            {
                try
                {
                    if (Context.CompanyInfo.Count() != 0)
                    {
                        var settings = Context.CompanyInfo.First();
                        settings.CompanyHeadId = SelCEOId;
                        settings.CompanyFinHeadId = SelFinId;
                        settings.CompanyHRHeadId = SelHRId;
                        settings.CompanyName = TB_CompanyName.Text;
                        settings.CompanyAdress = TB_Addr.Text;
                        settings.CompanyCity = TB_City.Text;
                        settings.CompanyEDRP = TB_EDRP.Text;
                        settings.CompanyPhine = TB_Phone.Text;

                    }
                    else
                    {
                        CompanyInfo settings = new CompanyInfo();
                        settings.CompanyHeadId = SelCEOId;
                        settings.CompanyFinHeadId = SelFinId;
                        settings.CompanyHRHeadId = SelHRId;
                        settings.CompanyName = TB_CompanyName.Text;
                        settings.CompanyAdress = TB_Addr.Text;
                        settings.CompanyCity = TB_City.Text;
                        settings.CompanyEDRP = TB_EDRP.Text;
                        settings.CompanyPhine = TB_Phone.Text;
                        Context.CompanyInfo.Add(settings);
                    }
                    Context.SaveChanges();
                }
                catch (Exception) { }
            }
        }

        private int GetProp(string _retPropName)
        {
            switch (_retPropName)
            {
                case "SelCEOId":
                    {
                        return SelCEOId;

                    }
                case "SelFinId":
                    {
                        return SelFinId;
                    }
                case "SelHRId":
                    {
                        return SelHRId;
                    }
                default:
                    return 1;
            }
        }

        private void SelectButtonClickFromEmployes(HRWorkEntities _context, string _retValueName, TextBox _textBox)
        {
            try
            {
                using (_context = new HRWorkEntities())
                {
                    SelectionWindow selection = new SelectionWindow(_context.Employes.ToList(), _retValueName);
                    selection.Owner = this;
                    selection.ShowDialog();
                    int prop = GetProp(_retValueName);
                    var result = _context.Employes.FirstOrDefault(x => x.Id == prop);
                    _textBox.Text = result.LastName + " " + result.FirstName + " " + result.MiddleName;
                }
            }
            catch (Exception) { }
        }

        private void BTN_SelCEO_Click(object sender, RoutedEventArgs e)
        {
            SelectButtonClickFromEmployes(Context, "SelCEOId", TB_CEO);
        }

        private void BTN_SelHR_Click(object sender, RoutedEventArgs e)
        {
            SelectButtonClickFromEmployes(Context, "SelHRId", TB_HRHead);
        }

        private void BTN_SelFin_Click(object sender, RoutedEventArgs e)
        {
            SelectButtonClickFromEmployes(Context, "SelFinId", TB_FinHead);
        }

        private void BTN_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void TB_EDRP_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = rgx_edrp_input.IsMatch(e.Text) && rgx_edrp_total.IsMatch(TB_EDRP.Text) ? false : true;
        }

        private void TB_Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = rgx_phone_input.IsMatch(e.Text) && rgx_phone_total.IsMatch(TB_Phone.Text) ? false : true;
        }

        private void TB_CompanyName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = rgx_maxsize100.IsMatch(TB_CompanyName.Text) ? false : true;
        }

        private void TB_Addr_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = rgx_maxsize100.IsMatch(TB_Addr.Text) ? false : true;
        }

        private void TB_City_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = rgx_maxsize100.IsMatch(TB_City.Text) ? false : true;
        }
    }
}
