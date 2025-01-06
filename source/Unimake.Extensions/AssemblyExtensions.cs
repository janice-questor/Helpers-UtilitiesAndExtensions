using System.Reflection;
using System.Text;

namespace System
{
    /// <summary>
    /// </summary>
    public static class AssemblyExtensions
    {
        #region Public Methods

        /// <summary>
        /// Recupera o recurso embutido de um <see cref="Assembly"/> e retorna como Base64.
        /// </summary>
        /// <param name="ass">Se <paramref name="assembly"/>, for nulo, recuepra deste</param>
        /// <param name="name">Nome do recurso</param>
        /// <param name="assembly">Assembly onde o recurso existe</param>
        /// <returns></returns>
        public static string GetManifestResourceAsBase64(this Assembly ass, string name, Assembly assembly = null)
        {
            assembly = assembly ?? ass;
            var stream = assembly.GetManifestResourceStream(name);
            var bytes = new byte[stream.Length];
            _ = stream.Read(bytes, 0, bytes.Length);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Recupera o recurso embutido de um <see cref="Assembly"/> e retorna como string.
        /// <para>Usa o <paramref name="encoding"/>, se nulo, o padrão é <see cref="Encoding.UTF8"/> </para>
        /// </summary>
        /// <param name="ass">Se <paramref name="assembly"/>, for nulo, recuepra deste</param>
        /// <param name="name">Nome do recurso</param>
        /// <param name="assembly">Assembly onde o recurso existe</param>
        /// <param name="encoding">Codificação do arquivo, padrão <see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static string GetManifestResourceAsString(this Assembly ass, string name, Assembly assembly = null, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            assembly = assembly ?? ass;
            var stream = assembly.GetManifestResourceStream(name);
            var bytes = new byte[stream.Length];
            _ = stream.Read(bytes, 0, bytes.Length);
            return encoding.GetString(bytes);
        }

        #endregion Public Methods
    }
}