using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mine.Models;

namespace Mine.Services
{
    public class MockDataStore : IDataStore<ItemModel>
    {
        readonly List<ItemModel> items;

        public MockDataStore()
        {
            items = new List<ItemModel>()
            {
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Caption", Description="Well, I am the head.", Value = 8 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Radar", Description="Put me on as a necklace.", Value = 5 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Main Weapon", Description="I should always be in your primary hand.", Value = 2 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Utility Item", Description="ummm ok, put me on your left finger.", Value = 3 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Engine", Description="Always the foot!", Value = 9 },
               
            };
        }

        public async Task<bool> CreateAsync(ItemModel item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(ItemModel item)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<ItemModel> ReadAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ItemModel>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}