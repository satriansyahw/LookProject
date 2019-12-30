using EFHelper;
using GenHelper;
using LookDAL.General.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TesterClass
{
    public class DALControllerBasic:IControllerBasic
    {
        APIReturn kembalian;
        DALRepoBasic repo;
        Helper help;
        public DALControllerBasic()
        {

        }

        public virtual async Task<APIReturn> HapusActiveBoolAsync<T>(int identityID, string picDelete) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            var hasil = await repo.HapusActiveBoolAsync<T>(identityID, picDelete);
            kembalian.Message = MessageInfo.APISuccess;
            kembalian.Data1 = hasil;
            return kembalian;
        }

        public virtual async Task<APIReturn> HapusActiveBoolAsync<T>(List<int> listIdentityID, string picDelete) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            var hasil = await repo.HapusActiveBoolAsync<T>(listIdentityID, picDelete);
            kembalian.Message = MessageInfo.APISuccess;
            kembalian.Data1 = hasil;
            return kembalian;
        }

        public virtual async Task<APIReturn> ListDataAsync<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn = "", bool isascending = false, int toptake = 100)
            where TSource : class
            where TResult : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            help = new Helper();
            var hasil = await repo.ListDataAsync<TSource, TResult>(SearchFieldList, sortColumn, isascending, toptake);
            kembalian.Message = MessageInfo.APISuccess;
            kembalian.Data1 = hasil;
            return kembalian;
        }

        public virtual async Task<APIReturn> SearchByIdAsync<T>(int Id) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            var hasil = await repo.SearchByIdAsync<T>(Id);
            kembalian.Message = MessageInfo.APISuccess;
            kembalian.Data1 = hasil;
            return kembalian;
        }

        public virtual async Task<APIReturn> SearchByIdAsync<T>(List<int> listId) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            var hasil = await repo.SearchByIdAsync<T>(listId);
            kembalian.Message = MessageInfo.APISuccess;
            kembalian.Data1 = hasil;
            return kembalian;
        }

        public virtual async Task<APIReturn> SimpanAsync<T>(List<T> listEntity) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            var hasil = await repo.SimpanAsync<T>(listEntity);
            kembalian.Data1 = hasil;
            if (hasil != null)
            {
                kembalian.Message = MessageInfo.APISuccess;
            }
            else
            {
                kembalian.Message = MessageInfo.APIFailed;
            }
            return kembalian;
        }

        public virtual async Task<APIReturn> SimpanAsync<T>(T entity) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            var hasil = await repo.SimpanAsync<T>(entity);
            kembalian.Data1 = hasil;
            if (hasil != null)
            {
                kembalian.Message = MessageInfo.APISuccess;
            }
            else
            {
                kembalian.Message = MessageInfo.APIFailed;
            }
            return kembalian;
        }

        public virtual async Task<APIReturn> UbahAsync<T>(T entity) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            var hasil = await repo.UbahAsync<T>(entity);
            kembalian.Data1 = hasil;
            if (hasil)
            {
                kembalian.Message = MessageInfo.APISuccess;
            }
            else
            {
                kembalian.Message = MessageInfo.APIFailed;
            }
            return kembalian;
        }

        public virtual async Task<APIReturn> UbahAsync<T>(List<T> listEntity) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            var hasil = await repo.UbahAsync<T>(listEntity);
            kembalian.Data1 = hasil;
            if (hasil)
            {
                kembalian.Message = MessageInfo.APISuccess;
            }
            else
            {
                kembalian.Message = MessageInfo.APIFailed;
            }
            return kembalian;
        }
    }
}
