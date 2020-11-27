using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudTechnologyMinorLab1.Models;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace CloudTechnologyMinorLab1.Controllers
{
    public class KlantController : Controller
    {
        FirestoreDb db;

        public void LoadDatabase()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"CloudMinorLab1-Firestore.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("cloudminor-lab1");
        }

        //public async void AddData()
        //{
        //    // Maak collection aan genaamd Klanten
        //    CollectionReference coll = db.Collection("Klanten");

        //    // Maak een collection genaamd DanielP add Klant in het document
        //    await coll.Document("DanielP").CreateAsync(new Klant { Naam = "Daniel", Achternaam = "Peterson", Leeftijd = 22 });
        //    await coll.Document("MoemenB").CreateAsync(new Klant { Naam = "Moemen", Achternaam = "Badawi", Leeftijd = 23 });
        //    await coll.Document("AlperS").CreateAsync(new Klant { Naam = "Alper", Achternaam = "Sahin", Leeftijd = 22 });
        //    await coll.Document("WaylH").CreateAsync(new Klant { Naam = "Wayl", Achternaam = "Hamham", Leeftijd = 24 });
        //    await coll.Document("TimothyB").CreateAsync(new Klant { Naam = "Timothy", Achternaam = "Benschop", Leeftijd = 23 });
        //}


        public async void AddData2()
        {
            CollectionReference coll = db.Collection("Klanten");

            var products = new Product[]
            {
                new Product{Naam = "MySQL", Storage = 4},
                new Product{Naam = "SQL Server", Storage = 1}
            };

            var productList = new List<Product>();

            foreach (Product product in products)
            {
                productList.Add(product);
            }
            
            await coll.Document("AlanS").CreateAsync(new Klant { Naam = "Alan", Achternaam = "Smith", Leeftijd = 27, products = productList });
        }


        public IActionResult Index()
        {
            return View();
        }
              
       
        public async Task<IActionResult> Overzicht2()
        {
            LoadDatabase();

            // Maak collection aan genaamd Klanten
            CollectionReference coll = db.Collection("Klanten");     

            // Maak snapshot van document DanielP
            DocumentSnapshot snapshot = await db.Document("Klanten/DanielP").GetSnapshotAsync();

            // Convert de snapshot naar Klant
            Klant fetchedKlant = snapshot.ConvertTo<Klant>();

            return View(fetchedKlant);
        }

        // Opdracht a: Maak een webpagina met een overzicht van alle klanten
        public async Task<IActionResult> Overzicht()
        {
            LoadDatabase(); 

            // Fetch collection genaamd klanten
            CollectionReference coll = db.Collection("Klanten");
                        
            List<Klant> klantenLijst = new List<Klant>();

            // Maak snapshot van hele klanten collection
            QuerySnapshot alleKlanten = await coll.GetSnapshotAsync();

            foreach(DocumentSnapshot document in alleKlanten.Documents)
            {
                Klant klant = document.ConvertTo<Klant>();

                klantenLijst.Add(klant);
            }
         
            return View(klantenLijst);
        }

        public async Task<IActionResult> Test()
        {
            LoadDatabase();

            CollectionReference coll = db.Collection("users");
            DocumentReference document = await coll.AddAsync(new
            {
                Name = new
                {
                    First = "Ada",
                    Last = "Lovelace"
                }
            });

            DocumentSnapshot snapshot = await document.GetSnapshotAsync();

            Dictionary<string, object> data = snapshot.ToDictionary();
            Dictionary<string, object> name = (Dictionary<string, object>)data["Name"];

            return View(name);
        }
    }
}