using swf.Data;
using swf.Models;
using swf.Models.DBObjects;

namespace swf.Repository
{
    public class EngineerRepository
    {
        private readonly ApplicationDbContext _db;
        public EngineerRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        private EngineerModel MapDBObjectToModel(Engineer dbobject)
        {
            var engineerModel = new EngineerModel();
            if (dbobject != null)
            {
                engineerModel.IdEngineer = dbobject.IdEngineer;
                engineerModel.FullName = dbobject.FullName;
            }
            return engineerModel;
        }
        private Engineer MapModelToDBObject(EngineerModel modelObj)
        {
            var dbobject = new Engineer();
            if (modelObj != null)
            {
                dbobject.IdEngineer=modelObj.IdEngineer;
                dbobject.FullName = modelObj.FullName;
            }
            return dbobject;
        }
        public List<EngineerModel> GetAllEngineers()
        {
            var engineersList = new List<EngineerModel>();
            foreach(var engineer in _db.Engineers)
            {
                engineersList.Add(MapDBObjectToModel(engineer));
            }
            return engineersList;
        }
        public EngineerModel GetEngineerById(Guid id)
        {
            return MapDBObjectToModel(_db.Engineers.FirstOrDefault(engineer => engineer.IdEngineer == id));
        }
        public void InsertEngineer(EngineerModel model)
        {
            model.IdEngineer = Guid.NewGuid();
            _db.Engineers.Add(MapModelToDBObject(model));
            _db.SaveChanges();
        }
        public void UpdateEngineer(EngineerModel model)
        {
            var dbobject = _db.Engineers.FirstOrDefault(engineer => engineer.IdEngineer == model.IdEngineer);
            if (dbobject != null)
            {
                dbobject.FullName = model.FullName;
            }
            _db.SaveChanges();
        }
        public void DeleteEngineer(Guid id)
        {
            var dbobject = _db.Engineers.FirstOrDefault(engineer => engineer.IdEngineer == id);
            if (dbobject != null)
            {
                _db.Engineers.Remove(dbobject);
                _db.SaveChanges();
            }
        }
    }
}
