using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public static class TupleExtensions
{
    #region Private Methods

    private static object ChangeType(object value, Type conversionType)
    {
        if(value == null)
        {
            return default;
        }

        var result = (object)null;

        try
        {
            if(conversionType.IsEnum)
            {
                var i = Convert.ToInt32(value);

                result = (from enun in Enum.GetValues(conversionType).Cast<int>()
                          where enun == i
                          select enun).First();
            }
            else if(conversionType == typeof(TimeSpan) ||
                    conversionType == typeof(Nullable<TimeSpan>))
            {
                var timeDate = new DateTime();

                if(DateTime.TryParse(value.ToString(), out timeDate))
                {
                    result = new TimeSpan(timeDate.Ticks);
                }
            }
            else
            {
                var conv = TypeDescriptor.GetConverter(conversionType);
                if(conv?.CanConvertFrom(value.GetType()) ?? false)
                {
                    try
                    {
                        result = conv.ConvertFrom(value);
                    }
                    catch
                    {
                        //do nothing
                    }
                }
                else
                {
                    result = System.Convert.ChangeType(value, conversionType);
                }
            }
        }
        catch
        {
            var conv = TypeDescriptor.GetConverter(conversionType);
            if(conv?.CanConvertFrom(value.GetType()) ?? false)
            {
                try
                {
                    result = conv.ConvertFrom(value);
                }
                catch
                {
                    //do nothing
                }
            };
        }

        return result;
    }

    private static T PrepareValue<T>(object value)
    {
        if(value is null ||
           value == DBNull.Value)
        {
            return default;
        }

        var t = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        T result;
        try
        {
            if(t.IsEnum)
            {
                result = (T)Enum.Parse(t, value.ToString());
            }
            else
            {
                result = t.FullName.Equals(typeof(string).FullName) ? (T)ChangeType(value.ToString(), t) : (T)ChangeType(value, t);
            }
        }
        catch
        {
            result = default;
        }

        return result;
    }

    #endregion Private Methods

    #region Public Methods

    public static T GetValue<T>(this IEnumerable<(string Name, object Value)> enumerable, string name) => PrepareValue<T>(GetValue(enumerable, name));

    public static object GetValue(this IEnumerable<(string Name, object Value)> enumerable, string name)
    {
        if(enumerable is null)
        {
            return default;
        }

        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        var value = enumerable.FirstOrDefault(w => w.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        return value.Value;
    }

    public static string GetValueAsString(this IEnumerable<(string Name, object Value)> enumerable, string name) => GetValue(enumerable, name)?.ToString() ?? "";

    #endregion Public Methods
}