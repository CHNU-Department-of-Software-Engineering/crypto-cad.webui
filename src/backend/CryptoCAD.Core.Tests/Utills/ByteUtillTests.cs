using NUnit.Framework;
using CryptoCAD.Core.Utilities;

namespace CryptoCAD.Core.Tests.Utills
{
    [TestFixture]
    class ByteUtillTests
    {

        [TestCase("Hello World!")]
        [TestCase("Some generic text for testing")]
        public void DataConverting(string text)
        {
            var byteText = ConvertUtill.FromString(text, ConvertMode.UTF8);

            var ulongArray = ByteUtill.SplitDataToBlocks64b(byteText);
            var byteArray = ByteUtill.JoinBlocks64bToData(ulongArray);

            var convertedText = ConvertUtill.ToString(byteArray, ConvertMode.UTF8);
            convertedText = convertedText.Trim('\0');
            Assert.AreEqual(text, convertedText);
        }
    }
}