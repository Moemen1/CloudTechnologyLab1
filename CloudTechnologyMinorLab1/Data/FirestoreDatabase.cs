using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudTechnologyMinorLab1.Data
{
    public class FirestoreDatabase
    {      
        public static FirestoreDb LoadDatabase()
        {
            FirestoreDb db;

            string path = AppDomain.CurrentDomain.BaseDirectory + @"CloudMinorLab1-Firestore.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            return db = FirestoreDb.Create("cloudminor-lab1");
        }
    }
}
