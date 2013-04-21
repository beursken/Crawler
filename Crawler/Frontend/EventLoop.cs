using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.Server;


namespace Crawler.Frontend
{
    /// <summary>
    /// An event loop processing animation and AI-events
    /// </summary>
    class EventLoop
    {
        #region "Private Fields"

        /// <summary>
        /// A timer used to control the event loop
        /// </summary>
        private System.Timers.Timer _timer = null;

        /// <summary>
        /// The form to which redraw-events will be passed
        /// </summary>
        private Dispatcher _opener = null;

        /// <summary>
        /// A reference to a map
        /// </summary>
        private Backend.Map _currentMap = null;

        #endregion

        /// <summary>
        /// Create a new event loop hooked to a Windows.Forms object and (optionally) a map
        /// </summary>
        /// <param name="opener">The Form to which Redraw events are passed</param>
        /// <param name="currentMap">(optional) map to use</param>
        public EventLoop(Dispatcher opener = null, Backend.Map currentMap = null)
        {
            _opener = opener;
            _currentMap = currentMap;
            _timer = new System.Timers.Timer(10);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        /// <summary>
        /// Determine whether event loop is processing events (read/write)
        /// </summary>
        public bool isRunning
        {
            get
            {
                return _timer.Enabled;
            }
            set
            {
                if (value)
                {
                    _timer.Start();
                }
                else
                {
                    _timer.Stop();
                }
            }
        }

        /// <summary>
        /// Change the map updated by the event loop (write only)
        /// </summary>
        public Backend.Map currentMap
        {
            set
            {
                _currentMap = value;
            }
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 1. AI Update


            // 2. Update Animation phase on all animated objects
            if (_currentMap != null)
            {
                _currentMap.UpdateAnimations();
            }

            // 4. Update Display
            if (_opener != null)
            {
                _opener.Repaint();
            }
        }
    }
}
