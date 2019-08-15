using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http; // FOR USE OF SESSIONS
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using bankAccount.Models; //change projectName to the name of project

namespace bankAccount.Controllers  //change projectName to the name of project
{
    public class HomeController : Controller{
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]               // GETS Main Registration and Login Page
        public IActionResult Index()
        {
            return View("Index");
        }

        // The rest of the Controller Code goes here (routes, Posts, Gets, Linq, etc)

        [HttpPost("register")]
        public IActionResult CreateUser(LoginRegView viewModel)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == viewModel.newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                viewModel.newUser.Password = Hasher.HashPassword(viewModel.newUser, viewModel.newUser.Password);

                dbContext.Users.Add(viewModel.newUser);
                dbContext.SaveChanges();

                HttpContext.Session.SetInt32("userInSess", viewModel.newUser.UserId);

                return RedirectToAction("Account");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult LoginUser(LoginRegView viewModel)
        {
            if(ModelState.IsValid)
            {
                var dbUser = dbContext.Users.FirstOrDefault(u => u.Email == viewModel.newLogin.loginEmail);
                if(dbUser == null)
                {
                    ModelState.AddModelError("Email", "Email does not exist; please create account");
                    return View("Index");
                }

                var hasher = new PasswordHasher<Login>();
                var result = hasher.VerifyHashedPassword(viewModel.newLogin, dbUser.Password, viewModel.newLogin.loginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("Password", "Password does not match Account on File");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("userInSess", dbUser.UserId);

                return RedirectToAction("Account");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("account")]
        public IActionResult Account(){
            if(HttpContext.Session.GetString("userInSess") != null){
                AccountView viewModel = new AccountView();

                viewModel.User = dbContext.Users
                    .Include(u => u.Transactions)
                    .FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userInSess"));

                
                decimal sum = viewModel.User.Transactions.Select(a => a.Amount).Sum();
                ViewBag.sum = sum;

                return View("Account", viewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost("createtransaction")]
        public IActionResult NewTransaction(Transaction transaction){
            if(HttpContext.Session.GetString("userInSess") != null){
                decimal balance = dbContext.Users
                    .Include(u => u.Transactions)
                    .FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userInSess"))
                    .Transactions.Select(u => u.Amount)
                    .Sum();
                if(balance + transaction.Amount < 0)
                {
                    ModelState.AddModelError("Amount", "Withdrawl Exceeds Available Amount");
                    return RedirectToAction("Account");
                }
                transaction.CreatedAt = DateTime.Now;
                transaction.UserId = (int)HttpContext.Session.GetInt32("userInSess");
                dbContext.Add(transaction);
                dbContext.SaveChanges();
                return RedirectToAction("Account");
            }

            return RedirectToAction("Index");
        }

        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        


        [HttpGet("success")]
        public IActionResult Success()
        {
            return View("Success");
        }

    }
}