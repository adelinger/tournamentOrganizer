using System;
using SQLite;


namespace TournamentManagerMobile.Resources.MyClasses
{
    class tournament
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int id             { get; set; }
        [Unique, Column("name")]
        public string name        { get; set; }
        public string type        { get; set; }
        public string numOfRounds { get; set; }

        public tournament(string Name, string Type, string NumOfRounds)
        {
            name = Name;
            type = Type; 
            numOfRounds = NumOfRounds;
        }

        public tournament()
        {

        }

        public override string ToString()
        {
            return name;
        }
    }
}