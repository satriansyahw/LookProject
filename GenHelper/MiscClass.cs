using System;
using System.Collections.Generic;
using System.Text;

namespace GenHelper
{
    public class ChunkData
    {
        public string DataChunk { get; set; }
        public string ChunkMaxCount { get; set; }
        public string ChunkCurrent { get; set; }
        public string ChunkKey { get; set; }
        public bool CompleteChunk { get; set; }
        public string FileName { get; set; }

    }
    public class SearchParameter
    {
        public List<SearchField> SearchFieldList { get; set; }
        public string SortColumn { get; set; }
        public bool IsAscending { get; set; }
        public int TopTake { get; set; }
    }
    public class DeleteByID
    {
        public int UserId { get; set; }
        public string UserByName { get; set; }
        public int IdentityId { get; set; }
    }
    public class DeleteByIDList
    {
        public int UserId { get; set; }
        public string UserByName { get; set; }
        public List<int> IdentityId { get; set; }
    }
    public class SearchField
    {
        public string Name { get; set; }
        public string Value1 { get; set; }
        public string Operator { get; set; }
    }
    public class SearchFieldType
    {
        public string Name { get; set; }
        public string Value1 { get; set; }
        public string Operator { get; set; }
        public Type FieldType { get; set; }
    }
    public class ParamSearchField
    {
        public string TabName { get; set; }
        public int TopTake { get; set; }
        public string SortColumn { get; set; }
        public bool IsSortAscending { get; set; }
        public string EmpnoLogin { get; set; }
        public List<SearchField> ListSearchField { get; set; }
        public bool AllDataBool { get; set; }

    }
    public class ParamCalendar
    {

        public short calTypeID { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }

    }
    public class APISetting
    {

        public string TokenKey { get; set; }
        public string TokenIssuer { get; set; }
        public string TokenAudience { get; set; }
        public string TokenAlgo { get; set; }
    }

}