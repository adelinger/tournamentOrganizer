using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TournamentManagerMobile.Resources.MyClasses
{
    [Table("club")]
    class club
    {
        [PrimaryKey, AutoIncrement]
        public int id      { get; set; }
        [Unique]
        public string name { get; set; }

        public club(string Name)
        {
            name = Name;
        }
        public club()
        {

        }
    }
}