using SQLite;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using TournamentManagerMobile.Resources.MyClasses;
using System.IO;
using TournamentManagerMobile.Resources;
using Android.Gms.Ads;

namespace TournamentManagerMobile
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "Create new tournament")]
    public class createNewTournament : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
          

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.createNewTournament);

            var id = "ca-app-pub-5385963311823976~5875287959";
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, id);
            var adView = FindViewById<AdView>(Resource.Id.adViewCNT);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);


            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "TournamentManagerDB.db3");
            SQLiteConnection db = new SQLiteConnection(dbPath);
            
            var typeSpinner  = FindViewById<Spinner>(Resource.Id.tournamentType);
            var insertName   = FindViewById<TextView>(Resource.Id.addText);
            var buttonNext   = FindViewById<Button>(Resource.Id.nextButton);
            var numOfSpinner = FindViewById<Spinner>(Resource.Id.numOfRounds);

            bool trigger = false;

            db.CreateTable<person>();
            db.CreateTable<winners>();
            db.CreateTable<personTournament>();
            db.CreateTable<club>();
            db.CreateTable<tournament>();
            db.CreateTable<scorers>();
            db.CreateTable<tournamentScorer>();
            db.CreateTable<results>();
            db.CreateTable<points>();

            buttonNext.Click += delegate
            {
                if (numOfSpinner.SelectedItem.ToString() == "choose an item")
                {
                    Toast.MakeText(ApplicationContext, "You must select number of rounds", ToastLength.Short).Show();
                    return;
                }
                if (typeSpinner.SelectedItem.ToString() == "choose an item")
                {
                    Toast.MakeText(ApplicationContext, "You must select tournament type", ToastLength.Short).Show();
                    return;
                }
                if (insertName.Text == "")
                {
                    Toast.MakeText(ApplicationContext, "Tournament name can't be empty!", ToastLength.Short).Show();
                    return;
                }

                try
                {
                    tournament tournament = new tournament(insertName.Text, typeSpinner.SelectedItem.ToString(), numOfSpinner.SelectedItem.ToString());
                    db.Insert(tournament);
                    trigger = false;
                }
                catch (System.Exception)
                {
                    Toast.MakeText(ApplicationContext, "Tournament name already exists", ToastLength.Long).Show();
                    trigger = true;
                }
                if (trigger == false)
                {
                    Intent intent = new Intent(this, typeof(addNewPlayers));           
                    intent.PutExtra("tournamentName", insertName.Text);
                    StartActivity(intent);
                }
            };
            

        }
    }
}