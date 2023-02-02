using swf.Models;
using swf.Repository;
using System.Globalization;

namespace swf.ViewModels
{
    public class WeeklySchedulesWithCustomersData
    {
        public DateTime WeekDayDate { get; set; }
        public string MorningShiftEngineer { get; set; }
        public string EveningShiftEngineer { get; set; }
        public int WeekNo { get; set; }

        public WeeklySchedulesWithCustomersData(
                                                WeeklyScheduleModel weeklySchedule,
                                                EngineerRepository engineers,
                                                int weekNo,
                                                WeekModel weekData
                                                )
        {
            WeekNo = weekNo;
            WeekDayDate = ISOWeek.ToDateTime(weekData.YearNo,weekData.WeekNo, (DayOfWeek)weeklySchedule.WeekDay);
            MorningShiftEngineer = engineers.GetEngineerById(weeklySchedule.FirstHalfEngineerId).FullName;
            EveningShiftEngineer = engineers.GetEngineerById(weeklySchedule.SecondHalfEngineerId).FullName;
        }
        
    }
}
