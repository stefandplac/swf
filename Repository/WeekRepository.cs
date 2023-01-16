using swf.Data;
using swf.Models;
using swf.Models.DBObjects;

namespace swf.Repository
{
    public class WeekRepository
    {
        private readonly ApplicationDbContext _db;
        public WeekRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        private WeekModel MapDBObjectToModel(Week dbobject)
        {
            var model = new WeekModel();
            if (dbobject != null)
            {
                model.IdWeek = dbobject.IdWeek;
                model.WeekNo = dbobject.WeekNo;
                model.YearNo=dbobject.YearNo;
                model.IsSetted = dbobject.IsSetted;
            }
            return model;
        }
        private Week MapModelToDBObject(WeekModel model)
        {
            var dbobject = new Week();
            if (model != null)
            {
                dbobject.IdWeek = model.IdWeek;
                dbobject.WeekNo = model.WeekNo;
                dbobject.YearNo= model.YearNo;
                dbobject.IsSetted = model.IsSetted;
            }
            return dbobject;
        }
        public List<WeekModel> GetWeeks()
        {
            var weeksList = new List<WeekModel>();
            foreach(var week in _db.Weeks)
            {
                weeksList.Add(MapDBObjectToModel(week));
            }
            return weeksList;
        }
        public WeekModel GetWeekById(Guid id)
        {
            return MapDBObjectToModel(_db.Weeks.FirstOrDefault(week => week.IdWeek == id));
        }
        public WeekModel GetWeekByWeekNO(int weekNo)
        {
            return MapDBObjectToModel(_db.Weeks.FirstOrDefault(week=>week.WeekNo==weekNo));
        }
        public void InsertWeek(WeekModel model)
        {
            //model.IdWeek=Guid.NewGuid();
            _db.Weeks.Add(MapModelToDBObject(model));
            _db.SaveChanges();
        }
        public void UpdateWeek(WeekModel model)
        {
            var dbobject = _db.Weeks.FirstOrDefault(week => week.IdWeek == model.IdWeek);
            if (dbobject != null)
            {
                dbobject.WeekNo = model.WeekNo;
                dbobject.IsSetted= model.IsSetted;
            }
            _db.SaveChanges();
        }
        public void DeleteWeek(Guid id)
        {
            var dbobject = _db.Weeks.FirstOrDefault(week=> week.IdWeek == id);
            if (dbobject != null)
            {
                _db.Weeks.Remove(dbobject);
                _db.SaveChanges();
            }
        }
    }
}
