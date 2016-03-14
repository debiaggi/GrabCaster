// --------------------------------------------------------------------------------------------------
// <copyright file = "LockSlimQueueEngine.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Engine
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Log;

    using Timer = System.Timers.Timer;

    public abstract class LockSlimQueueEngine<T> : ConcurrentQueue<T>
        where T : class
    {
        /// <summary>
        /// The cap limit.
        /// </summary>
        protected int CapLimit;

        /// <summary>
        /// The locker.
        /// </summary>
        protected ReaderWriterLockSlim Locker;

        /// <summary>
        /// The on publish executed.
        /// </summary>
        protected int OnPublishExecuted;

        /// <summary>
        /// The time limit.
        /// </summary>
        protected int TimeLimit;

        /// <summary>
        /// The internal timer.
        /// </summary>
        protected Timer InternalTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="LockSlimQueueEngine{T}"/> class.
        /// </summary>
        protected LockSlimQueueEngine()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockSlimQueueEngine{T}"/> class.
        /// </summary>
        /// <param name="capLimit">
        /// TODO The cap limit.
        /// </param>
        /// <param name="timeLimit">
        /// TODO The time limit.
        /// </param>
        protected LockSlimQueueEngine(int capLimit, int timeLimit)
        {
            this.Init(capLimit, timeLimit);
        }

        /// <summary>
        /// TODO The on publish.
        /// </summary>
        public event Action<List<T>> OnPublish = delegate { };

        /// <summary>
        /// TODO The enqueue.
        /// </summary>
        /// <param name="item">
        /// TODO The item.
        /// </param>
        public new virtual void Enqueue(T item)
        {
            base.Enqueue(item);
            if (this.Count >= this.CapLimit)
            {
                LogEngine.DebugWriteLine($"Log capture limit: {this.CapLimit} > Publish!");
                this.Publish();
            }
        }

        /// <summary>
        /// TODO The init.
        /// </summary>
        /// <param name="capLimit">
        /// TODO The cap limit.
        /// </param>
        /// <param name="timeLimit">
        /// TODO The time limit.
        /// </param>
        private void Init(int capLimit, int timeLimit)
        {
            this.CapLimit = capLimit;
            this.TimeLimit = timeLimit;
            this.Locker = new ReaderWriterLockSlim();
            this.InitTimer();
        }

        /// <summary>
        /// TODO The init timer.
        /// </summary>
        protected virtual void InitTimer()
        {
            this.InternalTimer = new Timer { AutoReset = false, Interval = this.TimeLimit * 1000 };
            this.InternalTimer.Elapsed += (s, e) =>
                {
                    //LogEngine.DebugWriteLine($"Log time limit: {this.TimeLimit} > Publish!");
                    this.Publish();
                };
            this.InternalTimer.Start();
        }

        /// <summary>
        /// TODO The publish.
        /// </summary>
        protected virtual void Publish()
        {
            var task = new Task(
                () =>
                    {
                        var itemsToLog = new List<T>();
                        try
                        {
                            if (this.IsPublishing())
                            {
                                return;
                            }

                            this.StartPublishing();
                            //LogEngine.DebugWriteLine($"Log start dequeue {this.Count} items!");
                            T item;
                            while (this.TryDequeue(out item))
                            {
                                itemsToLog.Add(item);
                            }
                        }
                        catch (ThreadAbortException tex)
                        {
                            LogEngine.DebugWriteLine($"Dequeue items failed > {tex.Message}");

                            LogEngine.WriteLog(
                                Configuration.EngineName, 
                                $"Error in {MethodBase.GetCurrentMethod().Name}", 
                                Constant.DefconOne, 
                                Constant.TaskCategoriesError, 
                                tex, 
                                EventLogEntryType.Error);
                        }
                        catch (Exception ex)
                        {
                            LogEngine.DebugWriteLine($"Dequeue items failed > {ex.Message}");

                            LogEngine.WriteLog(
                                Configuration.EngineName, 
                                $"Error in {MethodBase.GetCurrentMethod().Name}", 
                                Constant.DefconOne, 
                                Constant.TaskCategoriesError, 
                                ex, 
                                EventLogEntryType.Error);
                        }
                        finally
                        {
                            //LogEngine.DebugWriteLine($"Log dequeued {itemsToLog.Count} items");
                            this.OnPublish(itemsToLog);
                            this.CompletePublishing();
                        }
                    });
            task.Start();
        }

        /// <summary>
        /// TODO The is publishing.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool IsPublishing()
        {
            return Interlocked.CompareExchange(ref this.OnPublishExecuted, 1, 0) > 0;
        }

        /// <summary>
        /// TODO The start publishing.
        /// </summary>
        private void StartPublishing()
        {
            this.InternalTimer.Stop();
        }

        /// <summary>
        /// TODO The complete publishing.
        /// </summary>
        private void CompletePublishing()
        {
            this.InternalTimer.Start();
            Interlocked.Decrement(ref this.OnPublishExecuted);
        }
    }
}