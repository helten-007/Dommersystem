using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NordicArenaTournament.Common
{
    public class FormFeedbackHandler
    {
        public void SetError(HttpContextBase context, String message)
        {
            context.Session["Error"]  = message;
        }

        public virtual MvcHtmlString GetFeedbackHtml()
        {
            if (HttpContext.Current.Session["Error"] != null)
            {
                String error = (String) HttpContext.Current.Session["Error"];
                HttpContext.Current.Session["Error"] = null;
                return new MvcHtmlString(error);
            }
            else return new MvcHtmlString(String.Empty);
        }

        public virtual bool HasFeedback()
        {
            return HttpContext.Current.Session["Error"] != null;
        }
    }
}