using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudTechnologyMinorLab1.Data;
using CloudTechnologyMinorLab1.Models;
using CloudTechnologyMinorLab1.ViewModels;
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
            await coll.Document("DanielP").CreateAsync(new Klant { Naam = "Daniel", Achternaam = "Peterson", Geslacht= "M", Leeftijd = 22, Bedrijf = "Albert Heijn", Postcode = "2432 DE" });
            await coll.Document("MoemenB").CreateAsync(new Klant { Naam = "Moemen", Achternaam = "Badawi", Geslacht = "M", Leeftijd = 23, Bedrijf = "Coolblue", Postcode = "1519 AZ" });
            await coll.Document("AlperS").CreateAsync(new Klant { Naam = "Alper", Achternaam = "Sahin", Geslacht = "M", Leeftijd = 28, Bedrijf = "Praxis", Postcode = "2015 TE" });
            await coll.Document("WaylH").CreateAsync(new Klant { Naam = "Wayl", Achternaam = "Hamham", Geslacht = "M", Leeftijd = 29, Bedrijf = "Lidl", Postcode = "2432 DE" });
            await coll.Document("TimothyB").CreateAsync(new Klant { Naam = "Timothy", Achternaam = "Benschop", Geslacht = "M", Leeftijd = 42, Bedrijf = "Sligro", Postcode = "2432 DE" });

            var products = new Product[]
           {
                new Product{ProductId = "MySQL", MemoryGB = 4},
                new Product{ProductId = "SQL Server", MemoryGB = 1}
           };

            var productList = new List<Product>();

            foreach (Product product in products)
            {
                productList.Add(product);
            }

            await coll.Document("AlanS").CreateAsync(new Klant { Naam = "Alan", Achternaam = "Smith", Geslacht = "M", Leeftijd = 18, Bedrijf = "Amazon", Postcode = "2325 UA", products = productList });
        }

        // Opdracht a: Maak een webpagina met een overzicht van alle klanten        
        public async Task<IActionResult> Index()
        {
            db = FirestoreDatabase.LoadDatabase();
            
            // Fetch collection genaamd klanten
            CollectionReference coll = db.Collection("Klanten");        
            List<DocumentKlant> documentKlantLijst = new List<DocumentKlant>();

            // Maak snapshot van hele klanten collection
            QuerySnapshot alleKlanten = await coll.GetSnapshotAsync();

            foreach(DocumentSnapshot document in alleKlanten.Documents)
            {        
                DocumentKlant documentKlant = new DocumentKlant
                {
                    DocumentId = document.Id,
                    Klant = document.ConvertTo<Klant>()
                };

                documentKlantLijst.Add(documentKlant);                
            }
         
            return View(documentKlantLijst);
        }

        // Opdracht b: Maak een webpagina met alle producten die 1 specifieke klant afneemt
        [Route("Klant/Producten/{documentId}")]
        public async Task<IActionResult> Producten(string documentId)
        {
            db = FirestoreDatabase.LoadDatabase();
            
            CollectionReference klantenColl = db.Collection("Klanten");
            DocumentReference document = klantenColl.Document(documentId);
            DocumentSnapshot klantMetNaam = await document.GetSnapshotAsync();
            
            DocumentKlant documentKlant = new DocumentKlant
            {
                DocumentId = document.Id,
                Klant = klantMetNaam.ConvertTo<Klant>()
            };

            Query query = klantenColl.WhereEqualTo("Naam", documentKlant.Klant.Naam);

            FirestoreChangeListener listener = query.Listen(snapshot =>
            {                
                foreach(DocumentChange change in snapshot.Changes)
                {
                    DocumentSnapshot documentSnapshot = change.Document;

                    if (documentSnapshot.Exists)
                    {
                        documentKlant.DocumentId = document.Id;
                        documentKlant.Klant = documentSnapshot.ConvertTo<Klant>();                        
                    }
                }                                   
            });

            await listener.StopAsync();

            return View(documentKlant);
        }

        [Route("Klant/Create/{documentId}")]
        // GET: Klant/Create
        public async Task<IActionResult> Create(string documentId, Product product)
        {
            db = FirestoreDatabase.LoadDatabase();

            // Fetch 'Klanten' collection
            CollectionReference klantenColl = db.Collection("Klanten");         
            DocumentReference document = klantenColl.Document(documentId);
            DocumentSnapshot klantMetNaam = await document.GetSnapshotAsync();          
            
            if(product.ProductId != null)
            {
                await document.UpdateAsync("products", FieldValue.ArrayUnion(product));

                return RedirectToAction(documentId, "Klant/Producten");
            }                    

            return View(product);
        }

        // d)	Maak een webpagina waarin je een veld van een klant kan editen
        [Route("Klant/Edit/{documentId}")]
        public async Task<IActionResult> Edit(string documentId, Klant updatedKlant)
        {
            db = FirestoreDatabase.LoadDatabase();

            CollectionReference klantenColl = db.Collection("Klanten");
            DocumentReference document = klantenColl.Document(documentId);
            DocumentSnapshot klantMetNaam = await document.GetSnapshotAsync();
                 
            Klant klant = klantMetNaam.ConvertTo<Klant>();

            if (updatedKlant.Naam != null && updatedKlant != klant)
            {
                updatedKlant.products = klant.products;
                await document.SetAsync(updatedKlant);
                              
                return RedirectToAction("Index");
            }

            return View(klant);
        }

        // c)	Maak een webpagina waarin je per klant een product kan toevoegen en verwijderen
        [Route("Klant/Create{documentId}&{productId}")]
        public async Task<IActionResult> Delete(string documentId, string productId)
        {
            db = FirestoreDatabase.LoadDatabase();

            CollectionReference klantenColl = db.Collection("Klanten");
            DocumentReference document = klantenColl.Document(documentId);
            DocumentSnapshot klantMetNaam = await document.GetSnapshotAsync();

            Klant klant = klantMetNaam.ConvertTo<Klant>();

            Product product = klant.products.FirstOrDefault(p => p.ProductId == productId);

            if(product != null)
            {
                await document.UpdateAsync("products", FieldValue.ArrayRemove(product));
            }

            return RedirectToAction(documentId, "Klant/Producten");
        }  
        
        [Route("Klant/Factuur{documentId}")]
        public async Task<IActionResult> Factuur(string documentId)
        {
            db = FirestoreDatabase.LoadDatabase();

            CollectionReference klantenColl = db.Collection("Klanten");
            DocumentReference document = klantenColl.Document(documentId);
            DocumentSnapshot klantMetNaam = await document.GetSnapshotAsync();
            
            return View();
        }
    }
}