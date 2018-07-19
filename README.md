# c-sharp-libs
A collection of C# code and libraries I am working on.

## Files
### LocalDb.cs
A wrapper class around the [LiteDB](https://github.com/mbdavid/LiteDB) library. Methods for creating new NoSQL databases, adding new collections, and CRUD functionality for colections.

### DbManager.cs
The DbManager is a [mediator](https://en.wikipedia.org/wiki/Mediator_pattern) to use to manage any database connections needed by an application. Currently setup to work with the LocalDb class but flexible enough to work with other types of databases.

## Notes
 * Using NUnit for unit tests.