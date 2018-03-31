using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;

namespace TournamentManagerMobile.Resources.MyClasses
{
    class connection
    {
        public string dbPath       { get; set; }
        public SQLiteConnection db { get; set; }

        public connection()
        {
            dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            dbPath = Path.Combine(dbPath, "TournamentManagerDB.db3");
            db = new SQLiteConnection(dbPath);
        }
    }
}