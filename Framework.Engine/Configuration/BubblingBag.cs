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
    public class BubblingBagObjet
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
    [Serializable]
    public class BubblingBag
    {
        public byte[] contentBubblingFolder { get; set; }
    }
}
