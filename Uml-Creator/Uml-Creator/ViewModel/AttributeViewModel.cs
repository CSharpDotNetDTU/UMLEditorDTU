using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Uml_Creator.Model;
using Uml_Creator.Model.ENUM;
using Uml_Creator.Model.Interfaces;

namespace Uml_Creator.ViewModel
{
    public class AttributeViewModel : ISerializable, IClassAttribute
    {
        public ClassAttribute Attribute { get; }
        public string Name { get; set; }
        public EVisibility Visibility { get; set; }
        public string Type { get; set; }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ((ISerializable)Attribute).GetObjectData(info, context);
        }
    }
}
