using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGB
{
    class ImageRepo
    {
        //Crop image to get tile from tiled image
        public static Image Crop(string img, int width, int height, int x, int y)
        {
            Image image = Image.FromFile(img);
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            bmp.SetResolution(80, 60);

            Graphics gfx = Graphics.FromImage(bmp);
            gfx.SmoothingMode = SmoothingMode.AntiAlias;
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
            gfx.DrawImage(image, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
            // Dispose to free up resources
            image.Dispose();
            bmp.Dispose();
            gfx.Dispose();

            return bmp;

        }

        //Method to merge multiple images into one image
        //Accepts a List of Bitmap library images
        public static Bitmap Merge(IEnumerable<Bitmap> images)
        {
            var enumerable = images as IList<Bitmap> ?? images.ToList();
            int newimagesize = 40;
            int newbitmapXsize = 800;
            int newbitmapYsize = 600;

            // Define list for resized images
            List<Bitmap> ImageArray = new List<Bitmap>();
            foreach (var image in enumerable)
            {
                //Resize images to fit onto tile in respect to square root values
                Bitmap ResizedImages = ResizeImage(image, newimagesize, newimagesize);
                //New image array of resized images
                ImageArray.Add(ResizedImages);
            }
           
            //initial x and y axis plot values for bitmap tiles
            int x = 0;
            int y = 0;
            //start image count at 0
            int imagecount = 0;
            //get square root of 400 for square creation of tiled image / mosaic
            int sqrt400 = Convert.ToInt32(Math.Sqrt(400));
            
            ////Define i and j for multidimensional array that will need to contain image count number and corresponding X and Y axis plot values
            //int i ,j ;
            ////2 dimensional array 400 rows (400 images) and 3 columns (col 1 = imagecountX , col 2 = X axis value , col 3 = Y axis value)
            //int[,] Tiles2ImageArray = new int[400, 3];

            // merge those images into one single image.
            var bitmap = new Bitmap(newbitmapXsize, newbitmapYsize);
            using (var g = Graphics.FromImage(bitmap))
            {
                //Iterate through the top 400 from the image array
                foreach (var image in ImageArray)
                {
                    g.DrawImage(image, x, y);
                    x = x + newimagesize;
                    if (imagecount % sqrt400 == 0 )
                    {
                        y = y + newimagesize;
                        x = 0;

                    }
                    imagecount++;

                    //    // Stored x+y axis values into the an array//
                    //    // set i == 0 and increment until i == 400
                    //    for (i = 0; i < 400; i++)
                    //    {

                    //        // j represents columns and j = coloumn 0 to start
                    //        for (j = 0; j < 1; j++)
                    //        {
                    //            //apply imagecount number to column 0
                    //            Tiles2ImageArray[i, j] = Convert.ToInt32(imagecount);
                    //            //j = column 1
                    //            for (j = 1; j < 2; j++)
                    //            {
                    //                //apply source X axis for corresponding image to column 1
                    //                Tiles2ImageArray[i, j] = Convert.ToInt32(x);

                    //                //j = column 2
                    //                for (j = 2; j < 3; j++)
                    //                {
                    //                    //apply source X axis for corresponding image to column 1
                    //                    Tiles2ImageArray[i, j] = Convert.ToInt32(y);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //    for (i = 0; i < 1; i++)
                    //    {
                    //        Console.Write("\n");
                    //        for (j = 0; j < 3; j++)

                    //            Console.Write("{0}\t", Tiles2ImageArray[i, j]);
                    //    }




                    //foreach (var tile in Tiles2ImageArray)
                    //{
                    //    //Generate Bitmap from each tile within large tiled image.
                    //    //Get AVG RGB Value from each independant tile.
                    //    Image newtile = Crop(@"C:\\img.png", 40, 40, x, y);
                    //    Bitmap newbmp = new Bitmap(newtile);
                    //    Console.WriteLine(getAverageColorSingleImage(newbmp));

                    //}
                }
            }


            return bitmap;
        }

        //Method used to calculate AVG RGB of input image
        public static Color getAverageColorSingleImage(Bitmap bmp)
        {

            //Used for tally
            int r = 0;
            int g = 0;
            int b = 0;

            int total = 0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color clr = bmp.GetPixel(x, y);

                    r += clr.R;
                    g += clr.G;
                    b += clr.B;

                    total++;
                }
            }

            //Calculate average
            r /= total;
            g /= total;
            b /= total;

            return Color.FromArgb(r, g, b);
        }
       
        /// Resize the image to the specified width and height.
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

    }
}
