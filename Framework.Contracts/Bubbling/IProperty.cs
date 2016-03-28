﻿// -----------------------------------------------------------------------------------
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

    /// <summary>
    ///     The Lower receive layer, this receive the raw data
    /// </summary>
    internal interface IProperty
    {
        /// <summary>
        ///     Property name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     PropertyInfo  to evaluate
        /// </summary>
        PropertyInfo AssemblyPropertyInfo { get; set; }

        /// <summary>
        ///     Property Type
        /// </summary>
        Type Type { get; set; }

        /// <summary>
        ///     Property Value
        /// </summary>
        object Value { get; set; }
    }
}