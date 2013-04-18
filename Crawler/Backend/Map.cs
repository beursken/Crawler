using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Backend
{

    /// <summary>
    /// Connections between maps (including teleports)
    /// </summary>
    class Exit
    {
        Tile _tile = null;
        string _toFile = "";
        bool _toRandom = true;
        public int x
        {
            get
            {
                if (_tile != null)
                {
                    return _tile.x;
                }
                return -1;
            }

        }
        public int y
        {
            get
            {
                if (_tile != null)
                {
                    return _tile.y;
                }
                return -1;
            }


        }

    }


    class Tile
    {
        string _description = "";
        List<Placeable> _placeables;
        List<Actor> _actors;
        Exit _exit = null;
        int _floorStyle = 0;
        int _wallStyle = -1;
        Row _parent = null;

        public bool canEnter()
        {
            return (_wallStyle != -1);
        }

        public List<Placeable> placeables
        {
            get
            {
                return _placeables;
            }
        }

        public int x
        {
            get
            {
                return _parent.Col(this);
            }
        }

        public int y
        {
            get
            {
                return _parent.y;
            }
        }

        public void AddWall(int style = -1)
        {

        }

        public void AddActor(Actor a)
        {
            _actors.Add(a);
        }

        public void AddPlaceable(Placeable p)
        {
            _placeables.Add(p);
        }

        public void ClearWalls()
        {

        }

        public void ClearPlaceables(int id)
        {
            if (id == -1)
            {
                _actors.Clear();
            }
            else
            {
                _actors.RemoveAt(id);
            }
        }

        public void ClearActors(int id)
        {
            if (id == -1)
            {
                _actors.Clear();
            }
            else
            {
                _actors.RemoveAt(id);
            }

        }

        public List<Placeable> GetPlaceables()
        {
            return _placeables;
        }

        public List<Actor> GetActors()
        {
            return _actors;
        }


        public Tile()
        {
            _placeables = new List<Placeable>();
            _actors = new List<Actor>();

        }
    }

    class Row
    {
        List<Tile> _cols;
        Map _parent;

        public int y
        {
            get
            {
                return _parent.Row(this);
            }
        }

        public int Col(Tile col)
        {
            return _cols.IndexOf(col);
        }

        public Tile getTile(int x)
        {
            if ((x > -1) && (x < _cols.Count))
            {
                return _cols[x];
            }
            else
            {
                return null;
            }
        }

        public bool canEnter(int x)
        {
            Tile target = getTile(x);
            if (target != null)
            {
                return target.canEnter();
            }
            else
            {
                return false;
            }
        }

        public void AddWall(int x = -1, int style = -1)
        {
            if (x == -1)
            {
                foreach (Tile cell in _cols)
                {
                    cell.AddWall(style);
                }
            }
            else
            {
                _cols[x].AddWall(style);
            };
        }

        public void ClearWalls(int x = -1)
        {
            if (x == -1)
            {
                foreach (Tile cell in _cols)
                {
                    cell.ClearWalls();
                }
            }
            else
            {
                _cols[x].ClearWalls();
            };
        }

        public void AddPlaceable(Placeable p, int x = -1)
        {
            if (x == -1)
            {
                foreach (Tile cell in _cols)
                {
                    cell.AddPlaceable(p);
                }
            }
            else
            {
                _cols[x].AddPlaceable(p);
            };
        }

        public void AddActor(Actor a, int x = -1)
        {
            if (x == -1)
            {
                foreach (Tile cell in _cols)
                {
                    cell.AddActor(a);
                }
            }
            else
            {
                _cols[x].AddActor(a);
            };
        }

        public void ClearPlaceables(int x = -1, int id = -1)
        {
            if (x == -1)
            {
                foreach (Tile cell in _cols)
                {
                    cell.ClearPlaceables(id);
                }
            }
            else
            {
                _cols[x].ClearPlaceables(id);
            };
        }

        public void ClearActors(int x = -1, int id = -1)
        {
            if (x == -1)
            {
                foreach (Tile cell in _cols)
                {
                    cell.ClearActors(id);
                }
            }
            else
            {
                _cols[x].ClearActors(id);
            };
        }


        public List<Placeable> GetPlaceables(int x)
        {
            List<Placeable> placeables = new List<Placeable>();

            if (x == -1)
            {
                foreach (Tile cell in _cols)
                {
                    placeables.AddRange(cell.GetPlaceables());
                }
            }
            else
            {
                placeables.AddRange(_cols[x].GetPlaceables());
            };
            return placeables;
        }

        public List<Actor> GetActors(int x)
        {
            List<Actor> actors = new List<Actor>();
            if (x == -1)
            {
                foreach (Tile cell in _cols)
                {
                    actors.AddRange(cell.GetActors());
                }
            }
            else
            {
                actors.AddRange(_cols[x].GetActors());
            };
            return actors;
        }

        /// <summary>
        /// Constructor: Create a Row of a specified number of tiles
        /// </summary>
        /// <param name="width">Number of tiles to create</param>
        public Row(int width)
        {
            _cols = new List<Tile>();
            for (int count = 0; count < width; count++)
            {
                _cols.Add(new Tile());
            }
        }

    }

    class Map
    {
        int _width = 0;
        int _height = 0;
        bool _valid = false;
        List<Row> _rows = null;
        string _description = "";

        /// <summary>
        /// Whether a location on the map can be entered by Actors/monsters/NPCs
        /// </summary>
        /// <param name="x">Horizontal coordinate</param>
        /// <param name="y">Vertical coordinate</param>
        /// <returns>True if place can be entered</returns>
        public bool canEnter(int x, int y)
        {
            if ((y > -1) && (y < _rows.Count))
            {
                return _rows[y].canEnter(x);
            };

            return false;
        }



        public int Row(Row row)
        {
            return _rows.IndexOf(row);
        }

        public void ClearPlaceables(int x = -1, int y = -1, int id = -1)
        {
            if (y == -1)
            {
                foreach (Row row in _rows)
                {
                    row.ClearPlaceables(x, id);

                }
            }
            else
            {
                _rows[y].ClearPlaceables(x, id);
            };
        }

        public void ClearActors(int x = -1, int y = -1, int id = -1)
        {
            if (y == -1)
            {
                foreach (Row row in _rows)
                {
                    row.ClearActors(x, id);

                }
            }
            else
            {
                _rows[y].ClearActors(x, id);
            };
        }

        public void AddWall(int x, int y, int style = 0)
        {

            ClearPlaceables(x, y);
            ClearActors(x, y);
            if (y == -1)
            {
                foreach (Row row in _rows)
                {
                    row.AddWall(x);

                }
            }
            else
            {
                _rows[y].AddWall(x);
            };


        }

        public void AddPlaceable(Placeable placeable, int x = -1, int y = -1)
        {
            if (y == -1)
            {
                foreach (Row row in _rows)
                {
                    row.AddPlaceable(placeable, x);

                }
            }
            else
            {
                _rows[y].AddPlaceable(placeable, x);
            };
        }


        public List<Placeable> GetPlaceables(int x, int y)
        {
            List<Placeable> placeables = new List<Placeable>(-1);

            if (y == -1)
            {
                foreach (Row row in _rows)
                {
                    placeables.AddRange(row.GetPlaceables(x));
                }
            }
            else
            {
                placeables.AddRange(_rows[y].GetPlaceables(x));
            };
            return placeables;
        }

        public List<Actor> GetActors(int x, int y)
        {
            List<Actor> actors = new List<Actor>();
            if (y == -1)
            {
                foreach (Row row in _rows)
                {
                    actors.AddRange(row.GetActors(x));

                }
            }
            else
            {
                actors.AddRange(_rows[y].GetActors(x));
            };
            return actors;
        }

        public void ClearWalls(int x = -1, int y = -1)
        {
            if (y == -1)
            {
                foreach (Row row in _rows)
                {
                    row.ClearWalls(x);

                }
            }
            else
            {
                _rows[y].ClearWalls(x);
            };
        }

        public void PlaceWalls()
        {

        }


        /// <summary>
        /// Constructor for an empty map
        /// </summary>
        /// <param name="width">Number of horizontal tiles</param>
        /// <param name="height">Number of vertical tiles</param>
        /// <param name="generate">Place random walls</param>
        public Map(int width = 0, int height = 0, bool generate = false)
        {
            _valid = false;
            _width = width;
            _height = height;
            _rows = new List<Row>();
            for (int count = 0; count < height; count++)
            {
                _rows.Add(new Row(width));
            }
            if (generate)
            {
                PlaceWalls();
                _valid = true;
            }
        }

        public bool Load(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Save the current map to an XML-File
        /// </summary>
        /// <param name="fileName">The filename to be used (XML-extension automatically appended)</param>
        public void Save(string fileName)
        {
            fileName += ".xml";
        }

        /// <summary>
        /// Constructor for a map loaded from a file
        /// </summary>
        /// <param name="fileName">An XML-file containing map-data (XML-extension automatically appended)</param>
        public Map(string fileName)
        {
            fileName += ".xml";
            if (Load(fileName))
            {

                _valid = true;
            }
            else
            {
                _width = 0;
                _height = 0;
                _valid = false;
            }
        }





    }
}
