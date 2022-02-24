namespace System.Reflection
{
    public static class PropertyExtensions
    {
        #region Public Methods

        public static Type GetPropertyDataType(this PropertyInfo propertyInfo) => propertyInfo.PropertyType.GetDataType();

        public static bool IsDateTime(this PropertyInfo propertyInfo) => propertyInfo.GetPropertyDataType().IsDateTime();

        public static bool IsDecimal(this PropertyInfo propertyInfo) => propertyInfo.GetPropertyDataType().IsDecimal();

        public static bool IsDouble(this PropertyInfo propertyInfo) => propertyInfo.GetPropertyDataType().IsDouble();

        public static bool IsEnum(this PropertyInfo propertyInfo) => propertyInfo.GetPropertyDataType().IsEnum();

        public static bool IsInt(this PropertyInfo propertyInfo) => propertyInfo.GetPropertyDataType().IsInt();

        public static bool IsLong(this PropertyInfo propertyInfo) => propertyInfo.GetPropertyDataType().IsLong();

        public static bool IsNullable(this PropertyInfo propertyInfo) => propertyInfo.PropertyType.IsNullable();

        public static bool IsNumeric(this PropertyInfo propertyInfo) => propertyInfo.GetPropertyDataType().IsNumeric();

        #endregion Public Methods
    }
}