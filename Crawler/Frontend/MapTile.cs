using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Crawler.Frontend
{
    class MapTiles
    {
        private Server.Dispatcher _opener;
        private Point _activeTile;
        public delegate void _Click(Point coords, Rectangle tile);



        public Point currentTile
        {
            get
            {
                return _activeTile;
            }
        }

        public void DrawMap(Backend.Map map, Graphics target)
        {
            target.FillRectangle(new SolidBrush(Color.DarkGray), new Rectangle(10, 50, 4 + (_opener.Zoom + 4) * _opener.map.Width, (_opener.Zoom + 4) * _opener.map.Height + 4));
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    map.GetTile(x, y).Draw(target, 12 + x * (_opener.Zoom), 52 + y * (_opener.Zoom), _opener.Zoom + 4);
                     foreach (Backend.Placeable placeable in map.GetTile(x, y).placeables)
                     {
                         placeable.Draw(target, 12 + x * (_opener.Zoom), 52 + y * (_opener.Zoom), _opener.Zoom + 4);
                     }
                     foreach (Backend.Actor actor in map.GetTile(x, y).actors)
                     {
                         actor.Draw(target, 12 + x * (_opener.Zoom), 52 + y * (_opener.Zoom), _opener.Zoom + 4);
                     } 
                }

            }
            if (_activeTile.X > -1)
            {

                target.DrawRectangle(new Pen(Color.Red, 2), new Rectangle(12 + _activeTile.X * (_opener.Zoom), 52 + _activeTile.Y * (_opener.Zoom), _opener.Zoom + 4, _opener.Zoom + 4));
                if (_opener.attachedImage != null)
                {
                    target.DrawImage(_opener.attachedImage,
                        new Rectangle(12 + _activeTile.X * (_opener.Zoom), 52 + _activeTile.Y * (_opener.Zoom), _opener.Zoom + 4, _opener.Zoom + 4),

                        new Rectangle(0, 0, 32, 32), GraphicsUnit.Pixel);

                }
            }
        }


        /// <summary>
        /// Determine whether certain coordinates are part of a button
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsHit(Point point, bool click = false)
        {
            point.X = point.X - 12;
            point.Y = point.Y - 52;
            point.X = point.X / _opener.Zoom;
            point.Y = point.Y / _opener.Zoom;
            System.Diagnostics.Debug.WriteLine(point.X + "/" + point.Y);

            if ((point.X > -1) && (point.Y > -1))
            {
                if ((point.X < _opener.map.Width) && (point.Y < _opener.map.Height))
                {
                    _activeTile = point;
                    return true;
                }
            }
            _activeTile = new Point(-1, -1);
            return false;
        }

        public _Click Click;

        public MapTiles(Server.Dispatcher opener)
        {
            _opener = opener;
        }
    }
}
