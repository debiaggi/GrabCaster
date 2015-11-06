// --------------------------------------------------------------------------------------------------
// <copyright file = "IEvent.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Contracts.Events
{
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// Interface for all event action classes.
    /// </summary>
    public interface IEventType
    {
        /// <summary>
        /// Gets or sets the internal context passed to the event (some other event to execute) to use in delegates events.
        /// </summary>
        /// <returns>The internal context passed to the event.</returns>
        EventActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the internal delegate to use in delegates events.
        /// </summary>
        /// <value>
        /// The set event action event.
        /// </value>
        SetEventActionEvent SetEventActionEvent { get; set; }

        /// <summary>
        /// Gets or sets the main default data.
        /// </summary>
        /// <value>
        /// The main default data.
        /// </value>
        byte[] DataContext { get; set; }

        /// <summary>
        /// Performs the execution of the event.
        /// </summary>
        /// <param name="setEventActionEvent">The The internal delegate to use</param>
        /// <param name="context">The internal context passed to the event.</param>
        void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context);
    }
}