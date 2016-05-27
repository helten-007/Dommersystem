using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace Tests.Helpers
{
    public class MvcFaker
    {
        public static void SetLanguage(String langCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCode);
        }

        public static HtmlHelper CreateHtmlHelper()
        {
            var vd = new ViewDataDictionary();
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>(
                new ControllerContext(
                    new Mock<HttpContextBase>().Object,
                    new RouteData(),
                    new Mock<ControllerBase>().Object),
                new Mock<IView>().Object,
                vd,
                new TempDataDictionary(),
                new Mock<TextWriter>().Object);
            var mockViewDataContainer = new Mock<IViewDataContainer>();
            mockViewDataContainer.Setup(v => v.ViewData)
                .Returns(vd);
            return new HtmlHelper(mockViewContext.Object,
                                    mockViewDataContainer.Object);
        }

        public static HtmlHelper<TModel> CreateHtmlHelper<TModel>(TModel model)
        {
            var vd = new ViewDataDictionary();
            vd.Model = model;
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>(
                new ControllerContext(
                    new Mock<HttpContextBase>().Object,
                    new RouteData(),
                    new Mock<ControllerBase>().Object),
                new Mock<IView>().Object,
                vd,
                new TempDataDictionary(),
                new Mock<TextWriter>().Object);
            var mockViewDataContainer = new Mock<IViewDataContainer>();
            mockViewDataContainer.Setup(v => v.ViewData)
                .Returns(vd);
            return new HtmlHelper<TModel>(mockViewContext.Object,
                                    mockViewDataContainer.Object);
        }
    }
}
