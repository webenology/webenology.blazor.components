using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.MailMerge;
internal static class OpenXmlHelper
{

    public static void ReplaceXml(this OpenXmlElement oldElement, OpenXmlElement newElement,
        bool insertInRun = true)
    {
        if (oldElement == null)
            return;

        var allFields = oldElement.GetFields();

        var isParentParagraph = allFields.First().Parent?.GetType() == typeof(Paragraph);
        if (newElement != null)
        {
            if (insertInRun || isParentParagraph)
            {
                var simpleR = new Run(new RunProperties());
                simpleR.AppendChild(newElement);
                allFields.Last()?.InsertAfterSelf(simpleR);
            }
            else
            {
                allFields.Last()?.InsertAfterSelf(newElement);
            }
        }

        allFields.ForEach(x =>
        {
            if (x != null) x.Remove();
        });
    }

    public static void ReplaceXml(this OpenXmlElement oldElement, List<OpenXmlElement> newElements,
        bool insertInRun = true)
    {
        if (oldElement == null)
            return;

        var allFields = oldElement.GetFields();

        var isParentParagraph = allFields.First().Parent?.GetType() == typeof(Paragraph);
        if (newElements.Any())
        {
            if (insertInRun || isParentParagraph)
            {
                var simpleR = new Run();
                simpleR.Append(newElements);
                allFields.Last()?.InsertAfterSelf(simpleR);
            }
            else
            {
                newElements.Reverse();
                foreach (var newElement in newElements)
                {
                    allFields.Last()?.InsertAfterSelf(newElement);
                }
            }
        }

        allFields.ForEach(x =>
        {
            if (x != null) x.Remove();
        });
    }

    public static void AddBefore(this OpenXmlElement oldElement, OpenXmlElement newElement,
        bool insertInRun = true)
    {
        if (oldElement == null)
            return;

        var allFields = oldElement.GetFields();

        var firstField = allFields.FirstOrDefault();
        var isParentParagraph = firstField?.Parent?.GetType() == typeof(Paragraph);
        if (newElement != null)
        {
            if (isParentParagraph)
            {
                firstField = firstField.Parent;
            }
            if (insertInRun)
            {
                var simpleR = new Run();
                simpleR.AppendChild(newElement);
                firstField?.InsertBeforeSelf(simpleR);
            }
            else
            {
                firstField?.InsertBeforeSelf(newElement);
            }
        }
    }


    private static List<OpenXmlElement> GetFields(this OpenXmlElement oldElement)
    {
        var simpleField = false;
        var allFields = new List<OpenXmlElement>();

        if (oldElement.GetType() == typeof(SimpleField))
        {
            allFields.Add(oldElement);
            simpleField = true;
        }

        if (!simpleField)
        {
            var first = (oldElement.Parent as Run).PreviousSibling<Run>();
            var second = first.NextSibling<Run>();
            var third = second.NextSibling<Run>();
            var fourth = third.NextSibling<Run>();
            var fifth = fourth.NextSibling<Run>();
            allFields.Add(first);
            allFields.Add(second);
            allFields.Add(third);
            allFields.Add(fourth);
            allFields.Add(fifth);
        }

        return allFields;
    }
}