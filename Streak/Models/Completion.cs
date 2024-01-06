using SQLite;
using System.ComponentModel.DataAnnotations.Schema;


namespace Streak.Models
{
    [SQLite.Table("Completions")]
    public class Completion
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        //[ForeignKey(nameof(Goal))]
        public int GoalID { get; set; }
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// This is the default constructor needed for database interaction
        /// </summary>
        public Completion()
        {

        }

        public Completion(Goal goal)
        {
            this.GoalID = goal.ID;
            this.CreationDate = DateTime.Now;
        }
    }
}
