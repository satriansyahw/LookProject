using GenHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TesterClass
{
    public interface IControllerBasic
    {
        Task<APIReturn> SimpanAsync<T>(List<T> listEntity) where T : class;
        Task<APIReturn> SimpanAsync<T>(T entity) where T : class;
        Task<APIReturn> UbahAsync<T>(T entity) where T : class;
        Task<APIReturn> UbahAsync<T>(List<T> listEntity) where T : class;
        Task<APIReturn> HapusActiveBoolAsync<T>(int identityID, string picDelete) where T : class;
        Task<APIReturn> HapusActiveBoolAsync<T>(List<int> listIdentityID, string picDelete) where T : class;
        Task<APIReturn> ListDataAsync<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn = "", bool isascending = false, int toptake = 100)
         where TSource : class where TResult : class;
        Task<APIReturn> SearchByIdAsync<T>(int Id) where T : class;
        Task<APIReturn> SearchByIdAsync<T>(List<int> listId) where T : class;
    }
}
