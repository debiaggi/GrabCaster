// --------------------------------------------------------------------------------------------------
// <copyright file = "SerializationHelper.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Serialization.Xml
{
    using System;
    using System.Collections.Generic;

    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Channels;
    using GrabCaster.Framework.Contracts.Configuration;
    using GrabCaster.Framework.Contracts.Points;

    using Newtonsoft.Json;

    /// <summary>
    /// TODO The serialization helper.
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// TODO The crete json trigger configuration template.
        /// </summary>
        /// <param name="BubblingEvent">
        /// TODO The bubbling event.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string CreteJsonTriggerConfigurationTemplate(BubblingEvent BubblingEvent)
        {
            var eventCorrelationTemplate = new Event(
                "{Event component ID to execute if Correlation = true}", 
                "{Configuration ID to execute if Correlation = true}",
                "Event Name Sample",
                "Event Description Sample");
            try
            {
                var triggerConfiguration = new TriggerConfiguration();
                triggerConfiguration.Trigger = new Trigger(
                    BubblingEvent.IdComponent,
                    BubblingEvent.IdConfiguration,
                    BubblingEvent.Name, 
                    BubblingEvent.Description);
                triggerConfiguration.Trigger.TriggerProperties = new List<TriggerProperty>();
                foreach (var Property in BubblingEvent.Properties)
                {
                    if (Property.Name != "DataContext")
                    {
                        var triggerProperty = new TriggerProperty(Property.Name, "Value to set");
                        triggerConfiguration.Trigger.TriggerProperties.Add(triggerProperty);
                    }
                }

                triggerConfiguration.Events = new List<Event>();

                // 1*
                var eventTriggerTemplate = new Event(
                    "{Event component ID  Sample to execute}",
                    "{Configuration ID  Sample to execute}",
                    "Event Name Sample",
                    "Event Description Sample");
                eventTriggerTemplate.Channels = new List<Channel>();
                var points = new List<Point>();
                points.Add(new Point("Point ID Sample", "Point Name Sample", "Point Description Sample"));
                eventTriggerTemplate.Channels.Add(
                    new Channel("Channel ID Sample", "Channel Name Sample", "Channel Description Sample", points));

                eventCorrelationTemplate.Channels = new List<Channel>();
                eventCorrelationTemplate.Channels.Add(
                    new Channel("Channel ID Sample", "Channel Name Sample", "Channel Description Sample", points));

                triggerConfiguration.Events.Add(eventTriggerTemplate);

                var events = new List<Event>();
                events.Add(eventCorrelationTemplate);
                eventTriggerTemplate.Correlation = new Correlation("Correlation Name Sample", "C# script", events);

                var serializedMessage = JsonConvert.SerializeObject(
                    triggerConfiguration, 
                    Formatting.Indented, 
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                // string serializedMessage = JsonConvert.SerializeObject(triggerConfiguration);
                return serializedMessage;

                // return "<![CDATA[" + serializedMessage + "]]>";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// TODO The crete json event configuration template.
        /// </summary>
        /// <param name="BubblingEvent">
        /// TODO The bubbling event.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string CreteJsonEventConfigurationTemplate(BubblingEvent BubblingEvent)
        {
            try
            {
                var eventConfiguration = new EventConfiguration();
                eventConfiguration.Event = new Event(
                    BubblingEvent.IdComponent, 
                    "{Configuration ID to execute}", 
                    BubblingEvent.Name, 
                    BubblingEvent.Description);

                eventConfiguration.Event.EventProperties = new List<EventProperty>();
                foreach (var Property in BubblingEvent.Properties)
                {
                    if (Property.Name != "DataContext")
                    {
                        var eventProperty = new EventProperty(Property.Name, "Value to set");
                        eventConfiguration.Event.EventProperties.Add(eventProperty);
                    }
                }

                var eventCorrelationTemplate = new Event(
                    "{Event component ID to execute if Correlation = true}", 
                    "{Configuration ID to execute if Correlation = true}", 
                    "EventName", 
                    "EventDescription");
                eventCorrelationTemplate.Channels = new List<Channel>();
                var points = new List<Point>();
                points.Add(new Point("Point ID", "Point Name", "Point Description"));
                eventCorrelationTemplate.Channels.Add(
                    new Channel("Channel ID", "Channel Name", "Channel Description", points));

                var events = new List<Event>();
                events.Add(eventCorrelationTemplate);
                eventConfiguration.Event.Channels = new List<Channel>();
                eventConfiguration.Event.Channels.Add(
                    new Channel("Channel ID", "Channel Name", "Channel Description", points));

                eventConfiguration.Event.Correlation = new Correlation("Correlation Name", "C# script", events);

                var serializedMessage = JsonConvert.SerializeObject(
                    eventConfiguration, 
                    Formatting.Indented, 
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                return serializedMessage;

                // return "<![CDATA[" + serializedMessage + "]]>";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}