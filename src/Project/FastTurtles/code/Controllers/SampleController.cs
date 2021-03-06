﻿
using Sitecore.XConnect.Collection.Model;

namespace FastTurtles
{
    using System.Web.Mvc;
    using FastTurtles.Model;
    using System;
    using System.Threading.Tasks;
    using Sitecore.Analytics;
    using Sitecore.Analytics.Tracking;
    using Sitecore.Mvc.Controllers;
    using Sitecore.XConnect;
    using Sitecore.XConnect.Client;
    using Contact = Sitecore.XConnect.Contact;
    using Interaction = Sitecore.XConnect.Interaction;

    public class SampleController : SitecoreController
    {
        public ActionResult AddContactUser()
        {
            return View("~/Views/Shared/CreateContact.cshtml");
        }

        [HttpPost]
        public JsonResult AddContactUser(ContactViewModel viewModel)
        {
            bool result = false;
            if (viewModel != null && !string.IsNullOrWhiteSpace(viewModel.Email))
            {
                result = Task.Run(() => CreateContact("website", viewModel.Email, viewModel.FirstName, viewModel.LastName, viewModel.Email)).Result;
            }

            return Json(result);
        }

        protected async Task<bool> CreateContact(string source, string identifier, string firstName, string lastName,
            string email)
        {
            using (XConnectClient client = GetClient())
            {
                try
                {
                    IdentifiedContactReference reference = new IdentifiedContactReference(source, identifier);


                    var contactTask = client.GetAsync<Contact>(
                        reference,
                        new ContactExpandOptions(
                            PersonalInformation.DefaultFacetKey,
                            EmailAddressList.DefaultFacetKey,
                            PhoneNumberList.DefaultFacetKey)
                    );

                    Contact existingContact = await contactTask;

                    if (existingContact != null)
                    {
                        return false;
                    }

                    var contactIdentifier = new[]
                    {
                    new ContactIdentifier(source, identifier, ContactIdentifierType.Known)
                };

                    Contact contact = new Contact(contactIdentifier);

                    var personal = new PersonalInformation
                    {
                        FirstName = firstName,
                        LastName = lastName
                    };

                    var preferredEmail = new EmailAddress(email, true);
                    var emails = new EmailAddressList(preferredEmail, "Work email");

                    client.AddContact(contact);
                    client.SetPersonal(contact, personal);
                    client.SetEmails(contact, emails);

                    Interaction interaction = new Interaction(contact, InteractionInitiator.Brand, Guid.NewGuid(), "test");
                    var ev = new PageViewEvent(DateTime.UtcNow, Guid.Parse("{11111111-1111-1111-1111-111111111111}"), 1, "en");
                    interaction.Events.Add(ev);

                    client.AddInteraction(interaction);

                    await client.SubmitAsync();

                    return true;
                }
                catch (XdbExecutionException ex)
                {
                    return false;
                }
            }

        }

        protected XConnectClient GetClient()
        {
            return Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient();
        }
    }
}