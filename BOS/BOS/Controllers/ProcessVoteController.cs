using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BOS.Models_Data;
using BOS.Models;
using Context = System.Web.HttpContext;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BOS.Common;

namespace OnlineBirthdaySystem.Controllers
{
  public class ProcessVoteController : Controller
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    public ActionResult Index()
    {
      return RedirectToAction("List");
    }

    public ActionResult List()
    {
      // check for bussines rules
      var currentLoggedUserId = Utility.GetCurrentLoggedUser(User.Identity.GetUserId()).Id;
      // TODDO Optimize query
      var votings = db.Votings.Include(v => v.CandidateForGift)
                              .Where(vf => !vf.VotingDetails
                                             .Any(vs => vs.ElectorateId == currentLoggedUserId)
                                             && !vf.IsCompleted)
                              .OrderBy(bd => bd.CandidateForGift.BirthDate)
                              .ToList();
      return View(votings);
    }

    public ActionResult StartVoting(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      // TODO remove Tolist and take only mandatory fields
      var currentVotingEntry = db.Votings.Where(v => v.VotingId == id).First();

      Context.Current.Session["VotingId"] = currentVotingEntry.VotingId;
      Context.Current.Session["FullName"] = currentVotingEntry.CandidateForGift.FullName;

      return RedirectToAction("ChoosePresent");
    }

    public ActionResult ChoosePresent()
    {
      var gifts = db.Gifts.ToList();
      return View(gifts);
    }

    public ActionResult CompleteProcessVoting(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      // TODO exctract it in helper class
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
