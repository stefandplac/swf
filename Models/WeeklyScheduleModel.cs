namespace swf.Models
{
    public class WeeklyScheduleModel
    {
        public Guid IdSchedule { get; set; }
        public Guid WeekId { get; set; }
        public short WeekDay { get; set; }
        public Guid FirstHalfEngineerId { get; set; }
        public Guid SecondHalfEngineerId { get; set; }
    }
}
