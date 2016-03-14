using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCasterUI
{
    using System.Reflection;
    using System.Windows.Forms;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Configuration;

    public enum GrabCasterComponentType
    {
        TriggerConfiguration,Event, EventConfiguration, TriggerComponent,EventComponent,Correlation,Root,
        TriggerConfigurationRoot, EventConfigurationRoot, TriggerComponentRoot, EventComponentRoot,none
    }

    public static class Global
    {
        public static DialogResult MessageBoxForm(string message, MessageBoxButtons messageBoxButtons, MessageBoxIcon messageBoxIcon)
        {
            return MessageBox.Show(message, "GrabCaster", messageBoxButtons, messageBoxIcon);
        }
    }
    public class TreeviewBag
    {
        public TreeviewBag(string File, GrabCasterComponentType GrabCasterComponentType, object component, object componentDetails, object DataBag, componentTrigger componentTrigger, componentEvent componentEvent)
        {
            this.File = File;
            this.GrabCasterComponentType = GrabCasterComponentType;
            this.Component = component;
            this.ComponentDetails = componentDetails;
            this.DataBag = DataBag;
            this.componentTrigger = componentTrigger;
            this.componentEvent = componentEvent;

            triggerConfigurationList = new List<TriggerConfiguration>() ?? null;
            eventConfigurationList= new List<EventConfiguration>() ?? null;
            componentTriggerList = new List<componentTrigger>() ?? null;
            componentEventList = new List<componentEvent>() ?? null;


        }

        public string File { get; set; }
        public GrabCasterComponentType GrabCasterComponentType { get; set; }
        public object Component { get; set; }
        public object ComponentDetails { get; set; }
        public object DataBag { get; set; }
        public componentTrigger componentTrigger { get; set; }
        public componentEvent componentEvent { get; set; }

        public List<TriggerConfiguration> triggerConfigurationList { get; set; }
        public List<EventConfiguration> eventConfigurationList { get; set; }
        public List<componentTrigger> componentTriggerList { get; set; }
        public List<componentEvent> componentEventList { get; set; }

    }

    public class componentTrigger
    {
        public componentTrigger(TriggerContract triggerContract, Type triggerClass,string file, Assembly assembly)
        {
            this.triggerContract = triggerContract;
            this.triggerClass = triggerClass;
            this.file = file;
            this.assembly = assembly;
        }
        public TriggerContract triggerContract { get; set; }
        public Type triggerClass { get; set; }
        public string file { get; set; }
        public Assembly assembly{ get; set; }

    }

    public class componentEvent
    {
        public componentEvent(EventContract eventContract, Type eventClass, string file, Assembly assembly)
        {
            this.eventContract = eventContract;
            this.eventClass = eventClass;
            this.file = file;
            this.assembly = assembly;
        }
        public EventContract eventContract { get; set; }
        public Type eventClass { get; set; }
        public string file { get; set; }
        public Assembly assembly { get; set; }
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
