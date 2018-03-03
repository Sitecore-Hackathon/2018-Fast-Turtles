using Sitecore.Analytics.Pipelines.InitializeTracker;
using Sitecore.Analytics.Tracking;
using Sitecore.XConnect;
using System;

namespace ContentExplorer.Processors.Session
{
    public class SwitchContact : InitializeTrackerProcessor
    {
        public const string ContactKey = "xdbcontactid";

        public ContactManager ContactManager { get; set; }

        public override void Process(InitializeTrackerArgs args)
        {
            if (args.Session.Contact != null)
            {
                return;
            }

            if (args.IsNewContact)
            {
                return;
            }

            string contactIDvalue = Sitecore.Web.WebUtil.GetQueryString(ContactKey);
            Guid contactID;

            if (!Guid.TryParse(contactIDvalue, out contactID))
            {
                return;
            }

            Sitecore.Diagnostics.Log.Warn("Switching to contact ID. " + contactIDvalue, this);

            //Sitecore.Analytics.Tracking.Contact target;
            try
            {
                //target = this.ContactManager.LoadContact(contactID, !args.Session.IsReadOnly);

                Sitecore.Diagnostics.Log.Warn("Could not load contact from xDB param. ", this);

                args.ContactId = contactID;
                //args.Session.Contact = target;
            }
            catch (XdbUnavailableException ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message + ex.StackTrace, this);
                Sitecore.Diagnostics.Log.Debug(string.Format("[Analytics]: The contact '{0}' could not be loaded from the database. Treating the contact as temporary.", (object)contactID));
            }
        }
    }
}
