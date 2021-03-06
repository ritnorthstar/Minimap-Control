﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Controls;
using Core.Data;

namespace DataTypes
{
    public class Map : MapObject
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Map FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Map>(json);
        }

        public static Map FromFile(string filename)
        {
            return Map.FromJson(File.ReadAllText(filename));
        }

        public void DrawOn(DrawingItemsSource source, double scalar)
        {
            foreach (MapComponent component in GetComponents())
            {
                source.AddChild(new Drawable(component, scalar));
            }
        }
    }
}
