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
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    ///     The Lower receive layer, this receive the raw data
    /// </summary>
    [DataContract]
    [Serializable]
    public class BaseAction : IAction
    {
        public BaseAction(string ID, string Name, string Description, MethodInfo AssemblyMethodInfo, string ReturnValue)
        {
            this.Id = ID;
            this.Name = Name;
            this.Description = Description;
            this.AssemblyMethodInfo = AssemblyMethodInfo;
            this.ReturnValue = ReturnValue;
        }

        /// <summary>
        ///     Unique Action ID
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        ///     Method name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Internal Method to invoke
        /// </summary>
        public MethodInfo AssemblyMethodInfo { get; set; }

        /// <summary>
        ///     Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Property Value
        /// </summary>
        [DataMember]
        public object ReturnValue { get; set; }

        /// <summary>
        ///     Bubbling parameters
        /// </summary>
        [DataMember]
        public List<Parameter> Parameters { get; set; }
    }
}