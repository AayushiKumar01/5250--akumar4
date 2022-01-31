using Mine.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mine.Services
{
    public class DatabaseService : IDataStore<ItemModel>
    {
        static readonly Lazy<SQLiteAsyncConnection> LazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => LazyInitializer.Value;
        static bool initialized = false;

        public DatabaseService()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        private async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(ItemModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(ItemModel)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        /// <summary>
        /// add item to database and return true if item was successfully inserted, otherwise false
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(ItemModel item)
        {
            if(item == null)
            {
                return false;
            }
            var result = await Database.InsertAsync(item);

            if(result == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// updates an item record in DB
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ItemModel item)
        {
            if (item == null)
            {
                return false;
            }
            //update record in DB
            var result = await Database.UpdateAsync(item);

            if (result == 0)
            {
                return false;
            }

            return true;
        }

    /// <summary>
    /// delete item record in DM matching the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            var data = await ReadAsync(id);
            if(data == null)
            {
                return false;
            }

            //calls DB and deletes item record
            var result = await Database.DeleteAsync(data);
            if(result == 0)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// returns item record from DB matching the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ItemModel> ReadAsync(string id)
        {
            if(id == null)
            {
                return null;
            }
            
            //Using Linq syntax to retrieve the first record matching the ID 
            var result = Database.Table<ItemModel>().FirstOrDefaultAsync(m => m.Id.Equals(id));

            return result;

        }

        /// <summary>
        /// returns list of item
        /// </summary>
        /// <param name="forceRefresh"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ItemModel>> IndexAsync(bool forceRefresh = false)
        {
            //Call to the ToListAsync method on Database with the Table called ItemModel

            var result = await Database.Table<ItemModel>().ToListAsync();
            return result;
        }
    }
}
