using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;
using TournamentManagerMobile.Resources.MyClasses;

namespace TournamentManagerMobile.Resources
{
    [Table ("people")]
    class person
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int id           { get; set; }
        [NotNull,Unique, Column("name")]
        public string name      { get; set; }
        [ForeignKey(typeof(club))]
        public string club      { get; set; }
        [ForeignKey(typeof(tournament))]
        public int tournamentID { get; set; }
          
        public person (string Name, string Club, int TournamentID)
        {
            name = Name;
            club = Club;
            tournamentID = TournamentID;
        }

        public person()
        {

        }

        public override string ToString()
        {
            return name + club;
        }
    }
}