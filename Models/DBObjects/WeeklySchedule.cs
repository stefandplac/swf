using System;
using System.Collections.Generic;

namespace swf.Models.DBObjects
{
    public partial class WeeklySchedule
    {
        public Guid IdSchedule { get; set; }
        public Guid WeekId { get; set; }
        public short WeekDay { get; set; }
        public Guid FirstHalfEngineerId { get; set; }
        public Guid SecondHalfEngineerId { get; set; }

        public virtual Engineer FirstHalfEngineer { get; set; } = null!;
        public virtual Engineer SecondHalfEngineer { get; set; } = null!;
        public virtual Week Week { get; set; } = null!;
    }
}
