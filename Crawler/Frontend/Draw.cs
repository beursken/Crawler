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
        int _x=0;
        int _y = 0;
        string _label = "";
        int _width = 0;
        int _height = 0;
    }


    class Draw
    {
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
        public static string _current="";
        public static Image _img = null;
        public static Image LoadImg(string name)
        {
            if (name == _current) return _img;
            _img = Image.FromFile(name);
            _current = name;
            return _img;
        }
    }


    class ViewObject
    {
        string _description;
        string _imageFile = "images/terrain_atlas.png";
        System.Drawing.Bitmap _pic = null;
        int _px = -1;
        int _py = -1;
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

        public void UpdateTile(int posX, int posY, string path)
        {
            _px = posX;
            _py = posY;
            _imageFile = path;
            _pic = null;

        }



        public System.Drawing.Image pic
        {
            get
            {
                if (_pic != null) return _pic;
                if (System.IO.File.Exists(_imageFile))
                {
                    _pic = new Bitmap(32, 32);
                    Graphics target = Graphics.FromImage(_pic);
                    target.DrawImage(ImageCache.LoadImg(_imageFile), new Rectangle(0, 0, 32, 32), new Rectangle(_px * 32, _py * 32, 32, 32), GraphicsUnit.Pixel);
                    return _pic;
                }
                return null;

            }
        }


        public void Load(XmlReader reader)
        {
            _px = XmlConvert.ToInt32(reader.GetAttribute("picX", ""));
            _py = XmlConvert.ToInt32(reader.GetAttribute("picY", ""));
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
            writer.WriteElementString("description", _description);
            writer.WriteElementString("imgFile", _imageFile);
            writer.WriteEndElement();
        }

    }

}
