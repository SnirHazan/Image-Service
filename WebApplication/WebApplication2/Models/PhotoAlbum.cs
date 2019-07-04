using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class PhotoAlbum
    {
        private static Configuration config;
        private string outputDir;
        public PhotoAlbum()
        {
            config = Configuration.GetInstance;
            outputDir = config.OutputDir;
            List<Photo> photos = InitializedAlbum();
            PhotosAlbum = photos;
        }

        private List<Photo> InitializedAlbum()
        {
            List<Photo> temp = new List<Photo>();
            if (outputDir != "" && (Directory.Exists(outputDir)))
            {
                string str1 = Path.Combine(outputDir, "Thumbnail");
                string[] subDirs = Directory.GetDirectories(Path.Combine(outputDir, "Thumbnail"));

                foreach (string yearDir in subDirs)
                {
                    string year = Path.GetFileName(yearDir);

                    if (year != "default")
                    {
                        string str2 = Path.Combine(outputDir, "Thumbnail", yearDir);
                        string[] subSubDirs = Directory.GetDirectories(Path.Combine(outputDir, "Thumbnail", yearDir));

                        foreach (string monthDir in subSubDirs)
                        {
                            string str3 = Path.Combine(outputDir, "Thumbnail", yearDir, monthDir);
                            string month = Path.GetFileName(monthDir);
                            string[] filesName = Directory.GetFiles(Path.Combine(outputDir, "Thumbnail", yearDir, monthDir));

                            foreach (string file in filesName)
                            {
                                temp.Add(new Photo(file, year, month));
                                
                            }
                        }

                    } else
                    {
                        string[] filesName = Directory.GetFiles(yearDir);
                        foreach (string file in filesName)
                        {
                            temp.Add(new Photo(file, "No Data", "No Data"));
                        }
                    }
                    


                }
            }


            return temp;
        }

        [Required]
        [Display(Name = "PhotosAlbum")]
        public List<Photo> PhotosAlbum { get; set; }



        public class Photo
        {
            public Photo(string path, string year, string month)
            {
                ImageName = Path.GetFileName(path);
                Year = year;
                Month = month;
                ImagePath = path.Replace("\\Thumbnail", "");
                ThumbnailPath = path;
                int imageLocation = path.IndexOf("Image");
                RelativeThumbnailPath = Path.Combine("~",path.Substring(imageLocation, path.Length - imageLocation));
                RelativeImagePath = RelativeThumbnailPath.Replace("\\Thumbnail", "");
            }

            [Required]
            [Display(Name = "ImageName")]
            public string ImageName { get; set; }

            [Required]
            [Display(Name = "Year")]
            public string Year { get; set; }

            [Required]
            [Display(Name = "Month")]
            public string Month { get; set; }

            [Required]
            [Display(Name = "ImagePath")]
            public string ImagePath { get; set; }

            [Required]
            [Display(Name = "ThumbnailPath")]
            public string ThumbnailPath { get; set; }

            [Required]
            [Display(Name = "RelativeImagePath")]
            public string RelativeImagePath { get; set; }

            [Required]
            [Display(Name = "RelativeThumbnailPath")]
            public string RelativeThumbnailPath { get; set; }
        }
    }
}