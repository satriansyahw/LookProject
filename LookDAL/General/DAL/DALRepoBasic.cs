using EFHelper;
using LookDAL.General.IFace;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LookDAL.General.DAL
{
    public class DALRepoBasic : IRepoBasic
    {
        DALHapusActiveBoolAsync hapus;
        DALListDataAsync list;
        DALSearchByIdAsync search;
        DALSimpanAsync simpan;
        DALUpdateAsync ubah;
        public virtual async Task<bool> HapusActiveBoolAsync<T>(int identityID, string picDelete) where T : class
        {
            hapus = new DALHapusActiveBoolAsync();
            return await hapus.HapusActiveBoolAsync<T>(identityID, picDelete);
        }

        public virtual async Task<bool> HapusActiveBoolAsync<T>(List<int> listIdentityID, string picDelete) where T : class
        {
            hapus = new DALHapusActiveBoolAsync();
            return await hapus.HapusActiveBoolAsync<T>(listIdentityID, picDelete);
        }

        public virtual async Task<List<TResult>> ListDataAsync<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn = "", bool isascending = false, int toptake = 100)
            where TSource : class
            where TResult : class
        {
            list = new DALListDataAsync();
            return await list.ListDataAsync<TSource, TResult>(SearchFieldList, sortColumn, isascending, toptake);
        }

        public virtual async Task<T> SearchByIdAsync<T>(int Id) where T : class
        {
            search = new DALSearchByIdAsync();
            return await search.SearchByIdAsync<T>(Id);
        }

        public virtual async Task<List<T>> SearchByIdAsync<T>(List<int> listId) where T : class
        {
            search = new DALSearchByIdAsync();
            return await search.SearchByIdAsync<T>(listId);
        }

        public virtual async Task<List<T>> SimpanAsync<T>(List<T> listEntity) where T : class
        {
            simpan = new DALSimpanAsync();
            return await simpan.SimpanAsync<T>(listEntity);
        }

        public virtual async Task<T> SimpanAsync<T>(T entity) where T : class
        {
            simpan = new DALSimpanAsync();
            return await simpan.SimpanAsync<T>(entity);
        }

        public virtual async Task<bool> UbahAsync<T>(T entity) where T : class
        {
            ubah = new DALUpdateAsync();
            return await ubah.UbahAsync<T>(entity);

        }

        public virtual async Task<bool> UbahAsync<T>(List<T> listEntity) where T : class
        {
            ubah = new DALUpdateAsync();
            return await ubah.UbahAsync<T>(listEntity);
        }
    }
}
