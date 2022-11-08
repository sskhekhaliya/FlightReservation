using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlightReservation.Models
{
    public class UserDetail
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required, DataType(DataType.Date)]
        [Column(TypeName ="date")]
        public DateTime DOB { get; set; }

        public int Age { get; set; }

        [Required]
        public int PhoneNo { get; set; }
        public string Address { get; set; }
        public string ProfilePic { get; set; }
        public double Wallet { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
