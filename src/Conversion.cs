/********************************************************************************
Copyright (C) Binod Nepal, Mix Open Foundation (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/

using System;
using System.Globalization;

namespace MixERP.Net.Utilities.PgDoc
{
    public static class Conversion
    {
        public static DateTime GetLocalDateTime(string timeZone, DateTime utc)
        {
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            return TimeZoneInfo.ConvertTimeFromUtc(utc, zone);
        }

        public static string GetLocalDateTimeString(string timeZone, DateTime utc)
        {
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            DateTime time = TimeZoneInfo.ConvertTimeFromUtc(utc, zone);
            return String.Format(CultureInfo.CurrentCulture, "{0} {1} {2}", time.ToLongDateString(), time.ToLongTimeString(), zone.DisplayName);
        }

        public static bool IsEmptyDate(DateTime date)
        {
			return (date.Equals(DateTime.MinValue));
        }

        public static bool IsNumeric(string value)
        {
            double number;
            return double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out number);
        }

        public static bool TryCastBoolean(object value)
        {
            bool retVal = false;

            if (value != null)
            {
                if (value is string)
                {
                    if (value.ToString().ToUpperInvariant().Equals("YES") || value.ToString().ToUpperInvariant().Equals("TRUE"))
                    {
                        return true;
                    }
                }

				bool.TryParse(value.ToString(), out retVal);
            }

            return retVal;
        }

        public static DateTime TryCastDate(object value)
        {
            try
            {
                if (value == DBNull.Value)
                {
                    return DateTime.MinValue;
                }

                if (value is DateTime)
                {
                    return (DateTime) value;
                }

                return Convert.ToDateTime(value, CultureInfo.CurrentCulture);
            }
            catch
            {
                //Swallow
            }

            return DateTime.MinValue;
        }

        public static decimal TryCastDecimal(object value)
        {
            decimal retVal = 0;

            if (value != null)
            {
                if (value is decimal)
                {
                    return (decimal) value;
                }

				decimal.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out retVal);
            }

            return retVal;
        }

        public static double TryCastDouble(object value)
        {
            double retVal = 0;

            if (value != null)
            {
                if (value is double)
                {
                    return (double) value;
                }

				double.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out retVal);
            }

            return retVal;
        }

        public static int TryCastInteger(object value)
        {
            int retVal = 0;

            if (value != null)
            {
                if (value is bool)
                {
                    if (Convert.ToBoolean(value, CultureInfo.InvariantCulture))
                    {
                        return 1;
                    }
                }

                if (value is int)
                {
                    return (int) value;
                }

				int.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out retVal);
            }

            return retVal;
        }

        public static long TryCastLong(object value)
        {
            long retVal = 0;

            if (value != null)
            {
                if (value is long)
                {
                    return (long) value;
                }

				long.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out retVal);
            }

            return retVal;
        }

        public static short TryCastShort(object value)
        {
            short retVal = 0;

            if (value != null)
            {
                if (value is short)
                {
                    return (short) value;
                }

				short.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out retVal);
            }

            return retVal;
        }

        public static float TryCastSingle(object value)
        {
            float retVal = 0;

            if (value != null)
            {
                if (value is float)
                {
                    return (float) value;
                }

				float.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out retVal);
            }
            return retVal;
        }

        public static string TryCastString(object value)
        {
			if (value == null)
				return string.Empty;

            if (value is bool)
            {
				return (Convert.ToBoolean(value, CultureInfo.InvariantCulture)) ? "true" : "false";
            }

            return (value == DBNull.Value) ? string.Empty: value.ToString();
    }

        public static DateTime? TryCastNullableDate(object value)
        {
            return (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString())) ? (DateTime?) null : Convert.ToDateTime(value, CultureInfo.CurrentCulture);
        }
    }
}