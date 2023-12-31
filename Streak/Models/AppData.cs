using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streak.Models
{
    public static class AppData
    {
        public static List<Goal> Goals =
        [
            new(){ Id = 1, Name = "Push ups", CreationDate = DateTime.Now},
            new(){ Id = 2, Name = "Squats", CreationDate = DateTime.Now},
            new(){ Id = 3, Name = "Programming", CreationDate = DateTime.Now},
            new(){ Id = 4, Name = "Read Book", CreationDate = DateTime.Now},
            new(){ Id = 5, Name = "Ukulele", CreationDate = DateTime.Now},
        ];
    }
}
