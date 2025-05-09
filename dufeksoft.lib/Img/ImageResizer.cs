using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace dufeksoft.lib.Img
{
    public class ImageResizer
    {
        public void MakeJpgSmaller(string filePath, int maxWidth, int maxHeight)
        {
            string tmpPath = string.Format("{0}-tmp.jpg", filePath);
            using (Image src = new Bitmap(filePath))
            {
                if ((maxWidth > 0 && src.Width > maxWidth) || (maxHeight > 0 && src.Height > maxHeight))
                {
                    using (Image dst = ResizeImage(src, maxWidth, maxHeight))
                    {
                        // We're going to save it as JPG, so create the EncoderParameters to do that
                        var encoderParameters = new EncoderParameters(1);
                        encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 60L);
                        dst.Save(tmpPath, GetEncoder(ImageFormat.Jpeg), encoderParameters);
                    }
                }
                else
                {
                    // No conversion needed
                    return;
                }
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.Move(tmpPath, filePath);
        }

        public void ResizeAndSaveJpgImage(string srcPath, string dstPath, int maxWidth, int maxHeight)
        {
            string tmpPath = string.Format("{0}-tmp.jpg", dstPath);
            using (Image src = new Bitmap(srcPath))
            {
                using (Image dst = ResizeImage(src, maxWidth, maxHeight))
                {
                    // We're going to save it as JPG, so create the EncoderParameters to do that
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 60L);
                    dst.Save(tmpPath, GetEncoder(ImageFormat.Jpeg), encoderParameters);
                }
            }

            if (File.Exists(dstPath))
            {
                File.Delete(dstPath);
            }
            File.Move(tmpPath, dstPath);
        }

        public Image ResizeImage(Image sourceImage, int maxWidth, int maxHeight)
        {
            // Determine which ratio is greater, the width or height, and use
            // this to calculate the new width and height. Effectually constrains
            // the proportions of the resized image to the proportions of the original.
            double xRatio = maxWidth > 0 ? (double)sourceImage.Width / maxWidth : 0;
            double yRatio = maxHeight > 0 ? (double)sourceImage.Height / maxHeight : 0;
            double ratioToResizeImage = Math.Max(xRatio, yRatio);

            int newWidth = (int)Math.Floor(sourceImage.Width / ratioToResizeImage);
            int newHeight = (int)Math.Floor(sourceImage.Height / ratioToResizeImage);

            // Create new image canvas -- use maxWidth and maxHeight in this function call if you wish
            // to set the exact dimensions of the output image.
            Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);

            // Render the new image, using a graphic object
            using (Graphics newGraphic = Graphics.FromImage(newImage))
            {
                // Set the background color to be transparent (can change this to any color)
                newGraphic.Clear(Color.Transparent);

                // Set the method of scaling to use -- HighQualityBicubic is said to have the best quality
                newGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // Apply the transformation onto the new graphic
                Rectangle sourceDimensions = new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);
                Rectangle destinationDimensions = new Rectangle(0, 0, newWidth, newHeight);
                newGraphic.DrawImage(sourceImage, destinationDimensions, sourceDimensions, GraphicsUnit.Pixel);
            }

            // Image has been modified by all the references to it's related graphic above. Return changes.
            return newImage;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
