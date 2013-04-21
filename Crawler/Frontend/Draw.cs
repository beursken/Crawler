using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml;
using Crawler.Server;

namespace Crawler.Frontend
{


    enum Direction
    {
        top = 0,
        left = 1,
        right = 2,
        bottom = 3
    }

    class Draw
    {
        #region "Private Fields"
        private int _x = 0, _y = 0;
        private Dispatcher _opener = null;
        private int _px = 0, _py = 0;
        /// <summary>
        /// A reference to a map
        /// </summary>

        private string _path = "images/terrain_atlas.png";
        #endregion


        #region "Static Fields"
        private static int _phase = 0;
        #endregion

        public void placeImage(Graphics target, Image image, Rectangle area)
        {
            target.DrawImage(image, area);

        }


        public void Refresh(Graphics target, Rectangle area)
        {
            foreach (Frontend.Button button in _opener.buttons)
            {
                button.Draw(target);
            };
            //Image i = Frontend.ImageCache.LoadImg(_path);


            // Bitmap bmp = new Bitmap(_opener.map.Width * 32, _opener.map.Height * 32);
            // Graphics temp = Graphics.FromImage(bmp);
            _opener.mapview.DrawMap(_opener.map, target);
            //target.DrawImage(bmp, new Rectangle(10, 50, _opener.Zoom * _opener.map.Width, _opener.Zoom * _opener.map.Height), new Rectangle(0, 0, _opener.map.Width * 32, _opener.map.Height * 32), GraphicsUnit.Pixel);
            //  target.DrawImage(i, new Rectangle(10 + _px * _opener.Zoom, 50 + _py * _opener.Zoom, _opener.Zoom, _opener.Zoom), new Rectangle(_x * 32, _y * 32, 31, 31), GraphicsUnit.Pixel);


            _opener.toolbox.Draw(target);

            if ((_opener.attachedImage != null)&&(_opener.attachedImagePos.X>-1))
            {
                placeImage(target, _opener.attachedImage, _opener.attachedImagePos);
            }
        }

        #region "Constructor"
        /// <summary>
        /// Create a new draw hooked to a Windows.Forms object and (optionally) a map
        /// </summary>
        /// <param name="opener">The Form to which Redraw events are passed</param>
        /// <param name="currentMap">(optional) map to use</param>
        public Draw(Dispatcher opener = null, Backend.Map currentMap = null)
        {
            _opener = opener;
        }
        #endregion

        #region "Static Methods"
        /// <summary>
        /// Animate an actor
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="target"></param>
        public static void Animate(Direction direction, int x, int y, Graphics target)
        {
            Bitmap _pic = new Bitmap(32, 32);
            Graphics temp = Graphics.FromImage(_pic);
            target.DrawImage(ImageCache.LoadImg("player.png"), new Rectangle(0, 0, 32, 32), new Rectangle(_phase * 32, (int)direction * 32, 32, 32), GraphicsUnit.Pixel);
            target.DrawImage(_pic, x, y);

            _phase += 1;
            if (_phase > 2) _phase = 0;
        }


        #endregion
    }



    public class ImageCache
    {
        class CacheElem
        {
            public string name;
            public Bitmap img;
            public CacheElem(string _name, Bitmap _img)
            {
                name = _name;
                img = _img;
            }
        }

        static List<CacheElem> _img = null;

        public static Bitmap LoadImg(string name)
        {
            name = name.ToLower().Trim();
            if (_img == null) { _img = new List<CacheElem>(); };
            foreach (CacheElem i in _img)
            {
                if (i.name == name) return i.img;
            }
            CacheElem newImg = new CacheElem(name, (Bitmap)Image.FromFile(name));
            _img.Add(newImg);
            return newImg.img;
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


}
