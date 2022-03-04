using System.ComponentModel;

namespace System
{
    /// <summary>
    /// Extensões para o tipo enum
    /// </summary>
    public static class EnumExtensions
    {
        #region Public Methods

        /// <summary>
        /// Retorna a descrição do enum com base no atributo [Description]
        /// </summary>
        /// <param name="value">Valor do enum para pesquisar a descrição</param>
        /// <returns>Descrição do valor do enum</returns>
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if(name != null)
            {
                var field = type.GetField(name);
                if(field != null)
                {
                    if(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                    {
                        return attr.Description;
                    }
                }
            }

            return value.ToString();
        }

        /// <summary>
        /// Retorna se o valor informado no enum é válido, se existir na lista de valores, irá retornar true
        /// </summary>
        /// <param name="value">possível valor do enum</param>
        /// <returns></returns>
        public static bool IsValid(this Enum value)
        {
            if(value == null)
            {
                return false;
            }

            return Enum.IsDefined(value.GetType(), value);
        }

        #endregion Public Methods
    }
}