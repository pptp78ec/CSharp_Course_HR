using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Words;
using System.IO;
using NameCaseLib;

namespace ReportModel
{
    /// <summary>
    /// Container class where necessary info for report is stored.
    /// </summary>
    public class ReportData
    {
        //Employee info
        public string EmpFirstName { get; set; } = "";
        public string EmpLastName { get; set; } = "";
        public string EmpMiddleName { get; set; } = "";

        public string EmpPosition { get; set; } = "";
        public string EmpDivision { get; set; } = "";
        public DateTime EmpDate { get; set; } = DateTime.Now.Date;
        public string AppOrderNumber { get; set; } = "";
        public DateTime AppOrderDate { get; set; } = DateTime.Now.Date;

        public int VacationDaysLeft { get; set; } = default;
        public DateTime DismissalDate { get; set; } = DateTime.Now.Date;

        //This order info
        public string CurrentOrderNumber { get; set; } = "";
        public DateTime CurrentOrderDate { get; set; } = DateTime.Now.Date;

        //Company info
        public string CeoFirstName { get; set; } = "";
        public string CeoLastName { get; set; } = "";
        public string CeoMiddleName { get; set; } = "";
        public string CeoPosition { get; set; } = "";

        public string CompanyName { get; set; } = "";
        public string CompanyCity { get; set; } = "";
        public string CompanyAddr { get; set; } = "";
        public string CompanyPhone { get; set; } = "";
        public string CompanyEDRP { get; set; } = "";

        public string HRHeadFirstName { get; set; } = "";
        public string HRHeadLastName { get; set; } = "";
        public string HRHeadMiddleName { get; set; } = "";
        public string HRHeadPosition { get; set; } = "";

        public string FinanceHeadFirstName { get; set; } = "";
        public string FinanceHeadLastName { get; set; } = "";
        public string FinanceHeadMiddleName { get; set; } = "";
        public string FinanceHeadPosition { get; set; } = "";

