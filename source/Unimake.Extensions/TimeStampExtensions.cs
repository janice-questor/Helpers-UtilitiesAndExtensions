using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// Extensões para o tipo timespan
    /// </summary>
    public static class TimeStampExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adiciona horas ao timespan
        /// </summary>
        /// <param name="hour">horas que deverão ser adicionadas</param>
        /// <param name="time">Hora definida</param>
        /// <returns></returns>
        public static TimeSpan AddHours(this TimeSpan time, int hour)
        {
            time += TimeSpan.FromHours(hour);
            return time;
        }

        /// <summary>
        /// Valida se a hora informada é válida. Não valida a data
        /// </summary>
        /// <param name="date">Data/Hora para validação</param>
        /// <returns></returns>
        public static bool IsTimeValid(this DateTime date) => IsValid(new TimeSpan(date.Ticks));

        /// <summary>
        /// Valida se a hora informada é válida. Não valida a data
        /// </summary>
        /// <param name="time">Hora para validação</param>
        /// <returns></returns>
        public static bool IsValid(this TimeSpan time)
        {
            var checktime = new Regex(@"(?:(?:0?[0-9]|1[0-2]):[0-5][0-9]\s?|(?:1[3-9]|2[0-3]):[0-5][0-9])");
            var result = checktime.IsMatch(time.ToString(@"hh\:mm\:ss"));
            return result;
        }

        #endregion Public Methods
    }
}