using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.Frontend;
using Crawler.Backend;
using System.Windows.Forms;

namespace Crawler.Server
{
    /// <summary>
    /// The Core Dispatcher connecting Frontend and Backend and the actual Windows Forms object
    /// </summary>
    class Dispatcher
    {
        #region "Private Fields"
        private Input _input = null;
        private EventLoop _timer = null;
        private Draw _output = null;
        private Map _map = null;
        private MainWindow _opener = null;
        private List<Frontend.Button> _buttons;
        private Frontend.MapTiles _mapview;
        private Frontend.Toolbox _toolbox;
        private System.Drawing.Image _attachedImage = null;
        private System.Drawing.Rectangle _attachedImagePos;
        private int _zoom = 128;
        private int _editLevel = 0;
        #endregion

        #region "Delegates (pass through)"
        public delegate void _Refresh(System.Drawing.Graphics target, System.Drawing.Rectangle area);
        public delegate void _Keyboard(Keys code);
        public delegate void _Click(System.Drawing.Point point);
        public delegate void _Move(System.Drawing.Point point);
        #endregion

        #region "Public Fields"
        public _Refresh Refresh;
        public _Move Move;

        public _Click Click;
        public _Keyboard Keyboard;
        public Map map
        {
            get
            {
                return _map;
            }
        }

        public System.Drawing.Rectangle attachedImagePos
        {
            get { return _attachedImagePos; }
        }

        public int editLevel
        {
            get
            {
                return _editLevel;
            }
        }
        public System.Drawing.Image attachedImage
        {
            get { return _attachedImage; }
            set
            {
                if (_attachedImage != null)
                {
                    if (_attachedImagePos != null)
                        _opener.Invalidate(_attachedImagePos);

                };
                _attachedImage = value;
                if (value != null)
                {
                    _attachedImagePos.Width = Zoom;
                    _attachedImagePos.Height = Zoom;
                }
            }
        }

        public MapTiles mapview
        {
            get
            { return _mapview; }
            set
            {

                _mapview = value;
            }
        }
        public List<Frontend.Button> buttons
        {
            get
            {
                return _buttons;
            }
            set
            {
                if (_buttons != null)
                {
                    _buttons.Clear();
                };
                _buttons = value;
            }
        }

        public Frontend.Toolbox toolbox
        {
            get { return _toolbox; }
        }


        public int X
        {
            get
            {
                return 0;
            }
            set
            {
                ;
            }
        }

        public int Y
        {
            get
            {
                return 0;
            }
            set
            {
                ;
            }
        }

