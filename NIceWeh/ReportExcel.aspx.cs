using NIceWeh.Model;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NIceWeh
{
    public partial class ReportExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var mese = int.Parse(Request.QueryString["m"]);
            var anno = int.Parse(Request.QueryString["y"]);
            var queryEmployee = Request.QueryString["e"];
            var context = new ReportContext();
            var Employee = context.Employees.Where(c => c.QueryTerm.ToLower() == queryEmployee.ToLower()).SingleOrDefault();
            var festivi = Utils.DateTimeExtensions.GetHolidays(new int[] { anno }, "IT", "Milano");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Set the default application version as Excel 2016
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2016;

                //Create a workbook with a worksheet
                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);

                //Access first worksheet from the workbook instance
                IWorksheet worksheet = workbook.Worksheets[0];
                worksheet.Range["A1"].Text = "Giorno";
                worksheet.Range["B1"].Text = "Ore";
                worksheet.Range["C1"].Text = "Ferie";
                worksheet.Range["D1"].Text = "Permesso";
                worksheet.Range["E1"].Text = "Malattia";

                var numgiorni = DateTime.DaysInMonth(anno, mese);
                for(int i = 1; i <= numgiorni; i++)
                {
                    var date = new DateTime(anno, mese, i);
                    worksheet.Range["A" + (i + 1)].Text = date.ToString("dddd dd MMMM yyyy");
                    if (festivi.Contains(date)) {
                        IStyle style = workbook.Styles.Add("NewStyle");
                        style.FillBackgroundRGB = Color.Yellow;
                        worksheet.Range["A" + (i + 1)].CellStyle = style;
                     }
                    var numorelavq = context.Reports.Where(c =>
                                    c.Date.Year == anno
                                    && c.Date.Month == mese
                                    && c.Date.Day == i
                                    && c.Activity.Descrizione != "Ferie"
                                    && c.Activity.Descrizione != "Permesso"
                                    && c.Activity.Descrizione != "Malattia"
                                    && c.EmployeeId == Employee.ID).Select(c => c.Hours);
                    var numorelav = numorelavq.Count() > 0 ? numorelavq.Sum() : 0;
                    var numoreferieq = context.Reports.Where(c =>
                                                        c.Date.Year == anno
                                                        && c.Date.Month == mese
                                                        && c.Date.Day == i
                                                        && c.Activity.Descrizione == "Ferie"
                                                        && c.EmployeeId == Employee.ID).Select(c => c.Hours);
                    var numoreferie = numoreferieq.Count() > 0 ? numoreferieq.Sum() : 0;
                    var numorepermessq = context.Reports.Where(c =>
                                                        c.Date.Year == anno
                                                        && c.Date.Month == mese
                                                        && c.Date.Day == i
                                                        && c.Activity.Descrizione == "Permesso"
                                                        && c.EmployeeId == Employee.ID).Select(c => c.Hours);
                    var numorepermess = numorepermessq.Count() > 0 ? numorepermessq.Sum() : 0;
                    var numoremalattiaq = context.Reports.Where(c =>
                                                        c.Date.Year == anno
                                                        && c.Date.Month == mese
                                                        && c.Date.Day == i
                                                        && c.Activity.Descrizione == "Malattia"
                                                        && c.EmployeeId == Employee.ID).Select(c => c.Hours);
                    var numoremalattia = numoremalattiaq.Count() > 0 ? numoremalattiaq.Sum() : 0;

                    worksheet.Range["B" + (i + 1)].Text = numorelav.ToString();
                    worksheet.Range["C" + (i + 1)].Text = numoreferie.ToString();
                    worksheet.Range["D" + (i + 1)].Text = numorepermess.ToString();
                    worksheet.Range["E" + (i + 1)].Text = numoremalattia.ToString();
                }                         
                

                //Save the workbook to disk in xlsx format
                workbook.SaveAs("ReportOre.xlsx", Response, ExcelDownloadType.Open, ExcelHttpContentType.Excel2016);
            }
        }
    }
}