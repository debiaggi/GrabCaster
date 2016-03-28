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