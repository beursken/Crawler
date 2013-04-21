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
    /// A generic Placeable-class from which all other non AI-objects derive
    /// </summary>
    class Placeable : ViewObject
    {
        #region "Private Fields"

        private bool _canEnter = false;
        private bool _canPickup = false;
        private bool _doesWarp = false;
        private Tile _parent = null;
        private string _name = "";

        #endregion


        #region "Public Properties"
        /// <summary>
        /// Displayable Name of Placeable
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Current Location (X/Y), read only
        /// </summary>
        public new System.Drawing.Point CurrentLocation
        {
            get
            {
                return new System.Drawing.Point(_parent.x, _parent.y);
            }
        }
        #endregion


        #region "Public Methods"

        /// <summary>
        /// Get Placeable from XML-file (assuming it is the next element)
        /// </summary>
        /// <param name="reader">An open XML-Textreader pointed at the "Placeable"-Tag</param>
        public new void Load(XmlTextReader reader)
        {
            _canEnter = (reader.GetAttribute("canEnter", "").Trim() == "1");
            _doesWarp = (reader.GetAttribute("doesWarp", "").Trim() == "1");
            _canPickup = (reader.GetAttribute("canPickup", "").Trim() == "1");
            _name = reader.GetAttribute("name", "").Trim();
            reader.Read();
            base.Load(reader);
        }

        /// <summary>
        /// Write Placeable to XML-file
        /// </summary>
        /// <param name="writer">An open XML-Textwriter pointed at the place to insert new "Placeable"-Tag</param>
        public new void Save(XmlTextWriter writer)
        {
            writer.WriteStartElement("Placeable");
            writer.WriteAttributeString("canEnter", _canEnter ? "1" : "0");
            writer.WriteAttributeString("doesWarp", _doesWarp ? "1" : "0");
            writer.WriteAttributeString("canPickup", _canPickup ? "1" : "0");
            writer.WriteAttributeString("name", _name.ToString().Trim());
            base.Save(writer);
            writer.WriteEndElement();
        }

        #endregion

        public Placeable(int x, int y, string filename)
        {
            UpdateTile(x, y, filename);
        }
    }

    /// <summary>
    /// A Placeable moving anyone entering it to another (specified) tile on the same or another map 
    /// </summary>
    class Teleporter : Placeable
    {
        public Teleporter(int x, int y, string filename)
            : base(x, y, filename)
        {
            UpdateTile(x, y, filename);
        }
    }

    /// <summary>
    /// A Placeable containing other objects
    /// </summary>
    class Chest : Placeable
    {
        public Chest(int x, int y, string filename)
            : base(x, y, filename)
        {
            UpdateTile(x, y, filename);
        }
    }

    /// <summary>
    /// A Placeable which damages anyone entering the tile
    /// </summary>
    class Trap : Placeable
    {
        public Trap(int x, int y, string filename)
            : base(x, y, filename)
        {
            UpdateTile(x, y, filename);
        }
    }

    /// <summary>
    /// A Placeable which may be opened and closed (blocking access to areas beyond)
    /// </summary>
    class Door : Placeable
    {
        public Door(int x, int y, string filename)
            : base(x, y, filename)
        {
            UpdateTile(x, y, filename);
        }
    }

    /// <summary>
    /// A Placeable which may be pushed/pulled into open squares
    /// </summary>
    class Pushable : Placeable
    {
        public Pushable(int x, int y, string filename):base(x, y, filename)
        {
            
        }
    }

}
