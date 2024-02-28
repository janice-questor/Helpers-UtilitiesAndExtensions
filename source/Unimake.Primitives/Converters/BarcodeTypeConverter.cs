using Unimake.Primitives.CommonTypes;
using Unimake.Primitives.Converters.Abstractions;

namespace Unimake.Primitives.Converters
{
    /// <summary>
    /// Conversor para o tipo CodeBar
    /// </summary>
    internal class BarcodeTypeConverter : TypeConverterBase
    {
        #region Public Methods

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) =>
            value == null ? "" : (object)(Barcode)(value as string);

        #endregion Public Methods
    }
}