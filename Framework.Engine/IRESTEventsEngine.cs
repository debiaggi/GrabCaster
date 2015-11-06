// --------------------------------------------------------------------------------------------------
// <copyright file = "IRESTEventsEngine.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Engine
{
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using GrabCaster.Framework.Base;

    /// <summary>
    /// The RestEventsEngine interface.
    /// </summary>
    [ServiceContract]
    public interface IRestEventsEngine
    {
        /// <summary>
        /// The sync send bubbling configuration.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string SyncSendBubblingConfiguration(string channelId, string pointId);

        /// <summary>
        /// The sync send file bubbling configuration.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="messageType">
        /// The message type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string SyncSendFileBubblingConfiguration(
            string channelId,
            string pointId,
            string fileName,
            Configuration.MessageDataProperty messageType);

        /// <summary>
        /// The sync send request bubbling configuration.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string SyncSendRequestBubblingConfiguration(string channelId, string pointId);

        /// <summary>
        /// The sync send component.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <param name="idComponent">
        /// The id component.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string SyncSendComponent(string channelId, string pointId, string idComponent);

        /// <summary>
        /// The sync send request configuration.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string SyncSendRequestConfiguration(string channelId, string pointId);

        /// <summary>
        /// The sync send request component.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <param name="idComponent">
        /// The id component.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string SyncSendRequestComponent(string channelId, string pointId, string idComponent);

        /// <summary>
        /// The configuration.
        /// </summary>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        Stream Configuration();

        /// <summary>
        /// The execute trigger.
        /// </summary>
        /// <param name="triggerId">
        /// The trigger id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string ExecuteTrigger(string triggerId);

        /// <summary>
        /// The refresh bubbling setting.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string RefreshBubblingSetting();
    }
}