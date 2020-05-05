
using Newtonsoft.Json;
using NIceWeh.Model;
using NIceWeh.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebGrease.Css.Extensions;

namespace NIceWeh
{
    public partial class _Default : Page
    {
        private Employee CurrentEmployee;
        private int CurrentRapId;
        protected void Page_Load(object sender, EventArgs e)
        {
            var con = new ReportContext();
            Calendar1.SelectedDate = DateTime.Now;
            Calendar1.VisibleDate = DateTime.Now;
            if (!string.IsNullOrEmpty(Request.QueryString[null]))
            {
                string searchTerm = Request.QueryString[null];
                this.CurrentEmployee = con.Employees.Where(c => c.QueryTerm.ToUpper() == searchTerm.ToUpper()).SingleOrDefault();

            }
            var selq = con.Employees.ToList().Select(c => new Employee { Nome = c.Nome + " " + c.Cognome, ID = c.ID }).ToList();

            Employee.DataSource = selq;
            selq.Add(new Model.Employee { ID = 0, Nome = "" });
            Employee.DataTextField = "Nome";
            Employee.DataValueField = "ID";
            Employee.DataBind();
            Employee.SelectedValue = CurrentEmployee?.ID.ToString();
            if (CurrentEmployee == null)
                return;
            var sel = con.Customers.Select(c => c).ToList();
            sel.Add(new Model.Customer { Id = 0, Nome = "" });
            Customer.DataSource = sel;
            Customer.DataTextField = "Nome";
            Customer.DataValueField = "ID";
            Customer.DataBind();
            Customer.SelectedValue = "0";

            var selx = con.Activities.Select(c => c).ToList();

            Activity.DataSource = selx;
            selx.Add(new Model.Activity { Id = 0, Descrizione = "" });
            Activity.DataTextField = "Descrizione";
            Activity.DataValueField = "ID";
            Activity.DataBind();
            Activity.SelectedValue = "0";


            //CaricaRapportini(con);
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            ViewState["year"] = Calendar1.VisibleDate.Year;
            ViewState["month"] = Calendar1.VisibleDate.Month;
            Datehidden.Text = Calendar1.SelectedDate.ToString("yyyy/MM/dd");
            if (CurrentEmployee == null)
                return;
            var con = new ReportContext();
            CaricaRapportini(con);
        }
        private void CaricaRapportini(ReportContext con)
        {
            var mese = Calendar1.VisibleDate.Month;
            var anno = Calendar1.VisibleDate.Year;
            var numg  = DateTime.DaysInMonth(anno, mese);
            var table = new Table();
            var festivi = DateTimeExtensions.GetHolidays(new int[] { anno }, "IT", "Milan");
            var rapportini = con.Reports.Where(c => c.Date.Month == mese && c.Date.Year == anno 
                                                && c.EmployeeId == CurrentEmployee.ID)
                    .ToList();
            for (int i = 1;i<= numg; i++)
            {
                var dt = new DateTime(anno, mese, i);
                var row = new TableRow();
                if (dt == Calendar1.SelectedDate)
                    row.CssClass += " selected ";
                int numspan = rapportini.Where(c => dt == new DateTime(c.Date.Year, c.Date.Month, c.Date.Day)).Count();
                //if (numspan == 0)
                //{
                var controldatacell = new TableCell();
                controldatacell.Controls.Add(new LiteralControl(dt.ToString("dd/MM ddd")));
                controldatacell.RowSpan = numspan+1;
                row.Cells.Add(new TableCell());
                row.Cells.Add(controldatacell);
            //controldatacell.ColumnSpan = 7;
                var restcel = new TableCell();
                restcel.Controls.Add(new LiteralControl("-"));
                restcel.ColumnSpan = 6;
                row.Cells.Add(restcel);
                if (dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday || festivi.Contains(dt))
                {
                    row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FA2");
                }
                ReptTable.Rows.Add(row);
                //}
                //else
                //{
                var dateinserted = false;
                foreach (var rapp in rapportini)
                {
                    if (EInData(rapp, dt))
                    {
                        var rrow = new TableRow();
                        rrow.CssClass += "report-td";
                        var controlcell = new TableCell();
                        var button = new HtmlGenericControl();
                        button.InnerText = "Edit";
                        button.Attributes["class"] = "command-action";
                        var json = JsonConvert.SerializeObject(rapp);
                        json = json.Replace('"', '\"');
                        button.Attributes["onClick"] = "event.stopPropagation();return Edit('" + json+ "')";
                        button.Attributes["href"] = "#";
                        button.Attributes["class"] = "hidden";
                        controlcell.Controls.Add(button);
                        rrow.Cells.Add(controlcell);
                        var datacell = new TableCell();
                        datacell.Controls.Add(new LiteralControl(dt.ToString("dd/MM ddd")));
                        datacell.RowSpan = numspan;
                        //if (!dateinserted)
                        //{
                        //    rrow.Cells.Add(datacell);
                        //}
                        var attivitacell = new TableCell();
                        attivitacell.Controls.Add(new LiteralControl(rapp.Activity.ToString()));
                        rrow.Cells.Add(attivitacell);
                        var clientecell = new TableCell();
                        clientecell.Controls.Add(new LiteralControl(rapp.Customer.ToString()));
                        rrow.Cells.Add(clientecell);
                        var orecell = new TableCell();
                        orecell.Controls.Add(new LiteralControl(rapp.Hours.ToString()));
                        rrow.Cells.Add(orecell);
                        var notecell = new TableCell();
                        notecell.Controls.Add(new LiteralControl(rapp.Notes.ToString()));
                        rrow.Cells.Add(notecell);
                        notecell.CssClass = "report-notes";
                        var noterapcell = new TableCell();
                        noterapcell.Controls.Add(new LiteralControl(rapp.ReportNotes.ToString()));
                        rrow.Cells.Add(noterapcell);
                        if (!dateinserted)
                        {
                            dateinserted = true;
                            var totg = rapportini.Where(c => dt == new DateTime(c.Date.Year, c.Date.Month, c.Date.Day))
                                .Select(c=>c.Hours).Sum();

                            var totcell = new TableCell();
                            totcell.Controls.Add(new LiteralControl(totg.ToString()));
                            totcell.RowSpan = numspan;
                            rrow.Cells.Add(totcell);
                        }
                        ReptTable.Rows.Add(rrow);
                    }
                }
                //}
            }



        }

