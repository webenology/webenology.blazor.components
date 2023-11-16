using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.BlazorPdfComponent
{
    public interface IWFileWriter
    {
        bool Exists(string fileLocation);
        byte[] ReadAllBytes(string fileLocation);
        void Delete(string fileLocation);
        string GetTempPath();
    }
    internal class WFileWriter : IWFileWriter
    {
        public bool Exists(string fileLocation)
        {
            return File.Exists(fileLocation);
        }

        public byte[] ReadAllBytes(string fileLocation)
        {
            return File.ReadAllBytes(fileLocation);
        }

        public void Delete(string fileLocation)
        {
            File.Delete(fileLocation);
        }

        public string GetTempPath()
        {
            return Path.GetTempPath();
        }

    }
}
