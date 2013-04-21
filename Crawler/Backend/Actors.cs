using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Crawler.Frontend;

namespace Crawler.Backend
{
    /// <summary>
    /// A player or computer controlled person
    /// </summary>
    class Actor : ViewObject
    {
        #region "Private Fields"        
        private bool _canEnter = false;
        private bool _doesWarp = false;
        private Tile _parent = null;
        private string _name = "";
        #endregion

        #region "Public Fields"
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public new System.Drawing.Point CurrentLocation
        {
            get
            {
                return new System.Drawing.Point(_parent.x, _parent.y);
            }
        }
        #endregion

        #region "Public Methods"
        public new void Load(XmlTextReader reader)
        {
            reader.ReadStartElement("Placeable");
            _canEnter = (reader.GetAttribute("canEnter", "").Trim() == "1");
            _doesWarp = (reader.GetAttribute("doesWarp", "").Trim() == "1");
            _name = reader.GetAttribute("name", "").Trim();
            base.Load(reader);
            reader.ReadEndElement();
        }

        /// <summary>
        /// Writ
        /// </summary>
        /// <param name="writer"></param>
        public new void Save(XmlTextWriter writer)
        {
            writer.WriteStartElement("Actor");
            writer.WriteAttributeString("canEnter", _canEnter ? "1" : "0");
            writer.WriteAttributeString("doesWarp", _doesWarp ? "1" : "0");
            writer.WriteAttributeString("name", _name.ToString().Trim());
            base.Save(writer);
            writer.WriteEndElement();
        }
        #endregion
    }

    /// <summary>
    /// An Actor controlled by a Player 
    /// </summary>
    class Player : Actor
    {

    }

    /// <summary>
    /// An Enemy controlled by the Computer
    /// </summary>
    class Enemy : Actor
    {

    }

    /// <summary>
    /// A Companion controlled by the Computer
    /// </summary>
    class Companion : Actor
    {

    }

    /// <summary>
    /// An NPC controlled by the Computer
    /// </summary>
    class NPC : Actor
    {

    }
}
