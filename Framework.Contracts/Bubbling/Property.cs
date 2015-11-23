// --------------------------------------------------------------------------------------------------
// <copyright file = "Property.cs" company="Nino Crudele">
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