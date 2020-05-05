using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Scorelink.web.Controllers
{
    public class DigitizeController : Controller
    {
        // GET: Digitize
        public ActionResult Index()
        {
            // Construct a bitmap from the button image resource.
            Bitmap bmp1 = new Bitmap("C:\\Tanasit\\MFEC\\Score Link\\SRC\\Scorelink\\Scorelink.web\\Temp\\Copy.tiff");

            // Save the image as a JPEG.
            bmp1.Save("C:\\Tanasit\\MFEC\\Score Link\\SRC\\Scorelink\\Scorelink.web\\Temp\\Temp\\Test_Copy.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            return View();
        }
        public ActionResult Index2()
        {
            return View("Index2");
        }

        public ActionResult Index3()
        {
            return View("Test");
        }

        public JsonResult SaveArea(List<String> values, string fileName)
        {
            String sFrom = "C:\\Tanasit\\MFEC\\Score Link\\SRC\\Scorelink\\Scorelink.web\\Temp\\"+fileName;
            for (int i = 0; i < values.Count; i++) {
                int iRunNo = i + 1;
                String sSave = "C:\\Tanasit\\MFEC\\Score Link\\SRC\\Scorelink\\Scorelink.web\\Temp\\Temp\\Test"+fileName+"_"+ iRunNo + ".jpg";
                String sCrop = values[i];
                String[] aArea = sCrop.Split('|');
                int x, y, w, h;

                //Check Convert String Before Crop Image.
                if(Int32.TryParse(aArea[1], out x) && Int32.TryParse(aArea[2], out y) && Int32.TryParse(aArea[3], out w) && Int32.TryParse(aArea[4], out h))
                {
                    cropImage(Image.FromFile(sFrom), new Rectangle(x, y, w, h)).Save(sSave, ImageFormat.Jpeg);
                }
            }
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }

        public bool SaveCroppedImage(Image image, int maxWidth, int maxHeight, string filePath)
        {
            ImageCodecInfo jpgInfo = ImageCodecInfo.GetImageEncoders()
                                     .Where(codecInfo =>
                                     codecInfo.MimeType == "image/jpeg").First();
            Image finalImage = image;
            System.Drawing.Bitmap bitmap = null;
            try
            {
                int left = 0;
                int top = 0;
                int srcWidth = maxWidth;
                int srcHeight = maxHeight;
                bitmap = new System.Drawing.Bitmap(maxWidth, maxHeight);
                double croppedHeightToWidth = (double)maxHeight / maxWidth;
                double croppedWidthToHeight = (double)maxWidth / maxHeight;

                if (image.Width > image.Height)
                {
                    srcWidth = (int)(Math.Round(image.Height * croppedWidthToHeight));
                    if (srcWidth < image.Width)
                    {
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                    else
                    {
                        srcHeight = (int)Math.Round(image.Height * ((double)image.Width / srcWidth));
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                }
                else
                {
                    srcHeight = (int)(Math.Round(image.Width * croppedHeightToWidth));
                    if (srcHeight < image.Height)
                    {
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                    else
                    {
                        srcWidth = (int)Math.Round(image.Width * ((double)image.Height / srcHeight));
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                }
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    new Rectangle(left, top, srcWidth, srcHeight), GraphicsUnit.Pixel);
                }
                finalImage = bitmap;
            }
            catch { }
            try
            {
                using (EncoderParameters encParams = new EncoderParameters(1))
                {
                    encParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)100);
                    //quality should be in the range 
                    //[0..100] .. 100 for max, 0 for min (0 best compression)
                    finalImage.Save(filePath, jpgInfo, encParams);
                    return true;
                }
            }
            catch { }
            if (bitmap != null)
            {
                bitmap.Dispose();
            }
            return false;
        }

    }

}