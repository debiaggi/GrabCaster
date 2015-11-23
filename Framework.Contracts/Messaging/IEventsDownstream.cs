// --------------------------------------------------------------------------------------------------
// <copyright file = "IEventsDownstream.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Contracts.Messaging
{
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// The EventsDownstream interface.
    /// </summary>
    public interface IEventsDownstream
    {
        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="setEventOnRampMessageReceived">
        /// The set event on ramp message received.
        /// </param>
        void Run(SetEventOnRampMessageReceived setEventOnRampMessageReceived);
    }
}
