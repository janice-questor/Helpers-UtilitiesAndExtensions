using Unimake.Primitives.CommonTypes;
using Unimake.Primitives.Converters.Abstractions;

namespace Unimake.Primitives.Converters
{
    internal class RenavamTypeConverter : TypeConverterBase
    {
        #region Public Methods

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) =>
            (Renavam)(value as string);

        #endregion Public Methods
    }
}