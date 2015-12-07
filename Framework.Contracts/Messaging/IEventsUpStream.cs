// --------------------------------------------------------------------------------------------------
// <copyright file = "IEventsUpStream.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
//    Info:   http://GrabCaster.io
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
namespace GrabCaster.Framework.Contracts.Messaging
{
    /// <summary>
    /// The EventsUpStream interface.
    /// </summary>
    public interface IEventsUpStream
    {
        /// <summary>
        /// The create event up stream.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool CreateEventUpStream();

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void SendMessage(SkeletonMessage message);
    }
}
