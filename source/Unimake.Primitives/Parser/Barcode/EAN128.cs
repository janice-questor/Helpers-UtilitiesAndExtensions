using System;
using System.Collections.Generic;
using System.Linq;

namespace Unimake.Primitives.Parser.Barcode
{
    /// <summary>
    /// Parse the EAN 128 code to
    /// </summary>
    public static class EAN128
    {
        /// <summary>
        /// Define the key constants for the parser
        /// </summary>
        public static class KeyConstants
        {
            /// <summary>
            /// Serial Shipping Container Code
            /// </summary>
            public const string SerialShippingContainerCode = "00";

            /// <summary>
            /// EAN (Number Of Trading Unit)
            /// </summary>
            public const string EAN_NumberOfTradingUnit = "01";

            /// <summary>
            /// EAN (Number Of The Wares In The Shipping Unit)
            /// </summary>
            public const string EAN_NumberOfTheWaresInTheShippingUnit = "02";

            /// <summary>
            /// Charge Number
            /// </summary>
            public const string Charge_Number = "10";

            /// <summary>
            /// Producer Date (MMDD)
            /// </summary>
            public const string ProducerDate_MMDD = "11";

            /// <summary>
            /// Due Date (MMDD)
            /// </summary>
            public const string DueDate_MMDD = "12";

            /// <summary>
            ///
            /// </summary>
            public const string PackingDate_MMMMDD = "13";

            /// <summary>
            ///
            /// </summary>
            public const string MinimumDurabilityDate_MMMMDD = "15";

            /// <summary>
            ///
            /// </summary>
            public const string ExpiryDate_MMMMDD = "17";

            /// <summary>
            ///
            /// </summary>
            public const string ProductModel = "20";

            /// <summary>
            ///
            /// </summary>
            public const string SerialNumber = "21";

            /// <summary>
            ///
            /// </summary>
            public const string HIBCCNumber = "22";

            /// <summary>
            ///
            /// </summary>
            public const string PruductIdentificationOfProducer = "240";

            /// <summary>
            ///
            /// </summary>
            public const string CustomerPartsNumber = "241";

            /// <summary>
            ///
            /// </summary>
            public const string SerialNumberOfAIntegratedModule = "250";

            /// <summary>
            ///
            /// </summary>
            public const string ReferenceToTheBasisUnit = "251";

            /// <summary>
            ///
            /// </summary>
            public const string GlobalIdentifierSerialisedForTrade = "252";

            /// <summary>
            ///
            /// </summary>
            public const string AmountInParts = "30";

            /// <summary>
            ///
            /// </summary>
            public const string NetWeight_Kilogram = "310d";

            /// <summary>
            ///
            /// </summary>
            public const string Length_Meter = "311d";

            /// <summary>
            ///
            /// </summary>
            public const string Width_Meter = "312d";

            /// <summary>
            ///
            /// </summary>
            public const string Heigth_Meter = "313d";

            /// <summary>
            ///
            /// </summary>
            public const string Surface_SquareMeter = "314d";

            /// <summary>
            ///
            /// </summary>
            public const string NetVolume_Liters = "315d";

            /// <summary>
            ///
            /// </summary>
            public const string NetVolume_CubicMeters = "316d";

            /// <summary>
            ///
            /// </summary>
            public const string NetWeight_Pounds = "320d";

            /// <summary>
            ///
            /// </summary>
            public const string Length_Inches = "321d";

            /// <summary>
            ///
            /// </summary>
            public const string Length_Feet = "322d";

            /// <summary>
            ///
            /// </summary>
            public const string Length_Yards = "323d";

            /// <summary>
            ///
            /// </summary>
            public const string Width_Inches = "324d";

            /// <summary>
            ///
            /// </summary>
            public const string Width_Feed = "325d";

            /// <summary>
            ///
            /// </summary>
            public const string Width_Yards = "326d";

            /// <summary>
            ///
            /// </summary>
            public const string Heigth_Inches = "327d";

            /// <summary>
            ///
            /// </summary>
            public const string Heigth_Feed = "328d";

            /// <summary>
            ///
            /// </summary>
            public const string Heigth_Yards = "329d";

            /// <summary>
            ///
            /// </summary>
            public const string GrossWeight_Kilogram = "330d";

