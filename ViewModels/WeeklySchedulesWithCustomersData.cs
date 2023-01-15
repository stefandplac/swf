using swf.Models;
using swf.Repository;

namespace swf.ViewModels
{
    public class WeeklySchedulesWithCustomersData
    {
        public string FirstHalfEngineerName { get; set; }
        public string SecondHalfEngineerName { get; set; }
        public Guid IdSchedule { get; set; }
        public Guid WeekId { get; set; }
        public short WeekDay { get; set; }
        public Guid FirstHalfEngineerId { get; set; }
        public Guid SecondHalfEngineerId { get; set; }
        public WeeklySchedulesWithCustomersData(
                                                WeeklyScheduleModel weeklySchedule,
                                                EngineerRepository engineers
                                                )
        {
            IdSchedule= weeklySchedule.IdSchedule;
            WeekId = weeklySchedule.WeekId;
            WeekDay = weeklySchedule.WeekDay;
            FirstHalfEngineerId = weeklySchedule.FirstHalfEngineerId;
            SecondHalfEngineerId = weeklySchedule.SecondHalfEngineerId;
            FirstHalfEngineerName = engineers.GetEngineerById(weeklySchedule.FirstHalfEngineerId).FullName;
            SecondHalfEngineerName = engineers.GetEngineerById(weeklySchedule.SecondHalfEngineerId).FullName;
        }
    }
}
