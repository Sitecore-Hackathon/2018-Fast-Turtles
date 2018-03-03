using System.Web.Mvc;
using Sitecore.Analytics;
using Sitecore.Mvc.Controllers;
using Sitecore.Web;

namespace FastTurtles
{
    public class SampleController : SitecoreController
    {
        // GET
        public ActionResult IdentifyContact(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                IdentifyUser(email);
            }
            return View();
        }

        public static void IdentifyUser(string username)
        {
            // Never identify an anonymous user
            if (username.ToLower() == "extranet\\anonymous")
                return;

            string identificationSource = "website";
            if (Tracker.Current != null && Tracker.Current.IsActive && Tracker.Current.Session != null)
            {
                Tracker.Current.Session.IdentifyAs(identificationSource, username);
            }
        }
    }
}