        private bool EInData(Report rapp, DateTime dt)
        {
            var ddt = new DateTime(rapp.Date.Year,rapp.Date.Month,rapp.Date.Day);
            if (dt == ddt)
                return true;
            return false;
        }

        protected void ReptTable_RowEditing(object sender, GridViewEditEventArgs e)
        {
            TableRow row = ReptTable.Rows[e.NewEditIndex];
        }
        protected void ReptTable_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            TableRow row = ReptTable.Rows[e.RowIndex];
        }

        protected void ButtonSalva_Click(object sender, EventArgs e)
        {
            DateTime? dt = null;
            if (string.IsNullOrEmpty(Request[Datehidden.UniqueID]))
                dt = DateTime.Now;
            else
                dt = DateTime.Parse(Request[Datehidden.UniqueID]);
            var context = new ReportContext();
            if (!CheckMandatoryFields())
            {
                ViewState["mandatory"] = "true";
                return;
            }
            else
                ViewState["mandatory"] = "false";
            var rapp = new Report
            {
                ActivityId = int.Parse(Request[Activity.UniqueID]),
                CustomerId = int.Parse(Request[Customer.UniqueID]),
                Date = dt.Value,
                EmployeeId = CurrentEmployee.ID,
                Hours = int.Parse(Request[Hours.UniqueID]),
                Notes = Request[Notes.UniqueID],
                ReportNotes = Request[ReportNotes.UniqueID]
            };
            var t = Request[Activity.UniqueID];
            if (!string.IsNullOrEmpty(Request[IdReport.UniqueID]))
                rapp.Id = int.Parse(Request[IdReport.UniqueID]);
            if (rapp.Id > 0)
            {
                var rapdb = context.Reports.Where(c => c.Id == rapp.Id).SingleOrDefault();
                rapdb.ActivityId = rapp.ActivityId;
                rapdb.CustomerId = rapp.CustomerId;
                rapdb.Date = rapp.Date;
                rapdb.EmployeeId = rapp.EmployeeId;
                rapdb.Hours = rapp.Hours;
                rapdb.Notes = rapp.Notes;
                rapdb.ReportNotes = rapp.ReportNotes;
            }
            else
                context.Reports.Add(rapp);
            context.SaveChanges();
            BlankAll();
        }

        private bool CheckMandatoryFields()
        {
            try
            {
                var activity = int.Parse(Request[Activity.UniqueID]);
                var customer = int.Parse(Request[Customer.UniqueID]);
                var employee = CurrentEmployee.ID;
                var hours = int.Parse(Request[Hours.UniqueID]);
                var motes = Request[Notes.UniqueID];
                var reportNotes = Request[ReportNotes.UniqueID];
                if (activity == 0
                    || customer == 0
                    || employee == 0
                    || hours == 0
                    || string.IsNullOrEmpty(motes))
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void BlankAll()
        {
            Notes.Text = "";
            ReportNotes.Text = "";
            Hours.Text = "0";
            Datehidden.Text = "";
            IdReport.Text = "";
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            this.Datehidden.Text = Calendar1.SelectedDate.ToString("yyyy/MM/dd");
        }

        protected void ButtonElimina_Click(object sender, EventArgs e)
        {
            if (Request[ConfirmDelete.UniqueID].ToLower() != "true")
                return;
            int id = 0;
            if (!string.IsNullOrEmpty(Request[IdReport.UniqueID]))
                id = int.Parse(Request[IdReport.UniqueID]);
            else
                return;
            var context = new ReportContext();
            context.Reports.Remove(context.Reports.Where(c => c.Id == id).SingleOrDefault());
            context.SaveChanges();
            BlankAll();
        }
        protected void ButtonRiepilogo_Click(object sender, EventArgs e)
        {
        }

        protected void Employee_SelectedIndexChanged(object sender, EventArgs e)
        {
            var emppid = int.Parse(Request[Employee.UniqueID]);
            var context = new ReportContext();
            var employee = context.Employees.Where(c => c.ID == emppid).SingleOrDefault();
            Response.Redirect("/?" + employee.QueryTerm);
        }
    }
}