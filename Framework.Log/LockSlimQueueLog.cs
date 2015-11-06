// --------------------------------------------------------------------------------------------------
// <copyright file = "LockSlimQueueLog.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Copyright (c) 2013 - 2015 Nino Crudele
//    Blog: http://ninocrudele.me
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License. 
// </summary>
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