using BOS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BOS.Common
{
  public static class Utility
  {
    public static ApplicationUser GetCurrentLoggedUser(string userId)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
      var currentUser = manager.FindById(userId);
      return currentUser;
    }
  }
}