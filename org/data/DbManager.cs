using System;
using System.Collections.Generic;

namespace DatabaseIO
{
  public sealed class DbManager
  {
    private static readonly Lazy<DbManager> lazy = new Lazy<DbManager>(() => new DbManager());

    public static DbManager Instance { get { return lazy.Value; }}

    private static List<LocalDb> LocalDbs = new List<LocalDb>();

    private DbManager() { }

    public Boolean AddDatabase(LocalDb db) {
      if(!this.HasDatabase(db.Name)) {
        DbManager.LocalDbs.Add(db);
        return true;
      }
      return false;
    }

    public Boolean HasDatabase(string name) {
      return DbManager.LocalDbs.Exists(x => x.Name == name);
    } 

    public LocalDb GetDatabase(string name) {
      return DbManager.LocalDbs.Find(x => x.Name == name);
    }

    public LocalDb[] GetDatabases() {
      return DbManager.LocalDbs.ToArray();
    }

    public Boolean RemoveDatabase(string name) {
      return DbManager.LocalDbs.Remove(new LocalDb(name, ""));
    }
  }
}