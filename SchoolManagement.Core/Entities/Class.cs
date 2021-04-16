﻿using System;
using System.Collections.Generic;

namespace SchoolManagement.Core.Entities
{
    public partial class Class
    {
        public int Id { get; set; }
        public string ClassCode { get; set; } = null!;
        public int CourseId { get; set; }
        public string TeacherId { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public DayOfWeek Day { get; set; }
        public int StartPeriods { get; set; }
        public int Periods { get; set; }
        public int? Slot { get; set; }
        public int? RestSlot { get; set; }


        public virtual Course? Course { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
