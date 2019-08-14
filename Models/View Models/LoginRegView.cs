using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bankAccount.Models{  //change projectName to the name of project

    public class LoginRegView
    {
        public User newUser {get;set;}  // public "ClassName" should match the class name in the file, not the file name itself
        public Login newLogin {get;set;} // include all classes and a new instance of the class within this file
    }
}