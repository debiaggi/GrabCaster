﻿// --------------------------------------------------------------------------------------------------
// <copyright file = "BaseAction.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
//    Info:   http://grabcaster.io/
// 
//    Unless explicitly acquired and licensed from Licensor under another
//    license, the contents of this file are subject to the Reciprocal Public
//    License ("RPL") Version 1.5, or subsequent versions as allowed by the RPL,
//    and You may not copy or use this file in either source code or executable
//    form, except in compliance with the terms and conditions of the RPL.
//    
//    All software distributed under the RPL is provided strictly on an "AS
//    IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND
//    LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
//    LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//    PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the RPL for specific
//    language governing rights and limitations under the RPL. 
//    
//    The Reciprocal Public License 1.5 (RPL1.5) license is described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
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