using System.Diagnostics;
using System.IO;
using Xunit;

namespace Unimake.Helpers_UtilitiesAndExtensions.Test.Utils
{
    public class FileHelperTest
    {
        #region Public Methods

        [Fact]
        public void GravarArquivoPDF()
        {
            var fi = new FileInfo(".\\Assets\\Files\\PDFFile.pdf");
            var base64 = FileHelper.ConvertFileToBase64(fi.FullName);
            var tempFile = $"{Path.GetTempFileName()}.pdf";

            PDFHelper.WriteStringToPDFFile(base64, tempFile);

            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = tempFile
            });

            fi.Delete();
        }

        [Fact]
        public void GravarArquivoTXT()
        {
            var fi = new FileInfo(".\\Assets\\Files\\TXTFile.txt");
            var base64 = FileHelper.ConvertFileToBase64(fi.FullName);
            var tempFile = $"{Path.GetTempFileName()}.txt";

            FileHelper.WriteStringToFile(base64, tempFile);

            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = tempFile
            });

            fi.Delete();
        }

        #endregion Public Methods
    }
}