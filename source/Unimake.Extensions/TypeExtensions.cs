using System.Reflection;
using Unimake;

namespace System
{
    public static class TypeExtensions
    {
        #region Public Methods

        public static Type GetDataType(this Type type) => type.IsNullable() ? Nullable.GetUnderlyingType(type) : type;

        public static bool IsDateTime(this Type type) => Type.GetTypeCode(type) == TypeCode.DateTime;

        public static bool IsDecimal(this Type type) => Type.GetTypeCode(type) == TypeCode.Decimal;

        public static bool IsDouble(this Type type) => Type.GetTypeCode(type) == TypeCode.Double;

        public static bool IsEnum(this Type type) => type.GetTypeInfo().IsEnum;

        public static bool IsInt(this Type type) => Type.GetTypeCode(type) == TypeCode.Int32;

        public static bool IsLong(this Type type) => Type.GetTypeCode(type) == TypeCode.UInt64;

        /// <summary>
        /// Retorna verdadeiro se o tipo é um tipo numérico
        /// </summary>
        /// <param name="type">Tipo para avaliação</param>
        /// <returns></returns>
        public static bool IsNumeric(this Type type)
        {
            switch(Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;

                default:
                    return false;
            }
        }

        #endregion Public Methods
    }
}