            /// <summary>
            ///
            /// </summary>
            public const string Length_Meter2 = "331d";

            /// <summary>
            ///
            /// </summary>
            public const string Width_Meter2 = "332d";

            /// <summary>
            ///
            /// </summary>
            public const string Heigth_Meter2 = "333d";

            /// <summary>
            ///
            /// </summary>
            public const string Surface_SquareMeter2 = "334d";

            /// <summary>
            ///
            /// </summary>
            public const string GrossVolume_Liters = "335d";

            /// <summary>
            ///
            /// </summary>
            public const string GrossVolume_CubicMeters = "336d";

            /// <summary>
            ///
            /// </summary>
            public const string KilogramPerSquareMeter = "337d";

            /// <summary>
            ///
            /// </summary>
            public const string GrossWeight_Pounds = "340d";

            /// <summary>
            ///
            /// </summary>
            public const string Length_Inches2 = "341d";

            /// <summary>
            ///
            /// </summary>
            public const string Length_Feet2 = "342d";

            /// <summary>
            ///
            /// </summary>
            public const string Length_Yards2 = "343d";

            /// <summary>
            ///
            /// </summary>
            public const string Width_Inches2 = "344d";

            /// <summary>
            ///
            /// </summary>
            public const string Width_Feed2 = "345d";

            /// <summary>
            ///
            /// </summary>
            public const string Width_Yards2 = "346d";

            /// <summary>
            ///
            /// </summary>
            public const string Heigth_Inches2 = "347d";

            /// <summary>
            ///
            /// </summary>
            public const string Heigth_Feed2 = "348d";

            /// <summary>
            ///
            /// </summary>
            public const string Heigth_Yards2 = "349d";

            /// <summary>
            ///
            /// </summary>
            public const string Surface_SquareInches = "350d";

            /// <summary>
            ///
            /// </summary>
            public const string Surface_SquareFeet = "351d";

            /// <summary>
            ///
            /// </summary>
            public const string Surface_SquareYards = "352d";

            /// <summary>
            ///
            /// </summary>
            public const string Surface_SquareInches2 = "353d";

            /// <summary>
            ///
            /// </summary>
            public const string Surface_SquareFeed = "354d";

            /// <summary>
            ///
            /// </summary>
            public const string Surface_SquareYards2 = "355d";

            /// <summary>
            ///
            /// </summary>
            public const string NetWeight_TroyOunces = "356d";

            /// <summary>
            ///
            /// </summary>
            public const string NetVolume_Ounces = "357d";

            /// <summary>
            ///
            /// </summary>
            public const string NetVolume_Quarts = "360d";

            /// <summary>
            ///
            /// </summary>
            public const string NetVolume_Gallonen = "361d";

            /// <summary>
            ///
            /// </summary>
            public const string GrossVolume_Quarts = "362d";

            /// <summary>
            ///
            /// </summary>
            public const string GrossVolume_Gallonen = "363d";

            /// <summary>
            ///
            /// </summary>
            public const string NetVolume_CubicInches = "364d";

            /// <summary>
            ///
            /// </summary>
            public const string NetVolume_CubicFeet = "365d";

            /// <summary>
            ///
            /// </summary>
            /// <summary>
            ///
            /// </summary>
            public const string NetVolume_CubicYards = "366d";

            /// <summary>
            ///
            /// </summary>
            public const string GrossVolume_CubicInches = "367d";

            /// <summary>
            ///
            /// </summary>
            /// <summary>
            ///
            /// </summary>
            public const string GrossVolume_CubicFeet = "368d";

            /// <summary>
            ///
            /// </summary>
            public const string GrossVolume_CubicYards = "369d";

            /// <summary>
            ///
            /// </summary>
            public const string QuantityInParts = "37";

            /// <summary>
            ///
            /// </summary>
            public const string AmountDue_DefinedValutaBand = "390d";

            /// <summary>
            ///
            /// </summary>
            public const string AmountDue_WithISOValutaCode = "391d";

            /// <summary>
            ///
            /// </summary>
            public const string BePayingAmount_DefinedValutaBand = "392d";

            /// <summary>
            ///
            /// </summary>
            public const string BePayingAmount_WithISOValutaCode = "393d";

