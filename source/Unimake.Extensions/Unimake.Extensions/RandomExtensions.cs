using System.Text;

namespace System
{
    /// <summary>
    /// Extensões para o tipo Random
    /// </summary>
    public static class RandomExtensions
    {
        #region Public Methods

        /// <summary>
        /// Retorna uma data aleatória dentro do período especificado.
        /// </summary>
        /// <param name="random">objeto random</param>
        /// <returns></returns>
        public static DateTime DateTime(this Random random) => DateTime(random, new DateTime(1900, 1, 1));

        /// <summary>
        /// Retorna uma data aleatória dentro do período especificado.
        /// </summary>
        /// <param name="random">objeto random</param>
        /// <param name="start">data de início da geração</param>
        /// <returns></returns>
        public static DateTime DateTime(this Random random, DateTime start) => DateTime(random, start, System.DateTime.Today);

        /// <summary>
        /// Retorna uma data aleatória dentro do período especificado.
        /// </summary>
        /// <param name="random">objeto random</param>
        /// <param name="start">data de início da geração</param>
        /// <param name="end">data de fim da geração</param>
        /// <returns></returns>
        public static DateTime DateTime(this Random random, DateTime start, DateTime end)
        {
            var range = end - start;
            var randTimeSpan = new TimeSpan((long)(random.NextDouble() * range.Ticks));
            return start + randTimeSpan;
        }

        /// <summary>
        /// Retorna um módulo 11 genérico
        /// </summary>
        /// <param name="rnd">O gerador randômico</param>
        /// <returns></returns>
        public static string Modulus11(this Random rnd)
        {
            if(rnd is null)
            {
                rnd = new Random();
            }

            var sum = 0;
            var multiplier1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplier2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var seed = rnd.Next(100000000, 999999999).ToString();

            for(var i = 0; i < 9; i++)
            {
                sum += int.Parse(seed[i].ToString()) * multiplier1[i];
            }

            var rest = sum % 11;
            if(rest < 2)
            {
                rest = 0;
            }
            else
            {
                rest = 11 - rest;
            }

            seed += rest;
            sum = 0;

            for(var i = 0; i < 10; i++)
            {
                sum += int.Parse(seed[i].ToString()) * multiplier2[i];
            }

            rest = sum % 11;

            if(rest < 2)
            {
                rest = 0;
            }
            else
            {
                rest = 11 - rest;
            }

            seed += rest;
            return seed;
        }

        /// <summary>
        /// Retorna um número randômico entre o mínimo e 85% a mais do mínimo do valor informado
        /// </summary>
        /// <param name="random">objeto random</param>
        /// <param name="minimum">mínimo valor retornado</param>
        /// <returns></returns>
        public static double NextDouble(this Random random, double minimum) => NextDouble(random, minimum, minimum * 1.85);

        /// <summary>
        /// Retorna um número randômico entre o mínimo e o máximo valor informado
        /// </summary>
        /// <param name="random">objeto random</param>
        /// <param name="minimum">mínimo valor retornado</param>
        /// <param name="maximum">máximo valor retornado</param>
        /// <returns></returns>
        public static double NextDouble(this Random random, double minimum, double maximum) => NextDouble(random, minimum, maximum, 75);

        /// <summary>
        /// Retorna um número randômico entre o mínimo e o máximo valor informado
        /// </summary>
        /// <param name="random">objeto random</param>
        /// <param name="minimum">mínimo valor retornado</param>
        /// <param name="maximum">máximo valor retornado</param>
        /// <param name="delay">tempo de espera antes de gerar o número aleatório. Evita a duplicidade de números. Seu valor padrão é 75</param>
        /// <returns></returns>
        public static double NextDouble(this Random random, double minimum, double maximum, int delay)
        {
            System.Threading.Thread.Sleep(delay);
            return minimum + random.NextDouble() * (maximum - minimum);
        }

        /// <summary>
        /// Gera uma string aleatória de tamanho 12 com os seguintes caracteres:
        /// <para>0123456789</para>
        /// <para>abcdefghijklmnopqrstuvwxyz</para>
        /// <para>ABCDEFGHIJKLMNOPQRSTUVWXYZ</para>
        /// <para> (espaço)</para>
        /// </summary>
        /// <param name="random">objeto do tipo random como base</param>
        /// <returns>String aleatória</returns>
        public static string NextString(this Random random) => NextString(random, 12);

        /// <summary>
        /// Gera uma string aleatória com os seguintes caracteres:
        /// <para>0123456789</para>
        /// <para>abcdefghijklmnopqrstuvwxyz</para>
        /// <para>ABCDEFGHIJKLMNOPQRSTUVWXYZ</para>
        /// <para> (espaço)</para>
        /// </summary>
        /// <param name="random">objeto do tipo random como base</param>
        /// <param name="length">tamanho da string que deverá ser gerada</param>
        /// <returns>String aleatória</returns>
        public static string NextString(this Random random, int length) => NextString(random, length, true);

        /// <summary>
        /// Gera uma string aleatória com os seguintes caracteres:
        /// <para>0123456789 (Se withNumbers for verdadeiro) </para>
        /// <para>abcdefghijklmnopqrstuvwxyz</para>
        /// <para>ABCDEFGHIJKLMNOPQRSTUVWXYZ</para>
        /// <para>(espaço)</para>
        /// </summary>
        /// <param name="random">objeto do tipo random como base</param>
        /// <param name="length">tamanho da string que deverá ser gerada</param>
        /// <param name="withNumbers">Se verdadeiro, utiliza números na geração </param>
        /// <returns>String aleatória</returns>
        public static string NextString(this Random random, int length, bool withNumbers) => NextString(random, length, withNumbers, true);

        /// <summary>
        /// Gera uma string aleatória com os seguintes caracteres:
        /// <para>0123456789 (Se withNumbers for verdadeiro) </para>
        /// <para>abcdefghijklmnopqrstuvwxyz</para>
        /// <para>ABCDEFGHIJKLMNOPQRSTUVWXYZ</para>
        /// <para>Espaço (se withSpace for verdadeiro)</para>
        /// </summary>
        /// <param name="random">objeto do tipo random como base</param>
        /// <param name="length">tamanho da string que deverá ser gerada</param>
        /// <param name="withNumbers">Se verdadeiro, utiliza números na geração </param>
        /// <param name="withSpace">Se verdadeiro, utiliza espaços na geração </param>
        /// <returns>String aleatória</returns>
        public static string NextString(this Random random, int length, bool withNumbers, bool withSpace)
        {
            var builder = new StringBuilder(length);
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                        (withNumbers ? "0123456789" : "") +
                        (withSpace ? " " : "");

            for(var i = 0; i < length; ++i)
            {
                builder.Append(chars[random.Next(chars.Length)]);
            }

            return builder.ToString();
        }

        #endregion Public Methods
    }
}