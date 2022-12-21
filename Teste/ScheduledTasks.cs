using FluentScheduler;
using System;

namespace Teste
{
    public class ScheduledTasks : Registry
    {
        private readonly IConfiguration _configuration;
        public ScheduledTasks(IConfiguration configuration)
        {
            _configuration = configuration;
            string[] horaMinuto = _configuration.GetValue<string>("HorarioImportacao").Split(':');
            Schedule(() => Connect.Start(_configuration, true)).ToRunEvery(1).Days().At(int.Parse(horaMinuto[0]), int.Parse(horaMinuto[1]));
        }
    }
}
