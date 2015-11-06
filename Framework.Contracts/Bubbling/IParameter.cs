// --------------------------------------------------------------------------------------------------
// <copyright file = "IParameter.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Copyright (c) 2013 - 2015 Nino Crudele
//    Blog: http://ninocrudele.me
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License. 
// </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Contracts.Bubbling
{
    using System;

    /// <summary>
    ///     The Lower receive layer, this receive the raw data
    /// </summary>
    internal interface IParameter
    {
        /// <summary>
        ///     Property name
        /// </summary>
        string Name { get; set; }

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