namespace Assignment1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LearningEvent")]
    public partial class LearningEvent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LearningEvent()
        {
            Attendances = new HashSet<Attendance>();
        }

        [Key]
        public int EventID { get; set; }

        public DateTime EventDateTime { get; set; }

        [Required]
        public string EventDuration { get; set; }

        [Required]
        public string EventType { get; set; }

        public int ModuleID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }

        public virtual Module Module { get; set; }
    }
}
