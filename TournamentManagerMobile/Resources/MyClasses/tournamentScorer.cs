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
    class tournamentScorer
    {
        [ForeignKey(typeof(tournament))]
        public int tournamentID { get; set; }
        [ForeignKey(typeof(scorers))]
        public int scorersID    { get; set; }
        [ForeignKey(typeof(scorers))]
        public string name      { get; set; }
        public int goals        { get; set; }

        public tournamentScorer(int TournamentID,int ScorersID, string Name, int numOfGoals)
        {
            tournamentID = TournamentID;
            scorersID    = ScorersID;
            name         = Name;
            goals        = numOfGoals;
        }

        public tournamentScorer()
        {

        }
        
    }
}