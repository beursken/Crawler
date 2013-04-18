using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Backend
{
    class Placeable
    {
        string _description = "";
        bool canEnter = false;
        bool canPickup = false;
        bool doesWarp = false;
    }

    class Teleporter : Placeable
    {
        
    }

    class Chest : Placeable
    {

    }

    class Trap : Placeable
    {

    }

    class Door : Placeable
    {

    }

    class Pushable : Placeable
    {

    }

}
