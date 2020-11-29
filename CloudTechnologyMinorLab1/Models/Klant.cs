using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string Geslacht { get; set; }

        [FirestoreProperty]
        public int Leeftijd { get; set; }

        [FirestoreProperty]
        public string Postcode { get; set; }

        [FirestoreProperty]
        public string Bedrijf { get; set; }

        [FirestoreProperty]
        public List<Product> products { get; set; }      
    }
}
