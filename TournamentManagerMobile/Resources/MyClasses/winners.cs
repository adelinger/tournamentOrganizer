using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TournamentManagerMobile.Resources.MyClasses
{
    class winners
    {
        [PrimaryKey, AutoIncrement]
        public int winnerID          { get; set; }
        [ForeignKey(typeof(person))]
        public string personName     { get; set; }
        [ForeignKey(typeof(tournament))]
        public string clubName       { get; set; }
        [ForeignKey(typeof(club))]
        public string tournamentName { get; set; }

        public winners(string name, string club, string tournament)
        {
            personName     = name;
            clubName       = club;
            tournamentName = tournament;
        }
        public winners()
        {

        }
    }
}