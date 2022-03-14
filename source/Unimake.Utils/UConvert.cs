using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Unimake
{
    /// <summary>
    /// Utilitários de conversão.
    /// Ignora diversos erros de conversão e retorna valores padrões.
    /// <para>É útil quando não é necessário tratar o erro de conversão e pode ser usado o valor padrão.</para>
    /// Use moderadamente.
    /// </summary>
    public class UConvert
    {
        #region Private Methods

        private static bool ConvertFromTypeDescriptor(object value, Type expectedType, ref object result)
        {
            var conv = TypeDescriptor.GetConverter(expectedType);

            if(!(conv?.CanConvertFrom(value.GetType()) ?? false))
            {
                return false;
            }

            try
            {
                result = conv.ConvertFrom(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Prepara o número antes de converter, remove os sinais de formatação
        /// </summary>
        /// <param name="number">Número para verificação </param>
        /// <param name="returnZeroIfNull">Se verdadeiro, retorna 0 se nulo</param>
        /// <returns>Número, se for número válido. Zero se o parâmetro <paramref name="returnZeroIfNull"/> for verdadeiro e <paramref name="number"/> for nulo., ou nulo.</returns>
        private static object PrepareNumber(object number, bool returnZeroIfNull = true)
        {
            if(number == null)
            {
                if(returnZeroIfNull)
                {
                    return 0;
                }

                return number;
            }

            if(number.GetType().IsEnum)
            {
                return number;
            }

            var value = OnlyNumbers(number);
            return value;
        }

        /// <summary>
        /// Prepara o valor antes de exibir ao usuário
        /// </summary>
        /// <typeparam name="T"> Tipo de valor esperado </typeparam>
        /// <param name="value"> valor para ser convertido </param>
        /// <returns> </returns>
        private static T PrepareValue<T>(object value)
        {
            if(value is null ||
               value == DBNull.Value)//ignorar dbnull
            {
                return default;
            }

            var type = typeof(T);

            //tratar apenas tipos não anuláveis
            type = Nullable.GetUnderlyingType(type) ?? type;

            try
            {
                if(type.IsEnum)
                {
                    return ToEnum<T>(value);
                }

                return (T)ChangeType(value, type);
            }
            catch
            {
                return default;
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Muda o tipo do objeto informado fazendo uma conversão para o tipo desejado
        /// </summary>
        /// <param name="value"> Objeto no qual será alterado o seu tipo </param>
        /// <param name="conversionType"> Tipo de conversão desejado </param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversionType)
        {
            if(value == null)
            {
                return value;
            }

            var result = default(object);

            try
            {
                if(conversionType.IsEnum)
                {
                    return ToEnum(conversionType, value);
                }

                if(conversionType == typeof(TimeSpan) ||
                   conversionType == typeof(TimeSpan?))
                {
                    return ToTimeSpan(value);
                }

                if(conversionType == typeof(bool) ||
                   conversionType == typeof(bool?))
                {
                    return ToBoolean(value);
                }

                if(!ConvertFromTypeDescriptor(value, conversionType, ref result))
                {
                    return Convert.ChangeType(value, conversionType);
                }
            }
            catch
            {
                ConvertFromTypeDescriptor(value, conversionType, ref result);
            }

            return result;
        }

        /// <summary>
        /// Retorna apenas os números.
        /// </summary>
        /// <param name="text"> texto a ser tratado </param>
        /// <param name="removeChars"> Não remove os caracteres como .,e -. Se deseja remover estes caracteres informe-os aqui. </param>
        /// <param name="returnZero"> Se true, retorna zero ao invés de vazio </param>
        /// <returns> </returns>
        public static object OnlyNumbers(object text, string removeChars, bool returnZero = true)
        {
            var result = OnlyNumbers(text, returnZero).ToString();

            foreach(var c in removeChars.ToCharArray())
            {
                result = result.Replace(c.ToString(), "");
            }

            return result;
        }

        /// <summary>
        /// Retorna apenas os números.
        /// <para>Este método mantem os caracteres ,. e -, se desejar utilize o segundo método OnlyNumbers e informe os caracteres que deverão ser removidos. </para>
        /// </summary>
        /// <param name="text"> texto a ser tratado </param>
        /// <param name="returnZero">Se true, retorna zero ao invés de vazio </param>
        /// <param name="force"> Se verdadeiro, irá retornar apenas os números e descartar totalmente qualquer caractere não numérico, incluindo o sinal negativo -, pontos e vírgulas </param>
        /// <returns>Se <paramref name="returnZero"/> for verdadeiro, retorna zero o invés de vazio ("") </returns>
        public static object OnlyNumbers(object text, bool returnZero = true, bool force = false)
        {
            var result = "";

            if(string.IsNullOrWhiteSpace(text?.ToString()))
            {
                return returnZero ? 0 : (object)"";
            }

            if(force)
            {
                var regexObj = new Regex(@"[^\d]");
                result = regexObj.Replace(text.ToString(), "");
                return result;
            }

            var flagNeg = text.ToString().TrimStart().StartsWith("-");

            foreach(var c in text.ToString().ToCharArray())
            {
                if(c.Equals('.') == true || c.Equals(',') == true || char.IsNumber(c) == true)
                {
                    result += c.ToString();
                    continue;
                }
            }

            if(flagNeg == true)
            {
                result = "-" + result;
            }

            if(!returnZero && int.TryParse(result, out var intResult) && intResult == 0)
            {
                result = "";
            }

            return result;
        }

        /// <summary>
        /// Tenta converter um tipo em outro com base nas propriedades
        /// </summary>
        /// <typeparam name="T"> Tipo esperado </typeparam>
        /// <param name="value"> Objeto para conversão </param>
        /// <returns>Tenta converter para o tipo em <typeparamref name="T"/></returns>
        public static T ToAny<T>(object value) => PrepareValue<T>(value);

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="bool"/>.
        /// </summary>
        /// <param name="value">Valor a ser convertido</param>
        /// <returns>
        /// <para>Se <paramref name="value"/> for nulo, retorna false</para>
        /// <para>Se <paramref name="value"/> for zero retorna false</para>
        /// <para>Se <paramref name="value"/> for diferente de zero retorna true</para>
        /// </returns>
        public static bool ToBoolean(object value)
        {
            if(value == null)
            {
                return false;
            }

            try
            {
                var first = value.ToString()[0];
                if(char.IsNumber(first) == true)
                {
                    return short.Parse(first.ToString()) != 0;
                }

                bool.TryParse(value?.ToString(), out var result);
                return result;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="byte"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static byte ToByte(object value)
        {
            value = PrepareNumber(value);

            try
            {
                byte.TryParse(value?.ToString(), out var result);
                return result;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Converte o valor informado para uma vetor do tipo <see cref="byte[]"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static byte[] ToByteArray(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                return default;
            }

            var result = Encoding.UTF8.GetBytes(value);
            return result;
        }

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="char"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static char ToChar(object value)
        {
            try
            {
                char.TryParse(value?.ToString(), out var result);
                return result;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="DateTime"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <param name="parseExactFormat"> Formato recebido da data para conversão no padrão do sistema </param>
        /// <returns> </returns>
        public static DateTime ToDateTime(object value, string parseExactFormat = "")
        {
            try
            {
                if(!string.IsNullOrWhiteSpace(parseExactFormat))
                {
                    return DateTime.ParseExact(value?.ToString(), parseExactFormat, CultureInfo.InvariantCulture);
                }

                return Convert.ToDateTime(value);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="DateTime"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <param name="provider"> Define a localização utilizada para a formatação de datas </param>
        /// <returns> </returns>
        public static DateTime ToDateTime(object value, IFormatProvider provider)
        {
            try
            {
                DateTime.TryParse(value?.ToString(), provider, DateTimeStyles.None, out var result);
                return result;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="decimal"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static decimal ToDecimal(object value, bool invariantCulture = false)
        {
            value = PrepareNumber(value);
            try
            {
                return invariantCulture ? Convert.ToDecimal(value, CultureInfo.InvariantCulture) :
                                          Convert.ToDecimal(value);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="double"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static double ToDouble(object value, bool invariantCulture = false)
        {
            value = PrepareNumber(value).ToString();

            try
            {
                return invariantCulture ? Convert.ToDouble(value, CultureInfo.InvariantCulture) :
                                          Convert.ToDouble(value);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// converte um objeto em um valor de <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"> tipo do enum </typeparam>
        /// <param name="value"> valor do objeto </param>
        /// <returns> </returns>
        public static T ToEnum<T>(object value)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return default;
            }

            try
            {
                return (T)Enum.Parse(typeof(T), value.ToString(), true);
            }
            catch(ArgumentException)
            {
                //tenta buscar pela descrição
                return (T)ToEnum(typeof(T), value);
            }
        }

        /// <summary>
        /// Tenta converter o valor em <paramref name="value"/> no tipo definido em <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType"> Tipo do enum esperado </param>
        /// <param name="value"> possível valor do enum </param>
        /// <returns> </returns>
        public static object ToEnum(Type enumType, object value)
        {
            if(value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var valueToString = value.ToString();

            // Aqui iremos garantir um número inteiro, pois pode ser usado no eField.GetValue(e)
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/enums#enum-declarations
            if(long.TryParse(value.ToString(), out var result))
            {
                valueToString = result.ToString();
            }

            var eField = enumType.GetField("value__");

            foreach(var e in Enum.GetValues(enumType))
            {
                if(e.ToString().Equals(valueToString, StringComparison.InvariantCultureIgnoreCase) ||
                   eField.GetValue(e)?.ToString() == valueToString)
                {
                    return e;
                }
            }

            return default;
        }

        /// <summary>
        /// Calcular o valor hexadecimal de uma string
        /// </summary>
        /// <param name="input">Valor a ser convertido</param>
        /// <returns>Valor convertido em hexadecimal</returns>
        public static string ToHexadecimal(string input)
        {
            var hexOutput = "";
            var values = input.ToCharArray();
            foreach(var letter in values)
            {
                var value = Convert.ToInt32(letter);
                hexOutput += string.Format("{0:x}", value);
            }

            return hexOutput;
        }

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="int"/>
        /// </summary>
        /// <returns> </returns>
        public static int ToInt(object value) => ToInt32(value);

        /// <summary>
        /// Converte o valor informado para o tipo short
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static short ToInt16(object value)
        {
            try
            {
                short.TryParse(PrepareNumber(value).ToString(), out var result);
                return result;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="int"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static int ToInt32(object value)
        {
            try
            {
                if(value.GetType().IsEnum)
                {
                    return Convert.ToInt32(value);
                }

                int.TryParse(PrepareNumber(value).ToString(), out var result);
                return result;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="long"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static long ToInt64(object value)
        {
            try
            {
                long.TryParse(PrepareNumber(value).ToString(), out var result);
                return result;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="long"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static long ToLong(object value) => ToInt64(value);

        /// <summary>
        /// Converte um tipo <see cref="string" /> em um tipo <see cref="MemoryStream" />
        /// </summary>
        /// <param name="value"> tipo <see cref="string" /> que deve ser convertido </param>
        /// <returns>String convertida em um <see cref="MemoryStream"/></returns>
        /// <exception cref="ArgumentNullException">Se <paramref name="value"/> for nulo</exception>
        public static MemoryStream ToMemoryStream(string value)
        {
            if(value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(value);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        /// <summary>
        /// Converte um tipo <see cref="string" /> em um tipo <see cref="MemoryStream" />
        /// </summary>
        /// <param name="value"> tipo <see cref="string" /> que deve ser convertido </param>
        /// <param name="encoding">Codificação utilizada para conversão</param>
        /// <returns>String convertida em um <see cref="MemoryStream"/></returns>
        /// <exception cref="ArgumentNullException">Se <paramref name="encoding"/> for nulo</exception>
        /// <exception cref="ArgumentNullException">Se <paramref name="value"/> for nulo</exception>
        public static MemoryStream ToMemoryStream(string value, Encoding encoding)
        {
            if(value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if(encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            return ToMemoryStream(encoding.GetBytes(value));
        }

        /// <summary>
        /// Converte um tipo <see cref="string" /> em um tipo <see cref="MemoryStream" />
        /// </summary>
        /// <param name="value"> tipo <see cref="string" /> que deve ser convertido </param>
        /// <returns>String convertida em um <see cref="MemoryStream"/></returns>
        /// <exception cref="ArgumentNullException">Se <paramref name="value"/> for nulo</exception>
        public static MemoryStream ToMemoryStream(byte[] byteArray)
        {
            if(byteArray is null)
            {
                throw new ArgumentNullException(nameof(byteArray));
            }

            var memoryStream = new MemoryStream(byteArray);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        /// <summary>
        /// Converte o valor informado para o <see cref="sbyte"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static sbyte ToSByte(object value)
        {
            try
            {
                sbyte.TryParse(PrepareNumber(value).ToString(), out var result);
                return result;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="float"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static float ToSingle(object value, bool invariantCulture = false)
        {
            value = PrepareNumber(value);

            try
            {
                return invariantCulture ? Convert.ToSingle(value, CultureInfo.InvariantCulture) :
                                          Convert.ToSingle(value);
            }
            catch
            {
                return Convert.ToSingle(null);
            }
        }

        /// <summary>
        /// Converte um array de bytes em uma string com codificação UTF-8
        /// </summary>
        /// <param name="byteArray"> </param>
        /// <returns> </returns>
        public static string ToString(byte[] byteArray) => byteArray == null ? "" : Encoding.UTF8.GetString(byteArray);

        /// <summary>
        /// Converte um enumerador em <see cref="string"/>
        /// <para>Se os valores do enumerador possuírem o atributo <see cref="DescriptionAttribute"/> e <paramref name="getFromDescriptionAttributeIfExist"/> for verdadeiro  usa descrição do atributo</para>
        /// </summary>
        /// <param name="value"> Enumerador a ser convertido em <see cref="string"/> </param>
        /// <returns> </returns>
        public static string ToString(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            if(fi == null)
            {
                return value.ToString();
            }

            if(getFromDescriptionAttributeIfExist)
            {
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if(attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
            }

            return value.ToString();
        }

        /// <summary>
        /// Converte um objeto em <see cref="string"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static string ToString(object value)
        {
            try
            {
                if(value is Enum e)
                {
                    return ToString(e);
                }

                if(value == null)
                {
                    return "";
                }

                return value.ToString();
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Converte o valor informado em <see cref="TimeSpan"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static TimeSpan ToTimeSpan(object value)
        {
            if(value == null)
            {
                return TimeSpan.Zero;
            }

            try
            {
                var type = value.GetType();

                if(type == typeof(DateTime))
                {
                    return new TimeSpan(ToDateTime(value).Ticks);
                }

                if(type == typeof(int))
                {
                    return new TimeSpan(ToInt32(value));
                }

                TimeSpan.TryParse(value.ToString(), out var result);
                return result;
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Converte uma string em um objeto do tipo Type.
        /// <para> A string deverá conter a assinatura completa do objeto para ser possível converter </para>
        /// <para>Tenta criar utilizando os assemblies de entrada e referenciados.</para>
        /// </summary>
        /// <param name="type">Assinatura completa do objeto a ser convertido.</param>
        /// <returns>Nulo, se o <paramref name="type"/> for nulo, ou <see cref="Type"/></returns>
        public static Type ToType(string type)
        {
            if(string.IsNullOrEmpty(type))
            {
                return null;
            }

            //-------------------------------------------------------------------------
            // Tentar criar o tipo
            //-------------------------------------------------------------------------
            var newType = Assembly.GetCallingAssembly().CreateInstance(type,
                        true, BindingFlags.CreateInstance, null,
                        null, CultureInfo.CurrentCulture, null);

            if(newType == null)
            {
                foreach(var assembly in Assembly.GetCallingAssembly().GetReferencedAssemblies())
                {
                    newType = Assembly.Load(assembly.ToString()).CreateInstance(type,
                        true, BindingFlags.CreateInstance, null,
                        null, CultureInfo.CurrentCulture, null);

                    if(newType != null)
                    {
                        return newType.GetType();
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Converte uma <see cref="string"/> em um objeto do <see cref="Type"/> definido em <typeparamref name="T"/>
        /// <para> A string deverá conter a assinatura completa do objeto para ser possível converter </para>
        /// </summary>
        /// <param name="type"> Assinatura completa do objeto a ser convertido </param>
        /// <returns> </returns>
        public static T ToType<T>(string type)
        {
            if(string.IsNullOrEmpty(type))
            {
                return default(T);
            }

            //-------------------------------------------------------------------------
            // Tentar criar o tipo
            //-------------------------------------------------------------------------
            object o = ToType(type);
            return ToAny<T>(o);
        }

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="ushort"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static ushort ToUInt16(object value)
        {
            try
            {
                ushort.TryParse(PrepareNumber(value).ToString(), out var result);
                return result;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="uint"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static uint ToUInt32(object value)
        {
            try
            {
                uint.TryParse(PrepareNumber(value).ToString(), out var result);
                return result;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Converte o valor informado para o tipo <see cref="ulong"/>
        /// </summary>
        /// <param name="value"> Valor a ser convertido </param>
        /// <returns> </returns>
        public static ulong ToUInt64(object value)
        {
            try
            {
                ulong.TryParse(PrepareNumber(value).ToString(), out var result);
                return result;
            }
            catch
            {
                return default;
            }
        }

        #endregion Public Methods
    }
}