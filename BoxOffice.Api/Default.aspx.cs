using System;

namespace BoxOffice.Api
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (AccessViolationException)
            {
                Response.StatusCode = 404;
                Response.End();
            }
        }
    }
}