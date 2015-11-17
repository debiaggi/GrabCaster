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
                    Constant.ErrorEventIdHighCriticalEventHubs,
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
                    Constant.ErrorEventIdHighCriticalEventHubs,
                    Constant.TaskCategoriesEventHubs,
                    ex,
                    EventLogEntryType.Error);
            }
        }
    }
}
