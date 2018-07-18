using LiteDB;

namespace TestModels
{
  public class Customer
  {
    public ObjectId _id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string[] Phones { get; set; }
    public bool IsActive { get; set; }
    public override string ToString()
    {
      return _id + ": " + Name + ": " + Age;
    }
  }
}