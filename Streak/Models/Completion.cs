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
    public class Completion(Goal goal)
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [ForeignKey(nameof(Goal))]
        public int GoalID { get; set; } = goal.ID;
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
