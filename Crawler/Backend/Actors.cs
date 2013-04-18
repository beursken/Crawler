using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Backend
{
    class Actor
    {
        string _description;
        bool _canEnter = false;
        bool _doesWarp = false;
        string _name;
        string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }

    class Player : Actor
    {

    }

    class Enemy : Actor
    {

    }

    class Companion : Actor
    {

    }

    class NPC : Actor
    {

    }
}
