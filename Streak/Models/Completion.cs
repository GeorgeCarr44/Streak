using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Streak.Models
{
    public class Completion
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [ForeignKey(nameof(Goal))]
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
