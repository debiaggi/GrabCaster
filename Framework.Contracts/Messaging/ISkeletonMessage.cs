using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Contracts.Messaging
{
    public interface ISkeletonMessage
    {
        IDictionary<string, object> Properties { get; set; }

        byte[] Body { get; set; }
    }
}
