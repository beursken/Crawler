using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Crawler.Backend
{
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
        public Row(Map parent, int width)
        {
            _cols = new List<Tile>();
            for (int count = 0; count < width; count++)
            {
                _cols.Add(new Tile(this));
            }
            _parent = parent;
        }

        public int Width
        {
            get { return _cols.Count; }
            set
            {
                while (_cols.Count < value)
                {
                    _cols.Add(new Tile(this));
                };

                while (_cols.Count > value)
                {
                    _cols.RemoveAt(_cols.Count - 1);

                }
            }
        }

        public void UpdateTile(int x, int posX, int posY, string path)
        {
            _cols[x].UpdateTile(posX, posY, path);
        }


        public void Load(XmlTextReader reader)
        {
            reader.Read();

            do
            {
                _cols[XmlConvert.ToInt32(reader.GetAttribute("id"))].Load(reader);
            }
            while (reader.ReadToNextSibling("Col"));
        }

        public void Save(XmlTextWriter writer)
        {
            writer.WriteStartElement("Row");
            writer.WriteAttributeString("id", y.ToString());
            foreach (Tile tile in _cols)
            {
                tile.Save(writer);
            }
            writer.WriteEndElement();
        }

        public Tile GetTile(int x)
        {
            return (_cols[x]);
        }

    }
}
