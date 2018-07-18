/*
  Uses https://github.com/mbdavid/LiteDB
   - Docs: https://github.com/mbdavid/LiteDB/wiki
 */
using LiteDB;
using System.Collections.Generic;
using System.IO;

namespace DatabaseIO
{
  public class LocalDb
  {
    // Used to generate the database filename; REQUIRED
    public string Name { get; private set; }
    // Database file directory; REQUIRED
    public string Uri { get; private set; }
    // Used to generate the database filename; OPTIONAL
    public string DbFilePrefix { get; private set; }

    public LiteDatabase db { get; private set; }

    public LocalDb(string name, string uri, string prefix = "")
    {
      this.Name = name;
      this.Uri = uri;
      this.DbFilePrefix = prefix;

      this.db = new LiteDatabase(this.DbUri);
    }

    // Returns pre-formatted fully-qualified uri for database
    public string DbUri
    {
      get
      {
        var prefix = (DbFilePrefix != "" && DbFilePrefix != null) ? $"{DbFilePrefix}." : "";
        return $"{this.Uri}\\{prefix}{this.Name}.db";
      }
    }

    public LiteCollection<T> GetCollection<T>(string name)
    {
      return this.db.GetCollection<T>(name);
    }

    public void InsertDocument<T>(LiteCollection<T> collection, T data)
    {
      collection.Insert(data);
    }

    public void InsertBulkDocuments<T>(LiteCollection<T> collection, IEnumerable<T> data)
    {
      collection.InsertBulk(data);
    }

    public void UpdateDocument<T>(LiteCollection<T> collection, T data)
    {
      collection.Update(data);
    }

    public void DeleteDocument<T>(LiteCollection<T> collection, ObjectId id)
    {
      collection.Delete(id);
    }

    public void DropCollection(string name)
    {
      this.db.DropCollection(name);
    }

    public void Destroy() {
      if(File.Exists(this.DbUri))
      {
          File.Delete(this.DbUri);
      }
    }
  }
}