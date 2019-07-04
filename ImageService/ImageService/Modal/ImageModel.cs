using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Configuration;
using Infrastructure;

namespace ImageService
{
    /// <summary>
    /// Image Model - logic of program
    /// </summary>
    class ImageModel : IImageModel
    {
        #region Members
        private string m_OutputFolder;          // The Output Folder
        private int m_thumbnailSize;            // The Size Of The Thumbnail Size
        private string m_ThumFolder;            //Thumbnails path Folder    
        private string m_defualtFolder;         //folder of undated images
        private string m_thumbDefaultFolder;    //folder of undated images (thumnnails)
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public ImageModel()
        {
            m_OutputFolder = ConfigurationManager.AppSettings.Get("OutputDir");
            m_thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));
            m_ThumFolder = Path.Combine(m_OutputFolder, "Thumbnail");
            m_defualtFolder = m_OutputFolder + "\\default";

            CreateOutputDir(); //create output dir (m_OutputFolder) if not exist.

        }
        /// <summary>
        /// The Function Addes A file to the outputFolder
        /// </summary>
        /// <param name="path">path of image to move</param>
        /// <param name="result">Indication if the Addition Was Successful</param>
        /// <param name="type">Indication of type to logger</param>
        /// <returns>string - path if success, else error msg</returns>
        public string AddFile(string path, out bool result,out MessageTypeEnum type)
        {
            string newPath; //path of the new file
            result = true;
            string realPath;
            try
            {
                DateTime imageCreate = GetDateTakenFromImage(path); //takes the original creation date ("TAKEN DATE") of the image
                string year = imageCreate.Year.ToString();
                string month = imageCreate.Month.ToString();
                
                //new pats is string of curren folder
                newPath = m_OutputFolder + "\\" + year + "\\" + month;
                //create year folder and year\month in outputDIR (if not exist)
                Directory.CreateDirectory(path: Path.Combine(m_OutputFolder, year));
                Directory.CreateDirectory(path: Path.Combine(m_OutputFolder, year, month));
                //create year folder and year\month in ThumbDIR (if not exist)
                Directory.CreateDirectory(path: Path.Combine(m_ThumFolder, year));
                string thumb_path = Path.Combine(m_ThumFolder, year, month);
                Directory.CreateDirectory(path: thumb_path);
                
                //realPath - will be the path after checking if there is image in this name already
                if (NamingToNewImage(path, newPath, out realPath))
                {
                    type = MessageTypeEnum.INFO;
                }
                else
                {
                    type = MessageTypeEnum.WARNING;
                }
                //move the image to new path (dated images)
                File.Move(path, realPath);

                //create thumnail (for dated images)
                CreateThumb(realPath, thumb_path);
            }
            
            catch (ArgumentException)
            {//if there is no InfoProperty of taken date - move to default folder
                type = MessageTypeEnum.WARNING;
                string dstImage = Path.Combine(m_defualtFolder, Path.GetFileName(path));

                NamingToNewImage(path, m_defualtFolder,out dstImage);
                
                File.Move(path, dstImage);
                
                CreateThumb(dstImage, m_thumbDefaultFolder);
                return "No \"TAKEN DATE\"\nadd " + Path.GetFileName(path) + " to default folder";
            }
            
            
            catch (Exception e)
            {
                result = false;
                type = MessageTypeEnum.FAIL;

                return e.ToString();
            }
            if(type == MessageTypeEnum.INFO)
            {
                return "AddedFile " + Path.GetFileName(path) + " to:\n" + realPath;
            }
            else
            {
                return "AlreadyExist - name changed AddFile " + Path.GetFileName(path) + " to:\n" + realPath;
            }

        }
        /// <summary>
        /// change the file name to not exist filename e,g:oren.txt ->oren(1).txt
        /// </summary>
        /// <param name="imagePath">path of image</param>
        /// <param name="folder">move folder</param>
        /// <param name="realPath"> the path + the new filename</param>
        /// <returns>true if the path was good, fale if change the name</returns>
        private bool NamingToNewImage(string imagePath, string folder, out string realPath)
        {
            int counter = 0;    //counter for add "(counter)" to file if already exist
            
            string file_name = Path.GetFileName(imagePath);     //file name e.g : oren.txt
            string file_name_without = Path.GetFileNameWithoutExtension(imagePath); //file without extenetion e.g: oren
            string extention = Path.GetExtension(imagePath);    //file extention e.g: .txt (with the DOT!)
            realPath = Path.Combine(folder, file_name);     //comabin- e.g:from - .../output/2018/4 and oren.txt, to- .../output/2018/4/oren.txt 
            
            //if exsist increment counter and try again
            while (File.Exists(realPath))
            {
                counter++;
                realPath = Path.Combine(folder, file_name_without + "(" + counter.ToString() + ")" + extention);
            }
            return (counter == 0);
        }
        /// <summary>
        /// creates the output directory, thumbnails and all the defaults
        /// </summary>
        private void CreateOutputDir()
        {
            //create output directory
            DirectoryInfo di_output = Directory.CreateDirectory(path: m_OutputFolder);
            //make the directory hidden
            di_output.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            //create thumbnail directory
            DirectoryInfo di_thum = Directory.CreateDirectory(path: m_ThumFolder);

            //create default in outputFolder - for all images without date
            Directory.CreateDirectory(path: m_OutputFolder + "\\default");

            //create default folder in ThumbFolder - for all images without date and convert to thumb
           m_thumbDefaultFolder = Path.Combine(m_ThumFolder, "default");
            Directory.CreateDirectory(path: m_thumbDefaultFolder);
        }

        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        // Google it - dont forget, google is your best friend
        private static Regex r = new Regex(":");

        public static DateTime GetDateTakenFromImage(string path)
        {
            
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                    PropertyItem propItem = myImage.GetPropertyItem(36867);
           
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken);

            }
        }
        /// <summary>
        /// create thumb image (with same extention), and put it in relevent folder
        /// </summary>
        /// <param name="imagePath">image</param>
        /// <param name="dstFolder">relevemt folder</param>
        private void CreateThumb(string imagePath, string dstFolder)
        {
            Image image = Image.FromFile(imagePath);
            Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
            thumb.Save(Path.Combine(dstFolder, Path.GetFileName(path: imagePath)));
            image.Dispose();
            thumb.Dispose();
        }
    }
}
