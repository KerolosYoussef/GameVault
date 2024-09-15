using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameVault.Common.Interfaces.Helpers
{
    public interface IImageHandler
    {
        public string SaveImageFromBase64(string imageSrc, string path);
        public string GetImageBase64(string path, string imageName);
    }
}
