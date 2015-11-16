// --------------------------------------------------------------------------------------------------
// <copyright file = "Common.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Common
{
    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Messaging;

    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// The communication diretion.
    /// </summary>
    public enum CommunicationDiretion
    {
        /// <summary>
        /// The off ramp.
        /// </summary>
        OffRamp,

        /// <summary>
        /// The on ramp.
        /// </summary>
        OnRamp
    }
    /// <summary>
    /// The common.
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// The get message context property value.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        /// <param name="messageDataProperty">
        /// The message data property.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object adsdGetMessageContextPropertyValue(
            ISkeletonMessage skeletonMessage, 
            Configuration.MessageDataProperty messageDataProperty)
        {
            try
            {
                return skeletonMessage.Properties[messageDataProperty.ToString()];
            }
            catch
            {
                return null;
            }
        }
    }
}