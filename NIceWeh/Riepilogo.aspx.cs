using NIceWeh.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NIceWeh
{
    public partial class Riepilogo : System.Web.UI.Page
    {
        public List<Employee> dipendenti { get; set; }
        public List<Customer> clienti { get; set; }
        ReportContext context;
        public int mese,anno;
        protected void Page_Load(object sender, EventArgs e)
        {
            context = new ReportContext();
            dipendenti = context.Employees.ToList();
            clienti = context.Customers.ToList();
            mese = int.Parse(Request.QueryString["m"]);
            anno = int.Parse(Request.QueryString["y"]);
        }
        public double GetOre(Customer cli, Employee dip)
        {
            var ss  =  context.Reports.Where(c => c.EmployeeId == dip.ID
                                && c.CustomerId == cli.Id
                                && c.Date.Month == mese
                                && c.Date.Year == anno)
                .Select(c => c.Hours);
            if (ss.Count() > 0)
                return ss.Sum();
            else
                return 0;
        }
    }
}