            /// <summary>
            ///
            /// </summary>
            public const string JobNumberOfGoodsRecipient = "400";

            /// <summary>
            ///
            /// </summary>
            public const string ShippingNumber = "401";

            /// <summary>
            ///
            /// </summary>
            public const string DeliveryNumber = "402";

            /// <summary>
            ///
            /// </summary>
            public const string RoutingCode = "403";

            /// <summary>
            ///
            /// </summary>
            public const string EAN_UCC_GlobalLocationNumber_GoodsRecipient = "410";

            /// <summary>
            ///
            /// </summary>
            public const string EAN_UCC_GlobalLocationNumber_InvoiceRecipient = "411";

            /// <summary>
            ///
            /// </summary>
            public const string EAN_UCC_GlobalLocationNumber_Distributor = "412";

            /// <summary>
            ///
            /// </summary>
            public const string EAN_UCC_GlobalLocationNumber_FinalRecipient = "413";

            /// <summary>
            ///
            /// </summary>
            public const string EAN_UCC_GlobalLocationNumber_PhysicalLocation = "414";

            /// <summary>
            ///
            /// </summary>
            public const string EAN_UCC_GlobalLocationNumber_ToBilligParticipant = "415";

            /// <summary>
            ///
            /// </summary>
            public const string ZipCodeOfRecipient_withoutCountryCode = "420";

            /// <summary>
            ///
            /// </summary>
            public const string ZipCodeOfRecipient_withCountryCode = "421";

            /// <summary>
            ///
            /// </summary>
            public const string BasisCountryOfTheWares_ISO3166Format = "422";

            /// <summary>
            ///
            /// </summary>
            public const string NatoStockNumber = "7001";

            /// <summary>
            ///
            /// </summary>
            public const string RolesProducts = "8001";

            /// <summary>
            ///
            /// </summary>
            public const string SerialNumberForMobilePhones = "8002";

            /// <summary>
            ///
            /// </summary>
            public const string GlobalReturnableAssetIdentifier = "8003";

            /// <summary>
            ///
            /// </summary>
            public const string GlobalIndividualAssetIdentifier = "8004";

            /// <summary>
            ///
            /// </summary>
            public const string SalesPricePerUnit = "8005";

            /// <summary>
            ///
            /// </summary>
            public const string IdentifikationOfAProductComponent = "8006";

            /// <summary>
            ///
            /// </summary>
            public const string IBAN = "8007";

            /// <summary>
            ///
            /// </summary>
            public const string DataAndTimeOfManufacturing = "8008";

            /// <summary>
            ///
            /// </summary>
            public const string GlobalServiceRelationNumber = "8018";

            /// <summary>
            ///
            /// </summary>
            public const string NumberBillCoverNumber = "8020";

            /// <summary>
            ///
            /// </summary>
            public const string CouponExtendedCode_NSC_offerCcode = "8100";

            /// <summary>
            ///
            /// </summary>
            public const string CouponExtendedCode_NSC_offerCcode_EndOfOfferCode = "8101";

            /// <summary>
            ///
            /// </summary>
            public const string CouponExtendedCode_NSC = "8102";

            /// <summary>
            ///
            /// </summary>
            public const string InformationForBilateralCoordinatedApplications = "90";

            /// <summary>
            ///
            /// </summary>
            public const string CompanySpecific91 = "91";

            /// <summary>
            ///
            /// </summary>
            public const string CompanySpecific92 = "92";

            /// <summary>
            ///
            /// </summary>
            public const string CompanySpecific93 = "93";

            /// <summary>
            ///
            /// </summary>
            public const string CompanySpecific94 = "94";

            /// <summary>
            ///
            /// </summary>
            public const string CompanySpecific95 = "95";

            /// <summary>
            ///
            /// </summary>
            public const string CompanySpecific96 = "96";

            /// <summary>
            ///
            /// </summary>
            public const string CompanySpecific97 = "97";

            /// <summary>
            ///
            /// </summary>
            public const string CompanySpecific98 = "98";

            /// <summary>
            ///
            /// </summary>
            public const string CompanySpecific99 = "99";
        }

        /// <summary>
        /// Define de identifiers code
        /// </summary>
        public enum DataType
        {
            /// <summary>
            /// only numbers
            /// </summary>
            Numeric,

