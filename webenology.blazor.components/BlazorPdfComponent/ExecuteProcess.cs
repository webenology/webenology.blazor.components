using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.BlazorPdfComponent
{
    public interface IExecuteProcess
    {
        void Execute(string title, string tempFile);
    }
    internal class ExecuteProcess : IExecuteProcess
    {
        private readonly IWFileWriter _fileWriter;

        public ExecuteProcess(IWFileWriter fileWriter)
        {
            _fileWriter = fileWriter;
        }
        public void Execute(string title, string tempFile)
        {
            var path = $"{_fileWriter.GetTempPath()}wkhtmltopdf.exe";
            if (!_fileWriter.Exists(path))
            {
                var fileStream = Assembly.GetCallingAssembly().GetManifestResourceStream("webenology.blazor.components.BlazorPdfComponent.wkhtmltopdf.exe");
                var ms = new MemoryStream();
                fileStream?.CopyTo(ms);
                _fileWriter.WriteAllBytes(path, ms.ToArray());
            }

            var args = new string[] { "-s Letter", "--print-media-type", $"--title \"{title}\"", "--no-background", "--javascript-delay 500", $"\"{tempFile}.html\"", $"\"{tempFile}.pdf\"" };
#pragma warning disable CA1416 // Validate platform compatibility
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                FileName = path,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = string.Join(" ", args)
            };
            using Process exeProcess = Process.Start(startInfo);
            if (exeProcess != null) exeProcess.WaitForExit();
#pragma warning restore CA1416 // Validate platform compatibility
        }
    }
}
