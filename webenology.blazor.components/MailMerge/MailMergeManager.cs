using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using AngleSharp.Diffing.Extensions;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using Spire.Pdf;
using FileFormat = Spire.Doc.FileFormat;
using Path = DocumentFormat.OpenXml.Drawing.Path;

namespace webenology.blazor.components.MailMerge;

public interface IMailMergeManager
{
    string Merge(Stream fileStream, object obj, bool returnAsPdf);
}

public class MailMergeManager : IMailMergeManager
{
    public string Merge(Stream fileStream, object obj, bool returnAsPdf)
    {
        using var fileStreamCopied = new MemoryStream();
        fileStream.CopyTo(fileStreamCopied);
        using var wp = WordprocessingDocument.Open(fileStreamCopied, true, new OpenSettings { AutoSave = true });
        var mergeFields = wp.GetMergeFields();
        foreach (var field in mergeFields)
        {
            var fieldName = string.Empty;
            if (field.GetType() == typeof(FieldCode))
                fieldName = ((FieldCode)field).Text.ScrubText();
            else if (field.GetType() == typeof(SimpleField))
                fieldName = ((SimpleField)field).Instruction?.Value.ScrubText();

            var foundText = FindValueFromObject(obj, fieldName);

            ReplaceWithText(field, foundText);
        }

        var fileName = Guid.NewGuid().ToString("N").Substring(0, 6);
        var tempFile = $"{System.IO.Path.GetTempPath()}{fileName}.docx";
        try
        {
            using var ms = new MemoryStream();
            wp.MainDocumentPart.Document.Save();
            if (returnAsPdf)
            {
                wp.Clone(ms);
                File.WriteAllBytes(tempFile, ms.ToArray());

                var doc = new Spire.Doc.Document();
                doc.LoadFromFile(tempFile, FileFormat.Docx);
                using var pdfMs = new MemoryStream();
                doc.SaveToStream(pdfMs, FileFormat.PDF);
                return Convert.ToBase64String(pdfMs.ToArray());
            }
            else
            {
                wp.MainDocumentPart.Document.Save(ms);
            }

            return Encoding.UTF8.GetString(ms.ToArray());

        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }

    }

    private static string FindValueFromObject(object obj, string fieldName)
    {
        var prop = obj.GetType().GetProperty(fieldName);
        if (prop != null)
        {
            return (string)prop.GetValue(obj);
        }

        return string.Empty;
    }

    private static void ReplaceWithText(OpenXmlElement field, string replacementText)
    {
        var text = new Text(replacementText);
        field.ReplaceXml(text);
    }

}
