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
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    ///     The Lower receive layer, this receive the raw data
    /// </summary>
    internal interface IAction
    {
        /// <summary>
        ///     Unique Action ID
        /// </summary>   
        string Id { get; set; }

        /// <summary>
        ///     Method name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Internal Method to invoke
        /// </summary>
        MethodInfo AssemblyMethodInfo { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        ///     Property Value
        /// </summary>
        object ReturnValue { get; set; }

        /// <summary>
        ///     Bubbling parameters
        /// </summary>
        List<Parameter> Parameters { get; set; }
    }
}