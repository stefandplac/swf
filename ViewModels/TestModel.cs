using swf.Models;
using swf.Repository;

namespace swf.ViewModels
{
    public class TestModel
    {
        public int WeekNo { get; set; }
        public int WeekDay { get; set; }
        public string Morning { get; set; }
        public string Evening { get; set; }
        
        public TestModel(WeeklyScheduleModel model, WeekRepository weekRep, EngineerRepository engineerRep)
        {
            WeekNo = weekRep.GetWeekById(model.WeekId).WeekNo;
            WeekDay=model.WeekDay;
            Morning = engineerRep.GetEngineerById(model.FirstHalfEngineerId).FullName;
            Evening = engineerRep.GetEngineerById(model.SecondHalfEngineerId).FullName;
        }
    }
}
