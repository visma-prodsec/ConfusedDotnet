using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ConfusedDotnet
{
    public static class CaseInsensitiveExtensionMethods  
    {  
        public static IEnumerable<XElement> ElementsIgnoreCase(this XContainer container, XName name)   
        {  
            foreach (var element in container.Elements())  
            {  
                if (element.Name.NamespaceName == name.NamespaceName &&  
                    string.Equals(element.Name.LocalName, name.LocalName, StringComparison.OrdinalIgnoreCase))  
                {  
                    yield return element;  
                }  
            }  
        }  
 
        public static IEnumerable<XAttribute> AttributesIgnoreCase(this XElement element, XName name)  
        {  
            foreach (var attr in element.Attributes())  
            {  
                if (attr.Name.NamespaceName == name.NamespaceName &&  
                    string.Equals(attr.Name.LocalName, name.LocalName, StringComparison.OrdinalIgnoreCase))  
                {  
                    yield return attr;  
                }  
            }  
        }  
    }
}