            /// <summary>
            /// Numbers and letters
            /// </summary>
            Alphanumeric
        }

        /// <summary>
        /// Information Class for an Application Identifier (AI)
        /// </summary>
        public class AII
        {
            /// <summary>
            /// Application Identifier
            /// </summary>
            public string AI { get; set; }

            /// <summary>
            /// Description of AI
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Length of AI
            /// </summary>
            public int LengthOfAI { get; set; }

            /// <summary>
            /// Data description of AI
            /// </summary>
            public DataType DataDescription { get; set; }

            /// <summary>
            /// Length of AI data
            /// </summary>
            public int LengthOfData { get; set; }

            /// <summary>
            /// The FNC1 is a single character Function Code 1, which  specifies that a barcode is a GS1 code
            /// </summary>
            public bool FNC1 { get; set; }

            /// <summary>
            /// Initialize the AI class
            /// </summary>
            /// <param name="ai">Aplication identifier of code</param>
            /// <param name="description">Description of AI identifier</param>
            /// <param name="lengthOfAI">Length of AI</param>
            /// <param name="dataDescription">Description of AI data</param>
            /// <param name="lengthOfData">Length of AI data</param>
            /// <param name="fnc1">if true, use identify this code as GS1 code</param>
            public AII(string ai, string description, int lengthOfAI, DataType dataDescription, int lengthOfData, bool fnc1)
            {
                this.AI = ai;
                this.Description = description;
                this.LengthOfAI = lengthOfAI;
                this.DataDescription = dataDescription;
                this.LengthOfData = lengthOfData;
                this.FNC1 = fnc1;
            }

            /// <summary>
            /// Return the string represetantion of this class
            /// </summary>
            /// <returns></returns>
            public override string ToString() => string.Format("{0} [{1}]", AI, Description);
        }

        private static readonly SortedList<string, AII> aiiDict = new SortedList<string, AII>();
        private static readonly int minLengthOfAI = 1;
        private static readonly int maxLengthOfAI = 4;
        private static char groutSeperator = (char)29;
        private static string ean128StartCode = "]C1";
        private static bool hasCheckSum = false;

        /// <summary>
        /// Returns if the code has check sum
        /// </summary>
        public static bool HasCheckSum
        {
            get => EAN128.hasCheckSum;
            set => EAN128.hasCheckSum = value;
        }

        /// <summary>
        /// Grout separator
        /// </summary>
        public static char GroutSeperator
        {
            get => EAN128.groutSeperator;
            set => EAN128.groutSeperator = value;
        }

        /// <summary>
        /// indicate the start code of EAN 128
        /// </summary>
        public static string EAN128StartCode
        {
            get => EAN128.ean128StartCode;
            set => EAN128.ean128StartCode = value;
        }

