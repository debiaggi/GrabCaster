// --------------------------------------------------------------------------------------------------
// <copyright file = "IAction.cs" company="Nino Crudele">
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