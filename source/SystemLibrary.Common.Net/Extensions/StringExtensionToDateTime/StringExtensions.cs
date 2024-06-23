using System;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;

partial class StringExtensions
{
    /// <summary>
    /// Returns a MinValue if input is null or blank
    /// 
    /// Returns a DateTime if successful conversion
    /// 
    /// Throws exception if input is in an unknown format and could therefore not be converted
    /// </summary>
    public static DateTime ToDateTime(this string date, string format = null)
    {
        if (date == null)
            return DateTime.MinValue;

        var l = date.Length;

        if (l < 4)
            return DateTime.MinValue;

        if(l == 4)
        {
            return new DateTime(Convert.ToInt32(date), 1, 1);
        }

        if (DateTime.TryParse(date, out DateTime res))
            return res;

        if (format.Is())
        {
            if (DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.None, out DateTime res2))
                return res2;

            if (DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.AssumeUniversal, out res2))
                return res2;
        }


        var monthName = char.IsAsciiLetter(date[4]) || char.IsAsciiLetter(date[0]);

        if (monthName)
        {
            foreach (var f in MonthlyNameDateTimeFormats)
            {
                if (DateTime.TryParseExact(date, f, null, System.Globalization.DateTimeStyles.None, out res))
                    return res;
            }
        }

        var z = date[l - 1] == 'Z' || date[l - 1] == 'z';

        if (l <= 12)
        {
            if (z)
            {
                foreach (var f in ShortDateTimeFormatsEndsInZ)
                {
                    if (DateTime.TryParseExact(date, f, null, System.Globalization.DateTimeStyles.None, out res))
                        return res;
                }
            }
            else
            {
                foreach (var f in ShortDateTimeFormats)
                {
                    if (DateTime.TryParseExact(date, f, null, System.Globalization.DateTimeStyles.None, out res))
                        return res;
                }
            }
        }
        else
        {
            if (z)
            {
                foreach (var f in LongDateTimeFormatsEndsInZ)
                {
                    if (DateTime.TryParseExact(date, f, null, System.Globalization.DateTimeStyles.None, out res))
                        return res;
                }
            }
            else
            {
                var plus = date[l - 6] == '+';

                if (plus)
                {
                    foreach (var f in LongDateTimeFormatsWithPlus)
                    {
                        if (DateTime.TryParseExact(date, f, null, System.Globalization.DateTimeStyles.None, out res))
                            return res;
                    }
                }
                else
                {
                    foreach (var f in LongDateTimeFormats)
                    {
                        if (DateTime.TryParseExact(date, f, null, System.Globalization.DateTimeStyles.None, out res))
                            return res;
                    }
                }
            }
        }

        if (long.TryParse(date, out long unixTimestamp))
        {
            if(unixTimestamp > 99999999999)
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(unixTimestamp);
            else
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimestamp);
        }

        foreach(var culture in Cultures)
        {
            foreach(var cultureFormat in DateTimeFormatsCulture)
            {
                if (DateTime.TryParseExact(date, cultureFormat, culture, DateTimeStyles.RoundtripKind, out res))
                    return res;
            }
        }

        foreach(var culture in Cultures)
        {
            if (DateTime.TryParseExact(date, culture.DateTimeFormat.GetAllDateTimePatterns(), culture, DateTimeStyles.None, out res))
                return res;
        }

        throw new Exception("Input was not recognized as a valid DateTime. No matching format provided for: " + date + ". You sent in: " + format);
    }

    static CultureInfo[] Cultures = new[]
    {
        new CultureInfo("no-NO"),
        new CultureInfo("es-ES"),
        new CultureInfo("en-US"),
        new CultureInfo("en-GB"),
        new CultureInfo("en-CA"),
        new CultureInfo("ru-RU"),
        new CultureInfo("fr-FR"),
        new CultureInfo("sv-SE"),
        new CultureInfo("da-DK"),
        new CultureInfo("de-DE"),
        new CultureInfo("pl-PL")
    };

    static string[] DateTimeFormatsCulture = new[]
    {
        "MMMM dd, yyyy",
        "dddd, dd MMMM yyyy HH:mm:ss",
        "dd. MMMM yyyy",
        "dd. MMM yyyy - HH:mm",
        "dd MMM yyyy",
        //"dd. MMM",
        //"dd MMM",
        //"dd. MMMM",
        //"dd MMMM",
    };

    static string[] AllDateTimeFormats = new[] {
        "dd-MM-yyyy",                           // Norwegian datetime formats
        "dd-MM-yyyy HH:mm",
        "dd-MM-yyyy HH:mm:ss",
        "dd-MM-yyyy HH:mm:ss.fff",
        "dd-MM-yyyy HH:mm:ss.fffffff",

        "dd.MM.yyyy",                           // Norwegian datetime formats
        "dd.MM.yyyy HH:mm",
        "dd.MM.yyyy HH:mm:ss",
        "dd.MM.yyyy HH:mm:ss.fff",
        "dd.MM.yyyy HH:mm:ss.fffffff",

        "MM/dd/yyyy HH:mm:ss.fffffff",          // English datetime formats
        "MM/dd/yyyy HH:mm:ss.fff",

        "ddd, dd MMM yyyy HH:mm:ss CET",        // RFC 1123

        "yyyyMMddTHHmmssK",                     // Basic format without separators

        "yyyyMMddTHHmmss.fff",                  // ISO 8601 Basic without dashes or colons
        "yyyyMMddTHHmmss.fffffff",              // ISO 8601 Basic without dashes or colons

        "yyyyMMdd HHmmss",                      // Compact format with space separator
        "yyyyMMdd HHmmss.fff",                  // Compact format with space separator
        "yyyyMMdd HHmmss.fffffff",              // Compact format with space separator

    };

    static string[] ShortDateTimeFormats = AllDateTimeFormats.Where(x => x.Length <= 12).ToArray();
    static string[] ShortDateTimeFormatsEndsInZ = ShortDateTimeFormats.Where(x => x[x.Length - 1] == 'Z' || x[x.Length - 1] == 'z').ToArray();

    static string[] LongDateTimeFormats = AllDateTimeFormats.Where(x => x.Length > 12).ToArray();
    static string[] LongDateTimeFormatsWithPlus = LongDateTimeFormats.Where(x => x.EndsWith("K") || x.EndsWith("Z") || x.EndsWith("Z")).ToArray();
    static string[] LongDateTimeFormatsEndsInZ = LongDateTimeFormatsWithPlus.Concat(LongDateTimeFormats.Where(x => x[x.Length - 1] == 'Z' || x[x.Length - 1] == 'z')).ToArray();

    static string[] MonthlyNameDateTimeFormats = AllDateTimeFormats.Where(x => x.Contains("MMMM ")).ToArray();
}
