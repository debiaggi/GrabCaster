using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Contracts.Syncronization
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Use to move data across the points
    /// </summary>
    [DataContract]
    [Serializable]
    public class GCPointBag
    {
        //General notation field
        public string Notes { get; set; }

        //General content field
        public byte[] content { get; set; }
    }
}
