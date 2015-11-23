// --------------------------------------------------------------------------------------------------
// <copyright file = "EventsUpStream.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Dcp.Redis
{
    using System.Diagnostics;
    using System.Reflection;
    using System;
    using System.IO;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Log;

    using Microsoft.ServiceBus.Messaging;

    using StackExchange.Redis;

    [EventsUpStreamContract("{A51FA36B-7778-47A1-B6DF-5CEC4B8F36B1}", "EventUpStream", "Redis EventUpStream")]
    class EventsUpStream : IEventsUpStream
    {
        private ISubscriber subscriber;
        public bool CreateEventUpStream()
        {
            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration.RedisConnectionString());
                this.subscriber = redis.GetSubscriber();
                return true;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Configuration.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.ErrorEventIdHighCriticalEventHubs,
                    Constant.TaskCategoriesEventHubs,
                    ex,
                    EventLogEntryType.Error);
                return false;
            }

        }

        public void SendMessage(SkeletonMessage message)
        {
            byte[] byteArrayBytes = SkeletonMessage.SerializeMessage(message);
            this.subscriber.Publish("*", byteArrayBytes);
        }
    }
}
