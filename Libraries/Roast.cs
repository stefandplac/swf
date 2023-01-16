using swf.Models;
using swf.Models.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWF
{
    static class Roast
    {
        public static void Return2WeeksSchedule(
                                                   List<EngineerModel> engineers, 
                                                   int startIndex,
                                                   Guid[,] weekDays,
                                                   int[] engineersSelection)
        {
            
            bool eligibleList = false;
            Random rnd = new Random();
            do
            {
                eligibleList = ReturnEligibleList(weekDays, engineersSelection, engineers, startIndex);

                if (!eligibleList)
                {
                    //reset engineersSelection and weekDays arrays in case the list builded is not eligible
                    //is breaking at weekday 8 for example
                    for (int i = startIndex; i < engineers.Count; i++) { engineersSelection[i] = 0; }
                    Guid guidX = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    for (int y = startIndex; y < 10; y++) { weekDays[0, y] = guidX; weekDays[1, y] = guidX; }
                }

            }
            while (!eligibleList);
            
        }
        private static bool ReturnEligibleList(Guid[,] weekDays, 
                                               int[] engineersSelection,
                                               List<EngineerModel> engineers,
                                               int startIndex)
        {
            for (var weekDay = startIndex; weekDay < weekDays.GetLength(1); weekDay++)
            {
                //## 1
                if (weekDay == 8 && engineersSelection.Min() == 0)
                {
                    return false;
                }

                var morningShift = SelectEngineerUntilEligible(engineers.Count, weekDays, weekDay, engineersSelection, engineers);
                weekDays[0, weekDay] = morningShift.Item1;
                engineersSelection[morningShift.Item2] += 1;

                var eveningShift = SelectEngineerUntilEligible(engineers.Count, weekDays, weekDay, engineersSelection, engineers);
                weekDays[1, weekDay] = eveningShift.Item1;
                engineersSelection[eveningShift.Item2] += 1;

                
            }
            return true;
        }
        private static (Guid, int) SelectEngineerUntilEligible(int selectionLength,
                                                   Guid[,] weekDays,
                                                   int weekDay,
                                                   int[] engineersSelection,
                                                   List<EngineerModel> engineers
                                                   )
        {

            Random rnd = new Random();
            int rndSelection = -1;
            bool isEligible = false;
            Guid engineerId;
            do
            {
                rndSelection = rnd.Next(0, selectionLength);
                engineerId = engineers[rndSelection].IdEngineer;
                isEligible = CheckEligibility(engineerId, weekDays, weekDay, engineersSelection,rndSelection);
                
            }
            while (!isEligible);

            return (engineerId,rndSelection);
        }
        private static Boolean CheckEligibility(Guid selectedEngineer, 
                                               Guid[,] weekDays, 
                                               int weekDay,
                                               int[] engineersSelection,
                                               int rndSelection)
        {
            Guid guidX = Guid.Parse("00000000-0000-0000-0000-000000000000");
            //## 2
            if ((weekDay!=0 && weekDay!=5) && (weekDays[0, weekDay - 1] == selectedEngineer || weekDays[1, weekDay - 1] == selectedEngineer))
            {
                return false;
            }
            //## 3
            if (weekDays[0, weekDay] == weekDays[1, weekDay] && weekDays[0,weekDay]!= guidX) return false;

            //## 4
            var minShifts = engineersSelection.Min();
            if (engineersSelection[rndSelection]>=2 && minShifts < engineersSelection[rndSelection])
            {
                return false;
            }

            return true;
        }
    }
}
//## 1
//with random can happen that an engineer to get FIRST TIME a shift day on the 9TH working day of the week
//period ....which makes impossible for the schedule to be created since one of the rules is:
//is not allowed that an engineer to have shifts in any consecutive days
//so in the 9 working day if there is any engineer that has 0 shifts the schedule is compromised
//will cause a continuous loop so we need to break it and start again

//## 2
//check this only if the weekday is not mondayFirstWeek or mondaySecondWeek - 
//the condition will not be checked when the previous days are weekend days
//check the condition of previous day -- no engineer can have shifts on 2 consecutive days

//## 3
//the MS(morningShift) and EV(eveningShift) must have 2 different engineers

//## 4
//if the current random engineer selection has already 2 shifts on 2 weeks basis
//then we will compare it with the minimum of shifts that exists in the array
//and if there is another engineer that will have less shifts than the current selection 
//the current selection is not eligible will be discarded 