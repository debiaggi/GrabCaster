// --------------------------------------------------------------------------------------------------
// <copyright file = "LogMessage.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog: http://ninocrudele.me
//    
//    By accessing GrabCaster code here, you are agreeing to the following licensing terms.
//    If you do not agree to these terms, do not access the GrabCaster code.
//    Your license to the GrabCaster source and/or binaries is governed by the 
//    Reciprocal Public License 1.5 (RPL1.5) license as described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Common
{
    using GrabCaster.Framework.Serialization;

    /// <summary>
    /// The object helper.
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// The clone object.
        /// </summary>
        /// <param name="clonedObject">
        /// The cloned object.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object CloneObject(object clonedObject)
        {
            var objserialized = SerializationEngine.ObjectToByteArray(clonedObject);
            var newobject = SerializationEngine.ByteArrayToObject(objserialized);
            return newobject;
        }
    }
}