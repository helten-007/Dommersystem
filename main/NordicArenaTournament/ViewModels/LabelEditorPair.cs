using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NordicArenaTournament.ViewModels
{
    /// <summary>
    /// For use with Shared View LabelEditor
    /// </summary>
    public class LabelEditorPair
    {
        public MvcHtmlString LabelHtml { get; set; }
        public MvcHtmlString EditorHtml { get; set; }
        public MvcHtmlString ValidationHtml { get; set; }
        public String LabelCssClass { get; set; }
        public String EditorCssClass { get; set; }
    }
}