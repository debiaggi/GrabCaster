// --------------------------------------------------------------------------------------------------
// <copyright file = "LockSlimQueueLog.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
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
namespace GrabCaster.Framework.Log
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using GrabCaster.Framework.Base;

    using Timer = System.Timers.Timer;

    public abstract class LockSlimQueueLog<T> : ConcurrentQueue<T>
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
        /// Initializes a new instance of the <see cref="LockSlimQueueLog{T}"/> class.
        /// </summary>
        protected LockSlimQueueLog()
        {
        }

        protected LockSlimQueueLog(int capLimit, int timeLimit)
        {
            this.Init(capLimit, timeLimit);
        }

        public event Action<List<T>> OnPublish = delegate { };

        public new virtual void Enqueue(T item)
        {
            base.Enqueue(item);
            if (this.Count >= this.CapLimit)
            {
                LogEngine.DebugWriteLine($"Log capture limit: {this.CapLimit} > Publish!");
                this.Publish();
            }
        }

        private void Init(int capLimit, int timeLimit)
        {
            this.CapLimit = capLimit;
            this.TimeLimit = timeLimit;
            this.Locker = new ReaderWriterLockSlim();
            this.InitTimer();
        }

        protected virtual void InitTimer()
        {
            this.InternalTimer = new Timer();
            this.InternalTimer.AutoReset = false;
            this.InternalTimer.Interval = this.TimeLimit * 1000;
            this.InternalTimer.Elapsed += (s, e) =>
                {
                    LogEngine.DebugWriteLine($"Log time limit: {this.TimeLimit} > Publish!");
                    this.Publish();
                };
            this.InternalTimer.Start();
        }

        /// <summary>
        /// The publish.
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
                            LogEngine.DebugWriteLine($"Log start dequeue {this.Count} items.");
                            T item;
                            while (this.TryDequeue(out item))
                            {
                                itemsToLog.Add(item);
                            }
                        }
                        catch (ThreadAbortException tex)
                        {
                            LogEngine.DebugWriteLine($"Warning-Dequeue items failed > {tex.Message}");

                            LogEngine.WriteLog(
                                Configuration.EngineName, 
                                $"Error in {MethodBase.GetCurrentMethod().Name}", 
                                Constant.ErrorEventIdHighCritical, 
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
                                Constant.ErrorEventIdHighCritical, 
                                Constant.TaskCategoriesError, 
                                ex, 
                                EventLogEntryType.Error);
                        }
                        finally
                        {
                            LogEngine.DebugWriteLine($"Log dequeued {itemsToLog.Count} items");
                            this.OnPublish(itemsToLog);
                            this.CompletePublishing();
                        }
                    });
            task.Start();
        }

        private bool IsPublishing()
        {
            return Interlocked.CompareExchange(ref this.OnPublishExecuted, 1, 0) > 0;
        }

        private void StartPublishing()
        {
            this.InternalTimer.Stop();
        }

        private void CompletePublishing()
        {
            this.InternalTimer.Start();
            Interlocked.Decrement(ref this.OnPublishExecuted);
        }
    }
}