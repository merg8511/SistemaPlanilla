using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.Model.ViewModels
{
    public class PayrollVM
    {
        public Payroll? Payroll { get; set; }
        public Employer? Employer { get; set; }
        public List<PayrollDetail>? PayrollDetails { get; set; }

        public decimal? TotalSalaryOvertime { get; set; }
        public decimal? TotalSubTotal { get; set; }
        public decimal? TotalBonus { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalEarned { get; set; }
        public decimal? TotalISSS { get; set; }
        public decimal? TotalAFP { get; set; }
        public decimal? TotalISR { get; set; }
        public decimal? TotalFinalDiscounts { get; set; }
        public decimal? TotalToPay { get; set; }

        public string? LastRecord { get; set; }

    }
}
