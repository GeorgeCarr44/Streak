using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Streak.Models
{
    [Table("Goals")]
    public class Goal
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string? Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Description { get; set; }

        // has a check with this goal ID been created today?
        // 
        // There are a couple ways that this can be done,
        //
        // I could create a list of all the collections that link to this goal
        // and look up how to set up database relationships
        // I would worry that this would become too much data over time
        //
        // I could create a boolean sql method in the Database class which
        // gets the top one of the completions where the goal id matched ordered by creation date
        // This would be faster than loading every completetion
        // I would worry that this has to run on every goal in the list
        // meaning running multiple queries this could cause the data to
        // load slowly when theres a lot of goals
        //
        // I could keep this simple checked boolean in the goals table and update it
        // when its i create a new completion and when a new day has started
        // the concern about this is the data becoming seperated and no longer aligning
        // whats that point in having the completion table if im not going to use it
        //
        // Decision
        // Do a boolean sql method but set this somewhere else dont do sql in getter.

        // Currently looking into making a mapped relationship in sqlite
        public bool Checked { get; set; }
        
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }

        public int OrderID { get; set; }

        public int SelectedFrequencyID { get; set; }

        public List<string> FrequencyNames
        {
            get
            {
                return Enum.GetNames(typeof(GoalFrequency)).Select(b => Regex.Replace(b, "(\\B[A-Z])", " $1")).ToList();
            }
        }

        // Selecting indiviual days
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public Goal()
        {
            this.CreationDate = DateTime.Now;
        }

        public bool DisplaysOnDay(DateTime queryDate)
        {
            switch ((GoalFrequency)SelectedFrequencyID)
            {
                case GoalFrequency.EveryOtherDay:
                    //(DateTime.Now - g.CreationDate).TotalDays
                    //This gets the age of the goal in days
                    //does a mod to determine if its an even or of number of days
                    //if it is even then it shows
                    return ((queryDate.Date - CreationDate.Date).Days % 2) == 0;
                case GoalFrequency.SelectDayOfWeek:
                    DayOfWeek dow = queryDate.DayOfWeek;
                    if (dow == DayOfWeek.Monday && Monday)
                        return true;
                    if (dow == DayOfWeek.Tuesday && Tuesday)
                        return true;
                    if (dow == DayOfWeek.Wednesday && Wednesday)
                        return true;
                    if (dow == DayOfWeek.Thursday && Thursday)
                        return true;
                    if (dow == DayOfWeek.Friday && Friday)
                        return true;
                    if (dow == DayOfWeek.Saturday && Saturday)
                        return true;
                    if (dow == DayOfWeek.Sunday && Sunday)
                        return true;

                    return false;
                case GoalFrequency.EveryDay:
                default:
                    return true;
            }
        }
    }
}
