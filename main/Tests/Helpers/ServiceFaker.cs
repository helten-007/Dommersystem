using System;
using Microsoft.Practices.Unity;
using Moq;
using NordicArenaTournament;
using NordicArenaTournament.Common;
using NordicArenaTournament.SignalR;

namespace Tests.Helpers
{
    internal class ServiceFaker
    {
        /// <summary>
        /// Creates a random generator which always will return 
        /// </summary>
        internal static Random GetPredictableRandomGenerator()
        {
            var predictableRng = new Mock<Random>();
            predictableRng.Setup(p => p.Next(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((min, max) => { return min; });
            return predictableRng.Object;
        }

        internal static NaHub GetFakeSignalRHub()
        {
            return new Mock<NaHub>().Object;
        }

        internal static void ResetIocContainer()
        { 
            UnityConfig.Reset();
        }

        public static FormFeedbackHandler GetFakeFormFeedbackHandler()
        {
            return new Mock<FormFeedbackHandler>().Object;
        }
    }
}
