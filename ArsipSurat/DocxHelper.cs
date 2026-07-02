using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ArsipSurat
{
    public static class DocxHelper
    {
        private static readonly XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        /// <summary>
        /// Replace all occurrences of placeholder in the docx file (body + headers + footers).
        /// Handles placeholders split across multiple Run elements.
        /// Returns true if placeholder was found and replaced.
        /// </summary>
        public static bool ReplacePlaceholder(string docxPath, string placeholder, string replacement)
        {
            bool found = false;

            using (Package package = Package.Open(docxPath, FileMode.Open, FileAccess.ReadWrite))
            {
                // Process main document body
                PackagePart docPart = package.GetPart(new Uri("/word/document.xml", UriKind.Relative));
                found |= ReplaceInPart(docPart, placeholder, replacement);

                // Process headers and footers
                foreach (PackageRelationship rel in docPart.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/header"))
                {
                    try
                    {
                        PackagePart headerPart = package.GetPart(new Uri("/word/" + rel.TargetUri, UriKind.Relative));
                        found |= ReplaceInPart(headerPart, placeholder, replacement);
                    }
                    catch { }
                }

                foreach (PackageRelationship rel in docPart.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/footer"))
                {
                    try
                    {
                        PackagePart footerPart = package.GetPart(new Uri("/word/" + rel.TargetUri, UriKind.Relative));
                        found |= ReplaceInPart(footerPart, placeholder, replacement);
                    }
                    catch { }
                }
            }

            return found;
        }

        private static bool ReplaceInPart(PackagePart part, string placeholder, string replacement)
        {
            bool found = false;

            using (Stream stream = part.GetStream(FileMode.Open, FileAccess.ReadWrite))
            {
                XDocument doc = XDocument.Load(stream);

                // Simple single-Run replacement
                foreach (var run in doc.Descendants(w + "r").ToList())
                {
                    var textEl = run.Elements(w + "t").FirstOrDefault();
                    if (textEl != null && textEl.Value.Contains(placeholder))
                    {
                        textEl.Value = textEl.Value.Replace(placeholder, replacement);
                        textEl.Attribute(XNamespace.Xml + "space")?.Remove();
                        found = true;
                    }
                }

                // Handle placeholder split across multiple Runs within a paragraph
                if (!found)
                {
                    foreach (var para in doc.Descendants(w + "p").ToList())
                    {
                        var runs = para.Elements(w + "r").ToList();
                        if (runs.Count < 2) continue;

                        // Concatenate all run text to find placeholder
                        StringBuilder sb = new StringBuilder();
                        foreach (var r in runs)
                        {
                            var t = r.Elements(w + "t").FirstOrDefault();
                            sb.Append(t?.Value ?? "");
                        }

                        string fullText = sb.ToString();
                        if (!fullText.Contains(placeholder)) continue;

                        // Replace in concatenated text
                        string newText = fullText.Replace(placeholder, replacement);

                        // Clear all runs except first, put all text in first run
                        var firstText = runs[0].Elements(w + "t").FirstOrDefault();
                        if (firstText != null)
                        {
                            firstText.Value = newText;
                            firstText.Attribute(XNamespace.Xml + "space")?.Remove();
                        }

                        // Remove remaining runs' text content
                        for (int i = 1; i < runs.Count; i++)
                        {
                            var t = runs[i].Elements(w + "t").FirstOrDefault();
                            if (t != null) t.Value = "";
                        }

                        found = true;
                    }
                }

                stream.Seek(0, SeekOrigin.Begin);
                stream.SetLength(0);
                doc.Save(stream);
            }

            return found;
        }
    }
}
