using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helpers
{
    /// <summary>
    /// Signifies that something went wrong in setup of the test
    /// </summary>
    public class TestSetupException : AssertFailedException
    {
        public TestSetupException()
            : base("Failed in setup phase of unit test")
        {

        }
    }
}
