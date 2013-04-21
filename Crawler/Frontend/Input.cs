using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crawler.Server;

namespace Crawler.Frontend
{

    class Input
    {
        #region "Private Fields"
        Dispatcher _opener = null;
        #endregion

        #region "Public Methods"

        public void Keyboard(Keys code)
        {
            switch (code)
            {

                case Keys.Down:
                    _opener.Y += 1;
                    break;
                case Keys.Up:
                    _opener.Y -= 1;
                    break;
                case Keys.Left:
                    _opener.X -= 1;
                    break;
                case Keys.Right:
                    _opener.X += 1;
                    break;
                case Keys.PageUp:
                    _opener.Zoom += 32;
                    break;
                case Keys.PageDown:
                    _opener.Zoom -= 32;
                    break;
                case Keys.Escape:
                    _opener.Quit();
                    break;
            }
        }

        public void Click(System.Drawing.Point point)
        {
            foreach (Button button in _opener.buttons) { if (button.IsHit(point, null, true)) { return; } };
            if (_opener.mapview.IsHit(point, true)) { _opener.UpdateTile(); return; };
            if (_opener.toolbox.SelectTool(point)) { _opener.attachedImage = _opener.toolbox.selectedTool; }
        }

        public void Move(System.Drawing.Point point)
        {
            foreach (Button button in _opener.buttons) { button.IsHit(point, null, false); };
            if (!_opener.mapview.IsHit(point, true))
            {
                _opener.moveAttachedTo(point);
            }
            else
            {
                _opener.moveAttachedTo(new Point(-1, -1));
            }
            _opener.toolbox.HighlightTool(point);

           
        }

        #endregion
        #region "Constructor"
        /// <summary>
        /// Create a new draw hooked to a Windows.Forms object and (optionally) a map
        /// </summary>
        /// <param name="opener">The Form to which Redraw events are passed</param>
        /// <param name="currentMap">(optional) map to use</param>
        public Input(Server.Dispatcher opener)
        {
            _opener = opener;
        }
        #endregion
    }
}
