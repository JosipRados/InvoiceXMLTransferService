using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccess.Models
{
    [XmlRoot(ElementName = "ItemList")]
    public class ItemsWrapperModel
    {
        [XmlElement(ElementName = "Item")]
        public List<ItemModel> Item { get; set; } = new List<ItemModel>();
    }
}
