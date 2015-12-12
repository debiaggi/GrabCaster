// --------------------------------------------------------------------------------------------------
// <copyright file = "PerformanceCounters.cs" company="Nino Crudele">
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