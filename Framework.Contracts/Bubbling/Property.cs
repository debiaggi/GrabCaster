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
namespace GrabCaster.Framework.Contracts.Bubbling
{
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    ///     The Lower receive layer, this receive the raw data
    /// </summary>
    [DataContract]
    [Serializable]
    public class Property : IProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Property"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <param name="assemblyPropertyInfo">
        /// The assembly property info.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public Property(string name, string description, PropertyInfo assemblyPropertyInfo, Type type, object value)
        {
            this.Name = name;
            this.Description = description;
            this.AssemblyPropertyInfo = assemblyPropertyInfo;
            this.Type = type;
            this.Value = value;
        }

        /// <summary>
        /// Description of method
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Initialize the clone
        /// </summary>
        /// <returns></returns>
        /// <summary>
        ///     Property name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     PropertyInfo  to evaluate
        /// </summary>
        public PropertyInfo AssemblyPropertyInfo { get; set; }

        /// <summary>
        ///     Property Type
        /// </summary>
        [DataMember]
        public Type Type { get; set; }

        /// <summary>
        ///     Property Value
        /// </summary>
        [DataMember]
        public object Value { get; set; }
    }
}