using NUnit.Framework;

namespace SharpFFTW.Tests
{
    [SetUpFixture]
    public class Setup
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            Library.SetImportResolver();
        }
    }
}
