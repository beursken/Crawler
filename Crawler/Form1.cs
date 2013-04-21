using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crawler.Server;

namespace Crawler
{
    /// <summary>
    /// Mainwindow used for drawing and capturing input
    /// </summary>
    public partial class MainWindow : Form
    {
        #region "Private Fields"
        private Dispatcher _dispatcher = null;
        #endregion

        #region "Constructor"
        public MainWindow()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            SetStyle(ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;
            Show(); // Necessary to recalc window size
            _dispatcher = new Dispatcher(this);          
        }
        #endregion



        #region "Event Handlers"
        private void MainWindow_Paint(object sender, PaintEventArgs e)
        {
            _dispatcher.Refresh(e.Graphics, e.ClipRectangle);
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            _dispatcher.Keyboard(e.KeyCode);
        }

        private void MainWindow_MouseClick(object sender, MouseEventArgs e)
        {
            _dispatcher.Click(new Point(e.X, e.Y));
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            _dispatcher.Move(new Point(e.X, e.Y));
        }
        #endregion

        private void MainWindow_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void MainWindow_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void MainWindow_MouseWheel(object sender, MouseEventArgs e)
        {

        }

    }
}