        //General order info
        public string GeneralOrderNumber { get; set; } = "";
        public DateTime GeneralOrderDate { get; set; } = DateTime.Now.Date;
        
    }
    /// <summary>
    /// Provides methods to create dismissal order documents based on provided data
    /// </summary>
    public static class HRReportGenerator
    {
        /// <summary>
        /// Creates a document with order to fire an employee in case of downsizing (staff cuts)
        /// </summary>
        /// <param name="data">ReportData class instance</param>
        /// <param name="savepath">path to save file. Default - this folder</param>
        public static void GenerateFireOrderCut(ReportData data, string savepath = ".") 
        {
            try
            {
                //building savepath. It's created from path of desired save folder + FireOrder mask and Last name with initials of the employee to be dismissed
                string fullSavePath = savepath + "\\FireOrder_" + data.EmpLastName + "_" + data.EmpFirstName[0] + "_" + data.EmpMiddleName[0] + ".docx" ;

                Document dismissalForm = new Document();
                DocumentBuilder builder = new DocumentBuilder(dismissalForm);

                Ua ukr = new Ua();
                ukr.FullReset();
                ukr.SetFullName(data.EmpLastName, data.EmpFirstName, data.EmpMiddleName);
                ukr.GenderAutoDetect();
                string firstName2ndCase = ukr.GetFirstNameCase()[1];
                string lastName2ndCase = ukr.GetSecondNameCase()[1];
                string middleName2ndCase = ukr.GetFatherNameCase()[1];

                //Global font params
                Font font = builder.Font;
                font.Color = System.Drawing.Color.Black;
                font.Name = "Times New Roman";

                //Local Font params. Here we paste a company name as header
                font.Size = 18;
                font.Bold = true;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Writeln(data.CompanyName);
                
                //Order
                builder.Writeln("");
                builder.Writeln("НАКАЗ");

                //Date of order + Company City Name + order number for this employee (do not confuse with general order);
                font.Size = 14;
                font.Bold = false;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Distributed;
                builder.Writeln("");
                builder.Writeln(data.CurrentOrderDate.ToString("dd.MM.yyyy") + " " + data.CompanyCity + " №" + data.CurrentOrderNumber);

                //Order's short annotation
                font.Italic = true;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
                builder.Writeln("");
                builder.Writeln("Щодо звільнення " + lastName2ndCase + " " + data.EmpFirstName[0] + ". " + data.EmpMiddleName[0] + ".");

                //Order's task
                font.Italic = false;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
                builder.Writeln("");
                builder.Writeln("ЗВІЛЬНИТИ:");

                //Maintext
                builder.ParagraphFormat.CharacterUnitFirstLineIndent = 3;
                builder.Writeln("");
                string mainText = $"{lastName2ndCase.ToUpper()} {firstName2ndCase} {middleName2ndCase} {data.EmpPosition} {data.EmpDivision}, {data.DismissalDate.ToString("dd.MM.yyyy")}р. у зв'язку"
                    + $" зі скороченням чисельності працівників, за п. 1 ст. 40 КЗпП, з виплатою вихідної допомоги в розмірі середнього місячного заробітку за ст. 44 КЗпП"
                    + $" та грошової компенсації за {data.VacationDaysLeft} календарних днів невикористаної щорічної основної відпустки за період работи з"
                    + $" {data.CurrentOrderDate.AddYears(-1).ToString("dd.MM.yyyy")}р. по {data.CurrentOrderDate.ToString("dd.MM.yyyy")}р.";
                builder.Writeln(mainText);

                //Cause
                builder.Writeln("");
                builder.Writeln("Підстава:");
                builder.ParagraphFormat.CharacterUnitFirstLineIndent = 8;
                builder.ListFormat.ApplyNumberDefault();
                if (data.GeneralOrderNumber != null) {
                    builder.Writeln($"Наказ №{data.GeneralOrderNumber} від {data.GeneralOrderDate.ToString("dd.MM.yyyy")}р.;");
                }
                builder.Writeln("Письмове повідомлення про вивільнення;");
                builder.ListFormat.RemoveNumbers();

                //CEO signature
                builder.Writeln("");
                builder.Writeln("");
                builder.ParagraphFormat.CharacterUnitFirstLineIndent = -8;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.Writeln($"{data.CeoPosition}\t\t\t\t\t\t\t\t\t{data.CeoFirstName[0]}. {data.CeoMiddleName[0]}. {data.CeoLastName}");

                //Saving the document
                dismissalForm.Save(fullSavePath);
            }
            catch (Exception ex) 
            {

            }

        }
        /// <summary>
        /// Creates a document with report about employee if employee is still working
        /// </summary>
        /// <param name="data">ReportData class instance</param>
        /// <param name="savepath">path to save file. Default - this folder</param>
        public static void GenerateEmplStatusReportWorking(ReportData data, string savepath = ".")
        {
            try
            {
                string fullSavePath = savepath + "\\StatusReport_" + data.EmpLastName + "_" + data.EmpFirstName[0] + "_" + data.EmpMiddleName[0] + ".docx";
                Document statReportWorking = new Document();
                DocumentBuilder builder = new DocumentBuilder(statReportWorking);
                Ua ukr = new Ua();
                ukr.FullReset();
                ukr.SetFullName(data.EmpLastName, data.EmpFirstName, data.EmpMiddleName);
                ukr.GenderAutoDetect();
                string firstName2ndCase = ukr.GetFirstNameCase()[1];
                string lastName2ndCase = ukr.GetSecondNameCase()[1];
                string middleName2ndCase = ukr.GetFatherNameCase()[1];

                Font font = builder.Font;
                font.Color = System.Drawing.Color.Black;
                font.Name = "Times New Roman";

                font.Size = 18;
                font.Bold = true;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.Writeln(data.CompanyName);

                font.Size = 14;
                font.Bold = false;
                builder.Writeln("");
                builder.Writeln($"{data.CompanyAddr}, м. {data.CompanyCity}");
                builder.Writeln($"Тел./факс: {data.CompanyPhone}");
                builder.Writeln("");
                builder.Writeln($"Код за ЄДРПОУ: {data.CompanyEDRP}");

                builder.Writeln("");
                builder.Writeln("");
                font.Size = 18;
                font.Bold = true;
                builder.Writeln("ДОВІДКА");

                builder.Writeln("");
                font.Bold = false;
                font.Size = 14;
                builder.Writeln($"{data.CurrentOrderDate.ToString("dd.MM.yyyy")} №{data.CurrentOrderNumber}");
                builder.Writeln($"м. {data.CompanyCity}");

                builder.Writeln("");
                builder.Writeln("");
                font.Italic = true;
                builder.Writeln("Про підтвердження місця");
                builder.Writeln($"роботи {lastName2ndCase} {firstName2ndCase[0]}. {middleName2ndCase[0]}.");

                builder.Writeln("");
                font.Italic = false;
                string maintext = $"{data.EmpLastName.ToUpper()} {data.EmpFirstName} {data.EmpMiddleName} дійсно працює на посаді {data.EmpPosition} {data.EmpDivision}"
                    + $" {data.CompanyName} із {data.AppOrderDate.ToString("dd.MM.yyyy")}р.(наказ {data.AppOrderNumber} від {data.AppOrderDate}р.).";
                builder.ParagraphFormat.CharacterUnitFirstLineIndent = 3;
                builder.Writeln(maintext);

                builder.Writeln("");
                builder.Writeln("");
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.ParagraphFormat.CharacterUnitFirstLineIndent = -3;
                builder.Writeln($"{data.CeoPosition}\t\t\t\t\t\t\t\t\t{data.CeoFirstName[0]}. {data.CeoMiddleName[0]}. {data.CeoLastName}");
                builder.Writeln("");
                builder.Writeln("");
                builder.Writeln($"{data.HRHeadPosition} відділу кадрів\t\t\t\t\t\t\t{data.HRHeadFirstName[0]}. {data.HRHeadMiddleName[0]}. {data.HRHeadLastName}");

                statReportWorking.Save(fullSavePath);
            }
            catch (Exception) { }

        }
    }
}
