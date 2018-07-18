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
using NUnit.Framework;

using DatabaseIO;
using TestModels;

namespace LocalDbTests
{
  [TestFixture]
  class ProgramTests {
    [TestCase("testNoPrefix", @"E:\Files\C#\LocalDbManager\tests\dbs", "", @"E:\Files\C#\LocalDbManager\tests\dbs\testNoPrefix.db")]
    [TestCase("testNoPrefix", @"E:\Files\C#\LocalDbManager\tests\dbs", null, @"E:\Files\C#\LocalDbManager\tests\dbs\testNoPrefix.db")]
    [TestCase("testWithPrefix", @"E:\Files\C#\LocalDbManager\tests\dbs", "test", @"E:\Files\C#\LocalDbManager\tests\dbs\test.testWithPrefix.db")]
    public void Should_Return_DbUri_After_Creating_New_Database(string name, string uri, string prefix, string dbUri) {
      var localDb = new LocalDb(name, uri, prefix);
      Assert.AreEqual(localDb.DbUri, dbUri);
      localDb.Destroy();
    }

    [TestCase("customers", "insert_test")]
    public void Inserts_Customer_In_Customers_Collection(string name, string dbName) {
      this.Destroy($@"E:\Files\C#\LocalDbManager\tests\dbs\{dbName}.db");
      var localDb = new LocalDb(dbName, @"E:\Files\C#\LocalDbManager\tests\dbs");
      var col = localDb.GetCollection<Customer>(name);
      col.EnsureIndex(x => x.Name, true);

      var customer = new Customer
      {
        Name = "John Doe",
        Phones = new string[] { "8000-0000", "9000-0000" },
        Age = 39,
        IsActive = true
      };

      localDb.InsertDocument<Customer>(col, customer);
      var results = col.Find(x => x.Age > 20);
      Assert.AreEqual(results.ToArray().Count(), 1);
    }

    [TestCase("customers", "bulkInsert_test")]
    public void Bulk_Inserts_Customers_In_Customers_Collection(string name, string dbName) {
      this.Destroy($@"E:\Files\C#\LocalDbManager\tests\dbs\{dbName}.db");
      var localDb = new LocalDb(dbName, @"E:\Files\C#\LocalDbManager\tests\dbs");
      var col = localDb.GetCollection<Customer>(name);
      col.EnsureIndex(x => x.Name, true);

      var customer = new Customer
      {
        Name = "John Doe",
        Phones = new string[] { "8000-0000", "9000-0000" },
        Age = 39,
        IsActive = true
      };

      IEnumerable<Customer> customers() {
        yield return customer;
        yield return new Customer
        {
          Name = "John Doe1",
          Phones = new string[] { "8000-0000", "9000-0000" },
          Age = 39,
          IsActive = true
        };
        yield return new Customer
        {
          Name = "John Doe2",
          Phones = new string[] { "8000-0000", "9000-0000" },
          Age = 39,
          IsActive = true
        };
        yield return new Customer
        {
          Name = "John Doe3",
          Phones = new string[] { "8000-0000", "9000-0000" },
          Age = 39,
          IsActive = true
        };
        yield return new Customer
        {
          Name = "John Doe4",
          Phones = new string[] { "8000-0000", "9000-0000" },
          Age = 39,
          IsActive = true
        };
        yield return new Customer
        {
          Name = "John Doe5",
          Phones = new string[] { "8000-0000", "9000-0000" },
          Age = 39,
          IsActive = true
        };
        yield return new Customer
        {
          Name = "John Doe6",
          Phones = new string[] { "8000-0000", "9000-0000" },
          Age = 39,
          IsActive = true
        };
        yield return new Customer
        {
          Name = "John Doe7",
          Phones = new string[] { "8000-0000", "9000-0000" },
          Age = 39,
          IsActive = true
        };
        yield return new Customer
        {
          Name = "John Doe8",
          Phones = new string[] { "8000-0000", "9000-0000" },
          Age = 39,
          IsActive = true
        };
        yield return new Customer
        {
          Name = "John Doe9",
          Phones = new string[] { "8000-0000", "9000-0000" },
          Age = 39,
          IsActive = true
        };
      };

      localDb.InsertBulkDocuments<Customer>(col, customers());
      var results = col.Find(x => x.Age > 20);
      Assert.AreEqual(results.ToArray().Count(), 10);
    }

    [TestCase("customers", "update_test")]
    public void Update_Customer_In_Customers_Collection(string name, string dbName) {
      this.Destroy($@"E:\Files\C#\LocalDbManager\tests\dbs\{dbName}.db");
      var localDb = new LocalDb(dbName, @"E:\Files\C#\LocalDbManager\tests\dbs");
      var col = localDb.GetCollection<Customer>(name);
      col.EnsureIndex(x => x.Name, true);

      var customer = new Customer
      {
        Name = "John Doe",
        Phones = new string[] { "8000-0000", "9000-0000" },
        Age = 39,
        IsActive = true
      };

      localDb.InsertDocument<Customer>(col, customer);
      customer.Name = "Joana Doe";
      localDb.UpdateDocument<Customer>(col, customer);
      var count = col.Find(x => x.Age > 20);
      Assert.AreEqual(count.ToArray().Count(), 1);
      var results = col.Find(x => x.Name == customer.Name);
      Assert.AreEqual(results.ToArray().Count(), 1);
    }

    [TestCase("customers", "delete_test")]
    public void Delete_Customer_In_Customers_Collection(string name, string dbName) {
      this.Destroy($@"E:\Files\C#\LocalDbManager\tests\dbs\{dbName}.db");
      var localDb = new LocalDb(dbName, @"E:\Files\C#\LocalDbManager\tests\dbs");
      var col = localDb.GetCollection<Customer>(name);
      col.EnsureIndex(x => x.Name, true);

      var customer = new Customer
      {
        Name = "John Doe",
        Phones = new string[] { "8000-0000", "9000-0000" },
        Age = 39,
        IsActive = true
      };

      localDb.InsertDocument<Customer>(col, customer);
      var results = col.Find(x => x.Name == customer.Name);
      Assert.AreEqual(results.ToArray().Count(), 1);

      var cust = (Customer)results.ToArray()[0];
      localDb.DeleteDocument<Customer>(col, cust._id);
      var newResults = col.Find(x => x.Name == customer.Name);
      Assert.AreEqual(newResults.ToArray().Count(), 0);
    }

    private void Destroy(string fileUri) {
      if(File.Exists(fileUri))
      {
          File.Delete(fileUri);
      }
    }
  }

  
}