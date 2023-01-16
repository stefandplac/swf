using System;
using System.Collections.Generic;

namespace swf.Models.DBObjects
{
    public partial class Week
    {
        public Week()
        {
            WeeklySchedules = new HashSet<WeeklySchedule>();
        }

        public Guid IdWeek { get; set; }
        public short WeekNo { get; set; }
        public int YearNo { get; set; }
        public bool IsSetted { get; set; }

        public virtual ICollection<WeeklySchedule> WeeklySchedules { get; set; }
    }
}
