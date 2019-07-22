using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestNinja.Mocking;

namespace TestNinja.Tests.Mocking
{
    [TestClass]
    public class VideoServiceTests
    {
        [TestMethod]
        public void ReadVideoTitle_EmptyFile_ReturnError()
        {
            var service = new VideoService(new FakeFileReader());
            Assert.Fail("error");
        }
    }
}
