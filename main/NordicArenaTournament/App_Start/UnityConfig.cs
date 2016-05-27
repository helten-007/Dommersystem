using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using NordicArenaServices;
using NordicArenaTournament.SignalR;

namespace NordicArenaTournament
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> _container = new Lazy<IUnityContainer>(CreateContainer);

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return _container.Value;
        }

        private static IUnityContainer CreateContainer()
        {
            var c = new UnityContainer();
            RegisterTypes(c);
            return c;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ITournamentService, EfTournamentService>();
            container.RegisterType<IHubContext>(new InjectionFactory(p => GlobalHost.ConnectionManager.GetHubContext<NaHub>()));
        }

        public static void Reset()
        {
            CreateContainer();
        }
    }
}
