// -----------------------------------------------------------------------------------
// 
// GRABCASTER LTD CONFIDENTIAL
// ___________________________
// 
// Copyright © 2013 - 2016 GrabCaster Ltd. All rights reserved.
// This work is registered with the UK Copyright Service: Registration No:284701085
// 
// 
// NOTICE:  All information contained herein is, and remains
// the property of GrabCaster Ltd and its suppliers,
// if any.  The intellectual and technical concepts contained
// herein are proprietary to GrabCaster Ltd
// and its suppliers and may be covered by UK and Foreign Patents,
// patents in process, and are protected by trade secret or copyright law.
// Dissemination of this information or reproduction of this material
// is strictly forbidden unless prior written permission is obtained
// from GrabCaster Ltd.
// 
// -----------------------------------------------------------------------------------
namespace GrabCaster.Framework.Common
{
    using System.Xml;

    /// <summary>
    /// TODO The xml helpers.
    /// </summary>
    public static class XmlHelpers
    {
        /// <summary>
        /// The add attribute.
        /// </summary>
        /// <param name="xmlDocument">
        /// The xml document.
        /// </param>
        /// <param name="xmlElement">
        /// The xml element.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void AddAttribute(XmlDocument xmlDocument, XmlElement xmlElement, string name, string value)
        {
            var xmlAttribute = xmlDocument.CreateAttribute(name);
            xmlAttribute.Value = value;
            xmlElement.Attributes.Append(xmlAttribute);
        }
    }
}