/*
  Uses https://github.com/mbdavid/LiteDB
   - Docs: https://github.com/mbdavid/LiteDB/wiki
 */
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using DatabaseIO;
using TestModels;

namespace CodeExamples
{
  public class Program
  {
    [STAThread]
    static void Main(string[] args)
    {
      var dbUri = @"E:\Files\C#\LocalDbManager";
      var dbManager = DatabaseIO.DbManager.Instance;
      
      Destroy(@"E:\Files\C#\LocalDbManager\sampleNoPrefix.db");
      Destroy(@"E:\Files\C#\LocalDbManager\sampleWithPrefix.db");

      dbManager.AddDatabase(new LocalDb("sampleNoPrefix", dbUri));
      dbManager.AddDatabase(new LocalDb("sampleWithPrefix", dbUri, "test"));

      // Create your new customer instance
      var customer = new Customer
      {
        Name = "John Doe",
        Phones = new string[] { "8000-0000", "9000-0000" },
        Age = 39,
        IsActive = true
      };

      foreach (LocalDb db in dbManager.GetDatabases())
      {
        // Get customer collection
        var col = db.GetCollection<Customer>("customers");
        // Create unique index in Name field
        col.EnsureIndex(x => x.Name, true);

        if(col.Count() < 1) {
          db.InsertDocument<Customer>(col, customer);
        } else {
          // Update a document inside a collection
          // customer.Name = "Joana Doe";
          // db.UpdateData<Customer>(col, customer);
        }

        // Use LINQ to query documents (with no index)
        var results = col.Find(x => x.Age > 20);
        Console.Write($"\n{db.Name} Results: " + results.ToArray().Count());
        foreach (Customer cust in results.ToArray())
        {
          Console.Write("\n" + cust.ToString());
        }
      }

      Console.WriteLine("\nEnd.");
      Console.ReadKey();
    }
    
    public static void Destroy(string fileUri) {
      if(File.Exists(fileUri))
      {
          File.Delete(fileUri);
      }
    }
  }

}