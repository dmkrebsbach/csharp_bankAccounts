using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace bankAccount.Models
{  //change projectName to the name of project
    
    public class Transaction {

        [Key]
        public int TransactionId {get;set;}

        [Required]
        public int UserId { get; set; }
        public User user { get; set; }
        public decimal Amount {get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.Now;

    }
    
}
