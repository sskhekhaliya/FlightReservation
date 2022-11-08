using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlightReservation.Models
{
    public class Schedule
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleID { get; set; }

        [Required]
        public string FlightNo { get; set; }
        [ForeignKey("FlightNo")]
        public FlightDetail FlightDetail { get; set; }

        [Required]
        public string From { get; set; }

        [Required]
        public DateTime BoardingTime { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public int ESeats { get; set; }

        [Required]
        public int BSeats { get; set; }
        
        [Required]
        public int EPrice { get; set; }

        [Required]
        public int BPrice { get; set; }

        public ICollection<Booking> Bookings { get; set; }

    }
}
