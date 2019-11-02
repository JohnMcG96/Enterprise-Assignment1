namespace Assignment1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Register")]
    public partial class Register
    {
        [Key]
        public int RegID { get; set; }

        public DateTime RegDate { get; set; }

        public int StudentID { get; set; }

        public int ModuleID { get; set; }

        public virtual Module Module { get; set; }

        public virtual Student Student { get; set; }
    }
}
