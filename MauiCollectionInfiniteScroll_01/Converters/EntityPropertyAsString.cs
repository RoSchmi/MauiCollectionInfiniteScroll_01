using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static Common.Models.EdmTypes.EdmTypes;

namespace RoSchmi.Azure.Converters
{
    public static class EntityPropertyAsString
    {
        // public static string Convert(KeyValuePair<string,object> pEntityProperty)
        public static string ConvertProperty(object pEntityProperty)
        {
            switch (pEntityProperty)
            {
                case null:
                    return string.Empty;
                case string StringValue:
                    {
                        return StringValue;
                    }

                case byte[] ByteArrayValue:
                    {
                        string retString = Convert.ToBase64String(ByteArrayValue);
                        if (string.IsNullOrEmpty(retString))
                            return string.Empty;

                        return retString.Length <= 15 ? retString : retString.Substring(0, 15) + "...";
                    }
                case bool BooleanValue:
                    {
                        return BooleanValue == new bool?(true) ? "true" : BooleanValue == new bool?(false) ? "false" : "null";
                    }
                case DateTimeOffset DateTimeOffsetValue:
                    {
                        return DateTimeOffsetValue.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.CurrentCulture);
                    }
                case Double DoubleValue:
                    {
                        return DoubleValue.ToString("G", CultureInfo.CurrentCulture);
                    }
                case Guid GuidValue:
                    {
                        return GuidValue.ToString();
                    }
                case Int32 Int32Value:
                    {
                        return Int32Value.ToString(CultureInfo.CurrentCulture);
                    }
                case Int64 Int64Value:
                    {
                        return Int64Value.ToString(CultureInfo.CurrentCulture);
                    }

                default:
                    {
                        return string.Empty;
                    }
            }
        }
    }
}
