using CloudTechnologyMinorLab1.Models;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudTechnologyMinorLab1.ViewModels
{
    [FirestoreData]
    public class DocumentKlant
    {
        [FirestoreProperty]
        public string DocumentId { get; set; }

        [FirestoreProperty]
        public Klant Klant { get; set; }
    }
}
