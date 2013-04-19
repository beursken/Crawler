using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Crawler.Frontend;

namespace Crawler.Backend
{
    class Placeable : ViewObject
    {

        bool _canEnter = false;
        bool _canPickup = false;
        bool _doesWarp = false;
        Tile _parent = null;


        string _name = "";
        string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public void Load(XmlReader reader)
        {
            reader.ReadStartElement("Placeable");
            _canEnter = (reader.GetAttribute("canEnter", "").Trim() == "1");
            _doesWarp = (reader.GetAttribute("doesWarp", "").Trim() == "1");
            _canPickup = (reader.GetAttribute("canPickup", "").Trim() == "1");
            _name = reader.GetAttribute("name", "").Trim();
            base.Load(reader);
            reader.ReadEndElement();
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement("Placeable");
            writer.WriteAttributeString("canEnter", _canEnter ? "1" : "0");
            writer.WriteAttributeString("doesWarp", _doesWarp ? "1" : "0");
            writer.WriteAttributeString("canPickup", _canPickup ? "1" : "0");
            writer.WriteAttributeString("name", _name.ToString().Trim());
            base.Save(writer);
            writer.WriteEndElement();
        }

        public Crawler.Backend.Location CurrentLocation
        {
            get
            {
                return new Crawler.Backend.Location(_parent.x, _parent.y);
            }
        }
    }

    class Teleporter : Placeable
    {

    }

    class Chest : Placeable
    {

    }

    class Trap : Placeable
    {

    }

    class Door : Placeable
    {

    }

    class Pushable : Placeable
    {

    }

}
