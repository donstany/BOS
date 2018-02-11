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

namespace OnlineBirthdaySystem.Controllers
{
  public class EndVoteController : Controller
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    public ActionResult Index()
    {
      return RedirectToAction("List");
    }

    // Create New Vote
    public ActionResult List()
    {
      // check for bussines rules
      // TODO for fast comparing parse string key to Guid
      var currentLoggedUserId = Utility.GetCurrentLoggedUser(User.Identity.GetUserId()).Id;

      var votings = db.Votings.Include(v => v.CandidateForGift)
                              .Where(vf => !vf.IsCompleted
                                     && vf.VoteCreator.Id.ToString() == currentLoggedUserId)
                              .OrderBy(bd => bd.CandidateForGift.BirthDate)
                              .ToList();
      return View(votings);
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
