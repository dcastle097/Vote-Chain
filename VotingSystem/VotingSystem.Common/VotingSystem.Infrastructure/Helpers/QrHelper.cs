using System;
using QRCoder;

namespace VotingSystem.Infrastructure.Helpers
{
    /// <summary>QR Code interactions helper.</summary>
    public class QrHelper
    {
        /// <summary>
        ///     Convert a string into a base64 encoded image string.
        /// </summary>
        /// <param name="content">String to be converted to a QR Code</param>
        /// <returns>A QR code .png image encoded as a base64 string</returns>
        public static string Base64QrFromString(string content)
        {
            return Convert.ToBase64String(
                new PngByteQRCode(new QRCodeGenerator().CreateQrCode(content, QRCodeGenerator.ECCLevel.Q))
                    .GetGraphic(10));
        }
    }
}