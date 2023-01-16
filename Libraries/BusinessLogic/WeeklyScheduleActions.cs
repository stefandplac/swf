using swf.Models;
using swf.Repository;
using SWF;
using System.Linq;

namespace swf.Libraries.BusinessLogic.WeeklySchedule
{
    public static class WeeklyScheduleActions
    {
        public static List<WeeklyScheduleModel>  Return2WeeksScheduleActionBuildRandom(
                                                      EngineerRepository engineerRepository,
                                                      WeekRepository weekRepository,
                                                      WeeklyScheduleRepository weeklyScheduleRepository
                                                      )
        {
            List<EngineerModel> engineers = engineerRepository.GetAllEngineers().OrderBy(engineer => engineer.FullName).ToList();
            var currentWeekNo = DateMethods.ReturnWeekNo();
            var currentYear = DateMethods.ReturnYear();

            //check if the currentWeek and previousWeek were setted
            var previousWeek = weekRepository.GetWeekByWeekNO(currentWeekNo - 1);
            var currentWeek = weekRepository.GetWeekByWeekNO(currentWeekNo);
            var nextWeek = weekRepository.GetWeekByWeekNO(currentWeekNo+ 1);

            var startIndex = 0;
            Guid[,] weekDays = new Guid[2, 10];
            int[] engineersSelection = new int[engineers.Count];
            List<WeeklyScheduleModel> weekScheduleToBeAdded = new List<WeeklyScheduleModel>();


            if (!currentWeek.IsSetted)
            {
                //the currentWeekSchedule is not setted
                if (previousWeek.IsSetted)
                {
                    //### 1
                    //## GENERATE THE CURRENT WEEK SCHEDULE
                    startIndex = 5;
                    PopulateFromDatabase(weekDays, engineersSelection, engineers, weeklyScheduleRepository, previousWeek);
                    Roast.Return2WeeksSchedule(engineers, startIndex, weekDays, engineersSelection);
                    WeekModel currentWeekModel = new WeekModel(Guid.NewGuid(), (short)(currentWeekNo), currentYear, true);

                    CreateWeekScheduleToBeAdded(weekScheduleToBeAdded, weekDays, null, currentWeekModel, startIndex);

                    //    adding currentWeekGeneratedSchedule data to database
                    weekRepository.InsertWeek(currentWeekModel);
                    weeklyScheduleRepository.InsertManyWeekDaySchedule(weekScheduleToBeAdded);

                    //### GENERATE THE NEXT WEEK SCHEDULE
                    weekScheduleToBeAdded = new List<WeeklyScheduleModel>();
                    PopulateFromDatabase(weekDays, engineersSelection, engineers, weeklyScheduleRepository, currentWeek);
                    Roast.Return2WeeksSchedule(engineers, startIndex, weekDays, engineersSelection);
                    WeekModel nextWeekModel = new WeekModel(Guid.NewGuid(), (short)(currentWeekNo + 1), currentYear, true);

                    CreateWeekScheduleToBeAdded(weekScheduleToBeAdded, weekDays, null, nextWeekModel, startIndex);

                    //   adding nextWeekGeneratedSchedule data to database
                    weekRepository.InsertWeek(nextWeekModel);
                    weeklyScheduleRepository.InsertManyWeekDaySchedule(weekScheduleToBeAdded);
                }
                else
                {
                    //### 2
                    Roast.Return2WeeksSchedule(engineers,startIndex, weekDays, engineersSelection);

                    WeekModel currentWeekModel = new WeekModel(Guid.NewGuid(), (short)currentWeekNo, currentYear, true);
                    WeekModel nextWeekModel = new WeekModel(Guid.NewGuid(), (short)(currentWeekNo+1), currentYear, true);
                    
                    CreateWeekScheduleToBeAdded(weekScheduleToBeAdded, weekDays, currentWeekModel, nextWeekModel, startIndex);

                    //ADDING DATA TO DATABASE
                    weekRepository.InsertWeek(currentWeekModel);
                    weekRepository.InsertWeek(nextWeekModel);
                    weeklyScheduleRepository.InsertManyWeekDaySchedule(weekScheduleToBeAdded);


                }
            }
            else
            {
                //the currentWeekSchedule already exists
                if (!nextWeek.IsSetted)
                {
                    //### 3

                    startIndex = 5;
                    PopulateFromDatabase(weekDays, engineersSelection, engineers, weeklyScheduleRepository, currentWeek);
                    Roast.Return2WeeksSchedule(engineers, startIndex, weekDays, engineersSelection);
                    WeekModel nextWeekModel = new WeekModel(Guid.NewGuid(), (short)(currentWeekNo + 1), currentYear, true);

                    CreateWeekScheduleToBeAdded(weekScheduleToBeAdded, weekDays, null, nextWeekModel, startIndex);

                    //ADDING DATA TO DATABASE
                    weekRepository.InsertWeek(nextWeekModel);
                    weeklyScheduleRepository.InsertManyWeekDaySchedule(weekScheduleToBeAdded);
                }
            }
            return weekScheduleToBeAdded;
            
        }

