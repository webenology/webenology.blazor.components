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
        void WriteAllText(string fileLocation, string text);
        bool Exists(string fileLocation);
        byte[] ReadAllBytes(string fileLocation);
        void Delete(string fileLocation);
        string GetTempPath();
        void WriteAllBytes(string path, byte[] bytes);
    }
    internal class WFileWriter : IWFileWriter
    {
        public void WriteAllText(string fileLocation, string text)
        {
            File.WriteAllText(fileLocation, text);
        }

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

        public void WriteAllBytes(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }
    }
}
