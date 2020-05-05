using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NIceWeh.Model
{
    public class Report
    {

        [ScaffoldColumn(false)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public double Hours { get; set; }
        public string Notes { get; set; }
        public string ReportNotes { get; set; }

    }
}