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
    class points
    {

        [ForeignKey (typeof(person))]
        public string playerName    { get; set; }
        public int numOfPoints      { get; set; }
        public int gamesPlayed      { get; set; }
        public int won              { get; set; }
        public int draw             { get; set; }
        public int lost             { get; set; }
        public int homeGoals        { get; set; }
        public int awayGoals        { get; set; }
        public int goalsScored      { get; set; }
        public int goalsReceived    { get; set; }
        public int goalDiference    { get; set; }
        [ForeignKey(typeof(tournament))]
        public int tournamentID     { get; set; }
        

        public points(string name, int points, int GamesPlayed, int Won, int Draw, int Lost, int homeG, int awayG, int scored, int received, int diference, int TournamentID)
        {
            won           = Won;
            draw          = Draw;
            lost          = Lost;
            gamesPlayed   = GamesPlayed;
            playerName    = name;
            numOfPoints   = points;
            homeGoals     = homeG;
            awayGoals     = awayG;
            goalsScored   = scored;
            goalsReceived = received;
            goalDiference = diference;
            tournamentID  = TournamentID;
        }

        public points()
        {

        }

        public override string ToString()
        {
            return playerName;
        }

    }
}