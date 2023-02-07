using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using swf.Data;
using swf.Libraries;
using swf.Libraries.BusinessLogic.WeeklySchedule;
using swf.Models;
using swf.Repository;
using swf.ViewModels;
using SWF;
using System.Globalization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace swf.Controllers
{
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyScheduleController : ControllerBase
    {
        private readonly WeeklyScheduleRepository weeklyScheduleRepository;
        private readonly EngineerRepository engineerRepository;
        private readonly WeekRepository weekRepository;

        public WeeklyScheduleController(ApplicationDbContext db)
        {
            weeklyScheduleRepository = new WeeklyScheduleRepository(db);
            engineerRepository = new EngineerRepository(db);
            weekRepository = new WeekRepository(db);
        }
        // GET: api/WeeklySchedule
        [HttpGet]
        [Route("Get2WeeksSchedule")]
        public Task<IActionResult> Return2WeeksSchedule()
        {
            //## 1
            var weekDays=WeeklyScheduleActions.Return2WeeksScheduleActionBuildRandom(engineerRepository, weekRepository, weeklyScheduleRepository);

            //## Response
            var x = 0;
            var currentWeekNo = DateMethods.ReturnWeekNo();
            var currentWeekID = weekRepository.GetWeekByWeekNO(currentWeekNo).IdWeek;
            var nextWeekID = weekRepository.GetWeekByWeekNO(currentWeekNo + 1).IdWeek;
            var scheduleCurrentNextWeeks = weeklyScheduleRepository.ReturnCurrentAndNextWeekSchedule(currentWeekID, nextWeekID).ToList();
            List<WeeklySchedulesWithCustomersData> responseData = new List<WeeklySchedulesWithCustomersData>();
            var firstWeekId = scheduleCurrentNextWeeks[0].WeekId;
            foreach (var weekDay in scheduleCurrentNextWeeks)
            {
                var weekData = weekRepository.GetWeekById(weekDay.WeekId);
                var weekNo = weekData.IdWeek == firstWeekId ? 1 : 2;
                var weekDayToDisplay = new WeeklySchedulesWithCustomersData(weekDay, engineerRepository,  weekNo, weekData);
                responseData.Add(weekDayToDisplay);
            }
            responseData = responseData.OrderBy(weekDay => weekDay.WeekDayDate).ToList();
            return StatusCode(StatusCodes.Status200OK, responseData);

            //FOR TEST PURPOSES ##########
            //List<TestModel> testModels = new List<TestModel>();
            //foreach (var weekDay in weekDays)
            //{
            //    var testModel = new TestModel(weekDay, weekRepository, engineerRepository);
            //    testModels.Add(testModel);
            //}
            //return StatusCode(StatusCodes.Status200OK, testModels);


        } 
        
        //// GET api/WeeklySchedule/5
        //[HttpGet("{id}")]
        
    }
}

//## 1
//we will return an ordered list of engineers from the database. ! a set has no order ...and any time can return the result in any order
//the engineers list needs to be ordered cause we will use an array and we will assume that the index of the array
//will correspond with the position of engineers in the list