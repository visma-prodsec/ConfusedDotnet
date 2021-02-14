using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.IO;

namespace ConfusedDotnet
{
    public class ProjectFileParser
    {
        private readonly ICakeContext _context;

        public ProjectFileParser(ICakeContext context)
        {
            _context = context;
        }
        
        public IEnumerable<PackageReference> ParseProjectFile(FilePath projectFilePath)
        {
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, XmlResolver = null };
            var reader = XmlReader.Create(projectFilePath.FullPath, settings);
            var doc = XDocument.Load(reader);
            
            var packageReferences = doc.XPathSelectElements("//PackageReference")
                .Select(GetPackageReferenceFromElement);

            return packageReferences;
        }

        private PackageReference GetPackageReferenceFromElement(XElement xElement)
        {
            if (xElement == null)
            {
                return default;
            }

            var includeAttributes = xElement.AttributesIgnoreCase("include").ToList();
            
            if (includeAttributes.Any() || !xElement.HasElements)
                return new PackageReference
                {
                    Include = includeAttributes.First().Value
                };
            
            var include = xElement.Descendants()
                .Where(x => x.Name.LocalName.Equals("include", StringComparison.Ordinal)).ToList();
            if (include.Any())
            {
                return new PackageReference
                {
                    Include = include.First().Value
                };
            }

            _context.Warning("Unable to determine the package id of this package reference: {0}", xElement.ToString());
            return default;
        }
    }
}