using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace ImageService
{
    interface IImageModel
    {
        /// <summary>
        /// The Function Addes A file to the outputFolder
        /// </summary>
        /// <param name="path">path of image to move</param>
        /// <param name="result">Indication if the Addition Was Successful</param>
        /// <param name="type">Indication of type to logger</param>
        /// <returns>string - path if success, else error msg</returns>
        string AddFile(string path, out bool result,out MessageTypeEnum type);
    }
}
