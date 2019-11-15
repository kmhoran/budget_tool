using System;
using System.Collections.Generic;
using MongoTest.Repositories;

namespace MongoTest.Repositories
{
  public interface IMongoTestRepository
  {
      List<Book> GetBooks();
      Book GetBook(string id);
      Book Create(Book book);
      void Update(string id, Book updateModel);
      void Remove(Book toDelete);
      void Remove(string id);
  }
}