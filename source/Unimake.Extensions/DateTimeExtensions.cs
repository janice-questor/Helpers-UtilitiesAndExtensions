using System.Globalization;

namespace System
{
    /// <summary>
    /// Extensões para os tipos DateTime
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Public Methods

        /// <summary>
        /// Retorna a diferença entre as datas
        /// </summary>
        /// <param name="value">primeira data da comparação</param>
        /// <param name="date">segunda data da comparação</param>
        /// <returns>timespan contendo as diferenças entre as datas</returns>
        public static TimeSpan DateDiff(this DateTime value, DateTime date) => value.Subtract(date);

        /// <summary>
        /// Calcula o primeiro dia do mês da data informada
        /// </summary>
        /// <param name="dateTime">Data</param>
        /// <returns>Data do primeiro dia do mês</returns>
        /// <remarks></remarks>
        public static DateTime FirstDayMonth(this DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, 1);

        /// <summary>
        /// Retorna qual é o primeiro dia da semana de acordo com a cultura
        /// </summary>
        /// <param name="date">Não é utilizado para nada, somente para atender a extension</param>
        /// <returns>Primeiro dia da semana de acordo com a cultura</returns>
        [Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "O parâmetro é usado apenas para extender o tipo")]
        public static DayOfWeek FirstDayOfWeek(this DateTime date) => CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

        /// <summary>
        /// Retorna o dia do mês do primeiro dia da semana de uma data especificada
        /// </summary>
        ///<param name="date">Data base para pesquisar o primeiro dia</param>
        public static DateTime FirstDayOfWeekDate(this DateTime date)
        {
            var firstDay = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var firstDayInWeek = date.Date;
            while(firstDayInWeek.DayOfWeek != firstDay)
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }

            return firstDayInWeek;
        }

        /// <summary>
        /// Retorna o fuso horário da aplicação
        /// </summary>
        /// <param name="date">Data para pesquisa do fuso</param>
        /// <param name="timezoneId">Se nada for informado, será recuperado o timezone "E. South America Standard Time"</param>
        /// <returns></returns>
        public static string GetTimezone(this DateTime date, TimeZoneId timezoneId = null)
        {
            if(timezoneId == null)
            {
                timezoneId = TimeZoneId.ESouthAmericaStandardTime;
            }

            var timespan = TimeZoneInfo.FindSystemTimeZoneById(timezoneId).BaseUtcOffset;
            var timezone = timespan.Hours;

            if(date.IsDaylightSavingTime())
            {
                timezone += 1;
            }

            return $"{timezone:D2}:{timespan.Seconds:D2}";
            ;
        }

        /// <summary>
        /// Compara se as datas são iguais desconsiderando as horas
        /// </summary>
        /// <param name="value">valor de comparação um</param>
        /// <param name="compareValue">Valor que será comparado se é meior que o valor 1</param>
        /// <returns></returns>
        public static bool IsEqual(this DateTime value, DateTime compareValue)
        {
            value = Convert.ToDateTime(value.ToShortDateString());
            compareValue = Convert.ToDateTime(compareValue.ToShortDateString());
            return value == compareValue;
        }

        /// <summary>
        /// Compara se a data é maior que a data de comparação desconsiderando as horas
        /// </summary>
        /// <param name="value">valor de comparação um</param>
        /// <param name="compareValue">Valor que será comparado se é maior que o valor 1</param>
        /// <returns></returns>
        public static bool IsGreaterThan(this DateTime value, DateTime compareValue)
        {
            value = Convert.ToDateTime(value.ToShortDateString());
            compareValue = Convert.ToDateTime(compareValue.ToShortDateString());
            return value > compareValue;
        }

        public static bool IsGreaterThanOrEqualTo(this DateTime value, DateTime compareValue)
        {
            value = Convert.ToDateTime(value.ToShortDateString());
            compareValue = Convert.ToDateTime(compareValue.ToShortDateString());
            return value >= compareValue;
        }

        /// <summary>
        /// retorna true se a data informada for um ano bissexto
        /// </summary>
        /// <param name="value">data válida</param>
        /// <returns></returns>
        public static bool IsLeapYear(this DateTime value) => DateTime.IsLeapYear(value.Year);

        /// <summary>
        /// Compara se a data é menor que a data de comparação desconsiderando as horas
        /// </summary>
        /// <param name="value">valor de comparação um</param>
        /// <param name="compareValue">Valor que será comparado se é menor que o valor 1</param>
        /// <param name="equal">Se true, compara usando menor igual a</param>
        /// <returns></returns>
        public static bool IsLessThan(this DateTime value, DateTime compareValue)
        {
            value = Convert.ToDateTime(value.ToShortDateString());
            compareValue = Convert.ToDateTime(compareValue.ToShortDateString());
            return value < compareValue;
        }

        /// <summary>
        /// Compara se a data é menor que a data de comparação desconsiderando as horas
        /// </summary>
        /// <param name="value">valor de comparação um</param>
        /// <param name="compareValue">Valor que será comparado se é menor que o valor 1</param>
        /// <returns></returns>
        public static bool IsLessThanOrEqualTo(this DateTime value, DateTime compareValue)
        {
            value = Convert.ToDateTime(value.ToShortDateString());
            compareValue = Convert.ToDateTime(compareValue.ToShortDateString());
            return value <= compareValue;
        }

        /// <summary>
        /// Retorna true se a data for válida
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>true se a data for válida</returns>
        public static bool IsValid(this DateTime value)
        {
            if(value <= DateTime.MinValue)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// retorna true se a data for válida
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>true se a data for válida</returns>
        public static bool IsValid(this DateTime? value) => IsValid(value.GetValueOrDefault(DateTime.MinValue));

        /// <summary>
        /// Calcula o último dia do mês da data informada
        /// </summary>
        /// <param name="date">Data</param>
        /// <returns>Data do ultimo dia do mês</returns>
        /// <remarks></remarks>
        public static DateTime LastDayMonth(this DateTime date) => LastDayMonth(date, date.Month, date.Year);

        /// <summary>
        /// Calcula o último dia do mês do mês informada
        /// </summary>
        /// <param name="date">Data</param>
        /// <param name="month">mes para pesquisar</param>
        /// <param name="year">Ano, se não informado usa o ano atual</param>
        /// <returns>Data do ultimo dia do mês</returns>
        /// <remarks></remarks>
        public static DateTime LastDayMonth(this DateTime date, int month, int year = 0)
        {
            if(year == 0)
            {
                year = date.Year;
            }

            return new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Converte um valor Unix epoch time para <see cref="DateTime"/>
        /// </summary>
        /// <param name="unixTimeStamp">Unix <see cref="DateTime"/> em segundos</param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        /// <summary>
        /// Calcula a semana da data informada
        /// </summary>
        /// <param name="dataTime">Data que é para calcular a semana</param>
        /// <returns>O Número da semana do mês da data informada</returns>
        public static int WeekOfDate(this DateTime dataTime)
        {
            var firstDayOfMonth = FirstDayMonth(dataTime);
            var calendar = CultureInfo.CurrentCulture.Calendar;

            var lastWeek = calendar.GetWeekOfYear(dataTime, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
            var firstWeek = calendar.GetWeekOfYear(firstDayOfMonth, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

            return lastWeek - firstWeek + 1;
        }

        /// <summary>
        /// Calcula a quantidade de semanas que tem no mês
        /// </summary>
        /// <param name="dateTime">Data</param>
        /// <returns>Quantidade de semanas</returns>
        /// <remarks></remarks>
        public static int WeekOfMonth(this DateTime dateTime)
        {
            var lastDayMonth = LastDayMonth(dateTime);
            return WeekOfDate(lastDayMonth);
        }

        #endregion Public Methods
    }
}