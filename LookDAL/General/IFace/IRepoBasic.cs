using EFHelper;
using GenHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LookDAL.General.IFace
{
    interface IRepoBasic
    {
        Task<List<T>> SimpanAsync<T>(List<T> listEntity) where T : class;
        Task<T> SimpanAsync<T>(T entity) where T : class;
        Task<bool> UbahAsync<T>(T entity) where T : class;
        Task<bool> UbahAsync<T>(List<T> listEntity) where T : class;
        Task<bool> HapusActiveBoolAsync<T>(int identityID, string picDelete) where T : class;
        Task<bool> HapusActiveBoolAsync<T>(List<int> listIdentityID, string picDelete) where T : class;
        Task<List<TResult>> ListDataAsync<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn = "", bool isascending = false, int toptake = 100)
       where TSource : class where TResult : class;
        Task<T> SearchByIdAsync<T>(int Id) where T : class;
        Task<List<T>> SearchByIdAsync<T>(List<int> listId) where T : class;


    }
}
