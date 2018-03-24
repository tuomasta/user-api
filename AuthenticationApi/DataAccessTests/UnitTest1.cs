using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess;
using FluentAssertions;

namespace DataAccessTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void WHEN_calculating_SHA256_THEN_returns_correct_hash_string()
        {
            var hash = "foobar".ToSHA256Hash();
            hash.Should().Be("c3ab8ff13720e8ad9047dd39466b3c8974e592c2fa383d4a3960714caef0c4f2");

        }
    }
}
