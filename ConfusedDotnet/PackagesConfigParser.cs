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
    public class PackagesConfigParser
    {
        private readonly ICakeContext _context;

        public PackagesConfigParser(ICakeContext context)
        {
            _context = context;
        }
        
        public IEnumerable<PackageReference> ParsePackagesConfig(FilePath projectFilePath)
        {
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, XmlResolver = null };
            var reader = XmlReader.Create(projectFilePath.FullPath, settings);
            var doc = XDocument.Load(reader);
            
            var packageReferences = doc.XPathSelectElements("//package")
                .Select(GetPackageIdFromElement);

            return packageReferences;
        }

        private PackageReference GetPackageIdFromElement(XElement xElement)
        {
            if (xElement == null)
            {
                return default;
            }

            var includeAttributes = xElement.AttributesIgnoreCase("id").ToList();
            
            if (includeAttributes.Any())
                return new PackageReference
                {
                    Include = includeAttributes.First().Value
                };

            _context.Warning("Unable to determine the package id of this package reference: {0}", xElement.ToString());
            return default;
        }
    }
}