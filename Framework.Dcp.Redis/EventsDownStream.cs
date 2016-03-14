// --------------------------------------------------------------------------------------------------
// <copyright file = "EventsDownStream.cs" company="Nino Crudele">
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
