using System;

namespace EOffice.WebAPI.Params
{
    public class LichCongTacParam : PagingParam
    {
        
    }
    
    public class PagingParamDate
    {
        public DateRange DateRange { get; set; }
        public string selectedHour { get; set; }
        public string selectedMinute { get; set; }
    }

    public class DateRange
    {
        public string end { get; set; }
        public string start { get; set; }
    }

}