﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Crawler.Frontend;

namespace Crawler.Backend
{
    class Tile : ViewObject
    {
        string _description = "";
        List<Placeable> _placeables;
        List<Actor> _actors;
        int _floorStyle = 0;
        int _wallStyle = -1;
        Row _parent = null;

        public bool canEnter()
        {
            return (_wallStyle != -1);
        }

        public new System.Drawing.Point CurrentLocation
        {
            get
            {
                return new System.Drawing.Point(x, y);
            }
        }

        public List<Placeable> placeables
        {
            get
            {
                return _placeables;
            }
        }

        public List<Actor> actors
        {
            get
            {
                return _actors;
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


        public Tile(Row Parent)
        {
            _placeables = new List<Placeable>();
            _actors = new List<Actor>();
            _parent = Parent;

        }

        public new void Load(XmlTextReader reader)
        {
            reader.Read();
            _description = reader.ReadElementString("description", "");
            _floorStyle = XmlConvert.ToInt32(reader.ReadElementString("floorStyle", ""));
            _wallStyle = XmlConvert.ToInt32(reader.ReadElementString("wallStyle", ""));

            // Solange Placeables da sind
            while (reader.Name == "Placeable")
            {
                Placeable p = new Placeable(0, 0, "");
                p.Load(reader);
                _placeables.Add(p);
                reader.Read();
            }

            // Solange Actors da sind
            while (reader.Name == "Actor")
            {
                Actor a = new Actor();
                a.Load(reader);
                _actors.Add(a);
                reader.Read();
            }
            base.Load(reader);

        }

        public new void Save(XmlTextWriter writer)
        {
            writer.WriteStartElement("Col");
            writer.WriteAttributeString("id", x.ToString());
            writer.WriteElementString("description", _description);
            writer.WriteElementString("floorStyle", _floorStyle.ToString());
            writer.WriteElementString("wallStyle", _wallStyle.ToString());
            foreach (Placeable placeable in _placeables)
            {
                placeable.Save(writer);
            }
            foreach (Actor actor in _actors)
            {
                actor.Save(writer);
            }
            base.Save(writer);
            writer.WriteEndElement();

        }

    }

}
