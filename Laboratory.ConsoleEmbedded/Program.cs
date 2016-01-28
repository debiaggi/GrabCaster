// --------------------------------------------------------------------------------------------------
// <copyright file = "Program.cs" company="Nino Crudele">
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
            GrabCaster.Framework.Library.Embedded.RefreshBubblingSetting();
            GrabCaster.Framework.Library.Embedded.ExecuteTrigger(
                "{82208FAA-272E-48A7-BB5C-4EACDEA538D2}",
                "{306DE168-1CEF-4D29-B280-225B5D0D76FD}",
                null);
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
