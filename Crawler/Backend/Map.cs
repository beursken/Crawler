using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Crawler.Frontend;

namespace Crawler.Backend
{

    public class Location{
    int x;
        int y;
        public Location(int _x=0,int _y=0){
            x=_x;
            y=_y;
        }
    }

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





    class Map
    {
        int _width = 0;
        int _height = 0;
        bool _valid = false;
        List<Row> _rows = null;
        string _description = "";
        List<ViewObject> _animated = null;

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

     

        public void UpdateTile(int x, int y, int posX, int posY, string path)
        {
            _rows[y].UpdateTile(x, posX, posY, path);
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

        public int Height
        {
            get { return _height; }
            set
            {
                while (_height < value)
                {
                    _height += 1;
                    _rows.Add(new Row(this, _width));
                }
                while ((value > -1) && (_height > value))
                {
                    _height -= -1;
                    _rows.RemoveAt(_rows.Count - 1);
                }
            }
        }

        public int Width
        {
            get { return _width; }
            set
            {
                if (_width < value)
                {
                    _width = value;
                    foreach (Row row in _rows)
                    {
                        row.Width = value;
                    }
                }
                if (_width > value)
                {
                    _width = value;
                    foreach (Row row in _rows)
                    {
                        row.Width = value;
                    }
                }
            }
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


        public void Setup(int width = 0, int height = 0)
        {
            _rows = new List<Row>();
            _animated = new List<ViewObject>();
            _width = width;
            _height = height;
            for (int count = 0; count < height; count++)
            {
                _rows.Add(new Row(this, width));
            }

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
            Setup(width, height);
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
                XmlTextReader reader = new XmlTextReader(fileName);
                reader.MoveToContent();
                try
                {

                    Setup(XmlConvert.ToInt32(reader.GetAttribute("height")), XmlConvert.ToInt32(reader.GetAttribute("width")));
                    reader.Read();

                    do
                    {
                        int y = XmlConvert.ToInt32(reader.GetAttribute("id"));
                        _rows[y].Load(reader);
                    }
                    while (reader.ReadToNextSibling("Row"));


                    return true;
                }
                //catch { }
                finally { reader.Close(); }
            }
            return false;
        }



        public Tile GetTile(int x, int y)
        {
            if ((y > -1) && (y < _height) && (x > -1) && (x < _height))
            {
                return _rows[y].GetTile(x);
            }
            return null;
        }
        /// <summary>
        /// Save the current map to an XML-File
        /// </summary>
        /// <param name="fileName">The filename to be used (XML-extension automatically appended)</param>
        public void Save(string fileName)
        {
            fileName += ".xml";
            XmlTextWriter writer = new XmlTextWriter(fileName, null);

            writer.WriteStartDocument();
            writer.WriteDocType("crawlMap", null, null, null);
            writer.WriteComment("Verbose Map file for Crawler Prototype");
            writer.WriteStartElement("map");
            writer.WriteAttributeString("width", _width.ToString());
            writer.WriteAttributeString("height", _height.ToString());
            if (_rows != null)
            {
                foreach (Row row in _rows)
                {
                    row.Save(writer);
                };
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

        }


        public List<Location> UpdateAnimations()
        {
            List<Location> result = new List<Location>();
            foreach (ViewObject v in _animated)
            {
                v.incPhase();
                result.Add(v.CurrentLocation);
            }
            return result;
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
