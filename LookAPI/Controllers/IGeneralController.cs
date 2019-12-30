using GenHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookWeb.Controllers
{
    interface IGeneralController
    {
        Task<byte[]> SimpanAsync<T>(List<T> listEntity) where T : class;
        Task<byte[]> SimpanAsync<T>(T entity) where T : class;
        Task<byte[]> UbahAsync<T>(T entity) where T : class;
        Task<byte[]> UbahAsync<T>(List<T> listEntity) where T : class;
        Task<byte[]> HapusActiveBoolAsync<T>(int identityID, string picDelete) where T : class;
        Task<byte[]> HapusActiveBoolAsync<T>(List<int> listIdentityID, string picDelete) where T : class;
        Task<byte[]> ListDataAsync<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn = "", bool isascending = false, int toptake = 100)
         where TSource : class where TResult : class;
        Task<byte[]> SearchByIdAsync<T>(int Id) where T : class;
        Task<byte[]> SearchByIdAsync<T>(List<int> listId) where T : class;
    }
}
