using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsOrm3
{
    internal static class AsContextExtension
    {
        internal static string FormatSqlValue(this string value)
        {
            return AsContext.FormatSqlValue(value);
        }

        /// <summary>
        /// Converte una data dal formato DateTime nell'intero con formato YYYYMMDD
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int ToIntDate(this DateTime date)
        {
            string year = "";
            if (date.Year.ToString().Length != 4)
            {
                year = ("20" + date.Year.ToString());
                year = year.Substring(year.Length - 4);
            }
            else
                year = date.Year.ToString();

            string month = "00" + date.Month.ToString();
            month = month.Substring(month.Length - 2);

            string day = "00" + date.Day.ToString();
            day = day.Substring(day.Length - 2);
            var dateAs = int.Parse(year + month + day);

            return dateAs;
        }

        /// <summary>
        /// Converte una data dal formato DateTime nell'intero con formato HHMMSS
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int ToIntTime(this DateTime date)
        {
            string hh = "00" + date.Hour.ToString();
            hh = hh.Substring(hh.Length - 2);

            string mm = "00" + date.Minute.ToString();
            mm = mm.Substring(mm.Length - 2);

            string ss = "00" + date.Second.ToString();
            ss = ss.Substring(ss.Length - 2);
            var timeAs = int.Parse(hh + mm + ss);

            return timeAs;
        }
    }
}
