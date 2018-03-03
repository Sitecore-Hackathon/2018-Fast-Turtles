using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ContentExplorer.Processors.Session;
using Sitecore.Analytics;

namespace ContentExplorer.sitecore.admin
{
    public partial class ContentExplorer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            if (!string.IsNullOrEmpty(Request.QueryString[SwitchContact.ContactKey]))
            {
                var identifier = Request.QueryString[SwitchContact.ContactKey];
                IdentifierText.Text = identifier;
            }
        }

        protected void SwitchToContact(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(IdentifierText.Text.Trim()))
            {
                Response.Redirect("~/sitecore/admin/ContentExplorer.aspx?" + SwitchContact.ContactKey + "=" + IdentifierText.Text.Trim());
            }
        }
    }
}