        static EAN128()
        {
            Add("00", "SerialShippingContainerCode", 2, DataType.Numeric, 18, false);
            Add("01", "EAN-NumberOfTradingUnit", 2, DataType.Numeric, 14, false);
            Add("02", "EAN-NumberOfTheWaresInTheShippingUnit", 2, DataType.Numeric, 14, false);
            Add("10", "Charge_Number", 2, DataType.Alphanumeric, 8, true);
            Add("11", "ProducerDate_JJMMDD", 2, DataType.Numeric, 6, false);
            Add("12", "DueDate_JJMMDD", 2, DataType.Numeric, 6, false);
            Add("13", "PackingDate_JJMMDD", 2, DataType.Numeric, 6, false);
            Add("15", "MinimumDurabilityDate_JJMMDD", 2, DataType.Numeric, 6, false);
            Add("17", "ExpiryDate_JJMMDD", 2, DataType.Numeric, 6, false);
            Add("20", "ProductModel", 2, DataType.Numeric, 2, false);
            Add("21", "SerialNumber", 2, DataType.Alphanumeric, 20, true);
            Add("22", "HIBCCNumber", 2, DataType.Alphanumeric, 29, false);
            Add("240", "PruductIdentificationOfProducer", 3, DataType.Alphanumeric, 30, true);
            Add("241", "CustomerPartsNumber", 3, DataType.Alphanumeric, 30, true);
            Add("250", "SerialNumberOfAIntegratedModule", 3, DataType.Alphanumeric, 30, true);
            Add("251", "ReferenceToTheBasisUnit", 3, DataType.Alphanumeric, 30, true);
            Add("252", "GlobalIdentifierSerialisedForTrade", 3, DataType.Numeric, 2, false);
            Add("30", "AmountInParts", 2, DataType.Numeric, 4, true);
            Add("310d", "NetWeight_Kilogram", 4, DataType.Numeric, 6, false);
            Add("311d", "Length_Meter", 4, DataType.Numeric, 6, false);
            Add("312d", "Width_Meter", 4, DataType.Numeric, 6, false);
            Add("313d", "Heigth_Meter", 4, DataType.Numeric, 6, false);
            Add("314d", "Surface_SquareMeter", 4, DataType.Numeric, 6, false);
            Add("315d", "NetVolume_Liters", 4, DataType.Numeric, 6, false);
            Add("316d", "NetVolume_CubicMeters", 4, DataType.Numeric, 6, false);
            Add("320d", "NetWeight_Pounds", 4, DataType.Numeric, 6, false);
            Add("321d", "Length_Inches", 4, DataType.Numeric, 6, false);
            Add("322d", "Length_Feet", 4, DataType.Numeric, 6, false);
            Add("323d", "Length_Yards", 4, DataType.Numeric, 6, false);
            Add("324d", "Width_Inches", 4, DataType.Numeric, 6, false);
            Add("325d", "Width_Feed", 4, DataType.Numeric, 6, false);
            Add("326d", "Width_Yards", 4, DataType.Numeric, 6, false);
            Add("327d", "Heigth_Inches", 4, DataType.Numeric, 6, false);
            Add("328d", "Heigth_Feed", 4, DataType.Numeric, 6, false);
            Add("329d", "Heigth_Yards", 4, DataType.Numeric, 6, false);
            Add("330d", "GrossWeight_Kilogram", 4, DataType.Numeric, 6, false);
            Add("331d", "Length_Meter", 4, DataType.Numeric, 6, false);
            Add("332d", "Width_Meter", 4, DataType.Numeric, 6, false);
            Add("333d", "Heigth_Meter", 4, DataType.Numeric, 6, false);
            Add("334d", "Surface_SquareMeter", 4, DataType.Numeric, 6, false);
            Add("335d", "GrossVolume_Liters", 4, DataType.Numeric, 6, false);
            Add("336d", "GrossVolume_CubicMeters", 4, DataType.Numeric, 6, false);
            Add("337d", "KilogramPerSquareMeter", 4, DataType.Numeric, 6, false);
            Add("340d", "GrossWeight_Pounds", 4, DataType.Numeric, 6, false);
            Add("341d", "Length_Inches", 4, DataType.Numeric, 6, false);
            Add("342d", "Length_Feet", 4, DataType.Numeric, 6, false);
            Add("343d", "Length_Yards", 4, DataType.Numeric, 6, false);
            Add("344d", "Width_Inches", 4, DataType.Numeric, 6, false);
            Add("345d", "Width_Feed", 4, DataType.Numeric, 6, false);
            Add("346d", "Width_Yards", 4, DataType.Numeric, 6, false);
            Add("347d", "Heigth_Inches", 4, DataType.Numeric, 6, false);
            Add("348d", "Heigth_Feed", 4, DataType.Numeric, 6, false);
            Add("349d", "Heigth_Yards", 4, DataType.Numeric, 6, false);
            Add("350d", "Surface_SquareInches", 4, DataType.Numeric, 6, false);
            Add("351d", "Surface_SquareFeet", 4, DataType.Numeric, 6, false);
            Add("352d", "Surface_SquareYards", 4, DataType.Numeric, 6, false);
            Add("353d", "Surface_SquareInches", 4, DataType.Numeric, 6, false);
            Add("354d", "Surface_SquareFeed", 4, DataType.Numeric, 6, false);
            Add("355d", "Surface_SquareYards", 4, DataType.Numeric, 6, false);
            Add("356d", "NetWeight_TroyOunces", 4, DataType.Numeric, 6, false);
            Add("357d", "NetVolume_Ounces", 4, DataType.Numeric, 6, false);
            Add("360d", "NetVolume_Quarts", 4, DataType.Numeric, 6, false);
            Add("361d", "NetVolume_Gallonen", 4, DataType.Numeric, 6, false);
            Add("362d", "GrossVolume_Quarts", 4, DataType.Numeric, 6, false);
            Add("363d", "GrossVolume_Gallonen", 4, DataType.Numeric, 6, false);
            Add("364d", "NetVolume_CubicInches", 4, DataType.Numeric, 6, false);
            Add("365d", "NetVolume_CubicFeet", 4, DataType.Numeric, 6, false);
            Add("366d", "NetVolume_CubicYards", 4, DataType.Numeric, 6, false);
            Add("367d", "GrossVolume_CubicInches", 4, DataType.Numeric, 6, false);
            Add("368d", "GrossVolume_CubicFeet", 4, DataType.Numeric, 6, false);
            Add("369d", "GrossVolume_CubicYards", 4, DataType.Numeric, 6, false);
            Add("37", "QuantityInParts", 2, DataType.Numeric, 8, true);
            Add("390d", "AmountDue_DefinedValutaBand", 4, DataType.Numeric, 15, true);
            Add("391d", "AmountDue_WithISOValutaCode", 4, DataType.Numeric, 18, true);
            Add("392d", "BePayingAmount_DefinedValutaBand", 4, DataType.Numeric, 15, true);
            Add("393d", "BePayingAmount_WithISOValutaCode", 4, DataType.Numeric, 18, true);
            Add("400", "JobNumberOfGoodsRecipient", 3, DataType.Alphanumeric, 30, true);
            Add("401", "ShippingNumber", 3, DataType.Alphanumeric, 30, true);
            Add("402", "DeliveryNumber", 3, DataType.Numeric, 17, false);
            Add("403", "RoutingCode", 3, DataType.Alphanumeric, 30, true);
            Add("410", "EAN_UCC_GlobalLocationNumber(GLN)_GoodsRecipient", 3, DataType.Numeric, 13, false);
            Add("411", "EAN_UCC_GlobalLocationNumber(GLN)_InvoiceRecipient", 3, DataType.Numeric, 13, false);
            Add("412", "EAN_UCC_GlobalLocationNumber(GLN)_Distributor", 3, DataType.Numeric, 13, false);
            Add("413", "EAN_UCC_GlobalLocationNumber(GLN)_FinalRecipient", 3, DataType.Numeric, 13, false);
            Add("414", "EAN_UCC_GlobalLocationNumber(GLN)_PhysicalLocation", 3, DataType.Numeric, 13, false);
            Add("415", "EAN_UCC_GlobalLocationNumber(GLN)_ToBilligParticipant", 3, DataType.Numeric, 13, false);
            Add("420", "ZipCodeOfRecipient_withoutCountryCode", 3, DataType.Alphanumeric, 20, true);
            Add("421", "ZipCodeOfRecipient_withCountryCode", 3, DataType.Alphanumeric, 12, true);
            Add("422", "BasisCountryOfTheWares_ISO3166Format", 3, DataType.Numeric, 3, false);
            Add("7001", "Nato Stock Number", 4, DataType.Numeric, 13, false);
            Add("8001", "RolesProducts", 4, DataType.Numeric, 14, false);
            Add("8002", "SerialNumberForMobilePhones", 4, DataType.Alphanumeric, 20, true);
            Add("8003", "GlobalReturnableAssetIdentifier", 4, DataType.Alphanumeric, 34, true);
            Add("8004", "GlobalIndividualAssetIdentifier", 4, DataType.Numeric, 30, true);
            Add("8005", "SalesPricePerUnit", 4, DataType.Numeric, 6, false);
            Add("8006", "IdentifikationOfAProductComponent", 4, DataType.Numeric, 18, false);
            Add("8007", "IBAN", 4, DataType.Alphanumeric, 30, true);
            Add("8008", "DataAndTimeOfManufacturing", 4, DataType.Numeric, 12, true);
            Add("8018", "GlobalServiceRelationNumber", 4, DataType.Numeric, 18, false);
            Add("8020", "NumberBillCoverNumber", 4, DataType.Alphanumeric, 25, false);
            Add("8100", "CouponExtendedCode_NSC_offerCcode", 4, DataType.Numeric, 10, false);
            Add("8101", "CouponExtendedCode_NSC_offerCcode_EndOfOfferCode", 4, DataType.Numeric, 14, false);
            Add("8102", "CouponExtendedCode_NSC", 4, DataType.Numeric, 6, false);
            Add("90", "InformationForBilateralCoordinatedApplications", 2, DataType.Alphanumeric, 30, true);
            Add("91", "Company specific", 2, DataType.Alphanumeric, 30, true);
            Add("92", "Company specific", 2, DataType.Alphanumeric, 30, true);
            Add("93", "Company specific", 2, DataType.Alphanumeric, 30, true);
            Add("94", "Company specific", 2, DataType.Alphanumeric, 30, true);
            Add("95", "Company specific", 2, DataType.Alphanumeric, 30, true);
            Add("96", "Company specific", 2, DataType.Alphanumeric, 30, true);
            Add("97", "Company specific", 2, DataType.Alphanumeric, 30, true);
            Add("98", "Company specific", 2, DataType.Alphanumeric, 30, true);
            Add("99", "Company specific", 2, DataType.Alphanumeric, 30, true);
#if WindowsCE
            int? minMax = null;
            foreach (AII el in aiiDict.Values)
            {
                if (minMax == null || el.LengthOfAI < minMax)
                    minMax = el.LengthOfAI;
            }

            minLengthOfAI = (int)minMax;

            minMax = null;
            foreach (AII el in aiiDict.Values)
            {
                if (minMax == null || el.LengthOfAI > minMax)
                    minMax = el.LengthOfAI;
            }
            maxLengthOfAI = (int)minMax;

            aiis = new string[aiiDict.Keys.Count - 1];

            for (int i = 0; i < aiis.Length; i++)
            {
                aiis[i] = aiiDict.Keys[i];
            }
#else
            minLengthOfAI = aiiDict.Values.Min(el => el.LengthOfAI);
            maxLengthOfAI = aiiDict.Values.Max(el => el.LengthOfAI);
#endif
        }

