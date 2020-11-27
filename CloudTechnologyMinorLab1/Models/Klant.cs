using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudTechnologyMinorLab1.Models
{
    [FirestoreData]
    public class Klant
    {
        public Klant()
        {
            products = new List<Product>();
        }

        [FirestoreProperty]
        public string Naam { get; set; }

        [FirestoreProperty]
        public string Achternaam { get; set; }

        [FirestoreProperty]
        public int Leeftijd { get; set; }

        [FirestoreProperty]
        public List<Product> products { get; set; }
    }
}
