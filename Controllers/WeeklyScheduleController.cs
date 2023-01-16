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
        public ActionResult Return2WeeksSchedule()
        {
            //## 1
            WeeklyScheduleActions.Return2WeeksScheduleActionBuildRandom(engineerRepository, weekRepository, weeklyScheduleRepository);
            
            //## Response
            var currentWeekNo = DateMethods.ReturnWeekNo();
            var currentWeekID = weekRepository.GetWeekByWeekNO(currentWeekNo).IdWeek;
            var currentWeekSchedule = weeklyScheduleRepository.GetCurrentWeekSchedule(currentWeekID).ToList();
            return StatusCode(StatusCodes.Status200OK, ISOWeek.ToDateTime(2023, currentWeekNo, (DayOfWeek)1));

        } 
        
        //// GET api/WeeklySchedule/5
        //[HttpGet("{id}")]
        
    }
}

//## 1
//we will return an ordered list of engineers from the database. ! a set has no order ...and any time can return the result in any order
//the engineers list needs to be ordered cause we will use an array and we will assume that the index of the array
//will correspond with the position of engineers in the list