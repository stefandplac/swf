using swf.Data;
using swf.Models;
using swf.Models.DBObjects;

namespace swf.Repository
{
    public class WeeklyScheduleRepository
    {
        private readonly ApplicationDbContext _db;
        public WeeklyScheduleRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        private WeeklyScheduleModel MapDBObjectToModel(WeeklySchedule dbobject)
        {
            var model = new WeeklyScheduleModel();
            if (dbobject != null)
            {
                model.IdSchedule = dbobject.IdSchedule;
                model.WeekId = dbobject.WeekId;
                model.WeekDay = dbobject.WeekDay;
                model.FirstHalfEngineerId= dbobject.FirstHalfEngineerId;
                model.SecondHalfEngineerId=dbobject.SecondHalfEngineerId;
            }
            return model;                                                                                                                                                                                                                                                                                              
        }
        private WeeklySchedule MapModelToDBObject(WeeklyScheduleModel model)
        {
            var dbobject = new WeeklySchedule();
            if (model != null)
            {
                dbobject.IdSchedule = model.IdSchedule;
                dbobject.WeekId = model.WeekId;
                dbobject.WeekDay = model.WeekDay;
                dbobject.FirstHalfEngineerId = model.FirstHalfEngineerId;
                dbobject.SecondHalfEngineerId = model.SecondHalfEngineerId;
            }
            return dbobject;
        }
        public List<WeeklyScheduleModel> GetAllWeeksSchedule()
        {
            var weeklySchedule = new List<WeeklyScheduleModel>();
            foreach(var schedule in _db.WeeklySchedules)
            {
                weeklySchedule.Add(MapDBObjectToModel(schedule));
            }
            return weeklySchedule;
        }
        public WeeklyScheduleModel GetScheduleById(Guid id)
        {
            return MapDBObjectToModel(_db.WeeklySchedules.FirstOrDefault(schedule => schedule.IdSchedule == id));
        }
        public List<WeeklyScheduleModel> GetCurrentWeekSchedule(Guid weekNoID)
        {
            var currentWeekScheduleDBObjects = _db.WeeklySchedules.Where(weekDay=> weekDay.WeekId==weekNoID);
            var currentWeekSchedule = new List<WeeklyScheduleModel>();
            foreach(var weekDay in currentWeekScheduleDBObjects)
            {
                currentWeekSchedule.Add(MapDBObjectToModel(weekDay));
            }
            return currentWeekSchedule;
        }
        public List<WeeklyScheduleModel> ReturnCurrentAndNextWeekSchedule(Guid currentWeekId, Guid nextWeekId)
        {
            var scheduleCurrentNextDBObjects = _db.WeeklySchedules.Where(weekDay=> weekDay.WeekId== currentWeekId || weekDay.WeekId== nextWeekId );
            var scheduleCurrentNext = new List<WeeklyScheduleModel>();
            foreach(var weekDay in scheduleCurrentNextDBObjects)
            {
                scheduleCurrentNext.Add(MapDBObjectToModel(weekDay));
            }
            return scheduleCurrentNext;
        }
        public void InsertWeekDaySchedule(WeeklyScheduleModel model)
        {
            model.IdSchedule = Guid.NewGuid();
            _db.WeeklySchedules.Add(MapModelToDBObject(model));
            _db.SaveChanges();
        }
        public void InsertManyWeekDaySchedule(List<WeeklyScheduleModel> weekDays)
        {
            foreach(var weekDay in weekDays)
            {
                _db.WeeklySchedules.Add(MapModelToDBObject(weekDay));
            }
            _db.SaveChanges();
        }
        public void UpdateWeekdaySchedule(WeeklyScheduleModel model)
        {
            var dbobject = _db.WeeklySchedules.FirstOrDefault(schedule => schedule.IdSchedule == model.IdSchedule);
            if (dbobject != null)
            {
                dbobject.WeekId = model.WeekId;
                dbobject.WeekDay = model.WeekDay;
                dbobject.FirstHalfEngineerId = model.FirstHalfEngineerId;
                dbobject.SecondHalfEngineerId = model.SecondHalfEngineerId;
            }
            _db.SaveChanges();
        }
        public void DeleteWeekdaySchedule(Guid id)
        {
            var dbobject = _db.WeeklySchedules.FirstOrDefault(schedule => schedule.IdSchedule == id);
            if (dbobject != null)
            {
                _db.WeeklySchedules.Remove(dbobject);
                _db.SaveChanges();
            }
        }
    }
}
