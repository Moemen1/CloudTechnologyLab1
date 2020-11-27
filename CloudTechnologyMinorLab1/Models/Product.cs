using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudTechnologyMinorLab1.Models
{
    [FirestoreData]
    public class Product
    {
        [FirestoreProperty]
        public string Naam { get; set; }

        [FirestoreProperty]
        public int Storage { get; set; }
    }
}