        /// <summary>
        /// Add an Application Identifier (AI)
        /// </summary>
        /// <param name="AI">Number of the AI</param>
        /// <param name="Description"></param>
        /// <param name="LengthOfAI"></param>
        /// <param name="DataDescription">The type of the content</param>
        /// <param name="LengthOfData">The max lenght of the content</param>
        /// <param name="FNC1">Support a group seperator</param>
        public static void Add(string AI, string Description, int LengthOfAI, DataType DataDescription, int LengthOfData, bool FNC1) => aiiDict[AI] = new AII(AI, Description, LengthOfAI, DataDescription, LengthOfData, FNC1);

        /// <summary>
        /// Parse the ean128 code
        /// </summary>
        /// <param name="data">The raw scanner data</param>
        /// <param name="throwException">If an exception will be thrown if an AI cannot be found</param>
        /// <returns>The different parts of the ean128 code</returns>

#if WindowsCE
        public static Dictionary<AII, string> Parse(string data, bool throwException)

#else

        public static Dictionary<AII, string> Parse(string data, bool throwException = false)
#endif
        {
            var result = new Dictionary<AII, string>();
            if(string.IsNullOrEmpty(data))
            {
                return result;
            }

            data = data.Trim();

            if(data.Length <= 14)
            {
                var aii = new AII("01", "EAN-NumberOfTradingUnit", 2, DataType.Numeric, 14, false);
                //get EAN 13
                if(data.Length == 14)
                {
                    data = data.Substring(1);
                }

                result[aii] = data;
                return result;
            }

            // cut off the EAN128 start code
            if(data.StartsWith(EAN128StartCode))
            {
                data = data.Substring(EAN128StartCode.Length);
            }
            // cut off the check sum
            if(HasCheckSum)
            {
                data = data.Substring(0, data.Length - 2);
            }

            var index = 0;
            // walkk through the EAN128 code
            while(index < data.Length)
            {
                // try to get the AI at the current position
                var ai = GetAI(data, ref index);
                if(ai == null)
                {
                    if(throwException)
                    {
                        throw new InvalidOperationException("AI not found");
                    }

                    return result;
                }
                // get the data to the current AI
                var code = GetCode(data, ai, ref index);
                result[ai] = code;
            }

            return result;
        }

#if WindowsCE
        public static Dictionary<AII, string> Parse(string data)
        {
            return Parse(data, false);
        }
#endif

