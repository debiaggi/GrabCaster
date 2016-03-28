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

namespace GrabCaster.Laboratory.ConsoleEmbedded
{
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    using System.Threading;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Library;
    class Program
    {
        /// <summary>
        /// The set event action event embedded.
        /// </summary>
        private static Embedded.SetEventActionEventEmbedded setEventActionEventEmbedded;


        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        static void Main(string[] args)
        {
            setEventActionEventEmbedded = EventReceivedFromEmbedded;
            Embedded.setEventActionEventEmbedded = setEventActionEventEmbedded;
            Console.WriteLine("Start GrabCaster Embedded Library");
            Thread t = new Thread(start);
            t.Start();

          
            if (args.Count() > 0)
            {
                while (!GrabCaster.Framework.Library.Embedded.engineLoaded)
                {
                    ;
                }
                byte[] content = Encoding.UTF8.GetBytes("Test content string");

                GrabCaster.Framework.Library.Embedded.ExecuteTrigger(
                    "{82208FAA-272E-48A7-BB5C-4EACDEA538D2}",
                    "{306DE168-1CEF-4D29-B280-225B5D0D76FD}",
                    content);
            }
        }

        static void start()
        {
            GrabCaster.Framework.Library.Embedded.StartEngine();
        }
        /// <summary>
        /// The event received from embedded.
        /// </summary>
        /// <param name="eventType">
        /// The event type.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        private static void EventReceivedFromEmbedded(IEventType eventType, EventActionContext context)
        {

            string stringValue = Encoding.UTF8.GetString(eventType.DataContext);
            Console.WriteLine("---------------EVENT RECEIVED FROM EMBEDDED LIBRARY---------------");
            Console.WriteLine(stringValue);
 

        }
    }
}
