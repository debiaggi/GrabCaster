// --------------------------------------------------------------------------------------------------
// <copyright file = "Channel.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Contracts.Channels
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using GrabCaster.Framework.Contracts.Points;

    /// <summary>
    /// The channel.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Channel : IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="channelName">
        /// The channel name.
        /// </param>
        /// <param name="channelDescription">
        /// The channel description.
        /// </param>
        /// <param name="points">
        /// The points.
        /// </param>
        public Channel(string channelId, string channelName, string channelDescription, List<Point> points)
        {
            this.ChannelId = channelId;
            this.ChannelName = channelName;
            this.ChannelDescription = channelDescription;
            this.Points = points;
        }

        /// <summary>
        /// Gets or sets the channel id.
        /// </summary>
        [DataMember]
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets the channel name.
        /// </summary>
        [DataMember]
        public string ChannelName { get; set; }

        /// <summary>
        /// Gets or sets the channel description.
        /// </summary>
        [DataMember]
        public string ChannelDescription { get; set; }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        [DataMember]
        public List<Point> Points { get; set; }
    }
}