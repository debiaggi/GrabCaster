// --------------------------------------------------------------------------------------------------
// <copyright file = "Point.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Contracts.Points
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The point.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Point : IPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public Point(string pointId, string name, string description)
        {
            this.PointId = pointId;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the point id.
        /// </summary>
        [DataMember]
        public string PointId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}