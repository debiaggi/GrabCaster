using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Engine
{
    using System.Runtime.Serialization;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Configuration;

    /// <summary>
    /// Contains the bubbling folder filse (trg, evn, and dlls)
    /// </summary>
    [Serializable,DataContract]
    public class BubblingBag
    {
        [DataMember]
        public List<TriggerConfiguration> TriggerConfigurationList { get; set; }
        [DataMember]
        public List<EventConfiguration> EventConfigurationList { get; set; }
        [DataMember]
        public List<BubblingEvent> GlobalEventListBaseDll { get; set; }
        [DataMember]
        public ConfigurationStorage ConfigurationStorage { get; set; }

    }
}
