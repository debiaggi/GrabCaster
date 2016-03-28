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
        /// <param name="configurationId"></param>
        /// <param name="value"></param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string ExecuteTrigger(string configurationId, string triggerId, string value);

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