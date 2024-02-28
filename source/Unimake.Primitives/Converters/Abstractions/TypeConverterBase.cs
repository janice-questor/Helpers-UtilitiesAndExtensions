using System;
using System.ComponentModel;

namespace Unimake.Primitives.Converters.Abstractions
{
    /// <summary>
    /// Classe de base para os tipos conversores
    /// </summary>
    public abstract class TypeConverterBase : TypeConverter
    {
        #region Public Properties

        /// <summary>
        /// Tipo de conversão permitida.
        /// <para>O padrão é o tipo string</para>
        /// </summary>
        public virtual Type AllowedConversionType => typeof(string);

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Retorna se o valor pode ser convertido no tipo esperado
        /// </summary>
        /// <param name="context">Contexto atual em que a conversão está sendo realizada</param>
        /// <param name="sourceType">Tipo do objeto que deverá ser convertido</param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if(sourceType == AllowedConversionType)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Realiza a conversão do objeto para o tipo esperado
        /// </summary>
        /// <param name="context">Contexto atual em que a conversão está sendo realizada</param>
        /// <param name="culture">A cultura em que a conversão está sendo realizada</param>
        /// <param name="value">O valor que deverá ser convertido</param>
        /// <returns></returns>
        public abstract override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value);

        #endregion Public Methods
    }
}