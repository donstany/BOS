using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BOS.Models;
using BOS.Models_Data;

namespace OnlineBirthdaySystem.Controllers
{
  //TO DO : Exctract all bussiness logic in Service Layer Assembly or in standalone Web API.
  [Authorize]
  public class CreateVoteController : Controller
  {
    //TO DO: Replace it with poor man and DI container AutoFac or Ninject
    private ApplicationDbContext db = new ApplicationDbContext();

    public ActionResult Index()
    {
      var emploees = db.Employees.Where(e => e.AppUser.UserName != this.User.Identity.Name);
      var activeEmployeeForCreatingVote = db.Votings.Where(v => (v.IsActive && v.IsOnlyOnceVoted
                                                                 && v.YearOfActivationVote == DateTime.Now.Year))
                                                    .Select(vr => vr.OwnerId);
      var filteredEmploees = emploees.Where(es => !activeEmployeeForCreatingVote.Contains(es.EmployeeId));
      return View(filteredEmploees);
    }

    public ActionResult Activate(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Employee employee = db.Employees.Find(id);
      if (employee == null)
      {
        return HttpNotFound();
      }

      //This prevent from http query injection from Backend
      var currentLoggedUserEmail = this.User.Identity.Name;
      if (employee.Email == currentLoggedUserEmail)
      {
        return RedirectToAction("Index");
      }

      var currentVoting = db.Votings.FirstOrDefault(v => v.InitializerOfVoteEmail == currentLoggedUserEmail
                                                          || v.IsActive == false
                                                          || v.IsOnlyOnceVoted == false);
      if (currentVoting == null)
      {
        var currVoting = new Voting()
        {
          InitializerOfVoteEmail = currentLoggedUserEmail,
          IsActive = true,
          IsOnlyOnceVoted = true,
          OwnerId = employee.EmployeeId,
          OwnerEmail = employee.Email,
          IsYearOfActivationAvailable = false,
        };
        db.Votings.Add(currVoting);
        db.SaveChanges();
        // TO DO : Create Call Notification service for email 
        return RedirectToAction("Confirmation");
      }

      else if (currentVoting.IsYearOfActivationAvailable == true
              || currentVoting.YearOfActivationVote != DateTime.Now.Year)
      {
        var currVoting = new Voting()
        {
          InitializerOfVoteEmail = currentLoggedUserEmail,
          IsActive = true,
          IsOnlyOnceVoted = true,
          OwnerId = employee.EmployeeId,
          OwnerEmail = employee.AppUser.Email,
          YearOfActivationVote = DateTime.Now.Year,
          IsYearOfActivationAvailable = false,
        };
        db.Votings.Add(currVoting);
        db.SaveChanges();
        //TO DO : Call Notification service for sending emails to all subscribed employees without man which have birthday
        return RedirectToAction("Confirmation");
      }
      else
      {
        return RedirectToAction("Unallowed");
      }
    }

    public ActionResult Unallowed()
    {
      return View();
    }

    public ActionResult Confirmation()
    {
      return View();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
