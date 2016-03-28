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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Dcp.Redis
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Log;

    using Microsoft.ServiceBus.Messaging;

    using StackExchange.Redis;

    [EventsDownStreamContract("{377B04BD-C80C-4AC5-BC70-C5CC571B2BDC}", "EventsDownStream", "Redis EventsDownStream")]
    public class EventsDownStream: IEventsDownstream
    {
        public void Run(SetEventOnRampMessageReceived setEventOnRampMessageReceived)
        {

            try
            {

                var myNewThread = new Thread(() => this.StartRedisListener(setEventOnRampMessageReceived));
                myNewThread.Start();
            }
            catch (Exception ex)
            {

                LogEngine.WriteLog(
                    Configuration.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.DefconOne,
                    Constant.TaskCategoriesEventHubs,
                    ex,
                    EventLogEntryType.Error);
            }
        }

        public void StartRedisListener(SetEventOnRampMessageReceived setEventOnRampMessageReceived)
        {

            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration.RedisConnectionString());

                ISubscriber sub = redis.GetSubscriber();

                sub.Subscribe("*", (channel, message) => {
                    byte[] byteArray = (byte[])message;
                    SkeletonMessage skeletonMessage = SkeletonMessage.DeserializeMessage(byteArray);
                    setEventOnRampMessageReceived(skeletonMessage);
                });
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {

                LogEngine.WriteLog(
                    Configuration.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.DefconOne,
                    Constant.TaskCategoriesEventHubs,
                    ex,
                    EventLogEntryType.Error);
            }
        }
    }
}
