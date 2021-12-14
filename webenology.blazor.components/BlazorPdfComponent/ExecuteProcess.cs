using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        public void Execute(string title, string tempFile)
        {
            var htmlToPdfLocation = $"{Directory.GetParent(Assembly.GetCallingAssembly().Location).FullName}\\blazorpdfcomponent\\wkhtmltopdf.exe";

            var args = new string[] { "-s Letter", "--print-media-type", $"--title \"{title}\"", "--no-background", "--javascript-delay 500", $"\"{tempFile}.html\"", $"\"{tempFile}.pdf\"" };
#pragma warning disable CA1416 // Validate platform compatibility
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                FileName = htmlToPdfLocation,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = string.Join(" ", args)
            };
            using Process exeProcess = Process.Start(startInfo);
            if (exeProcess != null) exeProcess.WaitForExit();
#pragma warning restore CA1416 // Validate platform compatibility
        }
    }
}
