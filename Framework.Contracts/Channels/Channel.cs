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