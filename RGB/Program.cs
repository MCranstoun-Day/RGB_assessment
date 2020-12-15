using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGB
{
    class Program
    {

        static void Main(string[] args)
        {
            //Input file or folder path
            Console.WriteLine("\nPlease provide file or folder path? ");
            var path = Console.ReadLine();
            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(@"" + path + "");
            //detect whether its a directory or file
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                Console.Write("Its a directory");
                var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
                var files = GetRepo.GetFilesFrom(path, filters, false);
                List<Bitmap> bitmaps = new List<Bitmap>();
                int runningimagecount = 0;
                int imagecount = 400;
                int rows = imagecount;
                int columns = 4;
                
                //Build array of imagecount number and AVG RGB values before resize (400,4)
                int[,] OriginalRGBArray = new int[400, 4];
                int i, j;

                foreach (var x in files.Take(imagecount))
                {

                    //Building up list of Bitmaps to  merge
                    Image img = Image.FromFile(@"" + x + "");
                    Bitmap bmp = new Bitmap(img);
                    bitmaps.Add(bmp);
                    img.Dispose();

                    //Get AVG RGB value for each bitmap generated before resized to tiled image
                    var y = ImageRepo.getAverageColorSingleImage(bmp);
                    //Console.Write("\n - " + y);

                    //WAS IN THE PROCESS OF DEFINING 2D ARRAY FOR FURTHER CALCUTIONS AND COMPARING ARRAY DATA LATER.

                ////    Stored rgb values into the an array//
                ////    set i == 0 and increment until i == 400
                ////    for (i = 0; i < imagecount; i++)
                ////    {
                ////        j represents columns and j = coloumn 0 to start
                ////        for (j = 0; j < 1; j++)
                ////        {
                ////            apply imagecount number to column 0
                ////              OriginalRGBArray[i, j] = Convert.ToInt32(runningimagecount);
                ////            for (j = 1; j < 2; j++)
                ////            {
                ////                OriginalRGBArray[i, j] = Convert.ToInt32(y.R);
                ////                for (j = 2; j < 3; j++)
                ////                {
                ////                    OriginalRGBArray[i, j] = Convert.ToInt32(y.G);
                ////                    for (j = 3; j < 4; j++)
                ////                    {
                ////                        OriginalRGBArray[i, j] = Convert.ToInt32(y.B);
                ////                    }

                ////                }
                ////            }
                ////        }
                ////    }
                ////    runningimagecount++;
                ////}
                ////for (i = 0; i < imagecount; i++)
                ////{
                ////    Console.Write("\n");
                ////    for (j = 0; j < columns; j++)

                ////        Console.Write("{0}\t", OriginalRGBArray[i, j]);


                }
                //Merge all files found in folder to single tiled image
                Bitmap OutputImage = ImageRepo.Merge(bitmaps);
                //Save location
                OutputImage.Save("C:\\img.png");

                Console.Write("\nPress any key to exit...");
                Console.ReadKey(true);
            }

            else
            {

                Console.Write("Its a file");
                Image img = Image.FromFile(@"" + path + "");
                Bitmap bmp = new Bitmap(img);

                var y = ImageRepo.getAverageColorSingleImage(bmp);
                Console.Write(y);

            }

            Console.Write("\nPress any key to exit...");
            Console.ReadKey(true);

        }
       
    }
}
 