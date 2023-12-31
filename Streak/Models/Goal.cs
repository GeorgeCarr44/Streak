using HealthKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Streak.Models
{
    public class Goal
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required DateTime CreationDate { get; set; }
        public string? Description { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }

        public Goal() { }
    }
}
