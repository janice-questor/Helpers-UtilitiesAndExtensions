namespace System.IO
{
    /// <summary>
    /// Extensões para os tipo FileInfo
    /// </summary>
    public static class FileInfoExtensions
    {
        #region Public Methods

        /// <summary>
        /// Valida o arquivo e retorna true se estiver em uso. Caso contrário false
        /// </summary>
        ///<param name="fi">Informações do arquivo.</param>
        /// <returns>Retorna true se o arquivo estiver em uso. Caso contrário false</returns>
        public static bool FileInUse(this FileInfo fi)
        {
            var result = false;

            try
            {
                using(var fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    fs.Close();
                }
            }
            catch
            {
                result = true;
            }

            return result;
        }

        #endregion Public Methods
    }
}