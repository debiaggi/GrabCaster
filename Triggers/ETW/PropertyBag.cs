// --------------------------------------------------------------------------------------------------
// <copyright file = "PropertyBag.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.ETW
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The property bag.
    /// </summary>
    [Serializable]
    public sealed class PropertyBag : Dictionary<string, object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBag"/> class.
        /// </summary>
        public PropertyBag()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBag"/> class.
        /// </summary>
        /// <param name="capacity">
        /// The capacity.
        /// </param>
        public PropertyBag(int capacity)
            : base(capacity, StringComparer.Ordinal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBag"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        private PropertyBag(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}