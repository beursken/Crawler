using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Crawler.Frontend
{
    /// <summary>
    /// A base class for all objects displayable on a game map
    /// </summary>
    class ViewObject
    {
        #region "Private Fields"
        /// <summary>
        /// A description to be displayed if object is examined
        /// </summary>
        string _description;
        /// <summary>
        /// The source file used for the tile (usually containing multiple tiles)
        /// </summary>
        string _imageFile = "images/terrain_atlas.png";
        /// <summary>
        /// The horizontal start location of the 32x32-tile in _imagefile
        /// </summary>
        int _px = -1;
        /// <summary>
        /// The vertical start location of the 32x32-tile in _imagefile 
        /// </summary>
        int _py = -1;
        /// <summary>
        /// The cached bitmap used to display the tile
        /// </summary>
        System.Drawing.Bitmap _pic = null;

        /// <summary>
        /// Number of horizontal frames next to each other available for animation (0 if static)
        /// </summary>
        int _hphase = 0;
        /// <summary>
        /// Number of vertical frames next to each other available for animation (0 if static)
        /// </summary>
        int _vphase = 0;

        /// <summary>
        /// Current phase of the animation (stays at 0 if static)
        /// </summary>
        int _currentPhase = 0;

        /// <summary>
        /// Current phase available as cached tile in _pic
        /// </summary>
        int _cachedPhase = 0;
        #endregion

        #region "Public Fields"

        /// <summary>
        /// Horizontal position of tile in image file (read only)
        /// </summary>
        public int PX
        {
            get { return _px; }
        }

        /// <summary>
        /// Vertical position of tile in image file (read only)
        /// </summary>
        public int PY
        {
            get { return _py; }
        }

        /// <summary>
        /// Current animation phase (read/write)
        /// </summary>
        public int Phase
        {
            get
            {
                return _currentPhase;
            }
            set
            {

                if (value <= (_vphase * _hphase))
                {
                    _currentPhase = value;
                }
                else
                {
                    _currentPhase = 0;
                }
            }
        }

        /// <summary>
        /// Whether the object is animated (read only)
        /// </summary>
        public bool Animated
        {
            get
            {
                return (_vphase * _hphase > 0);
            }
        }
        #endregion




        public System.Drawing.Image pic
        {
            get
            {

                if ((_pic != null) && (_cachedPhase == _currentPhase)) return _pic;
                if (_imageFile == "") return null;
                _pic = new Bitmap(32, 32);
                Graphics target = Graphics.FromImage(_pic);
                target.DrawImage(ImageCache.LoadImg(_imageFile), new Rectangle(0, 0, 32, 32), new Rectangle(_px * 32, _py * 32, 32, 32), GraphicsUnit.Pixel);
                return _pic;
            }
        }

        public System.Drawing.Point CurrentLocation
        {
            get
            {
                return new System.Drawing.Point(-1, -1);
            }
        }

        #region "Public Methods"

        /// <summary>
        /// Draw the current object
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Draw(System.Drawing.Graphics target, int x, int y, int zoom = 32)
        {
            if (pic != null)
            {
                target.DrawImage(pic, x, y, zoom, zoom);
            }
        }

        public void incPhase()
        {
            if (_currentPhase <= (_vphase * _hphase))
            {
                _currentPhase += 1;
            }
            else
            {
                _currentPhase = 0;
            }
        }

        public void UpdateTile(int posX = -2, int posY = -2, string path = "#")
        {
            if (posX != -2) _px = posX;
            if (posY != -2) _py = posY;
            if (path != "#") _imageFile = path;
            _pic = null;

        }

        public void Load(XmlTextReader reader)
        {
            _px = XmlConvert.ToInt32(reader.GetAttribute("picX", ""));
            _py = XmlConvert.ToInt32(reader.GetAttribute("picY", ""));
            try
            {
                _hphase = XmlConvert.ToInt32(reader.GetAttribute("hPhase", ""));
                _vphase = XmlConvert.ToInt32(reader.GetAttribute("vPhase", ""));
            }
            catch { }
            reader.Read();
            _description = reader.ReadElementContentAsString("description", "").Trim();
            _imageFile = reader.ReadElementContentAsString("imgFile", "").Trim();
            reader.ReadEndElement();

        }

        public void Save(XmlTextWriter writer)
        {
            writer.WriteStartElement("img");
            writer.WriteAttributeString("picX", _px.ToString());
            writer.WriteAttributeString("picY", _py.ToString());
            writer.WriteAttributeString("hPhase", _hphase.ToString());
            writer.WriteAttributeString("vPhase", _vphase.ToString());


            writer.WriteElementString("description", _description);
            writer.WriteElementString("imgFile", _imageFile);
            writer.WriteEndElement();
        }
        #endregion

    }

}