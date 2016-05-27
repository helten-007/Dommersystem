using System;
using System.Linq.Expressions;
using System.Resources;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using NordicArenaDomainModels.Resources;
using NordicArenaTournament.ViewModels;

namespace NordicArenaTournament.Common
{
    /// <summary>
    /// Helper class similar to System.Web.Mvc.WebViewpage.Html, only for app-specific needs (Na=NordicArena).
    /// If the method doesnt return app-specific content, it doesn't belong here.
    /// </summary>
    public static class NaHtml
    {
        /// <summary>
        /// Returns a formatted page title prefixed with App title
        /// </summary>
        /// <param name="title">Title of the page</param>
        /// <returns>A formatted page title prefixed with App title</returns>
        public static String PageTitle(String title)
        {
            return Text.AppName + " - " + title;
        }

        /// <summary>
        /// Creates LabelFor + EditorFor + ValidationMessageFor, wrapped in two sets of div's to which you can assign custom classes
        /// </summary>
        public static MvcHtmlString ValidatedEditPairFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String labelCssClass = null, String valueCssClass = null)
        {
            return html.EditPairFor(expression, true, labelCssClass, valueCssClass);
        }

        /// <summary>
        /// Creates LabelFor + EditorFor, wrapped in two sets of div's to which you can assign custom classes
        /// </summary>
        public static MvcHtmlString EditPairFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String labelCssClass = null, String valueCssClass = null)
        {
            return html.EditPairFor(expression, false, labelCssClass, valueCssClass);
        }

        /// <summary>
        /// Creates LabelFor + EditorFor + ValidationMessageFor, wrapped in two sets of div's to which you can assign custom classes
        /// </summary>
        private static MvcHtmlString EditPairFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool validate, String labelCssClass = null, String valueCssClass = null)
        {
            var model = new LabelEditorPair();
            model.LabelHtml = html.LabelFor(expression);
            model.LabelCssClass = labelCssClass;
            model.EditorHtml = html.EditorFor(expression);
            model.EditorCssClass = valueCssClass;
            if (validate) model.ValidationHtml = html.ValidationMessageFor(expression);
            return html.Partial(MVC.Shared.Views.LabelAndEditor, model);
        }
        
        /// <summary>
        /// Creates LabelFor + ValueFor + ValidationMessageFor, wrapped in two sets of div's to which you can assign custom classes
        /// </summary>
        public static MvcHtmlString DisplayPairFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String labelCssClass = null, String valueCssClass = null)
        {
            var model = new LabelEditorPair();
            model.LabelHtml = html.LabelFor(expression);
            model.LabelCssClass = labelCssClass;
            model.EditorHtml = html.NaValueFor(expression);
            model.EditorCssClass = valueCssClass;
            return html.Partial(MVC.Shared.Views.LabelAndValue, model);
        }

        /// <summary>
        /// ValueFor with EnumValueFor-support
        /// </summary>
        public static MvcHtmlString NaValueFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var membExp = (MemberExpression)expression.Body;
            if (membExp.Type.IsEnum) return html.EnumValueFor(expression);
            return html.DisplayFor(expression);
        }

        /// <summary>
        /// Returns a resource-translated enum value by convention.
        /// An enum with name "Tournament" and value "Ended" will look for a resource string
        /// named "Tournament_Ended" in the PropertyNames resource file.
        /// </summary>
        public static MvcHtmlString EnumValueFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var enumObject = expression.Compile().Invoke(html.ViewData.Model);
            var translateEnum = EnumValueFor(enumObject);
            var cont = new ViewDataDictionaryContainer();
            cont.ViewData.Model = translateEnum.ToString();
            var tempHelper = new HtmlHelper<String>(html.ViewContext, cont);
            return tempHelper.DisplayFor(p => p); // Ensuring our custom displayTemplate is used
        }

        /// <summary>
        /// Returns an enum value from PropertyNames resourcs based on the
        /// enum type and value. 
        /// An enum with name "Tournament" and value "Ended" will look for a resource string
        /// named "Tournament_Ended".
        /// </summary>
        public static MvcHtmlString EnumValueFor(Object enumProperty)
        {
            // Resolve localized string by convention
            String enumString = enumProperty.ToString();
            String enumKey = enumProperty.GetType().Name + "_" + enumString;
            var resMan = new ResourceManager(typeof(PropertyNames));
            String translatedVal = resMan.GetString(enumKey);
            // Return as MvcHtmlString
            if (translatedVal != null) return new MvcHtmlString(translatedVal);
            return new MvcHtmlString(enumString);
        }

        public static String Decimals(this decimal? val, int numDecimals = 1)
        {
            if (numDecimals < 0)throw new ArgumentException("NumDecimals must be > 0");
            if (val == null) return String.Empty;
            return val.Value.ToString("F" + numDecimals); 
        }
    }

    public class ViewDataDictionaryContainer : IViewDataContainer
    {
        public ViewDataDictionary ViewData { get; set; }
        public ViewDataDictionaryContainer() { ViewData = new ViewDataDictionary(); }
    }
}