using Microsoft.EntityFrameworkCore;
using SistemaOrbita.DAL.Data;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaOrbita.DAL.Repository
{
    public class PayrollDetailRepository : Repository<PayrollDetail>, IPayrollDetailRepository
    {
        private readonly OrbitaDbContext _context;

        public PayrollDetailRepository(OrbitaDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<PayrollDetail> GetPayrollDetails(DateTime? startDate, DateTime? endDate)
        {
            List<PayrollDetail> result = new List<PayrollDetail>();

            var employees = _context.Employees.Where(e => e.IsActive == 1)
                .Include(e => e.Position)
                .Include(e => e.EventLogEmployees)
                .Include(e => e.OverTimes);

            foreach (var employee in employees)
            {
                var detail = GetEmployeeDetail(employee, startDate, endDate);
                result.Add(detail);
            }

            return result.AsEnumerable();
        }

        private PayrollDetail GetEmployeeDetail(Employee employee, DateTime? startDate, DateTime? endDate)
        {
            OverTime overTime = GetOverTime(startDate, endDate, employee.Id);
            var perDay = GetNumberFormat(employee.Salary / 30);
            var perHour = GetNumberFormat(perDay / 8);
            var workedDays = IsMonthly() ? 30 : 15;
            var dayOvertime = perHour * 2 * overTime.DayOvertime;
            var nightOvertime = GetNumberFormat(perHour * decimal.Parse("2.25")) * overTime.NightOvertime;
            var subTotal = (perDay * workedDays) + nightOvertime + dayOvertime;
            var bonus = GetEventLog("Bonus", employee.Id, startDate, endDate);
            var discount = GetEventLog("Discount", employee.Id, startDate, endDate);
            var earned = ((perDay * workedDays) + nightOvertime + dayOvertime) + bonus - discount;
            var isss = GetQuotation("ISSS", "Employee", earned);
            var afp = GetQuotation("AFP", "Employee", earned);
            var isr = GetIsr((earned - isss - afp));
            var totalDiscounts = isss + afp + isr;
            var toPay = earned - totalDiscounts;
            PayrollDetail detail = new PayrollDetail()
            {
                Id = NUlid.Ulid.NewUlid().ToString(),
                Employee = employee,
                EmployeeId = employee.Id,
                SalaryBase = employee.Salary,
                PerDay = perDay,
                WorkedDays = workedDays,
                DayOvertime = dayOvertime,
                NightOvertime = nightOvertime,
                SalaryOvertime = dayOvertime + nightOvertime,
                SubTotal = subTotal,
                Bonus = bonus,
                Discount = discount,
                Earned = earned,
                Isss = isss,
                Afp = afp,
                Isr = isr,
                EmployeerAfp = GetQuotation("AFP", "Employer", earned),
                EmployeerIsss = GetQuotation("ISSS", "Employer", earned),
                TotalDiscount = totalDiscounts,
                ToPay = toPay,
            };

            return detail;
        }

        private decimal? GetEventLog(string type, string id, DateTime? startDate, DateTime? endDate)
        {
            decimal? result = 0;

            var eventLog = _context.EventLogs
                .Where(e => e.IsActive == 1 && e.EmployeeId == id && e.Recurring == 0)
                .Include(e => e.Event);

            bool isDeduction = type != "Bonus";

            result += eventLog
                .Where(e => e.CreatedAt <= endDate && e.CreatedAt >= startDate && e.Event.IsDeduction == (isDeduction ? 1 : 0))
                .Select(e => e.Amount)
                .Sum();

            return result;
        }

        private decimal? GetNumberFormat(decimal? number)
        {
            return Math.Round(number.Value, 2, MidpointRounding.AwayFromZero);
        }

        private OverTime GetOverTime(DateTime? startDate, DateTime? endDate, string id)
        {
            var overTime = _context.OverTimes
                .Where(o => o.PeriodStart >= startDate && o.PeriodEnd <= endDate
                && o.EmployeeId == id).SingleOrDefault();
            //o => o.PeriodStart >= DateTime.Parse(startDate) && o.PeriodEnd <= DateTime.Parse(endDate)
            return overTime;
        }

        private bool IsMonthly()
        {
            var type = _context.Employers.FirstOrDefault();

            if (type.PayrollType == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private decimal? GetQuotation(string name, string type, decimal? earned)
        {
            var quotes = _context.QuotationTypes
                .Where(q => q.IsActive == 1 && q.Name == name)
                .ToList();

            if (!quotes.Any())
            {
                throw new InvalidOperationException("No active quotation found for the given name");
            }

            var quote = quotes.FirstOrDefault();

            decimal? percentage = 0m;
            decimal? contribution = 0m;

            if (type == "Employee")
            {
                percentage = quote.EmployeePercentage;

                if (name == "ISSS" && earned > 1000)
                {
                    earned = 1000;
                }

                contribution = (earned * percentage) / 100;
            }

            else if (type == "Employer")
            {
                percentage = quote.EmployerPercentage;
                contribution = (earned * percentage) / 100;
            }

            return GetNumberFormat(contribution);
        }

        private decimal? GetIsr(decimal? earned)
        {
            var brackets = _context.IncomeTaxBrackets
                                   .Where(b => b.IsActive == 1)
                                   .OrderBy(b => b.FromAmount)
                                   .ToList();

            foreach (var bracket in brackets)
            {
                if (bracket.ToAmount == null)
                {
                    if (earned > bracket.FromAmount)
                    {
                        return GetNumberFormat(((earned - bracket.AboutExcess) * bracket.Percentage / 100) + bracket.FixedAmount);
                    }
                }
                else if (earned > bracket.FromAmount && earned <= bracket.ToAmount)
                {
                    return GetNumberFormat(((earned - bracket.AboutExcess) * bracket.Percentage / 100) + bracket.FixedAmount);
                }
            }

            return GetNumberFormat(0);
        }

    }
}
