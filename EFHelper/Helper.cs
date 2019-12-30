
using GenHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EFHelper.Helper
{
    public class GenHelperEF
    {
        private static GenHelperEF instance;

        public static GenHelperEF Getinstance
        {
            get
            {
                if (instance == null)
                    instance = new GenHelperEF();
                return instance;
            }
        }
        public IEnumerable<DateTime> ListAllDates(DateTime dateFrom, DateTime dateTo)
        {
            List<DateTime> dateRange = new List<DateTime>();
            TimeSpan difference = (dateTo - dateFrom);
            for (int i = 0; i <= difference.Days; i++)
            {
                dateRange.Add(Convert.ToDateTime(dateFrom.AddDays(i).ToString("yyyy-MM-dd")));
            }
            return dateRange;
        }
        public Guid CreateCryptographicallySecureGuid()
        {
            Guid guid = Guid.NewGuid();
            try
            {

                using (var provider = System.Security.Cryptography.RandomNumberGenerator.Create())
                {
                    var bytes = new byte[16];
                    provider.GetBytes(bytes);
                    guid = new Guid(bytes);
                }
            }
            catch { }
            return guid;

        }
        public List<MemberInfo> GetListMemberInfo<T>(List<string> selectedField) where T : class
        {
            List<MemberInfo> list = new List<MemberInfo>();
            string itemlist = string.Empty;
            string itemcheck = string.Empty;
            var sourceType = typeof(T);

            foreach (string item in selectedField)
            {
                itemlist = item.ToLower().Trim();

                foreach (PropertyInfo prop in sourceType.GetRuntimeProperties())
                {
                    itemcheck = prop.Name.ToString().ToLower().Trim();
                    itemcheck = itemcheck.Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim();

                    if (itemlist == itemcheck)
                    {

                        list.Add((MemberInfo)prop);
                        break;
                    }
                }
            }
            return list;
        }
        public T SetColValue<T>(T entity, string colName, object value) where T : class
        {
            PropertyInfo pi = this.GetColumnProps<T>(colName.ToLower());
            if (pi != null)
            {
                if (pi.CanWrite)
                {
                    if (value == null)
                    {
                        colName = colName.ToLower().Trim().Replace("_", "");
                        if (colName == "insertby") value = string.Empty;
                        else if (colName == "insertbyid") value = 0;
                        else if (colName == "updatebyid") value = 0;
                        else if (colName == "updateby") value = string.Empty;
                        else if (colName == "insertdate") value = DateTime.Now;
                        else if (colName == "inserttime") value = DateTime.Now;
                        else if (colName == "updatedate") value = DateTime.Now;
                        else if (colName == "updatetime") value = DateTime.Now;
                        else if (colName == "activebool") value = true;
                        else if (colName == "boolactive") value = true;
                    }
                    //   pi.Attributes.HasFlag(UpdateItems.None);
                    //entity.GetType().
                    pi.SetValue(entity, value);

                }
            }
            return entity;
        }
        [Flags]
        public enum UpdateItems
        {
            None = 0,
            Update = 1,
        }

        public T SetColValue<T>(T entity, List<GenColNameValue> listGenColNameValue) where T : class
        {

            string colName = string.Empty;
            object value = null;
            foreach (var item in listGenColNameValue)
            {
                colName = item.ColName.ToLower().Trim();
                value = item.ColNameValue;
                PropertyInfo pi = this.GetColumnProps<T>(colName);
                if (pi != null)
                {
                    if (pi.CanWrite)
                    {
                        if (value == null)
                        {
                            colName = colName.Replace("_", "");
                            if (colName == "insertby") value = 0;
                            else if (colName == "insertbyid") value = 0;
                            else if (colName == "updatebyid") value = 0;
                            else if (colName == "updateby") value = string.Empty;
                            else if (colName == "insertdate") value = DateTime.Now;
                            else if (colName == "inserttime") value = DateTime.Now;
                            else if (colName == "updatedate") value = DateTime.Now;
                            else if (colName == "updatetime") value = DateTime.Now;
                            else if (colName == "activebool") value = true;
                            else if (colName == "boolactive") value = true;
                        }
                        pi.SetValue(entity, value);
                    }
                }
            }

            return entity;
        }

        public T SetColID<T>(T entity, int ID) where T : class
        {
            PropertyInfo pi = this.GetIdentityColumnProps<T>();
            if (pi == null)
            {
                pi = this.GetColumnProps<T>("id");
            }
            if (pi != null)
            {
                if (pi.CanWrite)
                {
                    pi.SetValue(entity, ID);
                }
            }

            return entity;
        }
        public T SetColIDZero<T>(T entity) where T : class
        {
            string myidentity = string.Empty;
            PropertyInfo pi = this.GetIdentityColumnProps<T>();
            if (pi == null)
            {
                pi = this.GetColumnProps<T>("id");
            }
            if (pi != null)
            {
                if (pi.CanWrite)
                {
                    pi.SetValue(entity, 0);
                }
            }
            return entity;
        }
        public List<T> SetColIDZero<T>(List<T> entityCollection) where T : class
        {
            for (int i = 0; i < entityCollection.Count; i++)
            {
                entityCollection[i] = SetColIDZero<T>(entityCollection[i]);
            }
            return entityCollection;
        }
        public SearchField SetActiveBoolOneParameter<T>() where T : class
        {
            SearchField sf = new SearchField();
            var entity = typeof(T);
            string itemcheck = string.Empty;

            foreach (PropertyInfo property in entity.GetRuntimeProperties())
            {
                if (!string.IsNullOrEmpty(property.Name))
                {
                    itemcheck = property.Name.ToString().ToLower().Trim();
                    itemcheck = itemcheck.Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim().Replace("_", "");

                    if (itemcheck == "activebool" || itemcheck == "boolactive")
                    {

                        sf.Name = property.Name;
                        sf.Operator = "=";
                        sf.Value1 = "true";
                        break;
                    }
                }
            }
            return sf;
        }
        public T SetColPicID<T>(T entity, string picid) where T : class
        {
            if (string.IsNullOrEmpty(picid))
            {
                picid = "System";
            }
            foreach (var property in entity.GetType().GetRuntimeProperties())
            {
                if (!string.IsNullOrEmpty(property.Name))
                {
                    if (property.Name.Trim().ToLower().Replace("_", "") == "picid")
                    {
                        if (property.CanWrite)
                        {
                            //  object list = property.GetValue(entity);
                            property.SetValue(entity, picid);
                        }
                        break;
                    }
                }

            }
            return entity;
        }
        public string GetValueByColname<T>(T entity, string[] colname) where T : class
        {
            bool adacolname = false;
            string hasil = string.Empty;
            if (entity != null & colname != null)
            {
                foreach (var property in entity.GetType().GetRuntimeProperties())
                {
                    adacolname = false;
                    string myprop = property.Name.Trim().ToLower();
                    if (!string.IsNullOrEmpty(myprop))
                    {
                        foreach (var mycolname in colname)
                        {
                            if (myprop == mycolname.Trim().ToLower())
                            {
                                adacolname = true;
                                break;
                            }
                        }
                    }
                    if (adacolname)
                    {
                        object value = property.GetValue(entity);
                        if (value != null)
                            hasil = value.ToString();
                        break;
                    }

                }
            }
            return hasil;
        }

        public Expression<Func<TSource, TResult>> SelectorTResult<TSource, TResult>() where TSource : class
        {

            try
            {
                string itemlist = string.Empty;
                string itemcheck = string.Empty;
                var sourceType = typeof(TSource);
                var resultType = typeof(TResult);
                List<string> selectedField = new List<string>();
                foreach (PropertyInfo prop in resultType.GetRuntimeProperties())
                {
                    itemcheck = prop.Name.ToString().ToLower().Trim();
                    itemcheck = itemcheck.Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim();
                    selectedField.Add(itemcheck);
                }
                List<MemberInfo> list = this.GetListMemberInfo<TSource>(selectedField);
                var parameter = Expression.Parameter(sourceType, "e");
                var bindings = list.Select(member => Expression.Bind(
                    resultType.GetProperty(member.Name), Expression.MakeMemberAccess(parameter, member)));
                var body = Expression.MemberInit(Expression.New(resultType), bindings);
                var selector = Expression.Lambda<Func<TSource, TResult>>(body, parameter);
                return selector;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }

        }
        public IQueryable<T> IncludeMultiple<T>(IQueryable<T> query,
        params string[] includes) where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        private string GetIdentityColumn<T>() where T : class
        {
            string result = string.Empty;
            T entity = (T)Activator.CreateInstance(typeof(T));

            foreach (var property in entity.GetType().GetRuntimeProperties())
            {

                var attributes = property.GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    if (attribute.GetType() == typeof(DatabaseGeneratedAttribute))
                    {
                        DatabaseGeneratedAttribute myidentity = (DatabaseGeneratedAttribute)attribute;
                        if (myidentity.DatabaseGeneratedOption.ToString().ToLower() == "identity")
                        {
                            result = property.Name.Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim();

                        }
                    }
                    if (!string.IsNullOrEmpty(result))
                    {
                        break;
                    }
                }
            }
            if (string.IsNullOrEmpty(result))
            {
                List<PropertyInfo> property = entity.GetType().GetRuntimeProperties().ToList();
                if (property != null)
                {
                    if (property.Count > 0)
                    {
                        result = property[0].Name.Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim();
                    }
                }
            }
            return result;
        }
        public PropertyInfo GetIdentityColumnProps<T>() where T : class
        {
            T entity = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo pi = null;
            foreach (PropertyInfo property in entity.GetType().GetRuntimeProperties())
            {

                var attributes = property.GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    if (attribute.GetType() == typeof(DatabaseGeneratedAttribute))
                    {
                        DatabaseGeneratedAttribute myidentity = (DatabaseGeneratedAttribute)attribute;
                        if (myidentity.DatabaseGeneratedOption.ToString().ToLower() == "identity")
                        {
                            pi = property;
                        }
                    }
                    if (pi != null)
                    {

                        break;
                    }
                }
            }
            if (pi == null)
            {
                List<PropertyInfo> property = entity.GetType().GetRuntimeProperties().ToList();
                if (property != null)
                {
                    if (property.Count > 0)
                    {
                        pi = property[0];
                    }
                }
            }
            return pi;
        }
        public PropertyInfo GetIdentityColumnPropsHeader<T>(string headerIdentityColName) where T : class
        {
            PropertyInfo pi = null;
            string[] myApprovalTransID = { headerIdentityColName, "transid", "trans_id" };
            pi = GenHelperEF.Getinstance.GetColumnProps<T>(myApprovalTransID);
            if (pi == null)
            {
                T entity = (T)Activator.CreateInstance(typeof(T));
                List<PropertyInfo> property = entity.GetType().GetRuntimeProperties().ToList();
                if (property != null)
                {
                    if (property.Count > 1)
                    {
                        pi = property[1];
                    }
                }
            }
            return pi;
        }
        public List<T> ActiveBoolColumnFalse<T>(List<int> identiyID, string pic) where T : class
        {
            List<T> entityCollection = new List<T>();
            for (int i = 0; i < identiyID.Count; i++)
            {
                T entity = (T)Activator.CreateInstance(typeof(T));
                entity = ActiveBoolColumnFalse<T>(identiyID[i], pic);
                if (entity != null)
                {
                    entityCollection.Add(entity);
                }
            }
            return entityCollection;
        }
        public List<T> ActiveBoolColumnFalse<T>(List<int> identiyID, int userId,string userByName) where T : class
        {
            List<T> entityCollection = new List<T>();
            for (int i = 0; i < identiyID.Count; i++)
            {
                T entity = (T)Activator.CreateInstance(typeof(T));
                entity = ActiveBoolColumnFalse<T>(identiyID[i], userId,userByName);
                if (entity != null)
                {
                    entityCollection.Add(entity);
                }
            }
            return entityCollection;
        }
        public T ActiveBoolColumnFalse<T>(int identiyID, string pic) where T : class
        {
            T entity = (T)Activator.CreateInstance(typeof(T));
            //Hashtable activeboolCol = new Hashtable();
            //activeboolCol.Add("updateby", pic);
            //activeboolCol.Add("updatedate", DateTime.Now);
            //activeboolCol.Add("updatetime", DateTime.Now);
            //activeboolCol.Add("activebool", false);
            //activeboolCol.Add("boolactive", false);
            object objpic = 0;
            string p2type = string.Empty;

            PropertyInfo p1 = this.GetIdentityColumnProps<T>();
            PropertyInfo p2 = this.GetColumnProps<T>(new string[] { "updateby","updatebyid","picid"});
            if (p2 == null) p2 = this.GetColumnProps<T>("picid");
            PropertyInfo p3 = this.GetColumnProps<T>("updatedate");
            if (p3 == null) p3 = this.GetColumnProps<T>("updatetime");
            PropertyInfo p4 = this.GetColumnProps<T>("activebool");
            if (p4 == null) p4 = this.GetColumnProps<T>("boolactive");

            p2type = p2.PropertyType.Name.ToString().ToLower();
            if (p2type == "int16")
                objpic = Convert.ToInt16(pic);
            else if (p2type == "int32")
                objpic = Convert.ToInt32(pic);
            else if (p2type == "int64")
                objpic = Convert.ToInt64(pic);
            else
                objpic = pic;

            if (p1 != null & p2 != null & p3 != null & p4 != null)
            {
                if (p1.CanWrite & p2.CanWrite & p3.CanWrite & p4.CanWrite)
                {
                    p1.SetValue(entity, identiyID);
                    p2.SetValue(entity, objpic);
                    p3.SetValue(entity, DateTime.Now);
                    p4.SetValue(entity, false);

                }
            }


            return entity;
        }
        public T ActiveBoolColumnFalse<T>(int identiyID,int userId, string userByName) where T : class
        {
            T entity = (T)Activator.CreateInstance(typeof(T));
            //Hashtable activeboolCol = new Hashtable();
            //activeboolCol.Add("updateby", pic);
            //activeboolCol.Add("updatedate", DateTime.Now);
            //activeboolCol.Add("updatetime", DateTime.Now);
            //activeboolCol.Add("activebool", false);
            //activeboolCol.Add("boolactive", false);
           

            PropertyInfo p1 = this.GetIdentityColumnProps<T>();
            PropertyInfo p2 = this.GetColumnProps<T>(new string[] { "updatebyid", "picid" });
            if (p2 == null) p2 = this.GetColumnProps<T>("picid");
            PropertyInfo p2a = this.GetColumnProps<T>(new string[] { "updateby", "updatebyname" });
            if (p2a == null) p2a = this.GetColumnProps<T>("updateby");

            PropertyInfo p3 = this.GetColumnProps<T>("updatedate");
            if (p3 == null) p3 = this.GetColumnProps<T>("updatetime");
            PropertyInfo p4 = this.GetColumnProps<T>("activebool");
            if (p4 == null) p4 = this.GetColumnProps<T>("boolactive");

           
            if (p1 != null & p2 != null & p3 != null & p4 != null)
            {
                if (p1.CanWrite & p2.CanWrite & p3.CanWrite & p4.CanWrite)
                {
                    p1.SetValue(entity, identiyID);
                    p2.SetValue(entity, userId);
                    p2a.SetValue(entity, userByName);
                    p3.SetValue(entity, DateTime.Now);
                    p4.SetValue(entity, false);

                }
            }


            return entity;
        }

        public PropertyInfo GetColumnProps<T>(string colname) where T : class
        {
            T entity = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo pi = null;
            string myProp = string.Empty;
            foreach (var property in entity.GetType().GetRuntimeProperties())
            {
                if (!string.IsNullOrEmpty(property.Name))
                {
                    if (colname.ToLower().Replace("_", "") == "activebool" || colname.ToLower().Replace("_", "") == "boolactive"
                        || colname.ToLower().Replace("_", "") == "inserttime" || colname.ToLower().Replace("_", "") == "insertdate"
                        || colname.ToLower().Replace("_", "") == "updatetime" || colname.ToLower().Replace("_", "") == "updatedate"
                        || colname.ToLower().Replace("_", "") == "insertby" || colname.ToLower().Replace("_", "") == "updateby"
                        || colname.ToLower().Replace("_", "") == "insertbyid" || colname.ToLower().Replace("_", "") == "updatebyid")
                    {
                        colname = colname.Trim().ToLower().Replace("_", "").Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim();
                        myProp = property.Name.Trim().ToLower().Replace("_", "").Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim();
                    }
                    else
                    {
                        myProp = property.Name.Trim().ToLower().Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim();
                    }
                    if (myProp == colname.Trim().ToLower())
                    {
                        pi = property;
                    }
                }
                if (pi != null)
                {

                    break;
                }

            }

            return pi;
        }
        public PropertyInfo GetColumnProps<T>(string[] colnames) where T : class
        {
            T entity = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo pi = null;
            string myProp = string.Empty;
            string myCheckColname = string.Empty;
            foreach (var colname in colnames)
            {
                pi = null;
                foreach (var property in entity.GetType().GetRuntimeProperties())
                {
                    if (!string.IsNullOrEmpty(property.Name))
                    {
                        if (colname.ToLower().Replace("_", "") == "activebool" || colname.ToLower().Replace("_", "") == "boolactive"
                            || colname.ToLower().Replace("_", "") == "inserttime" || colname.ToLower().Replace("_", "") == "insertdate"
                            || colname.ToLower().Replace("_", "") == "updatetime" || colname.ToLower().Replace("_", "") == "updatedate"
                            || colname.ToLower().Replace("_", "") == "insertby" || colname.ToLower().Replace("_", "") == "updateby"
                            || colname.ToLower().Replace("_", "") == "insertbyid" || colname.ToLower().Replace("_", "") == "updatebyid")
                        {
                            myCheckColname = colname.Trim().ToLower().Replace("_", "").Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim();
                            myProp = property.Name.Trim().ToLower().Replace("_", "").Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim();
                        }
                        else
                        {
                            myProp = property.Name.Trim().ToLower().Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim();
                            myCheckColname = colname.Trim().ToLower().Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim();
                        }
                        if (myProp == colname.Trim().ToLower())
                        {
                            pi = property;
                        }
                    }
                    if (pi != null)
                    {
                        break;
                    }

                }
                if (pi != null)
                {
                    break;
                }
            }


            return pi;
        }
        public object GetConvertedValue(string _Fieldtype, object myvalue)
        {

            _Fieldtype = _Fieldtype.Trim().ToLower();
            if (myvalue != null)
            {
                if (_Fieldtype == "int32")
                {
                    return Convert.ToInt32(myvalue);
                }
                else if (_Fieldtype == "int16")
                {
                    return Convert.ToInt16(myvalue);
                }
                else if (_Fieldtype == "int64")
                {
                    return Convert.ToInt64(myvalue);
                }
                else if (_Fieldtype == "single")
                {

                    return Convert.ToSingle(myvalue);
                }
                else if (_Fieldtype == "double")
                {
                    return Convert.ToDouble(myvalue);

                }
                else if (_Fieldtype == "char")
                {
                    return Convert.ToChar(myvalue);
                }
                else if (_Fieldtype == "boolean")
                {
                    return Convert.ToBoolean(myvalue);
                }
                else if (_Fieldtype == "string")
                {
                    return myvalue;
                }
                else if (_Fieldtype == "guid")
                {
                    return new Guid(myvalue.ToString());
                }
                else if (_Fieldtype == "decimal")
                {
                    return Convert.ToDecimal(myvalue);
                }
                else if (_Fieldtype == "datetime")
                {
                    return Convert.ToDateTime(Convert.ToDateTime(myvalue).ToString("yyyy-MM-dd HH:mm:ss"));

                }
                else if (_Fieldtype == "byte")
                {
                    return Convert.ToByte(myvalue);

                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        public string GetPropertyType(PropertyInfo pi)
        {
            string result = string.Empty;
            string _nulltype = "nullable`1";
            string check1 = pi.PropertyType.Name.ToLower().Trim();
            if (check1 == _nulltype)
            {
                string _fullname = pi.PropertyType.FullName.ToLower().Split(',')[0].ToString();
                result = _fullname.Replace("system." + _nulltype + "[[", string.Empty).Replace("system.", string.Empty);
            }
            else
            {
                result = check1;
            }
            return result;
        }
        public T CopyClass<T>(T entity) where T : class
        {
            T result = (T)Activator.CreateInstance(typeof(T));
            foreach (var item in result.GetType().GetRuntimeProperties())
            {
                item.SetValue(result, item.GetValue(entity));
            }
            return result;
        }
        public List<T> CopyClass<T>(List<T> entity) where T : class
        {
            List<T> result = new List<T>();
            foreach (var item in entity)
            {
                result.Add(CopyClass<T>(item));
            }
            return result;
        }
    }
    public class GenColNameValue
    {
        public string ColName { get; set; }
        public object ColNameValue { get; set; }
    }
   
}

