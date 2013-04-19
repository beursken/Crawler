using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml;

namespace Crawler.Frontend
{
    class Button
    {
        bool _Highlight;
        int _x = 0;
        int _y = 0;
        string _label = "";
        int _width = 0;
        int _height = 0;
    }

    enum Direction
    {
        top = 0,
        left = 1,
        right = 2,
        bottom = 3
    }
    class Draw
    {
        private static int _phase = 0;

        public static void Animate(Direction direction, int x, int y, Graphics target)
        {
            Bitmap _pic = new Bitmap(32, 32);
            Graphics temp = Graphics.FromImage(_pic);
            target.DrawImage(ImageCache.LoadImg("player.png"), new Rectangle(0, 0, 32, 32), new Rectangle(_phase * 32, (int)direction * 32, 32, 32), GraphicsUnit.Pixel);
            target.DrawImage(_pic, x, y);

            _phase += 1;
            if (_phase > 2) _phase = 0;
        }

        public static void DrawMap(Backend.Map map, Graphics target)
        {
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    map.GetTile(x, y).Draw(target, x * 32, y * 32);
                    /* foreach (Backend.Placeable placeable in map.GetTile(x, y).placeables)
                     {
                         placeable.Draw(target, x * 32, y * 32);
                     }
                     foreach (Backend.Actor actor in map.GetTile(x, y).actors)
                     {
                         actor.Draw(target, x * 32, y * 32);
                     } */
                }
            }
        }
    }



    public class ImageCache
    {
        class CacheElem
        {
            public string name;
            public Image img;
            public CacheElem(string _name, Image _img)
            {
                name = _name;
                img = _img;
            }
        }

        static List<CacheElem> _img = null;

        public static Image LoadImg(string name)
        {
            name = name.ToLower().Trim();
            if (_img == null) { _img = new List<CacheElem>(); };
            foreach (CacheElem i in _img)
            {
                if (i.name == name) return i.img;
            }
            _img.Add(new CacheElem(name, Image.FromFile(name)));
            return _img.Last().img;
        }
        public static void EmptyCache()
        {
            foreach (CacheElem i in _img)
            {
                i.img.Dispose();
            }
            _img.Clear();
        }
    }


    class ViewObject
    {
        string _description;
        string _imageFile = "images/terrain_atlas.png";
        System.Drawing.Bitmap _pic = null;
        int _px = -1;
        int _py = -1;
        int _hphase = 0;
        int _vphase = 0;
        int _currentPhase = 0;
        int _cachedPhase = 0;
        /// <summary>
        /// Draw the current object
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Draw(System.Drawing.Graphics target, int x, int y)
        {
            if (pic != null)
            {
                target.DrawImage(pic, x, y);
            }
        }

        public int PX
        {
            get { return _px; }
        }

        public int PY
        {
            get { return _py; }
        }

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

        public bool Animated
        {
            get
            {
                return (_vphase * _hphase > 0);
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

        public Crawler.Backend.Location CurrentLocation
        {
            get
            {
                return new Crawler.Backend.Location(-1, -1);
            }
        }

        public void Load(XmlReader reader)
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

        public void Save(XmlWriter writer)
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

    }

}
