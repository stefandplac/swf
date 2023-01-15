using Microsoft.AspNetCore.Mvc;
using swf.Data;
using swf.Libraries;
using swf.Models;
using swf.Repository;
using swf.ViewModels;
using SWF;

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
            List<EngineerModel> engineers = engineerRepository.GetAllEngineers().OrderBy(engineer=>engineer.FullName).ToList();
            var currentWeekNo = DateMethods.ReturnWeekNo();

            //check if the currentWeek and previousWeek were setted
            var previousWeek = weekRepository.GetWeekByWeekNO(currentWeekNo-1);
            var currentWeek = weekRepository.GetWeekByWeekNO(currentWeekNo);
            Guid[,] weekSchedule;
            if (currentWeek.IsSetted)
            {
                //return roast for the current week that already exist in the database
                var currentWeekID = weekRepository.GetWeekByWeekNO(currentWeekNo).IdWeek;
                var currentWeekSchedule = weeklyScheduleRepository.GetCurrentWeekSchedule(currentWeekID).ToList();
            }
            else
            {
                //the current week is not setted yet
                //check first to see if the previous week is setted
                if (previousWeek.IsSetted)
                {
                    //we take all the previous week data and we will use that data to generate the current week schedule

                }
                else
                {
                    //we will generate both the current and next week
                    weekSchedule = Roast.Return2WeeksSchedule(engineers);
                }
            }


            return StatusCode(StatusCodes.Status200OK,weekSchedule);
        } 
        
        //// GET api/WeeklySchedule/5
        //[HttpGet("{id}")]
        

        
    }
}

//## 1
//we will return an ordered list of engineers from the database. ! a set has no order ...and any time can return the result in any order
//the engineers list needs to be ordered cause we will use an array and we will assume that the index of the array
//will correspond with the position of engineers in the list