using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bankAccount.Models{  //change projectName to the name of project

    public class AccountView
    {
        public Transaction Transaction{get;set;}
        
        public User User {get;set;}// include all classes and a new instance of the class within this file
    }
}