using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    /* Uferdig klasse. Tanken var å lage et system som oppdater properties som ikke har en Resource-tekst knyttet til seg, men
      usikker på om det er verdt å implmentere. */
    public class PropertyResourceTest
    {
        Regex filter = new Regex(".*Models\\..*"); // Which classes to process

        [TestMethod]
        public void TestAllPropertiesResources()
        {
            var assembly = typeof(NordicArenaTournament.MvcApplication).Assembly;
            var types = assembly.GetExportedTypes();
            foreach (var type in types) 
            {
                if (filter.IsMatch(type.FullName))
                {
                    AssertAllPropertiesIn(type);
                }
            }
        }

        private void AssertAllPropertiesIn(Type type)
        {
            /*var types = type.GetProperties(BindingFlags.Public);
            foreach (var type in types)
            {
                var displayAttributes = type.   
            }*/
        }
    }
}
