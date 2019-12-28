using EFHelper;
using LookDAL.General.DAL;
using LookDAL.Mem.IFace;
using LookDB.Model.Member;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LookDAL.Mem.DAL
{
    public class DALDtMember : DALRepoBasic,IDtMember
    {
        public DALDtMember()
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

        public virtual async Task<List<BufferClass>> SearchAllMemberInfoByMemberId(int id)
        {
            List<BufferClass> lbc = new List<BufferClass>();
            if (id > 0)
            {
                List<SearchField> lsf = new List<SearchField>();
                lsf.Add(new SearchField { Name="ID",Operator="=",Value1=id.ToString()});
                var hasilDtMember = base.ListDataAsync<DtMember, VW_DtMember>(lsf, "", false, 100);
                lsf = new List<SearchField>();
                lsf.Add(new SearchField { Name = "MemberID", Operator = "=", Value1 = id.ToString() });
                var hasilDtCertification = base.ListDataAsync<DtCertification, VW_DtCertification>(lsf, "", false, 100);
                var hasilDtEducation = base.ListDataAsync<DtEducation, VW_DtEducation>(lsf, "", false, 100);
                var hasilDtExpertise = base.ListDataAsync<DtExpertise, VW_DtExpertise>(lsf, "", false, 100);
                var hasilDtLanguage = base.ListDataAsync<DtLanguage, VW_DtLanguage>(lsf, "", false, 100);
                var hasilDtOrgExperience = base.ListDataAsync<DtOrgExperience, VW_DtOrgExperience>(lsf, "", false, 100);
                var hasilDtWorkingExperience = base.ListDataAsync<DtWorkingExperience, VW_DtWorkingExperience>(lsf, "", false, 100);
                var hasilDtWorkingInterest = base.ListDataAsync<DtWorkingInterest, VW_DtWorkingInterest>(lsf, "", false, 100);

                await Task.WhenAll(hasilDtMember, hasilDtCertification,hasilDtEducation,hasilDtExpertise,hasilDtLanguage
                    ,hasilDtOrgExperience,hasilDtWorkingExperience,hasilDtWorkingInterest);
              
                lbc.Add(new BufferClass { ObjectName = "Member", ObjectValue = hasilDtMember });
                lbc.Add(new BufferClass { ObjectName = "Certification", ObjectValue = hasilDtMember });
                lbc.Add(new BufferClass { ObjectName = "Education", ObjectValue = hasilDtMember });
                lbc.Add(new BufferClass { ObjectName = "Expertise", ObjectValue = hasilDtMember });
                lbc.Add(new BufferClass { ObjectName = "Language", ObjectValue = hasilDtMember });
                lbc.Add(new BufferClass { ObjectName = "OrgExperience", ObjectValue = hasilDtMember });
                lbc.Add(new BufferClass { ObjectName = "WorkingExprience", ObjectValue = hasilDtMember });
                lbc.Add(new BufferClass { ObjectName = "WorkingInterest", ObjectValue = hasilDtMember });
            }

            return lbc;
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
