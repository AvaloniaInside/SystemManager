using System.Globalization;

namespace SystemManager.Helpers;

public static class ByteHelper
{
    private static readonly string[] SizeUnit = { "B", "KB", "MB", "GB", "TB" };
    
    public static ulong Parse(string s) =>
        Parse(s, NumberFormatInfo.CurrentInfo);
    public static ulong Parse(string s, IFormatProvider formatProvider) =>
        Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, formatProvider);
    public static ulong Parse(string str, NumberStyles numberStyles, IFormatProvider formatProvider)
    {
        str = str.Trim();
        
        var numberFormatInfo = NumberFormatInfo.GetInstance(formatProvider);
        var decimalSeparator = Convert.ToChar(numberFormatInfo.NumberDecimalSeparator);
        var groupSeparator = Convert.ToChar(numberFormatInfo.NumberGroupSeparator);

        int index;
        var hasNoneNumberCharacter = false;
        for (index = 0; index < str.Length; index++)
            if (!(char.IsDigit(str[index]) || str[index] == decimalSeparator || str[index] == groupSeparator))
            {
                hasNoneNumberCharacter = true;
                break;
            }

        if (!hasNoneNumberCharacter)
            return ulong.Parse(str, numberStyles, formatProvider);
        
        
        var numberStr = str.Substring(0, index).Trim();
        var sign = str.Substring(index, str.Length - index).Trim();

        if (!double.TryParse(numberStr, numberStyles, formatProvider, out var number))
            throw new FormatException($"Cannot parse string as file size.");

        return sign switch
        {
            "B" => (ulong)number,
            "b" => (ulong)(number / 8),
            _ => sign.ToLower() switch
            {
                "kb" => (ulong)number * 1024,
                "mb" => (ulong)number * 1048576,
                "gb" => (ulong)number * 1073741824,
                "tb" => (ulong)number * 1099511627776,
                "pb" => (ulong)number * 1125899906842624,
                _ => throw new ArgumentOutOfRangeException($"unknown sign found: {sign}")
            }
        };
    }
    
    public static string BytesToString(this ulong len)
    {
        var order = 0;
        while (len >= 1024 && order < SizeUnit.Length - 1) {
            order++;
            len /= 1024;
        }

        return $"{len:0.##} {SizeUnit[order]}";
    }

}