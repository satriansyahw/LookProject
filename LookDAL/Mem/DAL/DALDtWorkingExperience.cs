using EFHelper;
using LookDAL.General.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LookDAL.Mem.DAL
{
    public class DALDtWorkingExperience : DALRepoBasic
    {
        public DALDtWorkingExperience()
        {

        }

        public override async Task<bool> HapusActiveBoolAsync<T>(int identityID, string picDelete)
        {
            return await base.HapusActiveBoolAsync<T>(identityID, picDelete);
        }

        public override async Task<bool> HapusActiveBoolAsync<T>(List<int> listIdentityID, string picDelete) 
        {
            return await base.HapusActiveBoolAsync<T>(listIdentityID, picDelete);
        }

        public override async Task<List<TResult>> ListDataAsync<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn = "", bool isascending = false, int toptake = 100)
        {
            return await base.ListDataAsync<TSource, TResult>(SearchFieldList, sortColumn, isascending, toptake);
        }
        public override async Task<T> SearchByIdAsync<T>(int Id) 
        {
            return await base.SearchByIdAsync<T>(Id);
        }

        public override async Task<List<T>> SearchByIdAsync<T>(List<int> listId) 
        {
            return await base.SearchByIdAsync<T>(listId);
        }

        public override async Task<List<T>> SimpanAsync<T>(List<T> listEntity) 
        {
            return await base.SimpanAsync<T>(listEntity);
        }

        public override async Task<T> SimpanAsync<T>(T entity) 
        {
            return await base.SimpanAsync<T>(entity);
        }

        public override async Task<bool> UbahAsync<T>(T entity) 
        {
            return await base.UbahAsync<T>(entity);
        }
        public override async Task<bool> UbahAsync<T>(List<T> listEntity) 
        {
            return await base.UbahAsync<T>(listEntity);
        }
    }
}
