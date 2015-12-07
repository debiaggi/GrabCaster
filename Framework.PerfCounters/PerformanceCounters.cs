// --------------------------------------------------------------------------------------------------
// <copyright file = "PerformanceCounters.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
//    Info:   http://GrabCaster.io
// 
//    By accessing GrabCaster code here, you are agreeing to the following licensing terms.
//    If you do not agree to these terms, do not access the GrabCaster code.
//    Your license to the GrabCaster source and/or binaries is governed by the 
//    Reciprocal Public License 1.5 (RPL1.5) license as described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//    
//    This work is registered with the UK Copyright Service.
//    Registration No:284695248  
//  </summary>
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