        public int Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                if ((value > 15) && (value < 170)) _zoom = value;
                _attachedImagePos.Width = Zoom;
                _attachedImagePos.Height = Zoom;
            }
        }

        public void Repaint()
        {
            _opener.Invalidate();
        }

        public void SaveMap()
        {
            _map.Save("start.xml");
        }

        public void Quit()
        {
            _opener.Close();
            Application.Exit();

        }

        public void Test()
        {
            MessageBox.Show(_opener.Width.ToString());
        }

        #endregion

        #region "Constructor and Destructor"

        public void moveAttachedTo(System.Drawing.Point target)
        {
            if (target != null)
            {
                // Altes Bild entfernen
                // _opener.Invalidate(_attachedImagePos);
                // Neues Bild zeichnen
                _attachedImagePos.X = target.X;
                _attachedImagePos.Y = target.Y;                
                _opener.Invalidate(_attachedImagePos);
                /*  System.Drawing.Graphics destination = _opener.CreateGraphics();
                  _output.placeImage(destination, _attachedImage, _attachedImagePos);
                  destination.Dispose(); */
            }
        }

        public void Redraw(object target, System.Drawing.Rectangle area)
        {
            _opener.Invalidate(area);
        }

        public void UpdateTile()
        {
            if (editLevel == 0)
            {
                _map.UpdateTile(_mapview.currentTile.X, _mapview.currentTile.Y, _toolbox.currentTool.X, _toolbox.currentTool.Y, _toolbox.filename);
            }
            else
            {
                _map.AddPlaceable(
                    new Placeable(_toolbox.currentTool.X, _toolbox.currentTool.Y, _toolbox.filename),
                    _mapview.currentTile.X, _mapview.currentTile.Y);
            }
        }

        public Dispatcher(MainWindow opener)
        {
            // References to Frontend and Backend objects
            _opener = opener;
            _input = new Input(this);
            _timer = new EventLoop(this);
            _output = new Draw(this);
            _toolbox = new Toolbox(this, "images/terrain_atlas.png", new System.Drawing.Rectangle(_opener.ClientRectangle.Width - 325, 60, 320, _opener.ClientRectangle.Height - 130));
            _map = new Backend.Map(this, "start");
            if (_map.Width < 1)
            {
                _map = new Backend.Map(this, 10, 10, false);
            }
            _attachedImagePos = new System.Drawing.Rectangle();


            // Hook up Delegates
            Refresh += _output.Refresh;
            Move += _input.Move;
            Click += _input.Click;
            Keyboard += _input.Keyboard;
            Repaint();

            // Generate lists for interaction objects
            _buttons = new List<Crawler.Frontend.Button>();
            _mapview = new MapTiles(this);

            _buttons.Add(new Frontend.Button("Map-Level", ToggleMap, 120, -1, -1, _opener.ClientRectangle.Height - 28));
            _buttons.Add(new Frontend.Button("Quit", Quit, -1, 5, _opener.ClientRectangle.Width - 5, -1));
            //  _buttons.Add(new Frontend.Button("Edit", Edit, -1, 5, _opener.ClientRectangle.Width - 64, -1));
            _buttons.Add(new Frontend.Button("Save", Save, -1, 5, _opener.ClientRectangle.Width - 114, -1));
            // _buttons.Add(new Frontend.Button("File", File, -1, 5, _opener.ClientRectangle.Width - 177, -1));

            _buttons.Add(new Frontend.Button("+", ZoomPageIn, -1, -1, _opener.ClientRectangle.Width - 130, _opener.ClientRectangle.Height - 28));
            _buttons.Add(new Frontend.Button("-", ZoomPageOut, -1, -1, _opener.ClientRectangle.Width - 105, _opener.ClientRectangle.Height - 28));

            _buttons.Add(new Frontend.Button("+", ZoomMapIn, 55, -1, -1, _opener.ClientRectangle.Height - 28));
            _buttons.Add(new Frontend.Button("-", ZoomMapOut, 29, -1, -1, _opener.ClientRectangle.Height - 28));


            _buttons.Add(new Frontend.Button("<", PrevPage, -1, -1, _opener.ClientRectangle.Width - 50, _opener.ClientRectangle.Height - 28));
            _buttons.Add(new Frontend.Button(">", NextPage, -1, -1, _opener.ClientRectangle.Width - 17, _opener.ClientRectangle.Height - 28));

        }

        public void Edit()
        {

        }

        public void Save()
        {
            _map.Save(_map.Filename);
        }

        public void File()
        {

        }

        public void ZoomPageIn()
        {
            _toolbox.zoom += 1;
        }

        public void ZoomPageOut()
        {
            _toolbox.zoom -= 1;
        }

        public void ZoomMapOut()
        {
            Zoom -= 16;
        }

        public void ZoomMapIn()
        {
            Zoom += 16;
        }

        public void PrevPage()
        {
            _toolbox.page -= 1;
            Repaint();
        }

        public void ToggleMap()
        {
            if (_editLevel == 0)
            {
                _buttons[0].label = "Top-Level";
                _editLevel = 1;
            }
            else
            {
                _buttons[0].label = "Map-Level";
                _editLevel = 0;
            }
        }


        public void NextPage()
        {
            _toolbox.page += 1;
            Repaint();
        }
        ~Dispatcher()
        {
            _timer.isRunning = false;

            if (_attachedImage != null)
            {
                _attachedImage.Dispose();
            }
        }
        #endregion
    }
}
