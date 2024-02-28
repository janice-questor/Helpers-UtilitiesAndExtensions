using System.ComponentModel;

namespace Unimake.Primitives.Enumerations
{
    /// <summary>
    /// São os tipos de códigos de barras, tais como: GTIN-8, GTIN-12, GTIN13, etc
    /// </summary>
    public enum BarcodeType
    {
        /// <summary>
        /// Não foi definido nenhum tipo de código
        /// </summary>
        [Description("Não definido")]
        Undefined = -1,

        /// <summary>
        /// Código interno definido pela empresa
        /// </summary>
        [Description("GTIN-8 (antigo EAN-8)")]
        Internal = 0,

        /// <summary>
        /// GTIN-8 (antigo EAN-8)
        /// </summary>
        [Description("GTIN-8 (antigo EAN-8)")]
        GTIN_8 = 1,

        /// <summary>
        /// GTIN-12 (antigo UPC)
        /// </summary>
        [Description("GTIN-12 (antigo UPC)")]
        GTIN_12 = 2,

        /// <summary>
        /// GTIN-13 (antigo EAN-13)
        /// </summary>
        [Description("GTIN-13 (antigo EAN-13)")]
        GTIN_13 = 3,

        /// <summary>
        /// GTIN-14 (antigo DUN-14)
        /// </summary>
        [Description("GTIN-14 (antigo DUN-14)")]
        GTIN_14 = 4,

        /// <summary>
        /// Código 3 de 9
        /// </summary>
        [Description("Código 3 de 9")]
        Code39 = 5,

        /// <summary>
        /// GS1-128 Antigo UCC/EAN-128
        /// </summary>
        [Description("GS1-128 (antigo UCC/EAN-128)")]
        GS1_128 = 6
    }
}