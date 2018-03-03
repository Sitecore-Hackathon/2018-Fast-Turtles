namespace FastTurtles
{
    using System.Web.Mvc;

    using FastTurtles.Model;

    using Sitecore.Analytics;
    using Sitecore.Mvc.Controllers;

    public class SampleController : SitecoreController
    {
        public static void IdentifyUser(string username)
        {
            // Never identify an anonymous user
            if (username.ToLower() == "extranet\\anonymous")
            {
                return;
            }

            var identificationSource = "website";
            if (Tracker.Current != null && Tracker.Current.IsActive && Tracker.Current.Session != null)
            {
                Tracker.Current.Session.IdentifyAs(identificationSource, username);
            }
        }

        public ActionResult AddContactUser()
        {
            return this.View();
        }

        [HttpPost]
        public void AddContactUser(ContactViewModel viewModel) { }

        // GET
        public ActionResult IdentifyContact(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                IdentifyUser(email);
            }

            return this.View();
        }
    }
}