using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawler
{
    public partial class MainWindow : Form
    {
        int _x = 0, _y = 0;
        int _px = 0, _py = 0;

        int _zoomLevel = 128;
        int _highlightElement = -1;
        string _path = "images/terrain_atlas.png";

        Backend.Map currentMap = null;

        public MainWindow()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            SetStyle(ControlStyles.ResizeRedraw, true);
            currentMap = new Backend.Map("start");           

            if (currentMap.Width < 1)
            {
                currentMap = new Backend.Map(10, 10, false);
            }
            Invalidate();
        }


        private void MainWindow_Paint(object sender, PaintEventArgs e)
        {

            Graphics target = e.Graphics;
            Image i = Image.FromFile(_path);

            target.FillRectangle(new SolidBrush(Color.DarkGray), new Rectangle(10, 50, _zoomLevel * currentMap.Width, _zoomLevel * currentMap.Height));
            Bitmap bmp = new Bitmap(currentMap.Width * 32, currentMap.Height * 32);
            Graphics temp = Graphics.FromImage(bmp);
            Crawler.Frontend.Draw.DrawMap(currentMap, temp);
            target.DrawImage(bmp, new Rectangle(10, 50, _zoomLevel * currentMap.Width, _zoomLevel * currentMap.Height), new Rectangle(0, 0, currentMap.Width * 32, currentMap.Height * 32), GraphicsUnit.Pixel);
            target.DrawImage(i, new Rectangle(10 + _px * _zoomLevel, 50 + _py * _zoomLevel, _zoomLevel, _zoomLevel), new Rectangle(_x * 32, _y * 32, 31, 31), GraphicsUnit.Pixel);
            Font myFont = new Font("Arial", 24);
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            SolidBrush blackBrush = new SolidBrush(Color.Black);

            target.DrawString(_x.ToString() + "/" + _y.ToString(), myFont, whiteBrush, 10 + _px * _zoomLevel, 50 + _py * _zoomLevel);

            if (_highlightElement == -1)
            {
                target.FillRectangle(blackBrush, new Rectangle(Width - 210, 5, 210, 50));

                target.DrawRectangle(new Pen(whiteBrush, 2), new Rectangle(Width - 210, 5, 210, 50));
                target.DrawString("Beenden", myFont, whiteBrush, Width - 170, 10);

            }
            else
            {
                target.FillRectangle(whiteBrush, new Rectangle(Width - 210, 5, 210, 50));

                target.DrawRectangle(new Pen(blackBrush, 2), new Rectangle(Width - 210, 5, 210, 50));
                target.DrawString("Beenden", myFont, blackBrush, Width - 170, 10);

            };


        }



        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) { _y += 1; };
            if (e.KeyCode == Keys.Up) { _y -= 1; };
            if (e.KeyCode == Keys.Left) { _x -= 1; }
            if (e.KeyCode == Keys.Right) { _x += 1; }
            if (e.KeyCode == Keys.PageUp) { _zoomLevel += 16; Invalidate(); return; }
            if (e.KeyCode == Keys.PageDown) { _zoomLevel -= 16; Invalidate(); return; }
            currentMap.UpdateTile(_px, _py, _x, _y, _path);
            Invalidate(new Rectangle(10 + _px * _zoomLevel, 50 + _py * _zoomLevel, _zoomLevel, _zoomLevel));
        }

        private void MainWindow_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Y < 90)
            {
                if (e.X > Width - 220)
                {
                    currentMap.Save("start");
                    this.Close();
                    Application.Exit();
                }
            }
            else
            {
                int targetX = (e.X - 20) / _zoomLevel;
                int targetY = (e.Y - 50) / _zoomLevel;
                if (targetX < currentMap.Width)
                {
                    _px = targetX;
                };
                if (targetY < currentMap.Height)
                {
                    _py = targetY;
                }
                currentMap.UpdateTile(_px, _py, _x, _y, _path);
                Invalidate(new Rectangle(10 + _px * _zoomLevel, 50 + _py * _zoomLevel, _zoomLevel, _zoomLevel));
            }
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.X > Width - 220) && (e.Y < 90))
            {
                if (_highlightElement != 1)
                {
                    Cursor = Cursors.Hand;
                    Invalidate(new Rectangle(Width - 220, 0, 220, 90));
                    _highlightElement = 1;
                }
            }
            else
            {
                Cursor = Cursors.Default;
                if (_highlightElement != -1)
                {
                    _highlightElement = -1;
                    Invalidate(new Rectangle(Width - 220, 0, 220, 90));

                }
            }

        }
    }
}
