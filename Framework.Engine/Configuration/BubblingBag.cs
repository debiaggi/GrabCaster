using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Engine
{
    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Configuration;

    /// <summary>
    /// Contains the bubbling folder filse (trg, evn, and dlls)
    /// </summary>
    public class BubblingBag
    {
        public List<TriggerConfiguration> TriggerConfigurationList { get; set; }
        public List<EventConfiguration> EventConfigurationList { get; set; }
        public List<BubblingEvent> GlobalEventListBaseDll { get; set; }
        public ConfigurationStorage ConfigurationStorage { get; set; }

    }
}
