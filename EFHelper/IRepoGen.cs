using EFHelper.Helper;
using GenHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFHelper.Helper
{
    public interface IRepoGen
    {
        T ListByID<T>(int IDIdentity) where T:class;
        IEnumerable<T> List<T>() where T : class;
        IEnumerable<T> List<T>(List<SearchField> SearchFieldList) where T : class;
        IEnumerable<T> List<T>(List<SearchField> SearchFieldList, int toptake) where T : class;
        IEnumerable<T> List<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending) where T : class;
        IEnumerable<T> List<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where T : class;
        IEnumerable<T> ListByID<T>(List<int> IDIdentitylist) where T : class;
        Task<IEnumerable<T>> ListByIDAsync<T>(List<int> IDIdentitylist) where T : class;
        IEnumerable<TResult> ListTResultByID<TSource, TResult>(List<int> IDIdentitylist) where TSource : class where TResult : class;
        Task<IEnumerable<TResult>> ListTResultByIDAsync<TSource, TResult>(List<int> IDIdentitylist) where TSource : class where TResult : class;


        Task<T> ListByIDAsync<T>(params object[] IDIdentity) where T : class;
        Task<IEnumerable<T>> ListAsync<T>() where T : class;
        Task<IEnumerable<T>> ListAsync<T>(List<SearchField> SearchFieldList) where T : class;
        Task<IEnumerable<T>> ListAsync<T>(List<SearchField> SearchFieldList, int toptake) where T : class;
        Task<IEnumerable<T>> ListAsync<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending) where T : class;
        Task<IEnumerable<T>> ListAsync<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where T : class;

        IEnumerable<T> ListInnerJoin<T>(params string[] colRelationsIdentity) where T : class;
        IEnumerable<T> ListInnerJoin<T>(List<SearchField> SearchFieldList, params string[] colRelationsIdentity) where T : class;
        IEnumerable<T> ListInnerJoin<T>(List<SearchField> SearchFieldList, int toptake, params string[] colRelationsIdentity) where T : class;
        IEnumerable<T> ListInnerJoin<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, params string[] colRelationsIdentity) where T : class;
        IEnumerable<T> ListInnerJoin<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake, params string[] colRelationsIdentity) where T : class;

        Task<IEnumerable<T>> ListInnerJoinAsync<T>(params string[] colRelationsIdentity) where T : class;
        Task<IEnumerable<T>> ListInnerJoinAsync<T>(List<SearchField> SearchFieldList, params string[] colRelationsIdentity) where T : class;
        Task<IEnumerable<T>> ListInnerJoinAsync<T>(List<SearchField> SearchFieldList, int toptake, params string[] colRelationsIdentity) where T : class;
        Task<IEnumerable<T>> ListInnerJoinAsync<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, params string[] colRelationsIdentity) where T : class;
        Task<IEnumerable<T>> ListInnerJoinAsync<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake, params string[] colRelationsIdentity) where T : class;

        IEnumerable<TResult> ListTResult<TSource, TResult>() where TSource : class where TResult : class;
        IEnumerable<TResult> ListTResult<TSource, TResult>(List<SearchField> SearchFieldList) where TSource : class where TResult : class;
        IEnumerable<TResult> ListTResult<TSource, TResult>(List<SearchField> SearchFieldList, int toptake) where TSource : class where TResult : class;
        IEnumerable<TResult> ListTResult<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn, bool isascending) where TSource : class where TResult : class;
        IEnumerable<TResult> ListTResult<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where TSource : class where TResult : class;

        Task<IEnumerable<TResult>> ListTResultAsync<TSource, TResult>() where TSource : class where TResult : class;
        Task<IEnumerable<TResult>> ListTResultAsync<TSource, TResult>(List<SearchField> SearchFieldList) where TSource : class where TResult : class;
        Task<IEnumerable<TResult>> ListTResultAsync<TSource, TResult>(List<SearchField> SearchFieldList, int toptake) where TSource : class where TResult : class;
        Task<IEnumerable<TResult>> ListTResultAsync<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn, bool isascending) where TSource : class where TResult : class;
        Task<IEnumerable<TResult>> ListTResultAsync<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where TSource : class where TResult : class;

        IEnumerable<TResult> ListJoinTResult<TResult>(IQueryable<TResult> queryable) where TResult : class;
        IEnumerable<TResult> ListJoinTResult<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList) where TResult : class;
        IEnumerable<TResult> ListJoinTResult<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList, int toptake) where TResult : class;
        IEnumerable<TResult> ListJoinTResult<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending) where TResult : class;
        IEnumerable<TResult> ListJoinTResult<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where TResult : class;

        Task<IEnumerable<TResult>> ListJoinTResultAsync<TResult>(IQueryable<TResult> queryable) where TResult : class;
        Task<IEnumerable<TResult>> ListJoinTResultAsync<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList) where TResult : class;
        Task<IEnumerable<TResult>> ListJoinTResultAsync<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList, int toptake) where TResult : class;
        Task<IEnumerable<TResult>> ListJoinTResultAsync<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending) where TResult : class;
        Task<IEnumerable<TResult>> ListJoinTResultAsync<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where TResult : class;

        T Save<T>(T entity) where T : class;
        IEnumerable<T> Save<T>(List<T> entityCollection) where T : class;
        Task<T> SaveAsync<T>(T entity) where T : class;
        Task<IEnumerable<T>> SaveAsync<T>(List<T> entityCollection) where T : class;

        bool Update<T>(T entity) where T : class;
        bool Update<T>(List<T> entityCollections) where T : class;
        bool UpdateAll<T>(T entity) where T : class;
        bool UpdateAll<T>(List<T> entityCollection) where T : class;

        Task<bool> UpdateAsync<T>(T entity) where T : class;
        Task<bool> UpdateAsync<T>(List<T> entityCollections) where T : class;
        Task<bool> UpdateAllAsync<T>(T entity) where T : class;
        Task<bool> UpdateAllAsync<T>(List<T> entityCollection) where T : class;

        bool Update<T>(T entity, params string[] whereColParams) where T : class;
        bool Update<T>(List<T> entityCollections, params string[] whereColParams) where T : class;
        Task<bool> UpdateAsync<T>(T entity, params string[] whereColParams) where T : class;
        Task<bool> UpdateAsync<T>(List<T> entityCollections, params string[] whereColParams) where T : class;

        bool Save<T1, T2>(T1 entity1, T2 entity2) where T1 : class where T2 : class ;
        bool Save<T1, T2, T3>(T1 entity1, T2 entity2, T3 entity3) where T1 : class where T2 : class where T3 : class;
        bool Save<T1, T2, T3, T4>(T1 entity1, T2 entity2, T3 entity3, T4 entity4) where T1 : class where T2 : class where T3 : class where T4 : class;
        bool Save<T1, T2, T3, T4, T5>(T1 entity1, T2 entity2, T3 entity3, T4 entity4, T5 entity5) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;

        bool Save<T1, T2>(List<T1> entityCollection1, List<T2> entityCollection25) where T1 : class where T2 : class;
        bool Save<T1, T2, T3>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection)where T1 : class where T2 : class where T3 : class;
        bool Save<T1, T2, T3, T4>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4)
        where T1 : class where T2 : class where T3 : class where T4 : class;
        bool Save<T1, T2, T3, T4, T5>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4, List<T5> entityCollection5)
        where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;

        Task<bool> SaveAsync<T1, T2>(T1 entity1, T2 entity2) where T1 : class where T2 : class;
        Task<bool> SaveAsync<T1, T2, T3>(T1 entity1, T2 entity2, T3 entity3) where T1 : class where T2 : class where T3 : class;
        Task<bool> SaveAsync<T1, T2, T3, T4>(T1 entity1, T2 entity2, T3 entity3, T4 entity4) where T1 : class where T2 : class where T3 : class where T4 : class;
        Task<bool> SaveAsync<T1, T2, T3, T4, T5>(T1 entity1, T2 entity2, T3 entity3, T4 entity4, T5 entity5) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;

        Task<bool> SaveAsync<T1, T2>(List<T1> entityCollection1, List<T2> entityCollection25) where T1 : class where T2 : class;
        Task<bool> SaveAsync<T1, T2, T3>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection) where T1 : class where T2 : class where T3 : class;
        Task<bool> SaveAsync<T1, T2, T3, T4>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4)
        where T1 : class where T2 : class where T3 : class where T4 : class;
        Task<bool> SaveAsync<T1, T2, T3, T4, T5>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4, List<T5> entityCollection5)
        where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;

        bool Update<T1, T2>(T1 entity1, T2 entity2) where T1 : class where T2 : class;
        bool Update<T1, T2, T3>(T1 entity1, T2 entity2, T3 entity3) where T1 : class where T2 : class where T3 : class;
        bool Update<T1, T2, T3, T4>(T1 entity1, T2 entity2, T3 entity3, T4 entity4) where T1 : class where T2 : class where T3 : class where T4 : class;
        bool Update<T1, T2, T3, T4, T5>(T1 entity1, T2 entity2, T3 entity3, T4 entity4, T5 entity5) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;

        bool Update<T1, T2>(List<T1> entityCollection1, List<T2> entityCollection2) where T1 : class where T2 : class;
        bool Update<T1, T2, T3>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection) where T1 : class where T2 : class where T3 : class;
        bool Update<T1, T2, T3, T4>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4)
        where T1 : class where T2 : class where T3 : class where T4 : class;
        bool Update<T1, T2, T3, T4, T5>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4, List<T5> entityCollection5)
        where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;

        Task<bool> UpdateAsync<T1, T2>(T1 entity1, T2 entity2) where T1 : class where T2 : class;
        Task<bool> UpdateAsync<T1, T2, T3>(T1 entity1, T2 entity2, T3 entity3) where T1 : class where T2 : class where T3 : class;
        Task<bool> UpdateAsync<T1, T2, T3, T4>(T1 entity1, T2 entity2, T3 entity3, T4 entity4) where T1 : class where T2 : class where T3 : class where T4 : class;
        Task<bool> UpdateAsync<T1, T2, T3, T4, T5>(T1 entity1, T2 entity2, T3 entity3, T4 entity4, T5 entity5) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;

        Task<bool> UpdateAsync<T1, T2>(List<T1> entityCollection1, List<T2> entityCollection25) where T1 : class where T2 : class;
        Task<bool> UpdateAsync<T1, T2, T3>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection) where T1 : class where T2 : class where T3 : class;
        Task<bool> UpdateAsync<T1, T2, T3, T4>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4)
        where T1 : class where T2 : class where T3 : class where T4 : class;
        Task<bool> UpdateAsync<T1, T2, T3, T4, T5>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4, List<T5> entityCollection5)
        where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;

        bool Delete<T>(T entity) where T : class;
        bool Delete<T>(List<T> entityCollection) where T : class;
        bool Delete<T>(int IDIdentity) where T : class;
        bool Delete<T>(List<int> IDIdentity) where T : class;
        bool DeleteActiveBool<T>(int identityID, string pic) where T : class;
        bool DeleteActiveBool<T>(List<int> identityID, string pic) where T : class;

        Task<bool> DeleteAsync<T>(T entity) where T : class;
        Task<bool> DeleteAsync<T>(List<T> entityCollection) where T : class;
        Task<bool> DeleteAsync<T>(int IDIdentity) where T : class;
        Task<bool> DeleteAsync<T>(List<int> IDIdentity) where T : class;
        Task<bool> DeleteActiveBoolAsync<T>(int identityID, string pic) where T : class;
        Task<bool> DeleteActiveBoolAsync<T>(List<int> identityID, string pic) where T : class;

        bool SaveHeaderDetail<T, T1>(T tblHeader, string IDConnectedColName, T1 tblDetail1)
             where T : class where T1 : class;
        bool SaveHeaderDetail<T, T1, T2>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2)
             where T : class where T1 : class where T2 : class;
        bool SaveHeaderDetail<T, T1, T2, T3>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2, T3 tblDetail3)
            where T : class where T1 : class where T2 : class where T3 : class;
        bool SaveHeaderDetail<T, T1, T2, T3, T4>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2, T3 tblDetail3, T4 tblDetail4)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class;
        bool SaveHeaderDetail<T, T1, T2, T3, T4, T5>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2, T3 tblDetail3, T4 tblDetail4, T5 tblDetail5)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;

        bool SaveHeaderDetail<T, T1>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1)
             where T : class where T1 : class;
        bool SaveHeaderDetail<T, T1, T2>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2)
             where T : class where T1 : class where T2 : class;
        bool SaveHeaderDetail<T, T1, T2, T3>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2, List<T3> tblDetail3)
            where T : class where T1 : class where T2 : class where T3 : class;
        bool SaveHeaderDetail<T, T1, T2, T3, T4>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2, List<T3> tblDetail3, List<T4> tblDetail4)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class;
        bool SaveHeaderDetail<T, T1, T2, T3, T4, T5>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2, List<T3> tblDetail3, List<T4> tblDetail, List<T5> tblDetail5)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;

        Task<bool> SaveHeaderDetailAsync<T, T1>(T tblHeader, string IDConnectedColName, T1 tblDetail1) where T : class where T1 : class;
        Task<bool> SaveHeaderDetailAsync<T, T1, T2>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2)  where T : class where T1 : class where T2 : class;
        Task<bool> SaveHeaderDetailAsync<T, T1, T2, T3>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2, T3 tblDetail3)  where T : class where T1 : class where T2 : class where T3 : class;
        Task<bool> SaveHeaderDetailAsync<T, T1, T2, T3, T4>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2, T3 tblDetail3, T4 tblDetail4)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class;
        Task<bool> SaveHeaderDetailAsync<T, T1, T2, T3, T4, T5>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2, T3 tblDetail3, T4 tblDetail4, T5 tblDetail5)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;

        Task<bool> SaveHeaderDetailAsync<T, T1>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1) where T : class where T1 : class;
        Task<bool> SaveHeaderDetailAsync<T, T1, T2>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2)
             where T : class where T1 : class where T2 : class;
        Task<bool> SaveHeaderDetailAsync<T, T1, T2, T3>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2, List<T3> tblDetail3)
            where T : class where T1 : class where T2 : class where T3 : class;
        Task<bool> SaveHeaderDetailAsync<T, T1, T2, T3, T4>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2, List<T3> tblDetail3, List<T4> tblDetail4)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class;
        Task<bool> SaveHeaderDetailAsync<T, T1, T2, T3, T4, T5>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2, List<T3> tblDetail3, List<T4> tblDetail, List<T5> tblDetail5)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;

        bool DeleteSaveHeader<T, T1>(int IDIdentity, string IDConnectedColName) where T : class where T1 : class;
        bool DeleteSaveHeader<T, T1, T2>(int IDIdentity, string IDConnectedColName) where T : class where T1 : class where T2 : class;
        bool DeleteSaveHeader<T, T1, T2, T3>(int IDIdentity, string IDConnectedColName)  where T : class where T1 : class where T2 : class where T3 : class;
        bool DeleteSaveHeader<T, T1, T2, T3, T4>(int IDIdentity, string IDConnectedColName) where T : class where T1 : class where T2 : class where T3 : class where T4 : class;
        bool DeleteSaveHeader<T, T1, T2, T3, T4, T5>(int IDIdentity, string IDConnectedColName)  where T : class where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;

        Task<bool> DeleteSaveHeaderAsync<T, T1>(int IDIdentity, string IDConnectedColName) where T : class where T1 : class;
        Task<bool> DeleteSaveHeaderAsync<T, T1, T2>(int IDIdentity, string IDConnectedColName) where T : class where T1 : class where T2 : class;
        Task<bool> DeleteSaveHeaderAsync<T, T1, T2, T3>(int IDIdentity, string IDConnectedColName) where T : class where T1 : class where T2 : class where T3 : class;
        Task<bool> DeleteSaveHeaderAsync<T, T1, T2, T3, T4>(int IDIdentity, string IDConnectedColName) where T : class where T1 : class where T2 : class where T3 : class where T4 : class;
        Task<bool> DeleteSaveHeaderAsync<T, T1, T2, T3, T4, T5>(int IDIdentity, string IDConnectedColName) where T : class where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;

        bool SaveUpdate<T1, T2, T3, T4, T5>(T1 entity1, T2 entity2, T3 entity3, T4 entity4, T5 entity5
            , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4, bool IsSaveT5) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;
        bool SaveUpdate<T1, T2, T3, T4>(T1 entity1, T2 entity2, T3 entity3, T4 entity4
           , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4) where T1 : class where T2 : class where T3 : class where T4 : class;
        bool SaveUpdate<T1, T2, T3>(T1 entity1, T2 entity2, T3 entity3
           , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3) where T1 : class where T2 : class where T3 : class;
        bool SaveUpdate<T1, T2>(T1 entity1, T2 entity2
           , bool IsSaveT1, bool IsSaveT2) where T1 : class where T2 : class;
        bool SaveUpdate<T1, T2, T3, T4, T5>(List<T1> listEntity1, List<T2> listEntity2, List<T3> listEntity3, List<T4> listEntity4, List<T5> listEntity5
            , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4, bool IsSaveT5) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;
        bool SaveUpdate<T1, T2, T3, T4>(List<T1> listEntity1, List<T2> listEntity2, List<T3> listEntity3, List<T4> listEntity4
            , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4) where T1 : class where T2 : class where T3 : class where T4 : class;
        bool SaveUpdate<T1, T2, T3>(List<T1> listEntity1, List<T2> listEntity2, List<T3> listEntity3
            , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3) where T1 : class where T2 : class where T3 : class;
        bool SaveUpdate<T1, T2>(List<T1> listEntity1, List<T2> listEntity2
            , bool IsSaveT1, bool IsSaveT2) where T1 : class where T2 : class;
        Task<bool> SaveUpdateAsync<T1, T2, T3, T4, T5>(T1 entity1, T2 entity2, T3 entity3, T4 entity4, T5 entity5
            , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4, bool IsSaveT5) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;
        Task<bool> SaveUpdateAsync<T1, T2, T3, T4>(T1 entity1, T2 entity2, T3 entity3, T4 entity4
           , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4) where T1 : class where T2 : class where T3 : class where T4 : class;
        Task<bool> SaveUpdateAsync<T1, T2, T3>(T1 entity1, T2 entity2, T3 entity3
           , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3) where T1 : class where T2 : class where T3 : class;
        Task<bool> SaveUpdateAsync<T1, T2>(T1 entity1, T2 entity2
           , bool IsSaveT1, bool IsSaveT2) where T1 : class where T2 : class;
        Task<bool> SaveUpdateAsync<T1, T2, T3, T4, T5>(List<T1> listEntity1, List<T2> listEntity2, List<T3> listEntity3, List<T4> listEntity4, List<T5> listEntity5
            , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4, bool IsSaveT5) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;
        Task<bool> SaveUpdateAsync<T1, T2, T3, T4>(List<T1> listEntity1, List<T2> listEntity2, List<T3> listEntity3, List<T4> listEntity4
            , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4) where T1 : class where T2 : class where T3 : class where T4 : class;
        Task<bool> SaveUpdateAsync<T1, T2, T3>(List<T1> listEntity1, List<T2> listEntity2, List<T3> listEntity3
            , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3) where T1 : class where T2 : class where T3 : class;
        Task<bool> SaveUpdateAsync<T1, T2>(List<T1> listEntity1, List<T2> listEntity2
            , bool IsSaveT1, bool IsSaveT2) where T1 : class where T2 : class;

    }
   
  
}
