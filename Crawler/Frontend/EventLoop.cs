using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Frontend
{
    class EventLoop
    {
        System.Timers.Timer _timer = null;
        System.Windows.Forms.Form _opener = null;
        Backend.Map _currentMap=null;
        public EventLoop(System.Windows.Forms.Form opener)
        {
             _timer = new System.Timers.Timer(10);
             _timer.Elapsed += _timer_Elapsed; 
            _timer.Start();
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 1. Update Animation phase on all animated objects
            //_currentMap.UpdateAnimations();

            // 2. Update Display
            //_opener.Invalidate();
        }
    }
}
