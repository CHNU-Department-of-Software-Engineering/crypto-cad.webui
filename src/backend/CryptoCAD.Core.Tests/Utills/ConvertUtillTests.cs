using NUnit.Framework;
using CryptoCAD.Core.Utilities;

namespace CryptoCAD.Core.Tests.Utills
{
    [TestFixture]
    public class ConvertUtillTests
    {
        [TestCase("Some generic text for testing!", ConvertMode.ASCII)]
        [TestCase("U29tZSBnZW5lcmljIHRleHQgZm9yIHRlc3Rpbmch", ConvertMode.BASE64)]
        [TestCase("Some generic text for testing!", ConvertMode.UTF8)]
        public void ConvertTextToFrom(string text, ConvertMode mode)
        {
            var bytes = ConvertUtill.FromString(text, mode);
            Assert.AreEqual(text, ConvertUtill.ToString(bytes, mode));
        }
    }
}