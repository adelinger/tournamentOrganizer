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
using SQLiteNetExtensions.Attributes;

namespace TournamentManagerMobile.Resources.MyClasses
{
    class results
    {
        [PrimaryKey, AutoIncrement, Column("matchID")]
        public int matchID            { get; set; }
        [ForeignKey (typeof(person))]
        public string homePlayerName  { get; set; }
        [ForeignKey (typeof(person))]
        public string awayPlayerName  { get; set; }
        public int homeGoals        { get; set; }
        public int awayGoals      { get; set; }
        [ForeignKey (typeof (tournament))]
        public int tournamentID       { get; set; }

        public results(string homePlayer, string awayPlayer, int GoalsHome, int GoalsAway, int TournamentID)
        {
            homePlayerName = homePlayer;
            awayPlayerName = awayPlayer;
            homeGoals      = GoalsHome;
            awayGoals      = GoalsAway;
            tournamentID   = TournamentID;
        }
        public results()
        {

        }

        public override string ToString()
        {
            return matchID.ToString();
        }

    }
}