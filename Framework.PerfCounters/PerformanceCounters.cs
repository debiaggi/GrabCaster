// --------------------------------------------------------------------------------------------------
// <copyright file = "PerformanceCounters.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.PerfCounters
{
    using System.Diagnostics;

    using GrabCaster.Framework.Base;

    /// <summary>
    /// The performance counters.
    /// </summary>
    internal class PerformanceCounters
    {
        /// <summary>
        ///     Counter for counting total number of operations
        /// </summary>
        private readonly PerformanceCounter totalOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCounters"/> class. 
        ///     Creates a new performance counter category "MyCategory" if it does not already exists and adds some counters to it.
        /// </summary>
        public PerformanceCounters()
        {
            if (PerformanceCounterCategory.Exists(Configuration.EngineName))
            {
                PerformanceCounterCategory.Delete(Configuration.EngineName);
            }

            var counters = new CounterCreationDataCollection();

            // Counter for counting totals: PerformanceCounterType.NumberOfItems32
            var totalOps = new CounterCreationData
                               {
                                   CounterName = "# operations executed", 
                                   CounterHelp = "Total number of operations executed", 
                                   CounterType = PerformanceCounterType.RateOfCountsPerSecond64
                               };
            counters.Add(totalOps);

            // create new category with the counters above
#pragma warning disable 618
            PerformanceCounterCategory.Create(Configuration.EngineName, "Sample category for Codeproject", counters);
#pragma warning restore 618

            // create counters to work with
            this.totalOperations = new PerformanceCounter();
            this.totalOperations.CategoryName = Configuration.EngineName;
            this.totalOperations.CounterName = "# operations executed";
            this.totalOperations.MachineName = ".";
            this.totalOperations.ReadOnly = false;
            this.totalOperations.RawValue = 0;
        }

        /// <summary>
        /// Increments counters.
        /// </summary>
        public void DoSomeProcessing()
        {
            // simply increment the counters
            this.totalOperations.Increment();
        }
    }
}