using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Web.Joblanes.Helper
{
    public static class ImageHelper
    {
        public static double MinimumSizeKB = 0;
        public static double MaximumSizeKB = 100;

        private static double _showSize;
        private const double Kb = 1024;

        public static double CalculateSize(double size)
        {
            try
            {
                if (size < Kb)
                {
                    _showSize = size;
                }
                else if (size.Equals(Kb))
                {
                    _showSize = size / Kb;
                }
                else
                {
                    _showSize = size / Kb;
                }

            }
            catch (Exception)
            {
                return 0;
            }
            return _showSize;
        }

        public static string CheckFileSettingsForTypeAndSize(string name, string contentType, int size)
        {
            try
            {
                if (name == null || contentType == null || size < 1)
                    return "This is not valid file. Check the file extension.";

                if (contentType.ToUpper() == "JPG" || contentType.ToUpper() == "JPEG" || contentType.ToUpper() == "IMAGE/JPEG")
                {
                    string message = "";
                    var imageSizeKB = CalculateSize(Convert.ToDouble(size));

                    if (imageSizeKB < MinimumSizeKB)
                        return "The image size is too small.";

                    if (imageSizeKB > MaximumSizeKB)
                        return "The image size is too large. Max valid size is up to " + MaximumSizeKB + "KB";

                    return message;
                }

                return "This file type is not valid. Only valid file type is image with *.jpg or *jpeg extension";
            }
            catch (Exception ex)
            {
                return "This file type is not valid. Only valid file type is image with *.jpg or *jpeg extension";
            }
        }
        public static string ComputeHash(byte[] file)
        {
            MD5 md5 = MD5.Create();

            byte[] hashAraay = md5.ComputeHash(file);

            var builder = new StringBuilder();

            foreach (byte b in hashAraay)
            {
                builder.AppendFormat("{0:x2}", b);
            }

            return builder.ToString();
        }

        public static byte[] ConvertFileInByteArray(Stream inputStream, int contentLength)
        {
            try
            {
                byte[] file = null;

                using (var binaryReader = new BinaryReader(inputStream))
                {
                    file = binaryReader.ReadBytes(contentLength);
                }

                return file;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                throw;
            }
        }
    }
}