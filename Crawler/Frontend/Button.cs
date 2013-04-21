using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Frontend
{
    /// <summary>
    /// Clickable UI element
    /// </summary>
    class Button
    {
        #region "Private Fields"
        private bool _highlight = false;
        private Rectangle _tile;
        private string _label = "";
        public delegate void _Click();
        #endregion

        #region "Public Fields"
        /// <summary>
        /// Change button activation state
        /// </summary>
        public bool highlight
        {
            get
            {
                return _highlight;
            }
            set
            {
                _highlight = value;
            }
        }

        public string label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }

        #endregion

        #region "Public Methods"
        /// <summary>
        /// Draw button
        /// </summary>
        /// <param name="target">A Graphics object to draw upon</param>
        public void Draw(Graphics target)
        {
            Font font = new Font("Segoe UI", 11);
            SolidBrush white = new SolidBrush(Color.White);
            SolidBrush black = new SolidBrush(Color.Black);

            if (!_highlight)
            {
                target.FillRectangle(black, _tile);
                target.DrawRectangle(new Pen(white, 2), _tile);
                target.DrawString(_label, font, white, _tile.X + 2, _tile.Y + 2);

            }
            else
            {
                target.FillRectangle(white, _tile);
                target.DrawRectangle(new Pen(black, 2), _tile);
                target.DrawString(_label, font, black, _tile.X + 2, _tile.Y + 2);
            };
            white.Dispose();
            black.Dispose();
            font.Dispose();
        }


        /// <summary>
        /// Determine whether certain coordinates are part of a button
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsHit(Point point, Graphics target = null, bool click = false)
        {
            if (_tile.Contains(point))
            {
                if (click) Click();
                if (!_highlight)
                {
                    _highlight = false;
                    if (target != null) Draw(target);
                }
                _highlight = true;
                return false;
            }
            else
            {
                if (_highlight)
                {
                    _highlight = false;
                    if (target != null) Draw(target);
                }
                return false;
            }
        }

        public _Click Click;

        #endregion

        #region "Constructor"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="onClick"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public Button(string text, _Click onClick, int left = -1, int top = -1, int right = -1, int bottom = -1)
        {
            Font font = new Font("Segoe UI", 11);
            Size textSize = System.Windows.Forms.TextRenderer.MeasureText(text, font);
            _tile = new Rectangle(left, top, textSize.Width + 4, textSize.Height + 4);
            font.Dispose();
            if ((left > -1) && (right > -1))
            {
                _tile.Width = right - left;
            };
            if ((left < 0) && (right > -1)) { _tile.X = right - _tile.Width; };

            if ((top > -1) && (bottom > -1))
            {
                _tile.Height = bottom - top;
            };

            if ((top < 0) && (bottom > -1)) { _tile.Y = bottom - _tile.Height; };
            _label = text;
            Click += onClick;

        }
        #endregion

    }
}
