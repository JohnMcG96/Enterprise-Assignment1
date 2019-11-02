namespace Assignment1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Attendance")]
    public partial class Attendance
    {
        [Key]
        public int AttendID { get; set; }

        public int StudentID { get; set; }

        public int EventID { get; set; }

        public bool AttendStatus { get; set; }

        public virtual LearningEvent LearningEvent { get; set; }

        public virtual Student Student { get; set; }
    }
}
