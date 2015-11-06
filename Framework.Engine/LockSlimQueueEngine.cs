// --------------------------------------------------------------------------------------------------
// <copyright file = "LockSlimQueueEngine.cs" company="Nino Crudele">
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
                    LogEngine.DebugWriteLine($"Log time limit: {this.TimeLimit} > Publish!");
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
                            LogEngine.DebugWriteLine($"Log start dequeue {this.Count} items!");
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