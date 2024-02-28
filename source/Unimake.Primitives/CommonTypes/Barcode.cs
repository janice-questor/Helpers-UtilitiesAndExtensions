using System;
using System.ComponentModel;
using Unimake.Primitives.Converters;
using Unimake.Primitives.Enumerations;
using Unimake.Primitives.Parser.Barcode;

namespace Unimake.Primitives.CommonTypes
{
    /// <summary>
    /// Tipo de CodeBar
    /// </summary>
    [TypeConverter(typeof(BarcodeTypeConverter))]
    public struct Barcode : ICloneable<Barcode>
    {
        #region Private Fields

        private readonly string _value;

        #endregion Private Fields

        #region Private Constructors

        /// <summary>
        /// Construtor do objeto CodeBar.
        /// </summary>
        /// <param name="value">
        /// Código de barras informado. Pode ser um GTIN 13 ou GS1 128 (EAN 128)
        /// </param>
        private Barcode(string value)
        {
            value = EAN128.GetEAN_NumberOfTradingUnit(value);
            _value = value ?? "";
            BarcodeType = BarcodeType.Undefined;
            BarcodeType = DetectType();
        }

        #endregion Private Constructors

        #region Private Methods

        private BarcodeType DetectType()
        {
            switch(_value.Length)
            {
                case 8:
                    return BarcodeType.GTIN_8;

                case 12:
                    return BarcodeType.GTIN_12;

                case 13:
                    return BarcodeType.GTIN_13;

                case 14:
                    return BarcodeType.GTIN_14;

                case 44:
                    return BarcodeType.GS1_128;

                default:
                    if(HasCharsAndNumbers)
                    {
                        return BarcodeType.Code39;
                    }

                    return BarcodeType.Undefined;
            }
        }

        #endregion Private Methods

        #region Public Properties

        /// <summary>
        /// Define o tipo de código de barras
        /// </summary>
        public BarcodeType BarcodeType { get; set; }

