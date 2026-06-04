using System.Linq;
using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Viewer.UnitTest
{
    [TestClass]
    public class FieldOrientationTest
    {
        private static ZplElementBase[] Analyze(string zpl)
        {
            IPrinterStorage printerStorage = new PrinterStorage();
            var analyzer = new ZplAnalyzer(printerStorage);
            var analyzeInfo = analyzer.Analyze(zpl);
            return analyzeInfo.LabelInfos.First().ZplElements;
        }

        [TestMethod]
        // Zebra firmware accepts the orientation letter in either case.
        [DataRow("R", FieldOrientation.Rotated90)]
        [DataRow("r", FieldOrientation.Rotated90)]
        [DataRow("N", FieldOrientation.Normal)]
        [DataRow("n", FieldOrientation.Normal)]
        [DataRow("I", FieldOrientation.Rotated180)]
        [DataRow("i", FieldOrientation.Rotated180)]
        [DataRow("B", FieldOrientation.Rotated270)]
        [DataRow("b", FieldOrientation.Rotated270)]
        public void FieldDefault_AppliesOrientation_CaseInsensitive(string orientation, FieldOrientation expected)
        {
            var elements = Analyze($"^XA^CF0,30^FW{orientation}^FO50,50^FDTest^FS^XZ");

            var textField = elements.OfType<ZplTextField>().Single();
            Assert.AreEqual(expected, textField.Font.FieldOrientation);
        }

        [TestMethod]
        [DataRow("R", FieldOrientation.Rotated90)]
        [DataRow("r", FieldOrientation.Rotated90)]
        [DataRow("N", FieldOrientation.Normal)]
        [DataRow("n", FieldOrientation.Normal)]
        public void Barcode_AppliesOrientation_CaseInsensitive(string orientation, FieldOrientation expected)
        {
            var elements = Analyze($"^XA^FO100,100^BY3^BC{orientation},230,N,N,N,A^FD123456^FS^XZ");

            var barcode = elements.OfType<ZplBarcode128>().Single();
            Assert.AreEqual(expected, barcode.FieldOrientation);
        }
    }
}
