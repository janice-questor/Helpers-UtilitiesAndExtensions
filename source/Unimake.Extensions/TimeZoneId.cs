using System;

namespace SystemUnimake
{
    public sealed class TimeZoneId
    {
        #region Private Constructors

        private TimeZoneId(string id) => TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(id);

        #endregion Private Constructors

        #region Public Fields

        public static TimeZoneId AfghanistanStandardTime = new TimeZoneId("Afghanistan Standard Time");
        public static TimeZoneId AlaskanStandardTime = new TimeZoneId("Alaskan Standard Time");
        public static TimeZoneId AleutianStandardTime = new TimeZoneId("Aleutian Standard Time");
        public static TimeZoneId AltaiStandardTime = new TimeZoneId("Altai Standard Time");
        public static TimeZoneId ArabianStandardTime = new TimeZoneId("Arabian Standard Time");
        public static TimeZoneId ArabicStandardTime = new TimeZoneId("Arabic Standard Time");
        public static TimeZoneId ArabStandardTime = new TimeZoneId("Arab Standard Time");
        public static TimeZoneId ArgentinaStandardTime = new TimeZoneId("Argentina Standard Time");
        public static TimeZoneId AstrakhanStandardTime = new TimeZoneId("Astrakhan Standard Time");
        public static TimeZoneId AtlanticStandardTime = new TimeZoneId("Atlantic Standard Time");
        public static TimeZoneId AUSCentralStandardTime = new TimeZoneId("AUS Central Standard Time");
        public static TimeZoneId AusCentralWStandardTime = new TimeZoneId("Aus Central W. Standard Time");
        public static TimeZoneId AUSEasternStandardTime = new TimeZoneId("AUS Eastern Standard Time");
        public static TimeZoneId AzerbaijanStandardTime = new TimeZoneId("Azerbaijan Standard Time");
        public static TimeZoneId AzoresStandardTime = new TimeZoneId("Azores Standard Time");
        public static TimeZoneId BahiaStandardTime = new TimeZoneId("Bahia Standard Time");
        public static TimeZoneId BangladeshStandardTime = new TimeZoneId("Bangladesh Standard Time");
        public static TimeZoneId BelarusStandardTime = new TimeZoneId("Belarus Standard Time");
        public static TimeZoneId BougainvilleStandardTime = new TimeZoneId("Bougainville Standard Time");
        public static TimeZoneId CanadaCentralStandardTime = new TimeZoneId("Canada Central Standard Time");
        public static TimeZoneId CapeVerdeStandardTime = new TimeZoneId("Cape Verde Standard Time");
        public static TimeZoneId CaucasusStandardTime = new TimeZoneId("Caucasus Standard Time");
        public static TimeZoneId CenAustraliaStandardTime = new TimeZoneId("Cen. Australia Standard Time");
        public static TimeZoneId CentralAmericaStandardTime = new TimeZoneId("Central America Standard Time");
        public static TimeZoneId CentralAsiaStandardTime = new TimeZoneId("Central Asia Standard Time");
        public static TimeZoneId CentralBrazilianStandardTime = new TimeZoneId("Central Brazilian Standard Time");
        public static TimeZoneId CentralEuropeanStandardTime = new TimeZoneId("Central European Standard Time");
        public static TimeZoneId CentralEuropeStandardTime = new TimeZoneId("Central Europe Standard Time");
        public static TimeZoneId CentralPacificStandardTime = new TimeZoneId("Central Pacific Standard Time");
        public static TimeZoneId CentralStandardTime = new TimeZoneId("Central Standard Time");
        public static TimeZoneId CentralStandardTime_Mexico = new TimeZoneId("Central Standard Time (Mexico)");
        public static TimeZoneId ChathamIslandsStandardTime = new TimeZoneId("Chatham Islands Standard Time");
        public static TimeZoneId ChinaStandardTime = new TimeZoneId("China Standard Time");
        public static TimeZoneId CubaStandardTime = new TimeZoneId("Cuba Standard Time");
        public static TimeZoneId DatelineStandardTime = new TimeZoneId("Dateline Standard Time");
        public static TimeZoneId EAfricaStandardTime = new TimeZoneId("E. Africa Standard Time");
        public static TimeZoneId EasterIslandStandardTime = new TimeZoneId("Easter Island Standard Time");
        public static TimeZoneId EasternStandardTime = new TimeZoneId("Eastern Standard Time");
        public static TimeZoneId EasternStandardTime_Mexico = new TimeZoneId("Eastern Standard Time (Mexico)");
        public static TimeZoneId EAustraliaStandardTime = new TimeZoneId("E. Australia Standard Time");
        public static TimeZoneId EEuropeStandardTime = new TimeZoneId("E. Europe Standard Time");
        public static TimeZoneId EgyptStandardTime = new TimeZoneId("Egypt Standard Time");
        public static TimeZoneId EkaterinburgStandardTime = new TimeZoneId("Ekaterinburg Standard Time");
        public static TimeZoneId ESouthAmericaStandardTime = new TimeZoneId("E. South America Standard Time");
        public static TimeZoneId FijiStandardTime = new TimeZoneId("Fiji Standard Time");
        public static TimeZoneId FLEStandardTime = new TimeZoneId("FLE Standard Time");
        public static TimeZoneId GeorgianStandardTime = new TimeZoneId("Georgian Standard Time");
        public static TimeZoneId GMTStandardTime = new TimeZoneId("GMT Standard Time");
        public static TimeZoneId GreenlandStandardTime = new TimeZoneId("Greenland Standard Time");
        public static TimeZoneId GreenwichStandardTime = new TimeZoneId("Greenwich Standard Time");
        public static TimeZoneId GTBStandardTime = new TimeZoneId("GTB Standard Time");
        public static TimeZoneId HaitiStandardTime = new TimeZoneId("Haiti Standard Time");
        public static TimeZoneId HawaiianStandardTime = new TimeZoneId("Hawaiian Standard Time");
        public static TimeZoneId IndiaStandardTime = new TimeZoneId("India Standard Time");
        public static TimeZoneId IranStandardTime = new TimeZoneId("Iran Standard Time");
        public static TimeZoneId IsraelStandardTime = new TimeZoneId("Israel Standard Time");
        public static TimeZoneId JordanStandardTime = new TimeZoneId("Jordan Standard Time");
        public static TimeZoneId KaliningradStandardTime = new TimeZoneId("Kaliningrad Standard Time");
        public static TimeZoneId KamchatkaStandardTime = new TimeZoneId("Kamchatka Standard Time");
        public static TimeZoneId KoreaStandardTime = new TimeZoneId("Korea Standard Time");
        public static TimeZoneId LibyaStandardTime = new TimeZoneId("Libya Standard Time");
        public static TimeZoneId LineIslandsStandardTime = new TimeZoneId("Line Islands Standard Time");
        public static TimeZoneId LordHoweStandardTime = new TimeZoneId("Lord Howe Standard Time");
        public static TimeZoneId MagadanStandardTime = new TimeZoneId("Magadan Standard Time");
        public static TimeZoneId MagallanesStandardTime = new TimeZoneId("Magallanes Standard Time");
        public static TimeZoneId MarquesasStandardTime = new TimeZoneId("Marquesas Standard Time");
        public static TimeZoneId MauritiusStandardTime = new TimeZoneId("Mauritius Standard Time");
        public static TimeZoneId MidAtlanticStandardTime = new TimeZoneId("Mid-Atlantic Standard Time");
        public static TimeZoneId MiddleEastStandardTime = new TimeZoneId("Middle East Standard Time");
        public static TimeZoneId MontevideoStandardTime = new TimeZoneId("Montevideo Standard Time");
        public static TimeZoneId MoroccoStandardTime = new TimeZoneId("Morocco Standard Time");
        public static TimeZoneId MountainStandardTime = new TimeZoneId("Mountain Standard Time");
        public static TimeZoneId MountainStandardTime_Mexico = new TimeZoneId("Mountain Standard Time (Mexico)");
        public static TimeZoneId MyanmarStandardTime = new TimeZoneId("Myanmar Standard Time");
        public static TimeZoneId NamibiaStandardTime = new TimeZoneId("Namibia Standard Time");
        public static TimeZoneId NCentralAsiaStandardTime = new TimeZoneId("N. Central Asia Standard Time");
        public static TimeZoneId NepalStandardTime = new TimeZoneId("Nepal Standard Time");
        public static TimeZoneId NewfoundlandStandardTime = new TimeZoneId("Newfoundland Standard Time");
        public static TimeZoneId NewZealandStandardTime = new TimeZoneId("New Zealand Standard Time");
        public static TimeZoneId NorfolkStandardTime = new TimeZoneId("Norfolk Standard Time");
        public static TimeZoneId NorthAsiaEastStandardTime = new TimeZoneId("North Asia East Standard Time");
        public static TimeZoneId NorthAsiaStandardTime = new TimeZoneId("North Asia Standard Time");
        public static TimeZoneId NorthKoreaStandardTime = new TimeZoneId("North Korea Standard Time");
        public static TimeZoneId OmskStandardTime = new TimeZoneId("Omsk Standard Time");
        public static TimeZoneId PacificSAStandardTime = new TimeZoneId("Pacific SA Standard Time");
        public static TimeZoneId PacificStandardTime = new TimeZoneId("Pacific Standard Time");
        public static TimeZoneId PacificStandardTime_Mexico = new TimeZoneId("Pacific Standard Time (Mexico)");
        public static TimeZoneId PakistanStandardTime = new TimeZoneId("Pakistan Standard Time");
        public static TimeZoneId ParaguayStandardTime = new TimeZoneId("Paraguay Standard Time");
        public static TimeZoneId QyzylordaStandardTime = new TimeZoneId("Qyzylorda Standard Time");
        public static TimeZoneId RomanceStandardTime = new TimeZoneId("Romance Standard Time");
        public static TimeZoneId RussianStandardTime = new TimeZoneId("Russian Standard Time");
        public static TimeZoneId RussiaTimeZone10 = new TimeZoneId("Russia Time Zone 10");
        public static TimeZoneId RussiaTimeZone11 = new TimeZoneId("Russia Time Zone 11");
        public static TimeZoneId RussiaTimeZone3 = new TimeZoneId("Russia Time Zone 3");
        public static TimeZoneId SAEasternStandardTime = new TimeZoneId("SA Eastern Standard Time");
        public static TimeZoneId SaintPierreStandardTime = new TimeZoneId("Saint Pierre Standard Time");
        public static TimeZoneId SakhalinStandardTime = new TimeZoneId("Sakhalin Standard Time");
        public static TimeZoneId SamoaStandardTime = new TimeZoneId("Samoa Standard Time");
        public static TimeZoneId SaoTomeStandardTime = new TimeZoneId("Sao Tome Standard Time");
        public static TimeZoneId SAPacificStandardTime = new TimeZoneId("SA Pacific Standard Time");
        public static TimeZoneId SaratovStandardTime = new TimeZoneId("Saratov Standard Time");
        public static TimeZoneId SAWesternStandardTime = new TimeZoneId("SA Western Standard Time");
        public static TimeZoneId SEAsiaStandardTime = new TimeZoneId("SE Asia Standard Time");
        public static TimeZoneId SingaporeStandardTime = new TimeZoneId("Singapore Standard Time");
        public static TimeZoneId SouthAfricaStandardTime = new TimeZoneId("South Africa Standard Time");
        public static TimeZoneId SriLankaStandardTime = new TimeZoneId("Sri Lanka Standard Time");
        public static TimeZoneId SudanStandardTime = new TimeZoneId("Sudan Standard Time");
        public static TimeZoneId SyriaStandardTime = new TimeZoneId("Syria Standard Time");
        public static TimeZoneId TaipeiStandardTime = new TimeZoneId("Taipei Standard Time");
        public static TimeZoneId TasmaniaStandardTime = new TimeZoneId("Tasmania Standard Time");
        public static TimeZoneId TocantinsStandardTime = new TimeZoneId("Tocantins Standard Time");
        public static TimeZoneId TokyoStandardTime = new TimeZoneId("Tokyo Standard Time");
        public static TimeZoneId TomskStandardTime = new TimeZoneId("Tomsk Standard Time");
        public static TimeZoneId TongaStandardTime = new TimeZoneId("Tonga Standard Time");
        public static TimeZoneId TransbaikalStandardTime = new TimeZoneId("Transbaikal Standard Time");
        public static TimeZoneId TurkeyStandardTime = new TimeZoneId("Turkey Standard Time");
        public static TimeZoneId TurksAndCaicosStandardTime = new TimeZoneId("Turks And Caicos Standard Time");
        public static TimeZoneId UlaanbaatarStandardTime = new TimeZoneId("Ulaanbaatar Standard Time");
        public static TimeZoneId USEasternStandardTime = new TimeZoneId("US Eastern Standard Time");
        public static TimeZoneId USMountainStandardTime = new TimeZoneId("US Mountain Standard Time");
        public static TimeZoneId UTC = new TimeZoneId("UTC");
        public static TimeZoneId UTC02 = new TimeZoneId("UTC-02");
        public static TimeZoneId UTC08 = new TimeZoneId("UTC-08");
        public static TimeZoneId UTC09 = new TimeZoneId("UTC-09");
        public static TimeZoneId UTC11 = new TimeZoneId("UTC-11");
        public static TimeZoneId UTC12 = new TimeZoneId("UTC+12");
        public static TimeZoneId UTC13 = new TimeZoneId("UTC+13");
        public static TimeZoneId VenezuelaStandardTime = new TimeZoneId("Venezuela Standard Time");
        public static TimeZoneId VladivostokStandardTime = new TimeZoneId("Vladivostok Standard Time");
        public static TimeZoneId VolgogradStandardTime = new TimeZoneId("Volgograd Standard Time");
        public static TimeZoneId WAustraliaStandardTime = new TimeZoneId("W. Australia Standard Time");
        public static TimeZoneId WCentralAfricaStandardTime = new TimeZoneId("W. Central Africa Standard Time");
        public static TimeZoneId WestAsiaStandardTime = new TimeZoneId("West Asia Standard Time");
        public static TimeZoneId WestBankStandardTime = new TimeZoneId("West Bank Standard Time");
        public static TimeZoneId WestPacificStandardTime = new TimeZoneId("West Pacific Standard Time");
        public static TimeZoneId WEuropeStandardTime = new TimeZoneId("W. Europe Standard Time");
        public static TimeZoneId WMongoliaStandardTime = new TimeZoneId("W. Mongolia Standard Time");
        public static TimeZoneId YakutskStandardTime = new TimeZoneId("Yakutsk Standard Time");

        #endregion Public Fields

        #region Public Properties

        public string DisplayName => TimeZoneInfo.DisplayName;
        public string Id => TimeZoneInfo.Id;
        public TimeZoneInfo TimeZoneInfo { get; }

        #endregion Public Properties

        #region Public Methods

        public static implicit operator string(TimeZoneId id) => id?.Id;

        public static bool operator !=(TimeZoneId lhs, TimeZoneId rhs) => !(lhs?.Equals((object)rhs) ?? false);

        public static bool operator ==(TimeZoneId lhs, TimeZoneId rhs) => lhs?.Equals((object)rhs) ?? false;

        public bool Equals(TimeZoneId obj) => obj?.Id == Id;

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }

            if(ReferenceEquals(this, obj))
            {
                return true;
            }

            if(obj is TimeZoneId id)
            {
                return Equals(id);
            }

            return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        #endregion Public Methods
    }
}