        /// <summary>
        /// Try to get the AI at the current position
        /// </summary>
        /// <param name="data">The row data from the scanner</param>
        /// <param name="index">The refrence of the current position</param>
        /// <param name="usePlaceHolder">Sets if the last character of the AI should replaced with a placehoder ("d")</param>
        /// <returns>The current AI or null if no match was found</returns>
#if WindowsCE
        private static AII GetAI(string data, ref int index, bool usePlaceHolder)
#else

        private static AII GetAI(string data, ref int index, bool usePlaceHolder = false)
#endif
        {
            AII result = null;
            // Step through the different lenghts of the AIs
            for(var i = minLengthOfAI; i <= maxLengthOfAI; i++)
            {
                if((index + (i - 1)) >= data.Length)
                {
                    continue;
                }
                // get the AI sub string
                var ai = data.Substring(index, i);
                if(usePlaceHolder)
                {
                    ai = ai.Remove(ai.Length - 1, 1) + "d";
                }
                // try to get the ai from the dictionary
                if(aiiDict.TryGetValue(ai, out result))
                {
                    // Shift the index to the next
                    index += i;
                    return result;
                }
                // if no AI found, try it with the next lenght
            }
            // if no AI found here, than try it with placeholders. Assumed that is the first sep where usePlaceHolder is false
            if(!usePlaceHolder)
            {
                result = GetAI(data, ref index, true);
            }

            return result;
        }

#if WindowsCE
        private static AII GetAI(string data, ref int index)
        {
            return GetAI(data, ref index, false);
        }
#endif

