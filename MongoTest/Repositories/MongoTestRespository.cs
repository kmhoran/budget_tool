using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace MongoTest.Repositories
{
    public class MongoTestRespository: IMongoTestRepository
    {
        private IMongoCollection<Book> _books;

        public MongoTestRespository(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }

        public List<Book> GetBooks() => _books.Find(book => true).ToList();
        
        public Book GetBook(string id) => _books.Find<Book>(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book updateModel) => _books.ReplaceOne(book => book.Id == id, updateModel);

        public void Remove(Book toDelete) => _books.DeleteOne(book => book.Id == toDelete.Id);

        public void Remove(string id) => _books.DeleteOne(book => book.Id == id);
    }
}
