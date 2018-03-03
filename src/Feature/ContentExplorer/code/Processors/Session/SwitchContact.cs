using Sitecore.Analytics.Pipelines.InitializeTracker;
using Sitecore.Analytics.Tracking;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using Sitecore.XConnect.Client;

namespace ContentExplorer.Processors.Session
{
    public class SwitchContact : InitializeTrackerProcessor
    {
        public const string ContactKey = "xdbcontactid";

        public ContactManager ContactManager { get; set; }

        public override void Process(InitializeTrackerArgs args)
        {
            string contactIdentifier = Sitecore.Web.WebUtil.GetQueryString(ContactKey);
            if (!IsValidEmail(contactIdentifier))
            {
                return;
            }

            Sitecore.Analytics.Tracker.Initialize();
            Sitecore.Analytics.Tracker.StartTracking();
        
             Sitecore.Analytics.Tracker.Current.Session.IdentifyAs("website", contactIdentifier);

            var contact =  GetContactByIdentifier(contactIdentifier);
            if (contact == null || !contact.Id.HasValue)
            {
                return;
            }

            Sitecore.Analytics.Tracking.Contact target;
            try
            {
                
                target = this.ContactManager.LoadContact(contact.Id.Value, !args.Session.IsReadOnly);
                if(target == null) { 
                    Sitecore.Diagnostics.Log.Warn("Could not load contact from xDB param. ", this);
                }

                args.ContactId = contact.Id;
                args.Session.Contact = target;
            }
            catch (XdbUnavailableException ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message + ex.StackTrace, this);
                Sitecore.Diagnostics.Log.Debug(string.Format("[Analytics]: The contact '{0}' could not be loaded from the database. Treating the contact as temporary.", (object)contact.Id));
            }
        }

        public Sitecore.XConnect.Contact GetContactByIdentifier(string identifier)
        {
            using (XConnectClient client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    var reference = new IdentifiedContactReference("website", identifier);

                    return client.Get<Sitecore.XConnect.Contact>(reference, new ContactExpandOptions() { });
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exceptions
                }

                return null;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
