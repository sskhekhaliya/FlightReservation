using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlightReservation.Models
{
    public class Booking
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingID { get; set; }

        [Required]
        public DateTime DateAndTime { get; set; }

        [Required]
        public string User { get; set; }
        [ForeignKey("User")]
        public virtual UserDetail UserDetail { get; set; }

        [Required]
        public string Class { get; set; }

        [Required]
        public int ScheduleID { get; set; }
        [ForeignKey("ScheduleID")]
        public virtual Schedule Schedule { get; set; }
    }
}
