using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudTechnologyMinorLab1.Data;
using CloudTechnologyMinorLab1.Models;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace CloudTechnologyMinorLab1.Controllers
{
    public class KlantController : Controller
    {
        FirestoreDb db;

        public async void AddData()
        {
            // Maak collection aan genaamd Klanten
            CollectionReference coll = db.Collection("Klanten");

            // Maak een collection genaamd DanielP add Klant in het document
            await coll.Document("DanielP").CreateAsync(new Klant { Naam = "Daniel", Achternaam = "Peterson", Leeftijd = 22 });
            await coll.Document("MoemenB").CreateAsync(new Klant { Naam = "Moemen", Achternaam = "Badawi", Leeftijd = 23 });
            await coll.Document("AlperS").CreateAsync(new Klant { Naam = "Alper", Achternaam = "Sahin", Leeftijd = 22 });
            await coll.Document("WaylH").CreateAsync(new Klant { Naam = "Wayl", Achternaam = "Hamham", Leeftijd = 24 });
            await coll.Document("TimothyB").CreateAsync(new Klant { Naam = "Timothy", Achternaam = "Benschop", Leeftijd = 23 });

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

        // Opdracht a: Maak een webpagina met een overzicht van alle klanten        
        public async Task<IActionResult> Index()
        {
            db = FirestoreDatabase.LoadDatabase();

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

        // Opdracht b: Maak een webpagina met alle producten die 1 specifieke klant afneemt
        [Route("Klant/Producten/{naam}")]
        public async Task<IActionResult> Producten(string naam)
        {
            db = FirestoreDatabase.LoadDatabase();

            CollectionReference coll = db.Collection("Klanten");
            Query klantQuery = coll.WhereEqualTo("Naam", naam);
            QuerySnapshot klantMetNaam = await klantQuery.GetSnapshotAsync();

            Klant klant;

            if (klantMetNaam.Documents.Count > 0)
            {
                klant = klantMetNaam.Documents[0].ConvertTo<Klant>();
            }
            else
            {
                klant = null;
            }            
            
            return View(klant);
        }

        [Route("Klant/Create/{naam}")]
        // GET: Klant/Create
        public async Task<IActionResult> Create(string naam, Product product)
        {
            db = FirestoreDatabase.LoadDatabase();

            // Fetch 'Klanten' collection
            CollectionReference coll = db.Collection("Klanten");           
            // Query to fetch 'Klant' with parameter name
            Query klantQuery = coll.WhereEqualTo("Naam", naam);         
            QuerySnapshot klantMetNaam = await klantQuery.GetSnapshotAsync();
            DocumentReference document = coll.Document(klantMetNaam.Documents[0].Id);

            
            await document.UpdateAsync("products", FieldValue.ArrayUnion(product));
           

            //if (ModelState.IsValid)
            //{

            //    return RedirectToAction("Index");
            //}          

            return View(product);
        }

        [Route("Klant/Delete/{naam}")]
        public async Task<IActionResult> Delete(string naam, Product product)
        {
            db = FirestoreDatabase.LoadDatabase();

            CollectionReference klantenRef = db.Collection("Klanten");
            Query query = klantenRef.WhereEqualTo("Naam", naam);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            DocumentReference document = klantenRef.Document(querySnapshot.Documents[0].Id);

            await document.UpdateAsync("products", FieldValue.ArrayRemove(product));

            return RedirectToAction("Index");
        }
    }
}