        /// <summary>
        /// Retorna verdadeiro se o código de barras possui caracteres e números
        /// </summary>
        public bool HasCharsAndNumbers
        {
            get
            {
                if(string.IsNullOrWhiteSpace(_value))
                {
                    return false;
                }

                var hasChar = false;
                var hasNumber = false;

                foreach(var c in _value)
                {
                    if(!hasNumber && char.IsNumber(c))
                    {
                        hasNumber = true;
                    }
                    else if(!hasChar && char.IsLetterOrDigit(c))
                    {
                        hasChar = true;
                    }

                    if(hasChar && hasNumber)
                    {
                        break;
                    }
                }

                return hasChar && hasNumber;
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Operador implícito do objeto CodeBar
        /// </summary>
        /// <param name="value">CodeBar informado</param>
        /// <returns></returns>
        public static implicit operator Barcode(string value) => new Barcode(value);

        /// <summary>
        /// Operador implícito do objeto CodeBar
        /// </summary>
        /// <param name="value">CodeBar informado</param>
        /// <returns></returns>
        public static implicit operator string(Barcode value) => value.ToString();

        /// <summary>
        /// Valida o código de barra de acordo com o tipo
        /// </summary>
        /// <param name="barcode">Código de barras que será validado</param>
        /// <param name="allowNullOrEmpty">
        /// Se verdadeiro, retorna verdadeiro quando o código for nulo ou vazio
        /// </param>
        /// <returns></returns>
        public static bool Validate(Barcode barcode, bool allowNullOrEmpty) => Validate(barcode, barcode.BarcodeType, allowNullOrEmpty);

        /// <summary>
        /// Valida o código de barra de acordo com o tipo
        /// </summary>
        /// <param name="barcode">Código de barras que será validado</param>
        /// <param name="type">Tipo do codebar</param>
        /// <param name="allowNullOrEmpty">
        /// Se verdadeiro, retorna verdadeiro quando o código for nulo ou vazio
        /// </param>
        /// <returns></returns>
        public static bool Validate(string barcode, BarcodeType type, bool allowNullOrEmpty)
        {
            if(string.IsNullOrWhiteSpace(barcode) && !allowNullOrEmpty)
            {
                return false;
            }

            var result = false;
            //Dígitos que serão multiplicados
            var checkSum = "3131313131313";
            //Somatória dos dígitos do código de barra
            double sum = 0;
            if(type != BarcodeType.Internal &&
                type != BarcodeType.Code39 &&
                type != BarcodeType.Undefined)
            {
                barcode = Utils.OnlyNumbers(barcode);
                if(string.IsNullOrEmpty(barcode) || barcode.Length == 0)
                {
                    return result;
                }
            }

            //Código de barra
            var code = barcode;
            //Dígito verificador
            var digit = Convert.ToInt32(barcode.Substring(barcode.Length - 1, 1));

            //Valor do resto da divisão
            int rest;
            //posição inicial para determinar qual o valor a ser utilizado do checkSum
            int position;
            //Valor a ser comparado com o digito verificador, para ver se é verdadeiro ou falso
            double calculated;
            switch(type)
            {
                case BarcodeType.GTIN_8:
                    if(barcode.Length > 8)
                    {
                        result = false;
                        break;
                    }
                    //Inicia a multiplicação dos valores de acordo com o tipo de código de barra
                    position = checkSum.Length + 1 - code.Length;
                    code = code.Substring(0, barcode.Length - 1);
                    //De acordo com o tipo, faz a multiplicação dos valores para achar a somatória dos digitos do código de barra
                    for(var i = 0; i <= code.Length - 1; i++)
                    {
                        sum += Convert.ToDouble(code.Substring(i, 1)) * Convert.ToDouble(checkSum.Substring(position, 1));
                        position++;
                    }

                    rest = Convert.ToInt32(sum % 10);
                    calculated = sum / 10;
                    //Quando o resto é maior, então temos que pegar o maior múltiplo
                    if(rest > 0)
                    {
                        calculated = Math.Truncate(calculated);
                        calculated = (calculated + 1) * 10;
                    }
                    else
                    {
                        calculated = Math.Round(calculated, 0);
                        calculated *= 10;
                    }

                    sum = Math.Round(sum, 0);
                    //Subtrai o calculado pela somatória dos dígitos do código de barra
                    calculated = Math.Round(calculated, 0) - sum;
                    //Se o dígito verificador for igual ao calculado, então o código de barra é válido
                    result = digit == Convert.ToInt32(calculated);
                    break;

                case BarcodeType.GTIN_12:
                    if(barcode.Length > 12)
                    {
                        result = false;
                        break;
                    }
                    //Inicia a multiplicação dos valores de acordo com o tipo de código de barra
                    position = checkSum.Length + 1 - code.Length;
                    code = code.Substring(0, barcode.Length - 1);
                    //De acordo com o tipo, faz a multiplicação dos valores para achar a somatória dos digitos do código de barra
                    for(var i = 0; i <= code.Length - 1; i++)
                    {
                        sum += Convert.ToDouble(code.Substring(i, 1)) * Convert.ToDouble(checkSum.Substring(position, 1));
                        position++;
                    }

                    rest = Convert.ToInt32(sum % 10);
                    calculated = sum / 10;
                    //Quando o resto é maior, então temos que pegar o maior múltiplo
                    if(rest > 0)
                    {
                        calculated = Math.Truncate(calculated);
                        calculated = (calculated + 1) * 10;
                    }
                    else
                    {
                        calculated = Math.Round(calculated, 0);
                        calculated *= 10;
                    }

                    sum = Math.Round(sum, 0);
                    //Subtrai o calculado pela somatória dos dígitos do código de barra
                    calculated = Math.Round(calculated, 0) - sum;
                    //Se o dígito verificador for igual ao calculado, então o código de barra é válido
                    result = digit == Convert.ToInt32(calculated);
                    break;

                case BarcodeType.GTIN_13:
                    if(barcode.Length > 13)
                    {
                        result = false;
                        break;
                    }
                    //Inicia a multiplicação dos valores de acordo com o tipo de código de barra
                    position = checkSum.Length + 1 - code.Length;
                    code = code.Substring(0, barcode.Length - 1);
                    //De acordo com o tipo, faz a multiplicação dos valores para achar a somatória dos digitos do código de barra
                    for(var i = 0; i <= code.Length - 1; i++)
                    {
                        sum += Convert.ToDouble(code.Substring(i, 1)) * Convert.ToDouble(checkSum.Substring(position, 1));
                        position++;
                    }

                    rest = Convert.ToInt32(sum % 10);
                    calculated = sum / 10;
                    //Quando o resto é maior, então temos que pegar o maior múltiplo
                    if(rest > 0)
                    {
                        calculated = Math.Truncate(calculated);
                        calculated = (calculated + 1) * 10;
                    }
                    else
                    {
                        calculated = Math.Round(calculated, 0);
                        calculated *= 10;
                    }

                    sum = Math.Round(sum, 0);
                    //Subtrai o calculado pela somatória dos dígitos do código de barra
                    calculated = Math.Round(calculated, 0) - sum;
                    //Se o dígito verificador for igual ao calculado, então o código de barra é válido
                    result = digit == Convert.ToInt32(calculated);
                    break;

                case BarcodeType.GTIN_14:
                    if(barcode.Length > 14)
                    {
                        result = false;
                        break;
                    }
                    //Inicia a multiplicação dos valores de acordo com o tipo de código de barra
                    position = checkSum.Length + 1 - code.Length;
                    code = code.Substring(0, barcode.Length - 1);
                    //De acordo com o tipo, faz a multiplicação dos valores para achar a somatória dos digitos do código de barra
                    for(var i = 0; i <= code.Length - 1; i++)
                    {
                        sum += Convert.ToDouble(code.Substring(i, 1)) * Convert.ToDouble(checkSum.Substring(position, 1));
                        position++;
                    }

                    rest = Convert.ToInt32(sum % 10);
                    calculated = sum / 10;
                    //Quando o resto é maior, então temos que pegar o maior múltiplo
                    if(rest > 0)
                    {
                        calculated = Math.Truncate(calculated);
                        calculated = (calculated + 1) * 10;
                    }
                    else
                    {
                        calculated = Math.Round(calculated, 0);
                        calculated *= 10;
                    }

                    sum = Math.Round(sum, 0);
                    //Subtrai o calculado pela somatória dos dígitos do código de barra
                    calculated = Math.Round(calculated, 0) - sum;
                    //Se o dígito verificador for igual ao calculado, então o código de barra é válido
                    result = digit == Convert.ToInt32(calculated);
                    break;

                case BarcodeType.Internal:
                    //Se for interno não é validado e sempre será válido
                    result = true;
                    break;

                case BarcodeType.Code39:
                    result = !string.IsNullOrWhiteSpace(barcode);
                    break;

                default:
                    result = false;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Retorna uma cópia deste objeto
        /// </summary>
        /// <returns>Uma cópia deste objeto</returns>
        public Barcode Clone() => _value;

        /// <summary>
        /// Compara o CodeBar informado
        /// </summary>
        /// <param name="value">CodeBar informado</param>
        /// <returns></returns>
        public bool Equals(Barcode value) => value != null && base.Equals(value) && _value == value._value;

        /// <summary>
        /// Compara o CodeBar através do objeto
        /// </summary>
        /// <param name="obj">Objeto a ser informado</param>
        /// <returns></returns>
        public override bool Equals(object obj) => obj != null && GetType() == obj.GetType() && Equals((Barcode)obj);

        /// <summary>
        /// Retorna o hashcode do valor
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => _value.GetHashCode();

        /// <summary>
        /// Converte para o tipo string
        /// </summary>
        /// <returns></returns>
        public override string ToString() => _value;

        /// <summary>
        /// Retorna verdadeiro se o código de barras for válido, ou falso.
        /// </summary>
        /// <returns></returns>
        public bool Validate() => Validate(this, BarcodeType, true);

        /// <summary>
        /// Retorna uma cópia deste objeto
        /// </summary>
        /// <returns>Uma cópia deste objeto</returns>
        object ICloneable.Clone() => Clone();

        #endregion Public Methods
    }
}