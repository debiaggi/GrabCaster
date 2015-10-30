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
    using GrabCaster.Framework.Base;

    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// The common.
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// The get message context property value.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        /// <param name="messageDataProperty">
        /// The message data property.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object GetMessageContextPropertyValue(
            EventData eventData, 
            Configuration.MessageDataProperty messageDataProperty)
        {
            try
            {
                return eventData.Properties[messageDataProperty.ToString()];
            }
            catch
            {
                return null;
            }
        }
    }
}