using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using TodoApiNet.Contexts;
using TodoApiNet.Extensions;
using TodoApiNet.Models;

namespace TodoApiNet.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IMongoCollection<Todo> _context;

        public TodoRepository(IMongoDBSettings context)
        {
            var client = new MongoClient(context.ConnectionString);
            var database = client.GetDatabase(context.Database);
            _context = database.GetCollection<Todo>("Todos");
        }

        /// <summary>
        /// GET
        /// </summary>

        #region snippet_GetAll

        public async Task<IEnumerable<Todo>> GetAllAsync(Request queryParameters) => 
            await MongoDBFilter<Todo>.GetDocuments(_context, queryParameters,  new Todo().Relations);

        #endregion

        #region snippet_GetById

        public async Task<Todo> GetByIdAsync(string id) => await _context.Find<Todo>(todo => todo.Id == id).FirstOrDefaultAsync();

        #endregion

        /// <summary>
        /// POST
        /// </summary>

        #region snippet_Create

        public async Task Create(Todo todo) => await _context.InsertOneAsync(todo);

        #endregion

        /// <summary>
        /// PATCH
        /// </summary>

        #region snippet_Update

        public async Task UpdateAsync(string id, Todo newTodo, JsonPatchDocument<Todo> currentTodo)
        {
            currentTodo.ApplyTo(newTodo);
            await _context.ReplaceOneAsync(todo => todo.Id == id, newTodo);
        }

        #endregion

        /// <summary>
        /// DELETE
        /// </summary>

        #region snippet_Delete

        public async Task DeleteAsync(string id) => await _context.DeleteOneAsync(todo => todo.Id == id);

        #endregion

        /// <summary>
        /// COUNT
        /// </summary>

        #region snippet_Count

        public async Task<int> CountAsync(Request queryParameters) => await MongoDBFilter<Todo>.GetNumberOfDocuments(_context, queryParameters);

        #endregion
    }
}