        /// <summary>
        /// Get the current code to the AI
        /// </summary>
        /// <param name="data">The row data from the scanner</param>
        /// <param name="ai">The current AI</param>
        /// <param name="index">The refrence of the current position</param>
        /// <returns>the data to the current AI</returns>
        private static string GetCode(string data, AII ai, ref int index)
        {
            // get the max lenght to read.
            var lenghtToRead = Math.Min(ai.LengthOfData, data.Length - index);
            // get the data of the current AI
            var result = data.Substring(index, lenghtToRead);
            // check if the AI support a group seperator
            if(ai.FNC1)
            {
                // try to find the index of the group seperator
                var indexOfGroupTermination = result.IndexOf(GroutSeperator);
                if(indexOfGroupTermination >= 0)
                {
                    lenghtToRead = indexOfGroupTermination + 1;
                }
                // get the data of the current AI till the gorup seperator
                result = data.Substring(index, lenghtToRead);
            }

            // Shift the index to the next
            index += lenghtToRead;
            return result;
        }

        /// <summary>
        /// Return the value for the defined key in KeyConstants
        /// </summary>
        /// <param name="data">EAN 128 data</param>
        /// <param name="keyConstants">KeyConstants defined in KeyConstants</param>
        /// <returns></returns>
        public static string Parse(string data, string keyConstants)
        {
            var aiis = Parse(data);
            foreach(var kv in aiis)
            {
                if(kv.Key.AI == keyConstants)
                {
                    if(keyConstants == KeyConstants.EAN_NumberOfTradingUnit && kv.Value.Length > 13)
                    {
                        return kv.Value.Substring(1);
                    }

                    return kv.Value;
                }
            }

            return data;
        }

        #region get methods

        /// <summary>
        /// Return the EAN 13 code
        /// </summary>
        /// <param name="data">Data to extract code</param>
        /// <returns></returns>
        public static string GetEAN_NumberOfTradingUnit(string data) => Parse(data, KeyConstants.EAN_NumberOfTradingUnit);

        /// <summary>
        /// Return the charge number, also know as batch number
        /// </summary>
        /// <param name="data">Data to extract charge number </param>
        /// <returns></returns>
        public static string GetCharge_Number(string data) => Parse(data, KeyConstants.Charge_Number);

        /// <summary>
        /// Return the box number
        /// </summary>
        /// <param name="data">Data to extract box number </param>
        /// <returns></returns>
        public static string GetBox_Number(string data) => Parse(data, KeyConstants.CompanySpecific92);

        #endregion get methods
    }
}