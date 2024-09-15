using GameVault.Common.Interfaces.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameVault.Common.Helpers
{
    public class ImageHandler : IImageHandler
    {
        public string SaveImageFromBase64(string imageSrc, string path)
        {
            // Create file name
            var fileName = $@"{DateTime.Now.Ticks}-{Guid.NewGuid()}.png";
            
            // Create file path
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), path, fileName);

            // Remove base64 prefix
            Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
            var imageBase64 = regex.Replace(imageSrc, string.Empty);

            // Create directory if not exists
            CreateDirectoryIfNotExist(Path.Combine(Directory.GetCurrentDirectory(), path));
            
            // Store the image
            File.WriteAllBytes(filePath, Convert.FromBase64String(imageBase64));

            return fileName;
        }

        public string GetImageBase64(string path, string imageName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), path, imageName);

            if (!CheckIfImagePathExist(filePath))
                return string.Empty;

            byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
            return Convert.ToBase64String(imageArray);
        }

        private void CreateDirectoryIfNotExist(string path)
        {
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
        }

        private bool CheckIfImagePathExist(string path)
        {
            return File.Exists(path);
        }
    }
}
