using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TournamentManagerMobile.Resources.MyClasses
{
    class scorers
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int      id { get; set; }
        [Unique, Column("name")]
        public string name { get; set; }
        public int goals   { get; set; }

        public scorers(string Name, int Goals)
        {
            name = Name;
            goals = Goals;
        }

        public scorers()
        {

        }

        public override string ToString()
        {
            return name;
        }
    }
}