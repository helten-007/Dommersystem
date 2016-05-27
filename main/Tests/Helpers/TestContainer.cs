using Microsoft.Practices.Unity;
using Moq;
using NordicArenaServices;
using NordicArenaTournament.Common;
using NordicArenaTournament.SignalR;

namespace Tests.Helpers
{
    /// <summary>
    /// Unity configuration for unit tests. All types should return mock or fakes of servics suitable for unit testing
    /// </summary>
    public class TestContainer 
    {
        public static UnityContainer GetInstance()
        {
            var container = new UnityContainer();
            Init(container);
            return container;
        }

        private static void Init(UnityContainer container)
        {
            container.RegisterType<ITournamentService, InMemoryDataContext>();
            container.RegisterInstance<NaHub>(new Mock<NaHub>().Object);
            container.RegisterInstance<FormFeedbackHandler>(ServiceFaker.GetFakeFormFeedbackHandler());
        }
    }
}