        private static void PopulateFromDatabase(
                                                Guid[,] weekDays,
                                                int[] engineersSelection,
                                                List<EngineerModel> engineers,
                                                WeeklyScheduleRepository weeklyScheduleRepository,
                                                WeekModel firstWeek
                                                )
        {
            var firstWeekDaysPopulated = weeklyScheduleRepository.GetCurrentWeekSchedule(firstWeek.IdWeek).OrderBy(weekDay=>weekDay.WeekDay);
            foreach(var weekDay in firstWeekDaysPopulated)
            {
                weekDays[0, weekDay.WeekDay] = weekDay.FirstHalfEngineerId;
                weekDays[1,weekDay.WeekDay] = weekDay.SecondHalfEngineerId;
                var indexOfFirstEngineer = engineers.FindIndex(engineer => engineer.IdEngineer == weekDay.FirstHalfEngineerId);
                var indexOfSecondEngineer= engineers.FindIndex(engineer => engineer.IdEngineer == weekDay.SecondHalfEngineerId);
                
                engineersSelection[indexOfFirstEngineer] +=1;
                engineersSelection[indexOfSecondEngineer] +=1;
            }
        }
        private static void CreateWeekScheduleToBeAdded(List<WeeklyScheduleModel> weekScheduleToBeAdded,
                                                        Guid[,] weekDays,
                                                        WeekModel week1,
                                                        WeekModel week2,
                                                        int startIndex)
        {
            for (int t = startIndex; t < weekDays.GetLength(1); t++)
            {
                var weekDayToBeAdded = new WeeklyScheduleModel();
                weekDayToBeAdded.IdSchedule = Guid.NewGuid();

                if (t < 5)
                {
                    weekDayToBeAdded.WeekId = week1.IdWeek;
                    weekDayToBeAdded.WeekDay = (short)(t + 1);
                }
                else
                {
                    weekDayToBeAdded.WeekId = week2.IdWeek;
                    weekDayToBeAdded.WeekDay = (short)(t - 4); //for the second week add again from day 1 to 5
                }


                weekDayToBeAdded.FirstHalfEngineerId = weekDays[0, t];
                weekDayToBeAdded.SecondHalfEngineerId = weekDays[1, t];

                weekScheduleToBeAdded.Add(weekDayToBeAdded);
            }
        }
    }
        
}

//### 1
//all the previousWeek data will be used to generate the current week schedule
//generate first the currentWeek and after the nextWeek
//weekDays and engineersSelection arrays needs to be populated with currentWeek data(database)
//startIndex=5 cause only second week of weekDays array will be generated

//### 2
//generate both the current and next week
//startIndex = 0 will be used for generating 10 days schedule
// only when 1 week schedule will need to be generated, startIndex = 5 (5 = MOnday second week)

//### 3
//weekDays and engineersSelection arrays needs to be populated with currentWeek data(database)
//startIndex=5 cause only second week of weekDays array will be generated
//after populating weekDays and engineersSelection with currentWeek Data
//start generating the nextWeek schedule