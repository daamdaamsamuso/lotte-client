using System;

namespace LotteCinemaService.Database.Helper
{
    public class DatabaseUtil
    {
        public static object TryConvert<T>(object obj)
        {
            T result = default(T);

            var type = typeof(T);
            
            if (!Convert.IsDBNull(obj))
            {
                try
                {
                    if (type == typeof(int))
                    {
                        return Convert.ToInt32(obj);
                    }
                    else if (type == typeof(double))
                    {
                        return Convert.ToDouble(obj);
                    }
                    else if (type == typeof(long))
                    {
                        return Convert.ToInt64(obj);
                    }
                    else if (type == typeof(char))
                    {
                        return Convert.ToChar(obj);
                    }
                    else if (type == typeof(string))
                    {
                        return Convert.ToString(obj);
                    }
                    else if (type == typeof(DateTime))
                    {
                        return Convert.ToDateTime(obj);
                    }
                }
                catch
                {

                }
            }

            return result;
        }

        public static int TryConvertToInteger(object obj,int result)
        {
            if (Convert.IsDBNull(obj))
            {
                return result;
            }
            else
            {
                return (int)TryConvert<int>(obj);
            }
        }

        public static int TryConvertToInteger(object obj)
        {
            return (int)TryConvert<int>(obj);
        }

        public static double TryConvertToDouble(object obj)
        {
            return (double)TryConvert<double>(obj);
        }

        public static long TryConvertToLong(object obj)
        {
            return (long)TryConvert<long>(obj);
        }

        public static char TryConvertToChar(object obj)
        {
            return (char)TryConvert<char>(obj);
        }

        public static bool TryConvertCharToBool(object obj)
        {
            string str = ((char)TryConvert<char>(obj)).ToString();
            if (str.Equals("1") || str.Equals("Y"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryConvertIsNullToBool(object obj)
        {
            if (!string.IsNullOrEmpty(obj.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static string TryConvertToString(object obj)
        {
            return (string)TryConvert<string>(obj);
        }

        public static string TryConvertToStringWithNull(object obj)
        {
            if(obj is string)
            {
                return (string)TryConvert<string>(obj);
            }
            else
            {
                return "0";
            }
            
        }

        public static DateTime TryConvertToDateTime(object obj)
        {
            return (DateTime)TryConvert<DateTime>(obj);
        }
    }
}