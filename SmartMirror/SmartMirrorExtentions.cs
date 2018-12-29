using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror
{
    static class SmartMirrorExtentions
    {
        public static Image ToImage(this byte[] ImgBytes)
        {
            MemoryStream ms = new MemoryStream(ImgBytes);
            ms.Position = 0;
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public static Image<Gray, Byte> ToGray(this Image img)
        {
            Bitmap masterImage = (Bitmap)img;
            Image<Gray, Byte> normalizedMasterImage = new Image<Gray, Byte>(masterImage);
            return normalizedMasterImage;
        }
        public static Image<Gray, Byte> ToGray(this byte[] ImgBytes){
            MemoryStream ms = new MemoryStream(ImgBytes);
            ms.Position = 0;
            Bitmap bmp = new Bitmap(ms);
            Image<Gray, Byte> normalizedMasterImage = new Image<Gray, Byte>(bmp);
            return normalizedMasterImage;
        }
    }
}
