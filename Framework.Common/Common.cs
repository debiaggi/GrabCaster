// --------------------------------------------------------------------------------------------------
// <copyright file = "Common.cs" company="Nino Crudele">
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
//    
//    This work is registered with the UK Copyright Service.
//    Registration No:284695248  
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Common
{
    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Messaging;

    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// The communication diretion.
    /// </summary>
    public enum CommunicationDiretion
    {
        /// <summary>
        /// The off ramp.
        /// </summary>
        OffRamp,

        /// <summary>
        /// The on ramp.
        /// </summary>
        OnRamp
    }
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
        public static object adsdGetMessageContextPropertyValue(
            ISkeletonMessage skeletonMessage, 
            Configuration.MessageDataProperty messageDataProperty)
        {
            try
            {
                return skeletonMessage.Properties[messageDataProperty.ToString()];
            }
            catch
            {
                return null;
            }
        }
    }
}