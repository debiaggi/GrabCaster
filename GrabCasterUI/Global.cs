using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCasterUI
{
    public enum GrabCasterComponentType
    {
        TriggerConfiguration,Event, EventConfiguration, TriggerComponent,EventComponent,Correlation,Root,
        TriggerConfigurationRoot, EventConfigurationRoot, TriggerComponentRoot, EventComponentRoot
    }

    class Global
    {

    }
    public class TreeviewBag
    {
        public TreeviewBag(string File, GrabCasterComponentType GrabCasterComponentType, object component, object componentDetails, object DataBag)
        {
            this.File = File;
            this.GrabCasterComponentType = GrabCasterComponentType;
            this.Component = component;
            this.ComponentDetails = componentDetails;
            this.DataBag = DataBag;

        }

        public string File { get; set; }
        public GrabCasterComponentType GrabCasterComponentType { get; set; }
        public object Component { get; set; }
        public object ComponentDetails { get; set; }
        public object DataBag { get; set; }
    }

    public class propertyConfiguration
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public propertyConfiguration(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;

        }

    }
}
