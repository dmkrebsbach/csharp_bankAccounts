using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bankAccount.Models{  //change projectName to the name of project

    public class Login
    {
        // No other fields!
        [Required(ErrorMessage = "Please enter your Email")]
        [EmailAddress(ErrorMessage = "Enter a valid email address ex: name@address.com")]
        public string loginEmail {get; set;}

        [Required(ErrorMessage = "Enter a password")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string loginPassword { get; set; }
    }
}