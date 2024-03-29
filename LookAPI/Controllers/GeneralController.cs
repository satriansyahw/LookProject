﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenHelper;
using LookDAL.General.DAL;
using Microsoft.AspNetCore.Mvc;

namespace LookWeb.Controllers
{
    public class GeneralController : IGeneralController
    {
        

        APIReturn kembalian;
        DALRepoBasic repo;
        Helper helper;
        public virtual async Task<byte[]> HapusActiveBoolAsync<T>(int identityID, string picDelete) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            helper = new Helper();
            var hasil = await repo.HapusActiveBoolAsync<T>(identityID, picDelete);
            kembalian.Message = MessageInfo.APISuccess;
            kembalian.Data1 = hasil;
            return helper.CompressedData(kembalian);
        }

        public virtual async Task<byte[]> HapusActiveBoolAsync<T>(List<int> listIdentityID, string picDelete) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            helper = new Helper();
            var hasil = await repo.HapusActiveBoolAsync<T>(listIdentityID, picDelete);
            kembalian.Message = MessageInfo.APISuccess;
            kembalian.Data1 = hasil;
            return helper.CompressedData(kembalian);
        }

        public virtual async Task<byte[]> ListDataAsync<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn = "", bool isascending = false, int toptake = 100)
            where TSource : class
            where TResult : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            helper = new Helper();
            var hasil = await repo.ListDataAsync<TSource, TResult>(SearchFieldList, sortColumn, isascending, toptake);
            kembalian.Message = MessageInfo.APISuccess;
            kembalian.Data1 = hasil;
            return helper.CompressedData(kembalian);
        }

        public virtual async Task<byte[]> SearchByIdAsync<T>(int Id) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            helper = new Helper();
            var hasil = await repo.SearchByIdAsync<T>(Id);
            kembalian.Message = MessageInfo.APISuccess;
            kembalian.Data1 = hasil;
            return helper.CompressedData(kembalian);
        }

        public virtual async Task<byte[]> SearchByIdAsync<T>(List<int> listId) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            helper = new Helper();
            var hasil = await repo.SearchByIdAsync<T>(listId);
            kembalian.Message = MessageInfo.APISuccess;
            kembalian.Data1 = hasil;
            return helper.CompressedData(kembalian);
        }

        public virtual async Task<byte[]> SimpanAsync<T>(List<T> listEntity) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            helper = new Helper();
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
            return helper.CompressedData(kembalian);
        }

        public virtual async Task<byte[]> SimpanAsync<T>(T entity) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            helper = new Helper();
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
            return helper.CompressedData(kembalian);
        }

        public virtual async Task<byte[]> UbahAsync<T>(T entity) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            helper = new Helper();
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
            return helper.CompressedData(kembalian);
        }

        public virtual async Task<byte[]> UbahAsync<T>(List<T> listEntity) where T : class
        {
            kembalian = new APIReturn();
            repo = new DALRepoBasic();
            helper = new Helper();
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
            return helper.CompressedData(kembalian);
        }
    }
}