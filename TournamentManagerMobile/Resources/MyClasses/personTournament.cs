using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;
using TournamentManagerMobile.Resources.MyClasses;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TournamentManagerMobile.Resources.MyClasses
{
    [Table("peopleTournament")]
    class personTournament
    {
        [ForeignKey(typeof(tournament))]
        public int tournamentID { get; set; }
        [ForeignKey(typeof(person))]
        public int personID { get; set; }
        [ForeignKey(typeof(person))]
        public string club  { get; set; }

        public personTournament(int TournamentID, int PersonID)
        {
            tournamentID = TournamentID;
            personID = PersonID;
        }
        public personTournament()
        {

        }

        public override string ToString ()
        {
            return tournamentID.ToString() + personID.ToString();
        }

    }
}