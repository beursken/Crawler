using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Crawler.Frontend
{
    class Toolbox
    {
        private Point _currentToolPos, _markedToolPos;
        private string _libraryFile = "images/terrain_atlas.png";
        private Rectangle _area;
        private bool _selected = false;
        private Server.Dispatcher _opener = null;
        private int _zoom = 64;
        private int _startX = 0, _startY = 0;
        private int _rows = 0, _cols = 0;
        private int _page = 0, _totalPages = 0;
        private int _totalRows = 0, _totalCols = 0;
        private Bitmap _selectedCache = null;


        public Point currentTool
        {
            get
            {
                return _currentToolPos;
            }
        }
        public int zoom
        {
            get { return _zoom / 16; }
            set
            {
                _zoom = value * 16;
                _totalPages = (_totalCols * _totalRows) / (((_area.Height - 24) / (_zoom + 4)) * ((_area.Width - 4) / (_zoom + 4))) + 1;
                page = 1;
            }
        }

        public Bitmap selectedTool
        {
            get
            {
                return _selectedCache;
            }
        }

        public void Draw(Graphics target)
        {
            Brush white = new SolidBrush(Color.White);
            Brush lavender = new SolidBrush(Color.Lavender);
            Brush red = new SolidBrush(Color.Red);

            Pen whitePen = new Pen(white, 1);
            Pen redPen = new Pen(red, 2);

            Font font = new Font("Segoe UI", 10);


            target.DrawString(_page.ToString() + "/" + _totalPages.ToString(), font, white, new Point(_area.Left + 4, _area.Top - 2));
            Bitmap pic = ImageCache.LoadImg(_libraryFile);
            int rowCount = _startY, colCount = _startX;
            for (int y = _area.Top + 4; y < _area.Bottom - _zoom + 2 - 20; y += _zoom + 4)
            {
                for (int x = _area.Left + 4; x < _area.Right - _zoom + 2; x += _zoom + 4)
                {
                    if ((_markedToolPos.X == colCount) && (_markedToolPos.Y == rowCount))
                    {
                        target.FillRectangle(lavender, new Rectangle(x + 1, 20 + y + 1, _zoom + 2, _zoom + 2));
                        target.DrawRectangle(redPen, new Rectangle(x + 1, 20 + y + 1, _zoom + 2, _zoom + 2));


                    }
                    else
                    {
                        if ((_currentToolPos.X == colCount) && (_currentToolPos.Y == rowCount))
                        {
                            target.FillRectangle(white, new Rectangle(x + 1, 20 + y + 1, _zoom + 2, _zoom + 2));
                            target.DrawRectangle(redPen, new Rectangle(x + 1, 20 + y + 1, _zoom + 2, _zoom + 2));

                        }
                        else
                        {
                            target.DrawRectangle(whitePen, new Rectangle(x + 1, 20 + y + 1, _zoom + 2, _zoom + 2));
                        };
                    };
                    target.DrawImage(pic, new Rectangle(x + 2, 20 + y + 2, _zoom, _zoom), new Rectangle(colCount * 32, rowCount * 32, 32, 32), GraphicsUnit.Pixel);
                    colCount += 1;
                    if (colCount > _totalCols)
                    {
                        if (rowCount < _totalRows)
                        {
                            colCount = 0;
                            rowCount += 1;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            };
            whitePen.Dispose();
            font.Dispose();
            white.Dispose();
            red.Dispose();
            redPen.Dispose();

            lavender.Dispose();
            return;
        }

        public int page
        {
            get
            {
                return _page;
            }
            set
            {
                if ((_page < _totalPages) && (_totalPages > 0))
                {
                    _page = value;
                    int startpic = (page - 1) * ((_area.Height - 24) / (_zoom + 4)) * ((_area.Width - 4) / (_zoom + 4));
                    System.Diagnostics.Debug.WriteLine(startpic);
                    _startY = startpic / (_totalCols + 1);
                    _startX = startpic % (_totalCols + 1);

                }

            }
        }

        public string filename
        {
            get
            {
                return _libraryFile;
            }
            set
            {
                _libraryFile = value;
                Image pic = ImageCache.LoadImg(_libraryFile);
                _page = 1;
                _totalRows = pic.Height / 32;
                _totalCols = pic.Width / 32;
                _startX = 0;
                _startY = 0;
                _totalPages = (_totalCols * _totalRows) / (((_area.Height - 24) / (_zoom + 4)) * ((_area.Width - 4) / (_zoom + 4))) + 1;
            }
        }

        public Point Pos2Location(Point pos)
        {
            Point result = new Point();
            result.X = -1;
            result.Y = -1;
            if (_area.Contains(pos))
            {
                pos.Y -= 24 + _area.Top;
                pos.X -= 4 + _area.Left;
                int addedPos = pos.X / (_zoom + 4);
                int _picsPerRow = (int)Math.Truncate((decimal)((_area.Width - 4) / (_zoom + 4)));
                addedPos += (pos.Y / (_zoom + 4)) * _picsPerRow;
                result.X = _startX + (addedPos % (_totalCols + 1));
                result.Y = _startY + (addedPos / (_totalCols + 1));


                if (result.X > _totalCols + 1)
                {
                    result.X -= _totalCols + 1;
                    result.Y += 1;
                }
                System.Diagnostics.Debug.WriteLine("per Row: " + _picsPerRow + " - " + pos.X + "/" + pos.Y + " - " + " => " + addedPos + ":" + result.X + "/" + result.Y);

            }
            return result;
        }
        /*
        public Point Location2Pos(Point location)
        {
            Point result = new Point();
            result.X = -1;
            result.Y = -1;
            int fields = (_startY * _totalCols) + _startX;

            if ((location.X < _startX) || (location.Y < _startY) || (location.Y > _totalRows) || (location.X > _totalCols))
            {
                return result;
            }
            int picsPerLine=;
            result.X = fields % picsPerLine;
            result.Y = fields / picsPerLine;

            if (result.Y > (_area.Height - 20) / (_zoom + 2))
            {
                result.X = -1;
                result.Y = -1;
            }
            return result;
        }
        */
        /*     public void DrawPos(Graphics target, Point pos)
             {
                 Image pic = ImageCache.LoadImg(_libraryFile);
                 int rowCount = _startY, colCount = _startX;
                 Point img = Location2Pos(pos);
                 target.DrawImage(pic, new Rectangle(pos.X + 2, pos.Y + 2, _zoom, _zoom), new Rectangle(img.X * 32, img.Y * 32, 32, 32), GraphicsUnit.Pixel);
                 return;
             } */

        public bool SelectTool(Point point)
        {
            Point start = Pos2Location(point);
            if ((start.X > -1) && (start.Y > -1))
            {
                Point tool = Pos2Location(start);
                _opener.Redraw(this, _area);
                _currentToolPos = start;
                if (_selectedCache != null) _selectedCache.Dispose();
                _selectedCache = new Bitmap(_zoom, _zoom);
                Graphics target = Graphics.FromImage(_selectedCache);
                Bitmap pic = ImageCache.LoadImg(_libraryFile);



                target.DrawImage(pic, new Rectangle(0, 0, 32, 32), new Rectangle(_currentToolPos.X * 32, _currentToolPos.Y * 32, 32, 32), GraphicsUnit.Pixel);
                target.Dispose();
                return true;
            }
            return false;

        }

        public bool HighlightTool(Point point)
        {
            Point start = Pos2Location(point);
            if ((start.X > -1) && (start.Y > -1))
            {
                _opener.Redraw(this, _area);
                _markedToolPos = start;
            }
            return false;
        }


        public Toolbox(Server.Dispatcher opener, string source, Rectangle area)
        {
            _opener = opener;
            _area = area;
            filename = source;
        }

    }
}
