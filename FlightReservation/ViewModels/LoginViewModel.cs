﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightReservation.ViewModels
{
    public class LoginViewModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

}
