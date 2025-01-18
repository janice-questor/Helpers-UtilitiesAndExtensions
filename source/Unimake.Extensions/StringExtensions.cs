using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensions
    {
        #region Private Fields

        private static readonly byte[] ByteMap = {
             0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x3F, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x3F, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F,
             0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,0x3F, 0x3F, 0x3F, 0x3F
        };

        private static readonly string[] HexMap = {
           "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A","0B", "0C", "0D", "0E", "0F",
           "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A","1B", "1C", "1D", "1E", "1F",
           "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A","2B", "2C", "2D", "2E", "2F",
           "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A","3B", "3C", "3D", "3E", "3F",
           "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A","4B", "4C", "4D", "4E", "4F",
           "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A","5B", "5C", "5D", "5E", "5F",
           "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6A","6B", "6C", "6D", "6E", "6F",
           "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7A","7B", "7C", "7D", "7E", "7F",
           "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8A","8B", "8C", "8D", "8E", "8F",
           "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9A","9B", "9C", "9D", "9E", "9F",
           "A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA","AB", "AC", "AD", "AE", "AF",
           "B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA","BB", "BC", "BD", "BE", "BF",
           "C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA","CB", "CC", "CD", "CE", "CF",
           "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA","DB", "DC", "DD", "DE", "DF",
           "E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA","EB", "EC", "ED", "EE", "EF",
           "F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA","FB", "FC", "FD", "FE", "FF"
        };

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Converte uma string em base 64 para uma string padrão
        /// </summary>
        /// <param name="base64EncodedData">String codificada em base 64</param>
        /// <returns></returns>
        public static string Base64Decode(this string base64EncodedData)
        {
            if(string.IsNullOrWhiteSpace(base64EncodedData))
            {
                throw new ArgumentNullException(nameof(base64EncodedData));
            }

            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Converte uma string em base 64
        /// </summary>
        /// <param name="plainText">Texto que será convertido</param>
        /// <param name="ignoreErrorIfNullOrEmpty">Se verdadeiro, ignora o erro se a string for vazia</param>
        /// <returns></returns>
        public static string Base64Encode(this string plainText, bool ignoreErrorIfNullOrEmpty = false)
        {
            if(string.IsNullOrWhiteSpace(plainText))
            {
                if(ignoreErrorIfNullOrEmpty)
                {
                    return "";
                }

                throw new ArgumentNullException(nameof(plainText));
            }

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Conta a quantidade de vezes que um carácter aparece em uma string
        /// </summary>
        /// <param name="value">string com os caracteres a ser analisado</param>
        /// <param name="charToCount">carácter a ser contado</param>
        /// <returns>A quantidade de vezes que o carácter (charToCount) foi encontrado na string (value)</returns>
        public static int CountChars(this string value, char charToCount)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            return value.Count(c => c == charToCount);
        }

        /// <summary>
        /// Escapa caracteres estranhos.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="escapeChar"></param>
        /// <returns></returns>
        public static string Escape(this string value, char escapeChar = '%')
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            var sb = new StringBuilder();

            for(var i = 0; i < value.Length; i++)
            {
                int ch = value[i];

                if(ch >= 'A' && ch <= 'Z')
                {
                    sb.Append((char)ch);
                }
                else if(ch >= 'a' && ch <= 'z')
                {
                    sb.Append((char)ch);
                }
                else if(ch >= '0' && ch <= '9')
                {
                    sb.Append((char)ch);
                }
                else if(ch == '-' || ch == '_' ||
                         ch == '.' || ch == '!' ||
                         ch == '~' || ch == '*' ||
                         ch == '\'' || ch == '(' ||
                         ch == ')')
                {
                    sb.Append((char)ch);
                }
                else if(ch <= 0x007F)
                {
                    sb.Append(escapeChar);
                    sb.Append(HexMap[ch]);
                }
                else
                {
                    sb.Append(escapeChar);
                    sb.Append('u');
                    sb.Append(HexMap[((uint)ch >> 8)]);
                    sb.Append(HexMap[(0x00FF & ch)]);
                }
            }

            return sb.ToString();
        }

        public static string First(this string str, int maxLength) => str.Length > maxLength ? str.Substring(0, maxLength) : str;

        /// <summary>
        /// Retorna sempre o mesmo HashCode para uma string.
        /// </summary>
        /// <param name="value">String para calcular o HashCode</param>
        /// <returns></returns>
        public static int GetStableHashCode(this string value)
        {
            unchecked
            {
                var hash1 = 5381;
                var hash2 = hash1;

                for(var i = 0; i < value.Length && value[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ value[i];
                    if(i == value.Length - 1 || value[i + 1] == '\0')
                    {
                        break;
                    }

                    hash2 = ((hash2 << 5) + hash2) ^ value[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }

        /// <summary>
        /// Retorna verdadeiro se possui apenas números
        /// </summary>
        /// <param name="value">string de testes</param>
        /// <returns></returns>
        public static bool HasOnlyNumbers(this string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                return false;//se não tem nada ou é espaços em brancos. ¯\_(ツ)_/¯
            }

            return Regex.IsMatch(value, @"^[0-9_]+$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Retorna verdadeiro se possui apenas números e letras
        /// </summary>
        /// <param name="value">string de testes</param>
        /// <returns></returns>
        public static bool HasOnlyNumbersAndLetters(this string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                return false;//se não tem nada ou é espaços em brancos. ¯\_(ツ)_/¯
            }

            return Regex.IsMatch(value, @"^[a-zA-Z0-9]+$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Retorna verdadeiro se possui apenas números e letras e sublinhado
        /// </summary>
        /// <param name="value">string de testes</param>
        /// <returns></returns>
        public static bool HasOnlyNumbersLettersAndUnderscore(this string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                return false;//se não tem nada ou é espaços em brancos. ¯\_(ツ)_/¯
            }

            return Regex.IsMatch(value, @"^[a-zA-Z0-9_]+$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Decodifica uma string com marcação html e retorna ao seu estado original
        /// </summary>
        /// <param name="value">string a ser codificada</param>
        /// <returns></returns>
        public static string HtmlDecode(this string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                value = "";
            }

            return Net.WebUtility.HtmlDecode(value);
        }

        /// <summary>
        /// Codifica uma string com marcação html
        /// </summary>
        /// <param name="value">string a ser codificada</param>
        /// <returns></returns>
        public static string HtmlEncode(this string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                value = "";
            }

            return Net.WebUtility.HtmlEncode(value);
        }

        /// <summary>
        /// Verifica se uma string é uma string válida codificada em Base64.
        /// </summary>
        /// <param name="input">A string a ser verificada.</param>
        /// <returns>True se a string for válida em Base64; caso contrário, False.</returns>
        public static bool IsBase64(this string input)
        {
            // Credit: oybek https://stackoverflow.com/users/794764/oybek
            if(string.IsNullOrWhiteSpace(input) ||
               input.Length % 4 != 0 ||
               input.Contains(" ") ||
               input.Contains("\t") ||
               input.Contains("\r") ||
               input.Contains("\n"))
            {
                return false;
            }

            try
            {
                _ = Convert.FromBase64String(input);
                return true;
            }
            catch
            {
                //ignore
            }

            return false;
        }

        /// <summary>
        /// Verifica se um valor contido em uma string é numérico.
        /// </summary>
        /// <param name="value">Valor a ser verificado</param>
        /// <returns></returns>
        public static bool IsNumeric(this string value) => decimal.TryParse(value, out var _);

        public static bool IsNumeric(this char value) => IsNumeric(value.ToString());

        /// <summary>
        /// Normaliza a string
        /// <para>Exemplo: </para>
        /// <para>String original: SISTEMA DE GERENCIAMENTO EMPRESARIAL</para>
        /// <para>String Normalizada: Sistema De Gerenciamento Empresarial</para>
        /// </summary>
        /// <param name="stringInput">string a ser tratada</param>
        /// <returns>string normalizada</returns>
        public static string ProperCase(this string stringInput)
        {
            if(string.IsNullOrWhiteSpace(stringInput))
            {
                return "";
            }

            var sb = new StringBuilder();
            var fEmptyBefore = true;

            foreach(var ch in stringInput)
            {
                var chThis = ch;
                if(char.IsWhiteSpace(chThis))
                {
                    fEmptyBefore = true;
                }
                else
                {
                    if(char.IsLetter(chThis) && fEmptyBefore)
                    {
                        chThis = char.ToUpper(chThis);
                    }
                    else
                    {
                        chThis = char.ToLower(chThis);
                    }

                    fEmptyBefore = false;
                }

                sb.Append(chThis);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Remove os caracteres informados da string
        /// </summary>
        /// <param name="value">string que deverá ser tratada</param>
        /// <param name="chars2Remove">caracteres que deverão ser removidos</param>
        /// <returns>Nova string com os caracteres removidos</returns>
        public static string RemoveChars(this string value, params char[] chars2Remove)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            var result = value;

            foreach(var c in chars2Remove)
            {
                result = result.Replace(c.ToString(), "");
            }

            return result;
        }

        /// <summary>
        /// Substitui  os caracteres acentuados pelo seu equivalente não acentuado
        /// </summary>
        /// <param name="value">string para substituir os caracteres acentuados</param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            var stFormD = value.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            for(var ich = 0; ich < stFormD.Length; ich++)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if(uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        /// <summary>
        /// Remove os espaços desnecessários da string, tais como
        /// espaços duplos, no inicio ou final da sentença.
        /// <para>Se a string for nula, vazia ou somente espaços em branco, retorna uma string vazia</para>
        /// </summary>
        /// <returns>Retorna a string sem os espaços desnecessários.</returns>
        public static string RemoveExtraSpaces(this string content)
        {
            if(string.IsNullOrWhiteSpace(content))
            {
                return "";
            }

            var regex = new Regex(@"\s{2,}");
            content = regex.Replace(content, " ")
                           .Trim();

            return content;
        }

        /// <summary>
        /// Remove todos os caracteres especiais, incluindo espaços se removeSpace for verdadeiro, da string e retorna
        /// </summary>
        /// <param name="value">string que deverá ser limpa.</param>
        /// <param name="removeSpace">Se verdadeiro, remove os espaços</param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(this string value, bool removeSpace = true)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                value = "";
            }

            var buffer = new char[value.Length];
            var idx = 0;

            foreach(var c in value)
            {
                if((c >= '0' && c <= '9') ||
                   (c >= 'A' && c <= 'Z') ||
                   (c >= 'a' && c <= 'z') ||
                   (c == '.') ||
                   (c == '_') ||
                   (c == ' ' && !removeSpace)
                   )
                {
                    buffer[idx] = c;
                    idx++;
                }
            }

            return new string(buffer, 0, idx);
        }

        /// <summary>
        /// Converte uma string em um padrão Latin1
        /// </summary>
        /// <param name="value">string</param>
        /// <returns></returns>
        public static string ToLatin1(this string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                value = "";
            }

            var iso = Encoding.Default;
            var utf8 = Encoding.UTF8;
            var utfBytes = utf8.GetBytes(value);
            var isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            var result = iso.GetString(isoBytes);

            //carateres que não existe no latin1
            result = result
                    .Replace("…", "...")
                    .Replace("–", "-");
            return result;
        }

        /// <summary>
        /// Converte a string em um stream de memória
        /// </summary>
        /// <param name="value">String que será convertida</param>
        /// <returns></returns>
        public static Stream ToStream(this string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                value = "";
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(value);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Trunca a string respeitando o tamanho definido em <paramref name="maxLength"/>
        /// </summary>
        /// <param name="value">String que será truncada</param>
        /// <param name="maxLength">Tamanho máximo da string</param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxLength)
        {
            if(string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Unescape(this string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            var sb = new StringBuilder();
            var i = 0;

            while(i < value.Length)
            {
                int ch = value[i];
                if(ch == '%')
                {
                    var cint = 0;
                    if(value[i + 1] != 'u')
                    {
                        cint = (cint << 4) | ByteMap[value[i + 1]];
                        cint = (cint << 4) | ByteMap[value[i + 2]];
                        i += 2;
                    }
                    else
                    {
                        cint = (cint << 4) | ByteMap[value[i + 2]];
                        cint = (cint << 4) | ByteMap[value[i + 3]];
                        cint = (cint << 4) | ByteMap[value[i + 4]];
                        cint = (cint << 4) | ByteMap[value[i + 5]];
                        i += 5;
                    }
                    sb.Append((char)cint);
                }
                else
                {
                    sb.Append((char)ch);
                }
                i++;
            }

            return sb.ToString();
        }

        #endregion Public Methods
    }
}