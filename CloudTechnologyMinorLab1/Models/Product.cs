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
        public string ProductId { get; set; }

        [FirestoreProperty]
        public int MemoryGB { get; set; }

        [FirestoreProperty]
        public double KostenPerMaand { get; set; }
    
        [FirestoreProperty]
        public int CPU { get; set; }

        [FirestoreProperty]
        public string DiskType { get; set; }
    }
}
