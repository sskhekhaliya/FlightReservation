using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlightReservation.Models
{
    public class FlightDetail
    {
        [Required, Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string FlightID { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Logo { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}
