using System.Diagnostics;

namespace System
{
    /// <summary>
    /// Representa duas datas de início e fim dentro de um intervalo.
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public struct DateTimeRange : IEquatable<DateTimeRange>, IFormattable, ICloneable
    {
        #region Public Fields

        /// <summary>
        /// Intervalo vazio <see cref="DateTime.MinValue"/>
        /// </summary>
        public static readonly DateTimeRange Empty = new DateTimeRange(DateTime.MinValue, DateTime.MinValue);

        #endregion Public Fields

        #region Public Properties

        /// <summary>
        /// Data final do intervalo
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Data inicial do intervalo
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Diferença entre o <see cref="End"/> menos o <see cref="Start"/>
        /// </summary>
        public TimeSpan TimeSpan => End - Start;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Cria uma nova instância de intervalo entre duas datas
        /// </summary>
        /// <param name="start">Data de início</param>
        /// <param name="end">data final</param>
        public DateTimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Cria uma instância de intervalo para a mesma data informada em <paramref name="date"/>
        /// </summary>
        /// <param name="date"></param>
        public DateTimeRange(DateTime date)
        {
            Start = date;
            End = date;
        }

        #endregion Public Constructors

        #region Public Methods

        public static bool operator !=(DateTimeRange lhs, DateTimeRange rhs) => !lhs.Equals(rhs);

        public static bool operator ==(DateTimeRange lhs, DateTimeRange rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Cria uma cópia desta instância
        /// </summary>
        /// <returns>Uma cópia desta instância</returns>
        public DateTimeRange Clone() => new DateTimeRange(new DateTime(Start.Ticks), new DateTime(End.Ticks));

        /// <summary>
        /// Se a data informado em <paramref name="other"/> existe dentro do intervalo
        /// </summary>
        /// <param name="other">Data para verificar se existe no intervalo</param>
        /// <returns>verdadeiro, se a data informada em <paramref name="other"/> existir no intervalo, caso contrário, falso.</returns>
        public bool Contains(DateTime other) => ((Start <= other) && (End >= other));

        /// <summary>
        /// Retorna verdadeiro se esta instância for igual a informada em <paramref name="other"/>.
        /// </summary>
        /// <param name="other">Outro intervalo para comparação</param>
        /// <remarks>Converte as datas para <see cref="DateTime.ToString()"/> no formato "yyyyMMddHHmmss"</remarks>
        /// <returns>Verdadeiro se as datas de início e fim desta instância e <paramref name="other"/> forem iguais. Caso contrário, falso.</returns>
        public bool Equals(DateTimeRange other)
        {
            if(ReferenceEquals(this, other))
            {
                return true;
            }

            return Start.ToString("yyyyMMddHHmmss") == other.Start.ToString("yyyyMMddHHmmss") &&
                   End.ToString("yyyyMMddHHmmss") == other.End.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// Retorna verdadeiro se esta instância for igual a informada em <paramref name="other"/>.
        /// </summary>
        /// <param name="obj">Outro intervalo para comparação</param>
        /// <remarks>Converte as datas para <see cref="DateTime.ToString()"/> no formato "yyyyMMddHHmmss"</remarks>
        /// <returns>Verdadeiro se as datas de início e fim desta instância e <paramref name="other"/> forem iguais. Caso contrário, falso.</returns>
        public override bool Equals(object obj)
        {
            if(obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((DateTimeRange)obj);
        }

        public override int GetHashCode() => Start.GetHashCode() ^ End.GetHashCode();

        public string ToShortDateString(string format = "dd/MM/yyyy")
        {
            if(string.IsNullOrWhiteSpace(format))
            {
                throw new ArgumentException($"'{nameof(format)}' cannot be null or whitespace.", nameof(format));
            }

            return $"{Start.ToString(format)} - {End.ToString(format)}";
        }

        public override string ToString() => $"{Start} - {End}";

        public string ToString(string format, IFormatProvider formatProvider) =>
            $"{Start.ToString(format, formatProvider)} - {End.ToString(format, formatProvider)}";

        object ICloneable.Clone() => Clone();

        #endregion Public Methods
    }
}