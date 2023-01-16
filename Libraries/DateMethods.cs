using System.Globalization;
using static System.Net.WebRequestMethods;

namespace swf.Libraries
{
    public static class DateMethods
    {
       
        public static int ReturnWeekNo()
        {
            //## 1
            return ISOWeek.GetWeekOfYear(DateTime.Now);
        }
        public static int ReturnYear()
        {
            
            return ISOWeek.GetYear(DateTime.Now);
        }
    }
}
//## 1
//returning week numnber according with ISO 8601 weekNO
//Date	        Day	    GetWeekOfYear	ISO 8601 Week
//12/31/2007	Monday	53 of 2007	    1 of 2008
//ISO 8601 always has 7 day weeks. if the last week of the previous year doesn't contain Thursday
//then its treated like the first week of the next year
// https://learn.microsoft.com/en-gb/archive/blogs/shawnste/iso-8601-week-of-year-format-in-microsoft-net