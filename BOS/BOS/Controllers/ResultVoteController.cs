using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BOS.Models_Data;
using BOS.Models;
using Context = System.Web.HttpContext;
using Microsoft.AspNet.Identity;
using BOS.Common;
using BOS.Models_View;

namespace OnlineBirthdaySystem.Controllers
{
  public class ResultVoteController : Controller
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    public ActionResult Index()
    {
      return RedirectToAction("List");
    }
    public ActionResult List()
    {
      var query = @"SELECT Ag.FullName as NameOfEmployeeWithBd,
	                         Ag.Email as EmailOfEmployeeWithBd,
	                         Ag.YearOfActivationVote,
	                         Ag.Description as GiftDescription,
	                         ue.FullName as NameOfVoter,
	                         ue.Email as EmailOfVoter
                          FROM 
                                (SELECT u.Id,u.FullName, u.Email, 
	                                    v.VotingId, v.CandidateForGiftId,v.IsCompleted, v.YearOfActivationVote,
	                                    vd.VotingDetailId, vd.ElectorateId, vd.GiftId,
	                                    g.Description
                                FROM AspNetUsers u
                                LEFT JOIN Votings as v ON u.Id = v.CandidateForGiftId
                                LEFT JOIN VotingDetails as vd on v.VotingId = vd.VotingId
                                LEFT JOIN Gifts as g on g.GiftId = vd.GiftId
                                WHERE v.IsCompleted = 1
                                ) as Ag
                            RIGHT JOIN AspNetUsers ue ON Ag.ElectorateId = ue.Id
                            ORDER BY Ag.YearOfActivationVote desc, Ag.FullName asc";

      var result = db.Database.SqlQuery<VotedResultViewModel>(query).ToList();
      var currentLoggedUserEmail = Utility.GetCurrentLoggedUser(User.Identity.GetUserId()).Email;
      var resultWithOutCurrentLoggedUser = result.Where(r => r.EmailOfEmployeeWithBd != currentLoggedUserEmail).ToList();

      return View(resultWithOutCurrentLoggedUser);

    }

    public ActionResult EndVoting(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      var currentVotingEntry = db.Votings.Where(v => v.VotingId == id).First();
      currentVotingEntry.IsCompleted = true;
      db.SaveChanges();
      return RedirectToAction("Confirmation");
    }

    public ActionResult CompleteProcessVoting(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      var currentLoggedUserId = Utility.GetCurrentLoggedUser(User.Identity.GetUserId()).Id;
      var currentVotingId = Context.Current.Session["VotingId"] as int?;

      var currentVotingDetail = new VotingDetail()
      {
        ElectorateId = currentLoggedUserId,
        GiftId = id.Value,
        VotingId = currentVotingId.Value
      };

      db.VotingDetials.Add(currentVotingDetail);
      db.SaveChanges();

      Context.Current.Session["VotingId"] = null;
      Context.Current.Session["FullName"] = null;
      return RedirectToAction("List");
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
