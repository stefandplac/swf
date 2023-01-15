using System;
using System.Collections.Generic;

namespace swf.Models.DBObjects
{
    public partial class Engineer
    {
        public Engineer()
        {
            WeeklyScheduleFirstHalfEngineers = new HashSet<WeeklySchedule>();
            WeeklyScheduleSecondHalfEngineers = new HashSet<WeeklySchedule>();
        }

        public Guid IdEngineer { get; set; }
        public string FullName { get; set; } = null!;

        public virtual ICollection<WeeklySchedule> WeeklyScheduleFirstHalfEngineers { get; set; }
        public virtual ICollection<WeeklySchedule> WeeklyScheduleSecondHalfEngineers { get; set; }
    }
}
