using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NIceWeh.Model
{
    public class ReportDatabaseInitializer : DropCreateDatabaseIfModelChanges<ReportContext>
    {
        public ReportDatabaseInitializer()
        {
            Seed(new ReportContext());
        }
        protected override void Seed(ReportContext context)
        {
            GetReports().ForEach(p => context.Reports.Add(p));
        }


        private static List<Report> GetReports()
        {
            return new List<Report>() { new Report
            {
                  Notes = "cao"
            } };
        }
    }
    }