using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BOS.Models;
using BOS.Models_Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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
      return RedirectToAction("List");
    }

    public ActionResult List()
    {
      var allEmployeesWithoutLoggedUser = db.Users.Where(e => e.UserName != this.User.Identity.Name).ToList();
      var activeEmployeeForCreatingVote = db.Votings.Where(v => (v.IsActive && v.IsOnlyOnceVoted
                                                                            && !v.IsYearOfActivationAvailable))
                                                    .Select(vr => vr.CandidateForGiftId.ToString()).ToList();
      var filteredEmployees = allEmployeesWithoutLoggedUser.Where(es => !activeEmployeeForCreatingVote.Contains(es.Id))
                                                           .OrderBy(bd => bd.BirthDate)
                                                           .ToList();
      return View(filteredEmployees);
    }

    public ActionResult Activate(Guid? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ApplicationUser employee = db.Users.Find(id.ToString());
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

      var currentVoting = db.Votings.FirstOrDefault(v => v.VoteCreator.Email == currentLoggedUserEmail
                                                          || v.IsActive == false
                                                          || v.IsOnlyOnceVoted == false);
      // Extract in helper class
      var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
      var currentUser = manager.FindById(User.Identity.GetUserId());

      if (currentVoting == null || currentVoting.CandidateForGiftId != employee.Id)
      {
        var currVoting = new Voting()
        {
          VoteCreatorId = currentUser.Id,
          CandidateForGiftId = employee.Id,
          IsActive = true,
          IsOnlyOnceVoted = true,
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
          IsActive = true,
          IsOnlyOnceVoted = true,
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
