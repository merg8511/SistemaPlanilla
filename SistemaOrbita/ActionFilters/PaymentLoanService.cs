
using Microsoft.Data.SqlClient;
using SistemaOrbita.DAL.Repository;
using SistemaOrbita.DAL.Repository.IRepository;
using SistemaOrbita.Model.Models;

namespace SistemaOrbita.ActionFilters
{
    public class PaymentLoanService : BackgroundService
    {
        private readonly int[] daysToRun = new[] { 14, 27 };
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentLoanService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var today = DateTime.Today.Day;
                if (Array.Exists(daysToRun, day => day == today))
                {
                    await RunScheduledTasks();
                }

                // Espera hasta el próximo día
                await WaitUntilNextDay(stoppingToken);
            }
        }

        private async Task RunScheduledTasks()
        {
            try
            {
                if (!await HasTaskBeenRunToday())
                {
                    var events = ProcessRecurringEvents();
                    await MarkTaskAsRun();
                }
                else
                {
                    Console.WriteLine("La tarea ya se ejecutó hoy.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error durante la ejecución de la tarea: {ex.Message}");
            }
        }

        private async Task WaitUntilNextDay(CancellationToken stoppingToken)
        {
            var now = DateTime.Now;
            var tomorrow = now.AddDays(1).Date;
            var timeToWait = tomorrow - now;
            await Task.Delay(timeToWait, stoppingToken);
        }

        private async Task<bool> HasTaskBeenRunToday()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var today = DateTime.Today;
                return await unitOfWork.TaskExecutionLog.CheckTaskExecutedLogToday("PaymentProcessing", today);
            }
        }

        private async Task MarkTaskAsRun()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var log = new TaskExecutionLog
                {
                    TaskId = "PaymentProcessing",
                    ExecutionDate = DateTime.Today,
                    Status = "Completed"
                };

                await unitOfWork.TaskExecutionLog.Create(log);
                await unitOfWork.Save();
            }
        }

        private async Task ProcessRecurringEvents()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var today = DateTime.Now;
                var activeEventLogs = await GetActiveRecurringEvents(unitOfWork);

                foreach (var item in activeEventLogs)
                {
                    if (ShouldProcessToday(item, today))
                    {
                        await ProcessEvent(item, unitOfWork);
                    }
                }
            }
        }

        private bool ShouldProcessToday(EventLog eventLog, DateTime today)
        {
            // Mensual: procesar solo el 27
            if (eventLog.Frequency == 1 && today.Day == 27)
            {
                return true;
            }
            // Quincenal: procesar el 14 y el 27
            else if (eventLog.Frequency == 0 && (today.Day == 14 || today.Day == 27))
            {
                return true;
            }
            return false;
        }
        private async Task<IEnumerable<EventLog>> GetActiveRecurringEvents(IUnitOfWork unitOfWork)
        {
            var today = DateTime.Today;
            return await unitOfWork.EventLog.GetAll(
                e => e.Recurring == 1 &&
                e.IsActive == 1 &&
                e.StartDate <= today && e.EndDate >= today
            );
        }

        private async Task ProcessEvent(EventLog eventLog, IUnitOfWork unitOfWork)
        {
            eventLog.InstallmentsPaid++;

            bool isLastInstallment = eventLog.InstallmentsPaid == eventLog.TotalInstallments;
            decimal amountToCharge = isLastInstallment ? (eventLog.LastFee ?? default(decimal)) : (eventLog.Fee ?? default(decimal));
            string note = isLastInstallment ? "Última cuota procesada" : $"Pago recurrente procesado. Cuota {eventLog.InstallmentsPaid}";

            var paymentLog = new EventLog
            {
                Id = NUlid.Ulid.NewUlid().ToString(),
                Notes = note,
                Amount = amountToCharge,
                EmployeeId = eventLog.EmployeeId,
                EventId = eventLog.EventId,
                AuthorizedBy = eventLog.AuthorizedBy,
                Recurring = 0,
                CreatedAt = DateTime.Now,
                InstallmentsPaid = eventLog.InstallmentsPaid,
                IsActive = 1
            };

            await unitOfWork.EventLog.Create(paymentLog);

            if (isLastInstallment)
            {
                eventLog.IsActive = 0;
            }

            await unitOfWork.Save();
        }
    }
}

