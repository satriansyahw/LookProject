using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace EFHelper.Helper
{
    public class RepoGen:IRepoGen
    {
        /*testing using() ya/... juga coba tambahkan transaction pda save update delete: pada update aja ya
         *          
CommandTimeout is for the duration of a single command A transaction scope can have multiple commands Therefore @GertArnold answer is correc

            SAVE HEADERDETAIL...must use begin trans
             */
    
        DbContext mydbContextBridge;
        private string mydbname = string.Empty;
        /// <summary>
        /// Obsolete.. Not Supported lagi ya... karena tidak independent. Jgn Dipake... Cuma untuk kepentingan testing
        /// </summary>
        /// <param name="dbname">Hoax ... jgn dipake.... kecuali testing.. diset manual db name nya</param>
        //public RepoGen(string dbname)
        //{
        //    mydbname = dbname;
        //}

        /// <summary>
        /// Sending db context to generator .Satu instance RepoGen untuk satu konteks database, jika ingin mengakses database berbeda, 
        /// maka harus menginstance RepoGen dengan konteks database yang baru.Ex. RepoGen(new CheckDBContext());
        /// </summary>
        /// <param name="dbContext"></param>
        public RepoGen(DbContext dbContext)
        {

            mydbContextBridge = dbContext;
          
        }
        /// <summary>
        /// Mendapatkan database context dari instance kelas .Context class berdasarkan inputan dari parameter constructornya
        /// </summary>
        /// <returns></returns>
        public DbContext GetDbContext()
        {

            DbContext mydbContext = mydbContextBridge;
            //mydbContextBridge.Database.Initialize(force: false);
            var entity = Activator.CreateInstance(mydbContext.GetType());

            mydbContext = (DbContext)entity;

            //mydbname = mydbname.ToLower().Trim();
            //if (!string.IsNullOrEmpty(mydbname))
            //{
            //    if (mydbname == "checkdb")
            //    {
            //        mydbContext = new CheckDBContext();

            //    }
            //}
            mydbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            mydbContext.ChangeTracker.LazyLoadingEnabled = false;

            return mydbContext;
        }
        /*Generator Query*/
        private IEnumerable<T> ListGenerator<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where T : class
        {
            IEnumerable<T> result = null;
            //var context = GetDbContext();
            try
            {
                using (var context = GetDbContext())
                {
                    var queryable = context.Set<T>().AsQueryable();
                    queryable = FilterLinq.QueryGeneratorList<T>(queryable, SearchFieldList, sortColumn, isascending, toptake);
                    result = queryable.ToList();
                }
            }
            catch (Exception ex) { string a = ex.ToString(); }
            return result;

        }
        private async Task<IEnumerable<T>> ListGeneratorAsync<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where T : class
        {
            IEnumerable<T> result = null;
            try { 
            using (var context = GetDbContext())
            {
                var queryable = context.Set<T>().AsQueryable();
                string ha = queryable.ToString();
                queryable = FilterLinq.QueryGeneratorList<T>(queryable, SearchFieldList, sortColumn, isascending, toptake);
                result = await queryable.ToListAsync();
            }
            }
            catch (Exception ex) { string a = ex.ToString(); }

            return result;

        }
        private IEnumerable<T> ListGeneratorInnerJoin<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake, params string[] includes) where T : class
        {
            IEnumerable<T> result = null;
            try { 
            using (var context = GetDbContext())
            {
                var queryable = context.Set<T>().AsQueryable();
                queryable = FilterLinq.QueryGeneratorListInnerJoin<T>(queryable, SearchFieldList, sortColumn, isascending, toptake, includes);
                result = queryable.ToList();
            }
            }
            catch (Exception ex) { string a = ex.ToString(); }

            return result;

        }
        private async Task<IEnumerable<T>> ListGeneratorInnerJoinAsync<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake, params string[] includes) where T : class
        {
            IEnumerable<T> result = null;
            try { 
            using (var context = GetDbContext())
            {
                var queryable = context.Set<T>().AsQueryable();
                queryable = FilterLinq.QueryGeneratorListInnerJoin<T>(queryable, SearchFieldList, sortColumn, isascending, toptake, includes);
                result = await queryable.ToListAsync();
            }
            }
            catch (Exception ex) { string a = ex.ToString(); }

            return result;

        }
        private IEnumerable<TResult> ListGeneratorTResult<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where TSource : class where TResult : class
        {
            /*Untuk membuat hasil seleksi sesuai dengan column yang ada di TResult, jadi tidak semua column di TSource di pilih*/
            IEnumerable<TResult> result = null;
            try { 
            using (var context = GetDbContext())
            {
                var queryable = context.Set<TSource>().AsQueryable();
                queryable = FilterLinq.QueryGeneratorList<TSource>(queryable, SearchFieldList, sortColumn, isascending, toptake);
                Expression<Func<TSource, TResult>> selector = GenHelperEF.Getinstance.SelectorTResult<TSource, TResult>();
                if (selector != null)
                {

                    result = queryable.Select(selector).ToList();
                }
            }
            }
            catch (Exception ex) { string a = ex.ToString(); }

            return result;
        }
        private async Task<IEnumerable<TResult>> ListGeneratorTResultAsync<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where TSource : class where TResult : class
        {
            /*Untuk membuat hasil seleksi sesuai dengan column yang ada di TResult, jadi tidak semua column di TSource di pilih*/
            IEnumerable<TResult> result = null;
            try { 
            using (var context = GetDbContext())
            {

                var queryable = context.Set<TSource>().AsQueryable();
                queryable = FilterLinq.QueryGeneratorList<TSource>(queryable, SearchFieldList, sortColumn, isascending, toptake);
                Expression<Func<TSource, TResult>> selector = GenHelperEF.Getinstance.SelectorTResult<TSource, TResult>();
                if (selector != null)
                {
                    result = await queryable.Select(selector).ToListAsync();
                }
            }
            }
            catch (Exception ex) { string a = ex.ToString(); }

            return result;
        }
        private IEnumerable<TResult> ListGeneratorJoinTResult<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where TResult : class
        {
            /*Contoh Pemakaian*/
            //RepoGen repogen = new RepoGen(new CheckDBContext());
            //var context = repogen.GetDbContext();
            //var context2 = repogen.GetDbContext();
            //var CInnerJoin = (from aa in context.Set<MT_Dept>()
            //                  join bb in context.Set<MT_Division>() on aa.DivisionID equals bb.ID
            //                  // where aa.ID == 55
            //                  select new MT_Division2 { DivisionName = aa.PicID, DivisionCode = bb.DivisionName }).AsQueryable();
            //var ALeftJoin = (from aa in context2.Set<MT_Dept>()
            //                 join bb in context2.Set<MT_Division>() on aa.DivisionID equals bb.ID into tableTemp
            //                 //where aa.ID == 55
            //                 from m in tableTemp.DefaultIfEmpty()
            //                 select new MT_Division2 { DivisionName = m.PicID, DivisionCode = m.DivisionName }).AsQueryable();
            //var sample2 = repogen.ListGeneratorJoinTResult<MT_Division2>(ALeftJoin); //MT_Division2 is TResult
            IEnumerable<TResult> result = null;
            try { 
            queryable = FilterLinq.QueryGeneratorList<TResult>(queryable, SearchFieldList, sortColumn, isascending, toptake);
            result = queryable.ToList();
            }
            catch (Exception ex) { string a = ex.ToString(); }

            return result;
        }
        private async Task<IEnumerable<TResult>> ListGeneratorJoinTResultAsync<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where TResult : class
        {
            /*Contoh Pemakaian*/
            //RepoGen repogen = new RepoGen(new CheckDBContext());
            //var context = repogen.GetDbContext();
            //var context2 = repogen.GetDbContext();
            //var CInnerJoin = (from aa in context.Set<MT_Dept>()
            //                  join bb in context.Set<MT_Division>() on aa.DivisionID equals bb.ID
            //                  // where aa.ID == 55
            //                  select new MT_Division2 { DivisionName = aa.PicID, DivisionCode = bb.DivisionName }).AsQueryable();
            //var sample1 = repogen.ListGeneratorJoinTResult<MT_Division2>(CInnerJoin); //MT_Division2 is TResult
            //var ALeftJoin = (from aa in context2.Set<MT_Dept>()
            //                 join bb in context2.Set<MT_Division>() on aa.DivisionID equals bb.ID into tableTemp
            //                 //where aa.ID == 55
            //                 from m in tableTemp.DefaultIfEmpty()
            //                 select new MT_Division2 { DivisionName = m.PicID, DivisionCode = m.DivisionName }).AsQueryable();
            //var sample2 = repogen.ListGeneratorJoinTResult<MT_Division2>(ALeftJoin); //MT_Division2 is TResult
            IEnumerable<TResult> result = null;
            try
            {
                queryable = FilterLinq.QueryGeneratorList<TResult>(queryable, SearchFieldList, sortColumn, isascending, toptake);
                result = await queryable.ToListAsync();
            }
            catch (Exception ex) { string a = ex.ToString(); }

            return result;
        }
        private T PrepareEntityForUpdateNoSelect<T>(T entity) where T : class
        {
            T entityTemp = GenHelperEF.Getinstance.CopyClass<T>(entity);
            var colNull = FilterLinq.PropertyColNull<T>(entity);
            var colNotNull = FilterLinq.PropertyColNotNull<T>(entity);
            var entityNull = FilterLinq.PropertyColDefaultValue<T>(entity);

            if (colNull != null & colNull.Count() > 0)
            {
                foreach (var itemNull in colNull)
                {
                    var myValue = itemNull.GetValue(entityNull);
                    GenHelperEF.Getinstance.SetColValue<T>(entity, itemNull.Name, myValue);
                }
            }
            foreach (var itemNotNull in colNotNull)
            {
                var myValue = itemNotNull.GetValue(entityTemp);
                GenHelperEF.Getinstance.SetColValue<T>(entity, itemNotNull.Name, myValue);
            }
            return entity;
        }
        private T PrepareEntityForUpdate<T>(T entity) where T : class
        {
            var colNull = FilterLinq.PropertyColNull<T>(entity);
            var colNotNull = FilterLinq.PropertyColNotNull<T>(entity);
            var pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
            object identityID = 0;
            T entityTemp = GenHelperEF.Getinstance.CopyClass<T>(entity);
            if (pi != null)
            {
                identityID = pi.GetValue(entity);
                List<SearchField> lsf = new List<SearchField>();
                lsf.Add(new SearchField { Name = pi.Name, Operator = "=", Value1 = identityID.ToString() });
                var THasil = this.List<T>(lsf);
                if (THasil != null & colNull != null & THasil.Count() > 0)
                {
                    var myHasil = THasil.ToList()[0];
                    foreach (var itemNull in colNull)
                    {
                        var myValue = itemNull.GetValue(myHasil);
                        GenHelperEF.Getinstance.SetColValue<T>(entity, itemNull.Name, myValue);
                    }
                }

            }
            foreach (var itemNotNull in colNotNull)
            {
                var myValue = itemNotNull.GetValue(entityTemp);
                GenHelperEF.Getinstance.SetColValue<T>(entity, itemNotNull.Name, myValue);
            }
            return entity;
        }
        private async Task<T> PrepareEntityForUpdateAsync<T>(T entity) where T : class
        {
            var colNull = FilterLinq.PropertyColNull<T>(entity);
            var colNotNull = FilterLinq.PropertyColNotNull<T>(entity);
            var pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
            object identityID = 0;
            T entityTemp = GenHelperEF.Getinstance.CopyClass<T>(entity);
            if (pi != null)
            {
                identityID = pi.GetValue(entity);
                List<SearchField> lsf = new List<SearchField>();
                lsf.Add(new SearchField { Name = pi.Name, Operator = "=", Value1 = identityID.ToString() });
                var THasil = await this.ListAsync<T>(lsf);
                if (THasil != null & colNull != null & THasil.Count() > 0)
                {
                    var myHasil = THasil.ToList().SingleOrDefault();
                    foreach (var itemNull in colNull)
                    {
                        var myValue = itemNull.GetValue(myHasil);
                        GenHelperEF.Getinstance.SetColValue<T>(entity, itemNull.Name, myValue);
                    }
                }

            }
            foreach (var itemNotNull in colNotNull)
            {
                var myValue = itemNotNull.GetValue(entityTemp);
                GenHelperEF.Getinstance.SetColValue<T>(entity, itemNotNull.Name, myValue);
            }
            return entity;
        }

        private RepoGenPrepareUpdate<T> PrepareEntityForSaveUpdate<T>(T entity) where T : class
        {
            RepoGenPrepareUpdate<T> result = new RepoGenPrepareUpdate<T>();
            var colNull = FilterLinq.PropertyColNull<T>(entity);
            var colNotNull = FilterLinq.PropertyColNotNull<T>(entity);
            var pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
            object identityID = 0;
            T entityTemp = GenHelperEF.Getinstance.CopyClass<T>(entity);
            if (pi != null)
            {
                identityID = pi.GetValue(entity);
                List<SearchField> lsf = new List<SearchField>();
                lsf.Add(new SearchField { Name = pi.Name, Operator = "=", Value1 = identityID.ToString() });
                var THasil = this.List<T>(lsf);
                if (THasil != null & colNull != null & THasil.Count() > 0)
                {
                    var myHasil = THasil.ToList()[0];
                    foreach (var itemNull in colNull)
                    {
                        var myValue = itemNull.GetValue(myHasil);
                        GenHelperEF.Getinstance.SetColValue<T>(entity, itemNull.Name, myValue);
                    }
                }

            }
            foreach (var itemNotNull in colNotNull)
            {
                var myValue = itemNotNull.GetValue(entityTemp);
                GenHelperEF.Getinstance.SetColValue<T>(entity, itemNotNull.Name, myValue);
            }
            if (entity != null & colNotNull != null)
            {
                result.Entity = entity;
                result.PropertyInfos = colNotNull;
            }
            return result;
        }
        private List<RepoGenPrepareUpdate<T>> PrepareEntityForSaveUpdate<T>(List<T> listEntity) where T : class
        {
            List<RepoGenPrepareUpdate<T>> result = new List<RepoGenPrepareUpdate<T>>();
            foreach (var entity in listEntity)
            {
                var check = PrepareEntityForSaveUpdate<T>(entity);
                if (check != null)
                {
                    result.Add(check);
                }
            }
            return result;
        }
        private async Task<RepoGenPrepareUpdate<T>> PrepareEntityForSaveUpdateAsync<T>(T entity) where T : class
        {
            RepoGenPrepareUpdate<T> result = new RepoGenPrepareUpdate<T>();
            var colNull = FilterLinq.PropertyColNull<T>(entity);
            var colNotNull = FilterLinq.PropertyColNotNull<T>(entity);
            var pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
            object identityID = 0;
            T entityTemp = GenHelperEF.Getinstance.CopyClass<T>(entity);
            if (pi != null)
            {
                identityID = pi.GetValue(entity);
                List<SearchField> lsf = new List<SearchField>();
                lsf.Add(new SearchField { Name = pi.Name, Operator = "=", Value1 = identityID.ToString() });
                var THasil = await this.ListAsync<T>(lsf);
                if (THasil != null & colNull != null & THasil.Count() > 0)
                {
                    var myHasil = THasil.ToList().SingleOrDefault();
                    foreach (var itemNull in colNull)
                    {
                        var myValue = itemNull.GetValue(myHasil);
                        GenHelperEF.Getinstance.SetColValue<T>(entity, itemNull.Name, myValue);
                    }
                }

            }
            foreach (var itemNotNull in colNotNull)
            {
                var myValue = itemNotNull.GetValue(entityTemp);
                GenHelperEF.Getinstance.SetColValue<T>(entity, itemNotNull.Name, myValue);
            }
            if (entity != null & colNotNull != null)
            {
                result.Entity = entity;
                result.PropertyInfos = colNotNull;
            }
            return result;
        }
        private async Task<List<RepoGenPrepareUpdate<T>>> PrepareEntityForSaveUpdateAsync<T>(List<T> listEntity) where T : class
        {
            List<RepoGenPrepareUpdate<T>> result = new List<RepoGenPrepareUpdate<T>>();
            foreach (var entity in listEntity)
            {
                var check = await PrepareEntityForSaveUpdateAsync<T>(entity);
                if (check != null)
                {
                    result.Add(check);
                }
            }
            return result;
        }
        private async Task<List<T>> PrepareEntityForUpdateAsync<T>(List<T> entityCollection) where T : class
        {
            for (int i = 0; i < entityCollection.Count; i++)
            {
                var entity = entityCollection[i];
                entity = await PrepareEntityForUpdateAsync<T>(entity);
                entityCollection[i] = entity;
            }
            return entityCollection;
        }
        /*Generator CRUD.*/
        private DbContext UpdateNotNullModifiedGakJadiYa<T>(DbContext context, T entity) where T : class
        {
            var colNotNull = FilterLinq.PropertyColNotNull<T>(entity);
            entity = FilterLinq.PropertyColDefaultValue<T>(entity);

            context.Set<T>().Attach(entity);
            context.Entry(entity).State = EntityState.Unchanged;

            PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
            foreach (var property in colNotNull)
            {
                if (piID != property)
                {
                    object value = property.GetValue(entity);
                    if (value != null)
                    {
                        context.Entry(entity).Property(property.Name).IsModified = true;
                    }
                }
            }
            //foreach (var property in entity.GetType().GetRuntimeProperties())
            //{
            //    if (piID != property)
            //    {
            //        object value = property.GetValue(entity);
            //        if (value != null)
            //        {
            //            context.Entry(entity).Property(property.Name).IsModified = true;
            //        }
            //    }

            //}
            return context;
        }

        /// <summary>
        /// Metode Sync untuk Mendapatkan object dari suatu entity. Data activeBool=1,Menghasilkan lebih dari 1 row. Harus punya DatabaseGeneratedKey sebagai primaryKey Ya
        /// </summary>
        /// <typeparam name="T">List Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="IDIdentity">id identity dari table db . Ex : 100,7334</param>
        /// <returns></returns>
        public IEnumerable<T> ListByID<T>(List<int> IDIdentitylist) where T : class
        {
            List<SearchField> lsf = new List<SearchField>();
            PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
            string myID = string.Empty;
            if (piID != null & IDIdentitylist.Count() > 0)
            {
                foreach (var item in IDIdentitylist)
                {
                    myID += item.ToString() + "|";
                }
                myID = myID.Remove(myID.Length - 1, 1);
                lsf.Add(new SearchField { Name = piID.Name, Operator = "IN", Value1 = myID });
                var hasil = this.List<T>(lsf);
                hasil = hasil != null ? hasil.ToList() : null;
                return hasil;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Metode Async untuk Mendapatkan object dari suatu entity. Data activeBool=1,Menghasilkan lebih dari 1 row. Harus punya DatabaseGeneratedKey sebagai primaryKey Ya
        /// </summary>
        /// <typeparam name="T">List Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="IDIdentity">id identity dari table db . Ex : 100,7334</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ListByIDAsync<T>(List<int> IDIdentitylist) where T : class
        {
            List<SearchField> lsf = new List<SearchField>();
            PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
            string myID = string.Empty;
            if (piID != null & IDIdentitylist.Count() > 0)
            {
                foreach (var item in IDIdentitylist)
                {
                    myID += item.ToString() + "|";
                }
                myID = myID.Remove(myID.Length - 1, 1);
                lsf.Add(new SearchField { Name = piID.Name, Operator = "IN", Value1 = myID });
                var hasil = await this.ListAsync<T>(lsf);
                hasil = hasil != null ? hasil.ToList() : null;
                return hasil;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Metode Sync untuk Mendapatkan list data sesuai dengan column yang ada di TResult berdasarkan ID identity. Jadi intinya kita bisa memilih kolom apa saja di TSource
        /// untuk ditampilkan di TResult. Ex. Pada tabel MT_Karyawan kita hanya membutuhkan kolom emp_no dan emp_name, jadi kita tinggal buat kelas TResult 
        /// yang hanya berisi emp_no dan emp_name,struktur kolomnya harus sama dengan TSource ya.
        /// Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="TSource">Table entitas source. Ex. MT_Karyawan</typeparam>
        /// <typeparam name="TResult">Table entitas untuk menampung hasil query. Ex. MT_Karyawan_hasil </typeparam>
        /// <returns></returns>
        public IEnumerable<TResult> ListTResultByID<TSource, TResult>(List<int> IDIdentitylist) where TSource : class where TResult : class
        {
            List<SearchField> lsf = new List<SearchField>();
            PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<TSource>();
            string myID = string.Empty;
            if (piID != null & IDIdentitylist.Count() > 0)
            {
                foreach (var item in IDIdentitylist)
                {
                    myID += item.ToString() + "|";
                }
                myID = myID.Remove(myID.Length - 1, 1);
                lsf.Add(new SearchField { Name = piID.Name, Operator = "IN", Value1 = myID });
                var hasil = this.ListTResult<TSource, TResult>(lsf);
                hasil = hasil != null ? hasil.ToList() : null;
                return hasil;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Metode Async untuk Mendapatkan list data sesuai dengan column yang ada di TResult berdasarkan ID identity. Jadi intinya kita bisa memilih kolom apa saja di TSource
        /// untuk ditampilkan di TResult. Ex. Pada tabel MT_Karyawan kita hanya membutuhkan kolom emp_no dan emp_name, jadi kita tinggal buat kelas TResult 
        /// yang hanya berisi emp_no dan emp_name,struktur kolomnya harus sama dengan TSource ya.
        /// Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="TSource">Table entitas source. Ex. MT_Karyawan</typeparam>
        /// <typeparam name="TResult">Table entitas untuk menampung hasil query. Ex. MT_Karyawan_hasil </typeparam>
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> ListTResultByIDAsync<TSource, TResult>(List<int> IDIdentitylist) where TSource : class where TResult : class
        {
            List<SearchField> lsf = new List<SearchField>();
            PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<TSource>();
            string myID = string.Empty;
            if (piID != null & IDIdentitylist.Count() > 0)
            {
                foreach (var item in IDIdentitylist)
                {
                    myID += item.ToString() + "|";
                }
                myID = myID.Remove(myID.Length - 1, 1);
                lsf.Add(new SearchField { Name = piID.Name, Operator = "IN", Value1 = myID });
                var hasil = await this.ListTResultAsync<TSource, TResult>(lsf);
                hasil = hasil != null ? hasil.ToList() : null;
                return hasil;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Metode Sync untuk Mendapatkan object dari suatu entity. Data activeBool=1,Default 1 row. 
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="IDIdentity">id identity dari table db . Ex : 100,7334</param>
        /// <returns></returns>
        public T ListByID<T>(int IDIdentity) where T : class
        {
            T result = null;
            try { 
            using (var context = GetDbContext())
            {
                result = context.Set<T>().Find(IDIdentity);
            }
            }
            catch (Exception ex) { string a = ex.ToString(); }

            return result;

        }
        /// <summary>
        /// Metode Sync untuk Mendapatkan list dari suatu entity. Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <returns></returns>
        public IEnumerable<T> List<T>() where T : class
        {
            var result = this.ListGenerator<T>(null, string.Empty, false, 0);
            return result;

        }


        /// <summary>
        ///  Metode Sync untuk Mendapatkan list dari suatu entity. Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc
        /// </summary>  
        /// <typeparam name="T"></typeparam>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <returns></returns>
        public IEnumerable<T> List<T>(List<SearchField> SearchFieldList) where T : class
        {
            var result = this.ListGenerator<T>(SearchFieldList, string.Empty, false, 0);
            return result;

        }
        /// <summary>
        ///  Metode Sync untuk Mendapatkan list dari suatu entity. Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public IEnumerable<T> List<T>(List<SearchField> SearchFieldList, int toptake) where T : class
        {
            var result = this.ListGenerator<T>(SearchFieldList, string.Empty, false, toptake);
            return result;

        }
        /// <summary>
        /// Metode Sync untuk Mendapatkan list dari suatu entity. Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <returns></returns>
        public IEnumerable<T> List<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending) where T : class
        {
            var result = this.ListGenerator<T>(SearchFieldList, sortColumn, isascending, 0);
            return result;

        }
        /// <summary>
        /// Metode Sync untuk Mendapatkan list dari suatu entity. Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public IEnumerable<T> List<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where T : class
        {
            var result = this.ListGenerator<T>(SearchFieldList, sortColumn, isascending, toptake);
            return result;

        }

        /// <summary>
        /// Metode Async untuk Mendapatkan object dari suatu entity. Data activeBool=1,Default 1 row. 
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="IDIdentity">id identity dari table db . Ex : 100,7334</param>
        /// <returns></returns>
        public async Task<T> ListByIDAsync<T>(params object[] IDIdentity) where T : class
        {
            T result = null;
            try
            {
                using (var context = GetDbContext())
                {
                    result = await context.Set<T>().FindAsync(IDIdentity);
                }
            }
            catch (Exception) {
                string a = string.Empty;
            }
            return result;

        }
        /// <summary>
        /// Metode Async untuk Mendapatkan list dari suatu entity. Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ListAsync<T>() where T : class
        {
            var result = await this.ListGeneratorAsync<T>(null, string.Empty, false, 0);
            return result;

        }
        /// <summary>
        ///  Metode Async untuk Mendapatkan list dari suatu entity. Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc
        /// </summary>  
        /// <typeparam name="T"></typeparam>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ListAsync<T>(List<SearchField> SearchFieldList) where T : class
        {
            var result = await this.ListGeneratorAsync<T>(SearchFieldList, string.Empty, false, 0);
            return result;

        }
        /// <summary>
        ///  Metode Async untuk Mendapatkan list dari suatu entity. Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ListAsync<T>(List<SearchField> SearchFieldList, int toptake) where T : class
        {
            var result = await this.ListGeneratorAsync<T>(SearchFieldList, string.Empty, false, toptake);
            return result;

        }
        /// <summary>
        /// Metode Async untuk Mendapatkan list dari suatu entity. Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ListAsync<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending) where T : class
        {
            var result = await this.ListGeneratorAsync<T>(SearchFieldList, sortColumn, isascending, 0);
            return result;

        }
        /// <summary>
        /// Metode Async untuk Mendapatkan list dari suatu entity. Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ListAsync<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where T : class
        {
            var result = await this.ListGeneratorAsync<T>(SearchFieldList, sortColumn, isascending, toptake);
            return result;

        }

        /// <summary>
        /// Metode Sync untuk Mendapatkan list Inner Join   dari suatu tabel entity yang columnnya mempunyai relationship ID Identity Not Null dengan tabel lain. 
        /// Ex: MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi dan  akan menghasilkan list karyawan dan detail dari mt_posisi.colRelationsIdentity adalah Foreign Key dari kelas relasi
        ///  , bentuknya params string[] jadi silakan tambahkan koma koma ya
        /// . Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="colRelationsIdentity">Column tabel yang punya relation ID Identity Not Null dengan table lain.Ex.MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi </param>
        /// <returns></returns>
        public IEnumerable<T> ListInnerJoin<T>(params string[] colRelationsIdentity) where T : class
        {
            var result = this.ListGeneratorInnerJoin<T>(null, string.Empty, false, 0, colRelationsIdentity);
            return result;

        }
        /// <summary>
        /// Metode Sync untuk Mendapatkan list Inner Join   dari suatu tabel entity yang columnnya mempunyai relationship ID Identity Not Null dengan tabel lain. 
        /// Ex: MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi dan  akan menghasilkan list karyawan dan detail dari mt_posisi.colRelationsIdentity adalah Foreign Key dari kelas relasi
        ///  , bentuknya params string[] jadi silakan tambahkan koma koma ya
        /// . Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="colRelationsIdentity">Column tabel yang punya relation ID Identity Not Null dengan table lain.Ex.MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi </param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <returns></returns>
        public IEnumerable<T> ListInnerJoin<T>(List<SearchField> SearchFieldList, params string[] colRelationsIdentity) where T : class
        {
            var result = this.ListGeneratorInnerJoin<T>(SearchFieldList, string.Empty, false, 0, colRelationsIdentity);
            return result;

        }
        /// <summary>
        /// Metode Sync untuk Mendapatkan list Inner Join   dari suatu tabel entity yang columnnya mempunyai relationship ID Identity Not Null dengan tabel lain. 
        /// Ex: MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi dan  akan menghasilkan list karyawan dan detail dari mt_posisi.colRelationsIdentity adalah Foreign Key dari kelas relasi
        ///  , bentuknya params string[] jadi silakan tambahkan koma koma ya
        /// . Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="colRelationsIdentity">Column tabel yang punya relation ID Identity Not Null dengan table lain.Ex.MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi </param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public IEnumerable<T> ListInnerJoin<T>(List<SearchField> SearchFieldList, int toptake, params string[] colRelationsIdentity) where T : class
        {
            var result = this.ListGeneratorInnerJoin<T>(SearchFieldList, string.Empty, false, toptake, colRelationsIdentity);
            return result;

        }
        /// <summary>
        /// Metode Sync untuk Mendapatkan list Inner Join   dari suatu tabel entity yang columnnya mempunyai relationship ID Identity Not Null dengan tabel lain. 
        /// Ex: MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi dan  akan menghasilkan list karyawan dan detail dari mt_posisi.colRelationsIdentity adalah Foreign Key dari kelas relasi
        ///  , bentuknya params string[] jadi silakan tambahkan koma koma ya
        /// . Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="colRelationsIdentity">Column tabel yang punya relation ID Identity Not Null dengan table lain.Ex.MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi </param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <returns></returns>
        public IEnumerable<T> ListInnerJoin<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, params string[] colRelationsIdentity) where T : class
        {
            var result = this.ListGeneratorInnerJoin<T>(SearchFieldList, sortColumn, isascending, 0, colRelationsIdentity);
            return result;

        }
        /// <summary>
        /// Metode Sync untuk Mendapatkan list Inner Join   dari suatu tabel entity yang columnnya mempunyai relationship ID Identity Not Null dengan tabel lain. 
        /// Ex: MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi dan  akan menghasilkan list karyawan dan detail dari mt_posisi.colRelationsIdentity adalah Foreign Key dari kelas relasi
        ///  , bentuknya params string[] jadi silakan tambahkan koma koma ya
        /// . Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="colRelationsIdentity">Column tabel yang punya relation ID Identity Not Null dengan table lain.Ex.MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi </param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public IEnumerable<T> ListInnerJoin<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake, params string[] colRelationsIdentity) where T : class
        {
            var result = this.ListGeneratorInnerJoin<T>(SearchFieldList, sortColumn, isascending, toptake, colRelationsIdentity);
            return result;

        }

        /// <summary>
        /// Metode Async untuk Mendapatkan list Inner Join   dari suatu tabel entity yang columnnya mempunyai relationship ID Identity Not Null dengan tabel lain. 
        /// Ex: MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi dan  akan menghasilkan list karyawan dan detail dari mt_posisi.colRelationsIdentity adalah Foreign Key dari kelas relasi
        ///  , bentuknya params string[] jadi silakan tambahkan koma koma ya
        /// . Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="colRelationsIdentity">Column tabel yang punya relation ID Identity Not Null dengan table lain.Ex.MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi </param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ListInnerJoinAsync<T>(params string[] colRelationsIdentity) where T : class
        {
            var result = await this.ListGeneratorInnerJoinAsync<T>(null, string.Empty, false, 0, colRelationsIdentity);
            return result;

        }
        /// <summary>
        /// Metode Async untuk Mendapatkan list Inner Join   dari suatu tabel entity yang columnnya mempunyai relationship ID Identity Not Null dengan tabel lain. 
        /// Ex: MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi dan  akan menghasilkan list karyawan dan detail dari mt_posisi.colRelationsIdentity adalah Foreign Key dari kelas relasi
        ///  , bentuknya params string[] jadi silakan tambahkan koma koma ya
        /// . Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="colRelationsIdentity">Column tabel yang punya relation ID Identity Not Null dengan table lain.Ex.MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi </param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ListInnerJoinAsync<T>(List<SearchField> SearchFieldList, params string[] colRelationsIdentity) where T : class
        {
            var result = await this.ListGeneratorInnerJoinAsync<T>(SearchFieldList, string.Empty, false, 0, colRelationsIdentity);
            return result;

        }
        /// <summary>
        /// Metode Async untuk Mendapatkan list Inner Join   dari suatu tabel entity yang columnnya mempunyai relationship ID Identity Not Null dengan tabel lain. 
        /// Ex: MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi dan  akan menghasilkan list karyawan dan detail dari mt_posisi.colRelationsIdentity adalah Foreign Key dari kelas relasi
        ///  , bentuknya params string[] jadi silakan tambahkan koma koma ya
        /// . Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="colRelationsIdentity">Column tabel yang punya relation ID Identity Not Null dengan table lain.Ex.MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi </param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ListInnerJoinAsync<T>(List<SearchField> SearchFieldList, int toptake, params string[] colRelationsIdentity) where T : class
        {
            var result = await this.ListGeneratorInnerJoinAsync<T>(SearchFieldList, string.Empty, false, toptake, colRelationsIdentity);
            return result;

        }
        /// <summary>
        /// Metode Async untuk Mendapatkan list Inner Join   dari suatu tabel entity yang columnnya mempunyai relationship ID Identity Not Null dengan tabel lain. 
        /// Ex: MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi dan  akan menghasilkan list karyawan dan detail dari mt_posisi.colRelationsIdentity adalah Foreign Key dari kelas relasi
        ///  , bentuknya params string[] jadi silakan tambahkan koma koma ya
        /// . Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="colRelationsIdentity">Column tabel yang punya relation ID Identity Not Null dengan table lain.Ex.MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi </param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ListInnerJoinAsync<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, params string[] colRelationsIdentity) where T : class
        {
            var result = await this.ListGeneratorInnerJoinAsync<T>(SearchFieldList, sortColumn, isascending, 0, colRelationsIdentity);
            return result;

        }
        /// <summary>
        /// Metode Async untuk Mendapatkan list Inner Join   dari suatu tabel entity yang columnnya mempunyai relationship ID Identity Not Null dengan tabel lain. 
        /// Ex: MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi dan  akan menghasilkan list karyawan dan detail dari mt_posisi.colRelationsIdentity adalah Foreign Key dari kelas relasi
        ///  , bentuknya params string[] jadi silakan tambahkan koma koma ya
        /// . Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="T">Class Entity.Ex: MT_Karyawan</typeparam>
        /// <param name="colRelationsIdentity">Column tabel yang punya relation ID Identity Not Null dengan table lain.Ex.MT_karyawan  include "MT_Position" yang merefer ke posisi_id pada  mt_posisi </param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ListInnerJoinAsync<T>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake, params string[] colRelationsIdentity) where T : class
        {
            var result = await this.ListGeneratorInnerJoinAsync<T>(SearchFieldList, sortColumn, isascending, toptake, colRelationsIdentity);
            return result;

        }

        /// <summary>
        /// Metode Sync untuk Mendapatkan list data sesuai dengan column yang ada di TResult. Jadi intinya kita bisa memilih kolom apa saja di TSource
        /// untuk ditampilkan di TResult. Ex. Pada tabel MT_Karyawan kita hanya membutuhkan kolom emp_no dan emp_name, jadi kita tinggal buat kelas TResult 
        /// yang hanya berisi emp_no dan emp_name,struktur kolomnya harus sama dengan TSource ya.
        /// Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="TSource">Table entitas source. Ex. MT_Karyawan</typeparam>
        /// <typeparam name="TResult">Table entitas untuk menampung hasil query. Ex. MT_Karyawan_hasil </typeparam>
        /// <returns></returns>
        public IEnumerable<TResult> ListTResult<TSource, TResult>() where TSource : class where TResult : class
        {
            var result = this.ListGeneratorTResult<TSource, TResult>(null, string.Empty, false, 0);
            return result;
        }
        /// <summary>
        /// Metode Sync untuk Mendapatkan list data sesuai dengan column yang ada di TResult. Jadi intinya kita bisa memilih kolom apa saja di TSource
        /// untuk ditampilkan di TResult. Ex. Pada tabel MT_Karyawan kita hanya membutuhkan kolom emp_no dan emp_name, jadi kita tinggal buat kelas TResult 
        /// yang hanya berisi emp_no dan emp_name,struktur kolomnya harus sama dengan TSource ya.
        /// Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="TSource">Table entitas source. Ex. MT_Karyawan</typeparam>
        /// <typeparam name="TResult">Table entitas untuk menampung hasil query. Ex. MT_Karyawan_hasil </typeparam>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>    
        /// <returns></returns>
        public IEnumerable<TResult> ListTResult<TSource, TResult>(List<SearchField> SearchFieldList) where TSource : class where TResult : class
        {
            var result = this.ListGeneratorTResult<TSource, TResult>(SearchFieldList, string.Empty, false, 0);
            return result;
        }
        /// <summary>
        /// Metode Sync untuk Mendapatkan list data sesuai dengan column yang ada di TResult. Jadi intinya kita bisa memilih kolom apa saja di TSource
        /// untuk ditampilkan di TResult. Ex. Pada tabel MT_Karyawan kita hanya membutuhkan kolom emp_no dan emp_name, jadi kita tinggal buat kelas TResult 
        /// yang hanya berisi emp_no dan emp_name,struktur kolomnya harus sama dengan TSource ya.
        /// Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="TSource">Table entitas source. Ex. MT_Karyawan</typeparam>
        /// <typeparam name="TResult">Table entitas untuk menampung hasil query. Ex. MT_Karyawan_hasil </typeparam>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>      
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public IEnumerable<TResult> ListTResult<TSource, TResult>(List<SearchField> SearchFieldList, int toptake) where TSource : class where TResult : class
        {
            var result = this.ListGeneratorTResult<TSource, TResult>(SearchFieldList, string.Empty, false, toptake);
            return result;
        }
        /// <summary>
        /// Metode Sync untuk Mendapatkan list data sesuai dengan column yang ada di TResult. Jadi intinya kita bisa memilih kolom apa saja di TSource
        /// untuk ditampilkan di TResult. Ex. Pada tabel MT_Karyawan kita hanya membutuhkan kolom emp_no dan emp_name, jadi kita tinggal buat kelas TResult 
        /// yang hanya berisi emp_no dan emp_name,struktur kolomnya harus sama dengan TSource ya.
        /// Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="TSource">Table entitas source. Ex. MT_Karyawan</typeparam>
        /// <typeparam name="TResult">Table entitas untuk menampung hasil query. Ex. MT_Karyawan_hasil </typeparam>
        ///  <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <returns></returns>
        public IEnumerable<TResult> ListTResult<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn, bool isascending) where TSource : class where TResult : class
        {
            var result = this.ListGeneratorTResult<TSource, TResult>(SearchFieldList, sortColumn, isascending, 0);
            return result;
        }
        /// <summary>
        /// Metode Sync untuk Mendapatkan list data sesuai dengan column yang ada di TResult. Jadi intinya kita bisa memilih kolom apa saja di TSource
        /// untuk ditampilkan di TResult. Ex. Pada tabel MT_Karyawan kita hanya membutuhkan kolom emp_no dan emp_name, jadi kita tinggal buat kelas TResult 
        /// yang hanya berisi emp_no dan emp_name,struktur kolomnya harus sama dengan TSource ya.
        /// Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="TSource">Table entitas source. Ex. MT_Karyawan</typeparam>
        /// <typeparam name="TResult">Table entitas untuk menampung hasil query. Ex. MT_Karyawan_hasil </typeparam>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public IEnumerable<TResult> ListTResult<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where TSource : class where TResult : class
        {
            var result = this.ListGeneratorTResult<TSource, TResult>(SearchFieldList, sortColumn, isascending, toptake);
            return result;
        }

        /// <summary>
        /// Metode Async untuk Mendapatkan list data sesuai dengan column yang ada di TResult. Jadi intinya kita bisa memilih kolom apa saja di TSource
        /// untuk ditampilkan di TResult. Ex. Pada tabel MT_Karyawan kita hanya membutuhkan kolom emp_no dan emp_name, jadi kita tinggal buat kelas TResult 
        /// yang hanya berisi emp_no dan emp_name,struktur kolomnya harus sama dengan TSource ya.
        /// Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="TSource">Table entitas source. Ex. MT_Karyawan</typeparam>
        /// <typeparam name="TResult">Table entitas untuk menampung hasil query. Ex. MT_Karyawan_hasil </typeparam>
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> ListTResultAsync<TSource, TResult>() where TSource : class where TResult : class
        {
            var result = await this.ListGeneratorTResultAsync<TSource, TResult>(null, string.Empty, false, 0);
            return result;
        }
        /// <summary>
        /// Metode Async untuk Mendapatkan list data sesuai dengan column yang ada di TResult. Jadi intinya kita bisa memilih kolom apa saja di TSource
        /// untuk ditampilkan di TResult. Ex. Pada tabel MT_Karyawan kita hanya membutuhkan kolom emp_no dan emp_name, jadi kita tinggal buat kelas TResult 
        /// yang hanya berisi emp_no dan emp_name,struktur kolomnya harus sama dengan TSource ya.
        /// Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="TSource">Table entitas source. Ex. MT_Karyawan</typeparam>
        /// <typeparam name="TResult">Table entitas untuk menampung hasil query. Ex. MT_Karyawan_hasil </typeparam>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>    
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> ListTResultAsync<TSource, TResult>(List<SearchField> SearchFieldList) where TSource : class where TResult : class
        {
            var result = await this.ListGeneratorTResultAsync<TSource, TResult>(SearchFieldList, string.Empty, false, 0);
            return result;
        }
        /// <summary>
        /// Metode Async untuk Mendapatkan list data sesuai dengan column yang ada di TResult. Jadi intinya kita bisa memilih kolom apa saja di TSource
        /// untuk ditampilkan di TResult. Ex. Pada tabel MT_Karyawan kita hanya membutuhkan kolom emp_no dan emp_name, jadi kita tinggal buat kelas TResult 
        /// yang hanya berisi emp_no dan emp_name,struktur kolomnya harus sama dengan TSource ya.
        /// Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="TSource">Table entitas source. Ex. MT_Karyawan</typeparam>
        /// <typeparam name="TResult">Table entitas untuk menampung hasil query. Ex. MT_Karyawan_hasil </typeparam>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>      
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> ListTResultAsync<TSource, TResult>(List<SearchField> SearchFieldList, int toptake) where TSource : class where TResult : class
        {
            var result = await this.ListGeneratorTResultAsync<TSource, TResult>(SearchFieldList, string.Empty, false, toptake);
            return result;
        }
        /// <summary>
        /// Metode Async untuk Mendapatkan list data sesuai dengan column yang ada di TResult. Jadi intinya kita bisa memilih kolom apa saja di TSource
        /// untuk ditampilkan di TResult. Ex. Pada tabel MT_Karyawan kita hanya membutuhkan kolom emp_no dan emp_name, jadi kita tinggal buat kelas TResult 
        /// yang hanya berisi emp_no dan emp_name,struktur kolomnya harus sama dengan TSource ya.
        /// Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="TSource">Table entitas source. Ex. MT_Karyawan</typeparam>
        /// <typeparam name="TResult">Table entitas untuk menampung hasil query. Ex. MT_Karyawan_hasil </typeparam>
        ///  <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> ListTResultAsync<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn, bool isascending) where TSource : class where TResult : class
        {
            var result = await this.ListGeneratorTResultAsync<TSource, TResult>(SearchFieldList, sortColumn, isascending, 0);
            return result;
        }
        /// <summary>
        /// Metode Async untuk Mendapatkan list data sesuai dengan column yang ada di TResult. Jadi intinya kita bisa memilih kolom apa saja di TSource
        /// untuk ditampilkan di TResult. Ex. Pada tabel MT_Karyawan kita hanya membutuhkan kolom emp_no dan emp_name, jadi kita tinggal buat kelas TResult 
        /// yang hanya berisi emp_no dan emp_name,struktur kolomnya harus sama dengan TSource ya.
        /// Data activeBool=1,Default 100 data. Default OrderBy ColumnIdentity Desc 
        /// </summary>
        /// <typeparam name="TSource">Table entitas source. Ex. MT_Karyawan</typeparam>
        /// <typeparam name="TResult">Table entitas untuk menampung hasil query. Ex. MT_Karyawan_hasil </typeparam>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> ListTResultAsync<TSource, TResult>(List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where TSource : class where TResult : class
        {
            var result = await this.ListGeneratorTResultAsync<TSource, TResult>(SearchFieldList, sortColumn, isascending, toptake);
            return result;
        }

        /// <summary>
        /// Metode Sync utntuk mendapatkan list dari suatu query join yang dikirimkan melalui parameter queryable. Hasil nya akan di tampung dalam TResult. 
        /// Data activeBool = di set sendiri pada parameter queryable,Default 100 data      
        /// </summary>
        /// <typeparam name="TResult">Table yang akan menampung hasil query. Semau column yang akan dipangggil pada whereclause harus didefinisikan di tabel ini, Jika tidak akan gagal.</typeparam>
        /// <param name="queryable">Query join yang dibuat di Client dan di kirimkan melalui parameter queryable</param>     
        /// <returns></returns>
        public IEnumerable<TResult> ListJoinTResult<TResult>(IQueryable<TResult> queryable) where TResult : class
        {
            var result = this.ListGeneratorJoinTResult<TResult>(queryable, null, string.Empty, false, 0);
            return result;

        }
        /// <summary>
        /// Metode Sync utntuk mendapatkan list dari suatu query join yang dikirimkan melalui parameter queryable. Hasil nya akan di tampung dalam TResult. 
        /// Data activeBool = di set sendiri pada parameter queryable,Default 100 data      
        /// </summary>
        /// <typeparam name="TResult">Table yang akan menampung hasil query. Semau column yang akan dipangggil pada whereclause harus didefinisikan di tabel ini, Jika tidak akan gagal.</typeparam>
        /// <param name="queryable">Query join yang dibuat di Client dan di kirimkan melalui parameter queryable</param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table TResult . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>      
        /// <returns></returns>
        public IEnumerable<TResult> ListJoinTResult<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList) where TResult : class
        {
            var result = this.ListGeneratorJoinTResult<TResult>(queryable, SearchFieldList, string.Empty, false, 0);
            return result;

        }
        /// <summary>
        /// Metode Sync utntuk mendapatkan list dari suatu query join yang dikirimkan melalui parameter queryable. Hasil nya akan di tampung dalam TResult. 
        /// Data activeBool = di set sendiri pada parameter queryable,Default 100 data      
        /// </summary>
        /// <typeparam name="TResult">Table yang akan menampung hasil query. Semau column yang akan dipangggil pada whereclause harus didefinisikan di tabel ini, Jika tidak akan gagal.</typeparam>
        /// <param name="queryable">Query join yang dibuat di Client dan di kirimkan melalui parameter queryable</param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table TResult . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>        
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public IEnumerable<TResult> ListJoinTResult<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList, int toptake) where TResult : class
        {
            var result = this.ListGeneratorJoinTResult<TResult>(queryable, SearchFieldList, string.Empty, false, toptake);
            return result;

        }
        /// <summary>
        /// Metode Sync utntuk mendapatkan list dari suatu query join yang dikirimkan melalui parameter queryable. Hasil nya akan di tampung dalam TResult. 
        /// Data activeBool = di set sendiri pada parameter queryable,Default 100 data      
        /// </summary>
        /// <typeparam name="TResult">Table yang akan menampung hasil query. Semau column yang akan dipangggil pada whereclause harus didefinisikan di tabel ini, Jika tidak akan gagal.</typeparam>
        /// <param name="queryable">Query join yang dibuat di Client dan di kirimkan melalui parameter queryable</param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <returns></returns>
        public IEnumerable<TResult> ListJoinTResult<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending) where TResult : class
        {
            var result = this.ListGeneratorJoinTResult<TResult>(queryable, SearchFieldList, sortColumn, isascending, 0);
            return result;

        }
        /// <summary>
        /// Metode Sync utntuk mendapatkan list dari suatu query join yang dikirimkan melalui parameter queryable. Hasil nya akan di tampung dalam TResult. 
        /// Data activeBool = di set sendiri pada parameter queryable,Default 100 data      
        /// </summary>
        /// <typeparam name="TResult">Table yang akan menampung hasil query. Semau column yang akan dipangggil pada whereclause harus didefinisikan di tabel ini, Jika tidak akan gagal.</typeparam>
        /// <param name="queryable">Query join yang dibuat di Client dan di kirimkan melalui parameter queryable</param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table TResult . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public IEnumerable<TResult> ListJoinTResult<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where TResult : class
        {
            var result = this.ListGeneratorJoinTResult<TResult>(queryable, SearchFieldList, sortColumn, isascending, toptake);
            return result;

        }

        /// <summary>
        /// Metode Async utntuk mendapatkan list dari suatu query join yang dikirimkan melalui parameter queryable. Hasil nya akan di tampung dalam TResult. 
        /// Data activeBool = di set sendiri pada parameter queryable,Default 100 data      
        /// </summary>
        /// <typeparam name="TResult">Table yang akan menampung hasil query. Semau column yang akan dipangggil pada whereclause harus didefinisikan di tabel ini, Jika tidak akan gagal.</typeparam>
        /// <param name="queryable">Query join yang dibuat di Client dan di kirimkan melalui parameter queryable</param>     
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> ListJoinTResultAsync<TResult>(IQueryable<TResult> queryable) where TResult : class
        {
            var result = await this.ListGeneratorJoinTResultAsync<TResult>(queryable, null, string.Empty, false, 0);
            return result;

        }
        /// <summary>
        /// Metode Async utntuk mendapatkan list dari suatu query join yang dikirimkan melalui parameter queryable. Hasil nya akan di tampung dalam TResult. 
        /// Data activeBool = di set sendiri pada parameter queryable,Default 100 data      
        /// </summary>
        /// <typeparam name="TResult">Table yang akan menampung hasil query. Semau column yang akan dipangggil pada whereclause harus didefinisikan di tabel ini, Jika tidak akan gagal.</typeparam>
        /// <param name="queryable">Query join yang dibuat di Client dan di kirimkan melalui parameter queryable</param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table TResult . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>      
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> ListJoinTResultAsync<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList) where TResult : class
        {
            var result = await this.ListGeneratorJoinTResultAsync<TResult>(queryable, SearchFieldList, string.Empty, false, 0);
            return result;

        }
        /// <summary>
        /// Metode Async utntuk mendapatkan list dari suatu query join yang dikirimkan melalui parameter queryable. Hasil nya akan di tampung dalam TResult. 
        /// Data activeBool = di set sendiri pada parameter queryable,Default 100 data      
        /// </summary>
        /// <typeparam name="TResult">Table yang akan menampung hasil query. Semau column yang akan dipangggil pada whereclause harus didefinisikan di tabel ini, Jika tidak akan gagal.</typeparam>
        /// <param name="queryable">Query join yang dibuat di Client dan di kirimkan melalui parameter queryable</param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table TResult . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>        
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> ListJoinTResultAsync<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList, int toptake) where TResult : class
        {
            var result = await this.ListGeneratorJoinTResultAsync<TResult>(queryable, SearchFieldList, string.Empty, false, toptake);
            return result;

        }
        /// <summary>
        /// Metode Async utntuk mendapatkan list dari suatu query join yang dikirimkan melalui parameter queryable. Hasil nya akan di tampung dalam TResult. 
        /// Data activeBool = di set sendiri pada parameter queryable,Default 100 data      
        /// </summary>
        /// <typeparam name="TResult">Table yang akan menampung hasil query. Semau column yang akan dipangggil pada whereclause harus didefinisikan di tabel ini, Jika tidak akan gagal.</typeparam>
        /// <param name="queryable">Query join yang dibuat di Client dan di kirimkan melalui parameter queryable</param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table db . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> ListJoinTResultAsync<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending) where TResult : class
        {
            var result = await this.ListGeneratorJoinTResultAsync<TResult>(queryable, SearchFieldList, sortColumn, isascending, 0);
            return result;

        }
        /// <summary>
        /// Metode Async utntuk mendapatkan list dari suatu query join yang dikirimkan melalui parameter queryable. Hasil nya akan di tampung dalam TResult. 
        /// Data activeBool = di set sendiri pada parameter queryable,Default 100 data      
        /// </summary>
        /// <typeparam name="TResult">Table yang akan menampung hasil query. Semau column yang akan dipangggil pada whereclause harus didefinisikan di tabel ini, Jika tidak akan gagal.</typeparam>
        /// <param name="queryable">Query join yang dibuat di Client dan di kirimkan melalui parameter queryable</param>
        /// <param name="SearchFieldList">List parameter dinamis berdasarkan colname table TResult . ex ==>  name : emp_name;Operator :"=";Value1:superman</param>
        /// <param name="sortColumn">sorting berdasarkan column table database. Jika empty akan diambil default dari ID Identity table ex: emp_name</param>
        /// <param name="isascending">sort column : jika isacending</param>
        /// <param name="toptake">top n from database . Jika toptake=0,maka menjadi default 100</param>
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> ListJoinTResultAsync<TResult>(IQueryable<TResult> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where TResult : class
        {
            var result = await this.ListGeneratorJoinTResultAsync<TResult>(queryable, SearchFieldList, sortColumn, isascending, toptake);
            return result;

        }

        /// <summary>
        /// Metode Sync untuk penyimpanan data,Jika return value != null maka penyimpanan sukses,jika return value ==null, maka gagal.
        /// </summary>
        /// <typeparam name="T">Kelas tabel database untuk return value dan paramater</typeparam>
        /// <param name="entity">Instance kelas yang akan disimpan datanya</param>
        /// <returns></returns>
        public T Save<T>(T entity) where T : class
        {
            int hasil = 0;
            using (var context = GetDbContext())
            {
                try
                {
                    if (entity != null)
                    {
                        entity = GenHelperEF.Getinstance.SetColIDZero<T>(entity);
                        context.Set<T>().Add(entity);
                        hasil = context.SaveChanges();
                    }
                }
                catch { }
            }
            if (hasil <= 0)
                entity = null;
            return entity;
        }
        /// <summary>
        ///  Metode Sync untuk penyimpanan data,Jika return value != null maka penyimpanan sukses,jika return value ==null, maka gagal.
        ///  Data yang disimpan bisa lebih dari satu. Jika Jika semua data berhasil disimpan , maka sukses. 
        ///  jika ada salah satu yang gagal, maka semua data tidak jadi/ batal disimpan. Ada Jaminan data masuk.
        /// </summary>
        /// <typeparam name="T">Kelas tabel database untuk return value dan paramater</typeparam>
        /// <param name="entityCollection">List instance kelas yang akan disimpan datanya</param>
        /// <returns></returns>
        public IEnumerable<T> Save<T>(List<T> entityCollection) where T : class
        {
            int hasil = 0;
            using (var context = GetDbContext())
            {
                try
                {
                    if (entityCollection != null)
                    {
                        /*Check Prop ID Identity ,akan diisi dgn 0*/
                        entityCollection = GenHelperEF.Getinstance.SetColIDZero<T>(entityCollection);
                        foreach (T entity in entityCollection)
                        {
                            context.Set<T>().Add(entity);
                        }

                        hasil = context.SaveChanges();
                    }
                }
                catch { }
            }
            if (hasil <= 0)
                entityCollection = null;
            return entityCollection;
        }
        /// <summary>
        /// Metode Sync untuk penyimpanan data,Jika return value != null maka penyimpanan sukses,jika return value ==null, maka gagal.
        /// </summary>
        /// <typeparam name="T">Kelas tabel database untuk return value dan paramater</typeparam>
        /// <param name="entity">Instance kelas yang akan disimpan datanya</param>
        /// <returns></returns>
        public async Task<T> SaveAsync<T>(T entity) where T : class
        {
            int hasil = 0;
            using (var context = GetDbContext())
            {
                try
                {
                    if (entity != null)
                    {
                        entity = GenHelperEF.Getinstance.SetColIDZero<T>(entity);
                        context.Set<T>().Add(entity);
                        hasil = await context.SaveChangesAsync();
                    }
                }
                catch(Exception ex) {
                    string a = ex.ToString();
                }
            }
            if (hasil <= 0)
                entity = null;
            return entity;
        }
        /// <summary>
        ///  Metode Sync untuk penyimpanan data,Jika return value != null maka penyimpanan sukses,jika return value ==null, maka gagal.
        ///  Data yang disimpan bisa lebih dari satu. Jika Jika semua data berhasil disimpan , maka sukses. 
        ///  jika ada salah satu yang gagal, maka semua data tidak jadi/ batal disimpan. Ada Jaminan data masuk.
        /// </summary>
        /// <typeparam name="T">Kelas tabel database untuk return value dan paramater</typeparam>
        /// <param name="entityCollection">List instance kelas yang akan disimpan datanya</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> SaveAsync<T>(List<T> entityCollection) where T : class
        {
            int hasil = 0;
            using (var context = GetDbContext())
            {
                try
                {
                    if (entityCollection != null)
                    {
                        /*Check Prop ID Identity ,akan diisi dgn 0*/
                        entityCollection = GenHelperEF.Getinstance.SetColIDZero<T>(entityCollection);
                        foreach (T entity in entityCollection)
                        {
                            context.Set<T>().Add(entity);
                        }

                        hasil = await context.SaveChangesAsync();
                    }
                }
                catch { }
            }
            if (hasil <= 0)
                entityCollection = null;
            return entityCollection;
        }

        /// <summary>
        /// Metode Sync untuk mengubah data. Data yang diubah adalah data yang not null pada sebuah entitas. Jadi klo mau update beberapa kolom saja
        /// silakan isi value kolomnya, yang tidak mau di update ya di null kan kolomnya. Jangan lupa identityID valuenya diisi ya,WAJIB
        /// </summary>
        /// <typeparam name="T">Kelas entitas yang akan di ubah</typeparam>
        /// <param name="entity">Entitas yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        /// 
        public bool Update<T>(T entity) where T : class
        {
            bool result = false;

            if (entity != null)
            {
                using (var context = GetDbContext())
                {
                    var colNotNull = FilterLinq.PropertyColNotNull<T>(entity);
                    //entity = FilterLinq.PropertyColDefaultValue<T>(entity);
                    entity = PrepareEntityForUpdateNoSelect<T>(entity);
                    context.Set<T>().Attach(entity);
                    context.Entry(entity).State = EntityState.Unchanged;

                    PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                    foreach (var property in colNotNull)
                    {
                        if (piID != property)
                        {
                            object value = property.GetValue(entity);
                            if (value != null)
                            {
                                context.Entry(entity).Property(property.Name).IsModified = true;
                            }
                        }
                    }

                    int hasil = context.SaveChanges();
                    if (hasil > 0)
                        result = true;

                }
            }
            return result;
        }
        /// <summary>
        /// Metode Sync untuk mengubah data. Data yang diubah adalah data yang not null pada List entitas. Jadi klo mau update beberapa kolom saja
        /// silakan isi value kolomnya, yang tidak mau di update ya di null kan kolomnya. Jangan lupa identityID valuenya diisi ya,WAJIB.
        /// Jaminan Jika hasilnya true maka semua data berhasil diubah,jika false maka semua data batal diubah
        /// </summary>
        /// <typeparam name="T">Kelas entitas yang akan di ubah</typeparam>
        /// <param name="entityCollections">List entitas yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        /// 
        public bool Update<T>(List<T> entityCollections) where T : class
        {
            bool result = false;
            int hasil = 0;

            if (entityCollections != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {

                        try
                        {
                            for (int i = 0; i < entityCollections.Count; i++)
                            {
                                var entity = entityCollections[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T>(entity);
                                //entity = FilterLinq.PropertyColDefaultValue<T>(entity);
                                entity = PrepareEntityForUpdateNoSelect<T>(entity);
                                context.Set<T>().Attach(entity);
                                context.Entry(entity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(entity);
                                        if (value != null)
                                        {
                                            context.Entry(entity).Property(property.Name).IsModified = true;
                                        }
                                    }
                                }

                            }
                            hasil = context.SaveChanges();
                            contextTrans.Commit();
                        }
                        catch
                        {

                            contextTrans.Rollback();
                        }

                    }

                    if (hasil > 0)
                        result = true;
                }

            }
            return result;
        }
        /// <summary>
        /// Metode Sync untuk mengubah data. Semua data yang ada pada kolom entitas akan di update semua,Hati-hati ya, nanti tanpa disadari meng-null-kan data. Jangan lupa identityID valuenya diisi ya,WAJIB
        /// </summary>
        /// <typeparam name="T">Kelas entitas yang akan di ubah</typeparam>
        /// <param name="entity">entitas yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool UpdateAll<T>(T entity) where T : class
        {
            bool result = false;
            using (var context = GetDbContext())
            {
                if (entity != null)
                {
                    try
                    {
                        context.Entry(entity).State = EntityState.Modified;
                        int hasil = context.SaveChanges();

                        if (hasil > 0)
                            result = true;
                    }
                    catch { }
                }
            }
            return result;
        }
        /// <summary>
        /// Metode Sync untuk mengubah data. Semua list data yang ada pada kolom entitas akan di update semua, 
        /// Hati-hati ya, nanti tanpa disadari meng-null-kan data. Jangan lupa identityID valuenya diisi ya,WAJIB.
        /// Jaminan Jika hasilnya true maka semua data berhasil diubah,jika false maka semua data batal diubah
        /// </summary>
        /// <typeparam name="T">Kelas entitas yang akan di ubah</typeparam>
        /// <param name="entity">List entitas yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool UpdateAll<T>(List<T> entityCollection) where T : class
        {
            bool result = false;
            using (var context = GetDbContext())
            {
                using (var contextTrans = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (entityCollection != null)
                        {

                            for (int i = 0; i < entityCollection.Count; i++)
                            {
                                var entity = entityCollection[i];
                                context.Entry(entity).State = EntityState.Modified;

                            }
                            int hasil = context.SaveChanges();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                    }
                    catch
                    {
                        contextTrans.Rollback();
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Metode Async untuk mengubah data. Data yang diubah adalah data yang not null pada sebuah entitas. Jadi klo mau update beberapa kolom saja
        /// silakan isi value kolomnya, yang tidak mau di update ya di null kan kolomnya. Jangan lupa identityID valuenya diisi ya,WAJIB
        /// </summary>
        /// <typeparam name="T">Kelas entitas yang akan di ubah</typeparam>
        /// <param name="entity">Entitas yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        /// 
        public async Task<bool> UpdateAsync<T>(T entity) where T : class
        {
            bool result = false;

            if (entity != null)
            {

                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var colNotNull = FilterLinq.PropertyColNotNull<T>(entity);
                            //entity = FilterLinq.PropertyColDefaultValue<T>(entity);
                            entity = PrepareEntityForUpdateNoSelect<T>(entity);
                            context.Set<T>().Attach(entity);
                            context.Entry(entity).State = EntityState.Unchanged;
                            PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                            foreach (var property in colNotNull)
                            {
                                if (piID != property)
                                {
                                    object value = property.GetValue(entity);
                                    if (value != null)
                                    {
                                        context.Entry(entity).Property(property.Name).IsModified = true;
                                    }
                                }

                            }

                            int hasil = await context.SaveChangesAsync();
                            if (hasil > 0)
                                result = true;
                            contextTrans.Commit();
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                            contextTrans.Rollback();
                        }

                    }
                }

            }

            return result;
        }
        /// <summary>
        /// Metode Async untuk mengubah data. Data yang diubah adalah data yang not null pada List entitas. Jadi klo mau update beberapa kolom saja
        /// silakan isi value kolomnya, yang tidak mau di update ya di null kan kolomnya. Jangan lupa identityID valuenya diisi ya,WAJIB.
        /// Jaminan Jika hasilnya true maka semua data berhasil diubah,jika false maka semua data batal diubah
        /// </summary>
        /// <typeparam name="T">Kelas entitas yang akan di ubah</typeparam>
        /// <param name="entityCollections">List entitas yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        /// 
        public async Task<bool> UpdateAsync<T>(List<T> entityCollections) where T : class
        {
            bool result = false;
            int hasil = 0;
            if (entityCollections != null)
            {

                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollections.Count; i++)
                            {
                                var entity = entityCollections[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T>(entity);
                                //entity = FilterLinq.PropertyColDefaultValue<T>(entity);
                                entity = PrepareEntityForUpdateNoSelect<T>(entity);
                                context.Set<T>().Attach(entity);
                                context.Entry(entity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(entity);
                                        if (value != null)
                                        {
                                            context.Entry(entity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }

                            }
                            hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }

            }

            return result;
        }
        /// <summary>
        /// Metode Async untuk mengubah data. Semua data yang ada pada kolom entitas akan di update semua,Hati-hati ya, nanti tanpa disadari meng-null-kan data. Jangan lupa identityID valuenya diisi ya,WAJIB
        /// </summary>
        /// <typeparam name="T">Kelas entitas yang akan di ubah</typeparam>
        /// <param name="entity">entitas yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> UpdateAllAsync<T>(T entity) where T : class
        {
            bool result = false;

            using (var context = GetDbContext())
            {
                if (entity != null)
                {
                    try
                    {
                        context.Entry(entity).State = EntityState.Modified;
                        int hasil = await context.SaveChangesAsync();

                        if (hasil > 0)
                            result = true;
                    }
                    catch(Exception ex) {
                        string a = ex.ToString();
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Metode Async untuk mengubah data. Semua list data yang ada pada kolom entitas akan di update semua, 
        /// Hati-hati ya, nanti tanpa disadari meng-null-kan data. Jangan lupa identityID valuenya diisi ya,WAJIB.
        /// Jaminan Jika hasilnya true maka semua data berhasil diubah,jika false maka semua data batal diubah
        /// </summary>
        /// <typeparam name="T">Kelas entitas yang akan di ubah</typeparam>
        /// <param name="entity">List entitas yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> UpdateAllAsync<T>(List<T> entityCollection) where T : class
        {
            bool result = false;
            using (var context = GetDbContext())
            {
                using (var contextTrans = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (entityCollection != null)
                        {
                            for (int i = 0; i < entityCollection.Count; i++)
                            {
                                var entity = entityCollection[i];
                                context.Entry(entity).State = EntityState.Modified;

                            }
                            int hasil = await context.SaveChangesAsync();
                            if (hasil > 0)
                                result = true;
                            contextTrans.Commit();

                        }
                    }
                    catch
                    {
                        contextTrans.Rollback();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Metode Sync untuk mengubah data. Data yang diubah adalah data yang not null pada sebuah entitas. Jadi klo mau update beberapa kolom saja
        /// silakan isi value kolomnya, yang tidak mau di update ya di null kan kolomnya. Jangan lupa identityID valuenya diisi ya,WAJIB
        /// </summary>
        /// <typeparam name="T">Kelas entitas yang akan di ubah</typeparam>
        /// <param name="entity">Entitas yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="whereColParams">Satu atau sekelompok parameter yang akan dijadikan where clasue untuk update</param>
        /// <returns></returns>        /// 
        public bool Update<T>(T entity, params string[] whereColParams) where T : class
        {
            bool result = false;

            if (entity != null)
            {
                using (var context = GetDbContext())
                {
                    var colNotNull = FilterLinq.PropertyColNotNull<T>(entity);
                    //entity = FilterLinq.PropertyColDefaultValue<T>(entity);
                    entity = PrepareEntityForUpdateNoSelect<T>(entity);
                    context.Set<T>().Attach(entity);
                    context.Entry(entity).State = EntityState.Unchanged;

                    foreach (var property in colNotNull)
                    {
                        IEnumerable<string> isWhereClauseParams = from a in whereColParams where a == property.Name select a;
                        if (isWhereClauseParams.Count() == 0)
                        {
                            object value = property.GetValue(entity);
                            if (value != null)
                            {
                                context.Entry(entity).Property(property.Name).IsModified = true;
                            }
                        }

                    }
                    int hasil = context.SaveChanges();
                    if (hasil > 0)
                        result = true;

                }
            }
            return result;
        }
        /// <summary>
        /// Metode Sync untuk mengubah data. Data yang diubah adalah data yang not null pada List entitas. Jadi klo mau update beberapa kolom saja
        /// silakan isi value kolomnya, yang tidak mau di update ya di null kan kolomnya. Jangan lupa identityID valuenya diisi ya,WAJIB.
        /// Jaminan Jika hasilnya true maka semua data berhasil diubah,jika false maka semua data batal diubah
        /// </summary>
        /// <typeparam name="T">Kelas entitas yang akan di ubah</typeparam>
        /// <param name="entityCollections">List entitas yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="whereColParams">Satu atau sekelompok parameter yang akan dijadikan where clasue untuk update</param>
        /// <returns></returns>
        /// 
        public bool Update<T>(List<T> entityCollections, params string[] whereColParams) where T : class
        {
            bool result = false;
            int hasil = 0;

            if (entityCollections != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {

                        try
                        {
                            for (int i = 0; i < entityCollections.Count; i++)
                            {
                                var entity = entityCollections[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T>(entity);
                                //entity = FilterLinq.PropertyColDefaultValue<T>(entity);
                                entity = PrepareEntityForUpdateNoSelect<T>(entity);
                                context.Set<T>().Attach(entity);
                                context.Entry(entity).State = EntityState.Unchanged;
                                foreach (var property in colNotNull)
                                {
                                    IEnumerable<string> isWhereClauseParams = from a in whereColParams where a == property.Name select a;
                                    if (isWhereClauseParams.Count() == 0)
                                    {
                                        object value = property.GetValue(entity);
                                        if (value != null)
                                        {
                                            context.Entry(entity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }

                            }
                            hasil = context.SaveChanges();
                            contextTrans.Commit();
                        }
                        catch
                        {

                            contextTrans.Rollback();
                        }

                    }

                    if (hasil > 0)
                        result = true;
                }

            }
            return result;
        }
        /// <summary>
        /// Metode Async untuk mengubah data. Data yang diubah adalah data yang not null pada sebuah entitas. Jadi klo mau update beberapa kolom saja
        /// silakan isi value kolomnya, yang tidak mau di update ya di null kan kolomnya. Jangan lupa identityID valuenya diisi ya,WAJIB
        /// </summary>
        /// <typeparam name="T">Kelas entitas yang akan di ubah</typeparam>
        /// <param name="entity">Entitas yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="whereColParams">Satu atau sekelompok parameter yang akan dijadikan where clasue untuk update</param>
        /// <returns></returns>    
        public async Task<bool> UpdateAsync<T>(T entity, params string[] whereColParams) where T : class
        {
            bool result = false;

            if (entity != null)
            {

                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var colNotNull = FilterLinq.PropertyColNotNull<T>(entity);
                            entity = PrepareEntityForUpdateNoSelect<T>(entity);
                            //entity = FilterLinq.PropertyColDefaultValue<T>(entity);
                            context.Set<T>().Attach(entity);
                            context.Entry(entity).State = EntityState.Unchanged;
                            foreach (var property in colNotNull)
                            {
                                IEnumerable<string> isWhereClauseParams = from a in whereColParams where a == property.Name select a;
                                if (isWhereClauseParams.Count() == 0)
                                {
                                    object value = property.GetValue(entity);
                                    if (value != null)
                                    {
                                        context.Entry(entity).Property(property.Name).IsModified = true;
                                    }
                                }

                            }

                            int hasil = await context.SaveChangesAsync();
                            if (hasil > 0)
                                result = true;
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }

                    }
                }

            }

            return result;
        }
        /// <summary>
        /// Metode Async untuk mengubah data. Data yang diubah adalah data yang not null pada List entitas. Jadi klo mau update beberapa kolom saja
        /// silakan isi value kolomnya, yang tidak mau di update ya di null kan kolomnya. Jangan lupa identityID valuenya diisi ya,WAJIB.
        /// Jaminan Jika hasilnya true maka semua data berhasil diubah,jika false maka semua data batal diubah
        /// </summary>
        /// <typeparam name="T">Kelas entitas yang akan di ubah</typeparam>
        /// <param name="entityCollections">List entitas yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="whereColParams">Satu atau sekelompok parameter yang akan dijadikan where clasue untuk update</param>
        /// <returns></returns>
        /// 
        public async Task<bool> UpdateAsync<T>(List<T> entityCollections, params string[] whereColParams) where T : class
        {
            bool result = false;
            int hasil = 0;
            if (entityCollections != null)
            {

                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollections.Count; i++)
                            {
                                var entity = entityCollections[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T>(entity);
                                //entity = FilterLinq.PropertyColDefaultValue<T>(entity);
                                entity = PrepareEntityForUpdateNoSelect<T>(entity);
                                context.Set<T>().Attach(entity);
                                context.Entry(entity).State = EntityState.Unchanged;
                                foreach (var property in colNotNull)
                                {
                                    IEnumerable<string> isWhereClauseParams = from a in whereColParams where a == property.Name select a;
                                    if (isWhereClauseParams.Count() == 0)
                                    {
                                        object value = property.GetValue(entity);
                                        if (value != null)
                                        {
                                            context.Entry(entity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }

                            }
                            hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }

            }

            return result;
        }

        /// <summary>
        /// Metode Sync untuk menyimpan  data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. Pengubahan hanya untuk kolom yang valuenya NotNull aja ya
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Save<T1, T2>(T1 entity1, T2 entity2) where T1 : class where T2 : class
        {
            //  var context = GetDbContext();
            int hasil = 0;
            bool result = false;

            if (entity1 != null & entity2 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            entity1 = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                            context.Set<T1>().Add(entity1);

                            entity2 = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                            context.Set<T2>().Add(entity2);

                            hasil = context.SaveChanges();

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;

            return result;
        }
        /// <summary>
        /// Metode Sync untuk menyimpan  data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Dept</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity3">Entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Save<T1, T2, T3>(T1 entity1, T2 entity2, T3 entity3) where T1 : class where T2 : class where T3 : class
        {

            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null & entity3 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            entity1 = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                            context.Set<T1>().Add(entity1);

                            entity2 = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                            context.Set<T2>().Add(entity2);

                            entity3 = GenHelperEF.Getinstance.SetColIDZero<T3>(entity3);
                            context.Set<T3>().Add(entity3);

                            hasil = context.SaveChanges();

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menyimpan  data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Dept</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity3">Entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity4">Entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Save<T1, T2, T3, T4>(T1 entity1, T2 entity2, T3 entity3, T4 entity4)
            where T1 : class where T2 : class where T3 : class where T4 : class
        {

            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null & entity3 != null & entity4 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            entity1 = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                            context.Set<T1>().Add(entity1);

                            entity2 = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                            context.Set<T2>().Add(entity2);

                            entity3 = GenHelperEF.Getinstance.SetColIDZero<T3>(entity3);
                            context.Set<T3>().Add(entity3);

                            entity4 = GenHelperEF.Getinstance.SetColIDZero<T4>(entity4);
                            context.Set<T4>().Add(entity4);

                            hasil = context.SaveChanges();

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menyimpan  data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Dept</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity3">Entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity4">Entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Save<T1, T2, T3, T4, T5>(T1 entity1, T2 entity2, T3 entity3, T4 entity4, T5 entity5)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {

            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null & entity3 != null & entity4 != null & entity5 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            entity1 = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                            context.Set<T1>().Add(entity1);

                            entity2 = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                            context.Set<T2>().Add(entity2);

                            entity3 = GenHelperEF.Getinstance.SetColIDZero<T3>(entity3);
                            context.Set<T3>().Add(entity3);

                            entity4 = GenHelperEF.Getinstance.SetColIDZero<T4>(entity4);
                            context.Set<T4>().Add(entity4);

                            entity5 = GenHelperEF.Getinstance.SetColIDZero<T5>(entity5);
                            context.Set<T5>().Add(entity5);

                            hasil = context.SaveChanges();


                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;

            return result;
        }

        /// <summary>
        /// Metode Sync untuk menyimpan  data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>

        /// <returns></returns>
        public bool Save<T1, T2>(List<T1> entityCollection1, List<T2> entityCollection2) where T1 : class where T2 : class
        {

            int hasil = 0;
            bool result = false;

            if (entityCollection1 != null & entityCollection2 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                entityCollection1[i] = GenHelperEF.Getinstance.SetColIDZero<T1>(entityCollection1[i]);
                                context.Set<T1>().Add(entityCollection1[i]);
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                entityCollection2[i] = GenHelperEF.Getinstance.SetColIDZero<T2>(entityCollection2[i]);
                                context.Set<T2>().Add(entityCollection2[i]);
                            }
                            hasil = context.SaveChanges();

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;

            return result;
        }
        /// <summary>
        /// Metode Sync untuk menyimpan  data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection3">List entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Save<T1, T2, T3>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3)
            where T1 : class where T2 : class where T3 : class
        {
            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null & entityCollection3 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                entityCollection1[i] = GenHelperEF.Getinstance.SetColIDZero<T1>(entityCollection1[i]);
                                context.Set<T1>().Add(entityCollection1[i]);
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                entityCollection2[i] = GenHelperEF.Getinstance.SetColIDZero<T2>(entityCollection2[i]);
                                context.Set<T2>().Add(entityCollection2[i]);
                            }
                            for (int i = 0; i < entityCollection3.Count; i++)
                            {
                                entityCollection3[i] = GenHelperEF.Getinstance.SetColIDZero<T3>(entityCollection3[i]);
                                context.Set<T3>().Add(entityCollection3[i]);
                            }
                            hasil = context.SaveChanges();

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menyimpan  data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Room</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection3">List entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection4">List entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Save<T1, T2, T3, T4>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4)
            where T1 : class where T2 : class where T3 : class where T4 : class
        {
            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null & entityCollection3 != null & entityCollection4 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                entityCollection1[i] = GenHelperEF.Getinstance.SetColIDZero<T1>(entityCollection1[i]);
                                context.Set<T1>().Add(entityCollection1[i]);
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                entityCollection2[i] = GenHelperEF.Getinstance.SetColIDZero<T2>(entityCollection2[i]);
                                context.Set<T2>().Add(entityCollection2[i]);
                            }
                            for (int i = 0; i < entityCollection3.Count; i++)
                            {
                                entityCollection3[i] = GenHelperEF.Getinstance.SetColIDZero<T3>(entityCollection3[i]);
                                context.Set<T3>().Add(entityCollection3[i]);
                            }
                            for (int i = 0; i < entityCollection4.Count; i++)
                            {
                                entityCollection4[i] = GenHelperEF.Getinstance.SetColIDZero<T4>(entityCollection4[i]);
                                context.Set<T4>().Add(entityCollection4[i]);
                            }
                            hasil = context.SaveChanges();

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menyimpan  data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Room</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection3">List entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection4">List entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Save<T1, T2, T3, T4, T5>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4, List<T5> entityCollection5)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null & entityCollection3 != null & entityCollection4 != null & entityCollection5 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                entityCollection1[i] = GenHelperEF.Getinstance.SetColIDZero<T1>(entityCollection1[i]);
                                context.Set<T1>().Add(entityCollection1[i]);
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                entityCollection2[i] = GenHelperEF.Getinstance.SetColIDZero<T2>(entityCollection2[i]);
                                context.Set<T2>().Add(entityCollection2[i]);
                            }
                            for (int i = 0; i < entityCollection3.Count; i++)
                            {
                                entityCollection3[i] = GenHelperEF.Getinstance.SetColIDZero<T3>(entityCollection3[i]);
                                context.Set<T3>().Add(entityCollection3[i]);
                            }
                            for (int i = 0; i < entityCollection4.Count; i++)
                            {
                                entityCollection4[i] = GenHelperEF.Getinstance.SetColIDZero<T4>(entityCollection4[i]);
                                context.Set<T4>().Add(entityCollection4[i]);
                            }
                            for (int i = 0; i < entityCollection5.Count; i++)
                            {
                                entityCollection5[i] = GenHelperEF.Getinstance.SetColIDZero<T5>(entityCollection5[i]);
                                context.Set<T5>().Add(entityCollection5[i]);
                            }
                            hasil = context.SaveChanges();

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Async untuk menyimpan data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        /// 

        public async Task<bool> SaveAsync<T1, T2>(T1 entity1, T2 entity2) where T1 : class where T2 : class
        {

            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            entity1 = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                            context.Set<T1>().Add(entity1);

                            entity2 = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                            context.Set<T2>().Add(entity2);

                            hasil = await context.SaveChangesAsync();
                            result = hasil > 0 ? true : false;

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Async untuk menyimpan data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Dept</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity3">Entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> SaveAsync<T1, T2, T3>(T1 entity1, T2 entity2, T3 entity3) where T1 : class where T2 : class where T3 : class
        {

            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null & entity3 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            entity1 = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                            context.Set<T1>().Add(entity1);

                            entity2 = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                            context.Set<T2>().Add(entity2);

                            entity3 = GenHelperEF.Getinstance.SetColIDZero<T3>(entity3);
                            context.Set<T3>().Add(entity3);

                            hasil = await context.SaveChangesAsync();
                            result = hasil > 0 ? true : false;

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Async untuk menyimpan data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Dept</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity3">Entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity4">Entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> SaveAsync<T1, T2, T3, T4>(T1 entity1, T2 entity2, T3 entity3, T4 entity4)
            where T1 : class where T2 : class where T3 : class where T4 : class
        {

            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null & entity3 != null & entity4 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            entity1 = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                            context.Set<T1>().Add(entity1);

                            entity2 = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                            context.Set<T2>().Add(entity2);

                            entity3 = GenHelperEF.Getinstance.SetColIDZero<T3>(entity3);
                            context.Set<T3>().Add(entity3);

                            entity4 = GenHelperEF.Getinstance.SetColIDZero<T4>(entity4);
                            context.Set<T4>().Add(entity4);

                            hasil = await context.SaveChangesAsync();
                            result = hasil > 0 ? true : false;

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;

            return result;
        }
        /// <summary>
        /// Metode Async untuk menyimpan data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Dept</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity3">Entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity4">Entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> SaveAsync<T1, T2, T3, T4, T5>(T1 entity1, T2 entity2, T3 entity3, T4 entity4, T5 entity5)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {

            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null & entity3 != null & entity4 != null & entity5 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            entity1 = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                            context.Set<T1>().Add(entity1);

                            entity2 = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                            context.Set<T2>().Add(entity2);

                            entity3 = GenHelperEF.Getinstance.SetColIDZero<T3>(entity3);
                            context.Set<T3>().Add(entity3);

                            entity4 = GenHelperEF.Getinstance.SetColIDZero<T4>(entity4);
                            context.Set<T4>().Add(entity4);

                            entity5 = GenHelperEF.Getinstance.SetColIDZero<T5>(entity5);
                            context.Set<T5>().Add(entity5);

                            hasil = await context.SaveChangesAsync();
                            result = hasil > 0 ? true : false;

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// Metode Async untuk menyimpan data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> SaveAsync<T1, T2>(List<T1> entityCollection1, List<T2> entityCollection2) where T1 : class where T2 : class
        {

            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                entityCollection1[i] = GenHelperEF.Getinstance.SetColIDZero<T1>(entityCollection1[i]);
                                context.Set<T1>().Add(entityCollection1[i]);
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                entityCollection2[i] = GenHelperEF.Getinstance.SetColIDZero<T2>(entityCollection2[i]);
                                context.Set<T2>().Add(entityCollection2[i]);
                            }

                            hasil = await context.SaveChangesAsync();
                            result = hasil > 0 ? true : false;

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Async untuk menyimpan data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection3">List entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> SaveAsync<T1, T2, T3>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3)
            where T1 : class where T2 : class where T3 : class
        {

            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null & entityCollection3 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                entityCollection1[i] = GenHelperEF.Getinstance.SetColIDZero<T1>(entityCollection1[i]);
                                context.Set<T1>().Add(entityCollection1[i]);
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                entityCollection2[i] = GenHelperEF.Getinstance.SetColIDZero<T2>(entityCollection2[i]);
                                context.Set<T2>().Add(entityCollection2[i]);
                            }
                            for (int i = 0; i < entityCollection3.Count; i++)
                            {
                                entityCollection3[i] = GenHelperEF.Getinstance.SetColIDZero<T3>(entityCollection3[i]);
                                context.Set<T3>().Add(entityCollection3[i]);
                            }

                            hasil = await context.SaveChangesAsync();
                            result = hasil > 0 ? true : false;

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Async untuk menyimpan data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Room</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection3">List entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection4">List entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> SaveAsync<T1, T2, T3, T4>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4)
            where T1 : class where T2 : class where T3 : class where T4 : class
        {
            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null & entityCollection3 != null & entityCollection4 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                entityCollection1[i] = GenHelperEF.Getinstance.SetColIDZero<T1>(entityCollection1[i]);
                                context.Set<T1>().Add(entityCollection1[i]);
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                entityCollection2[i] = GenHelperEF.Getinstance.SetColIDZero<T2>(entityCollection2[i]);
                                context.Set<T2>().Add(entityCollection2[i]);
                            }
                            for (int i = 0; i < entityCollection3.Count; i++)
                            {
                                entityCollection3[i] = GenHelperEF.Getinstance.SetColIDZero<T3>(entityCollection3[i]);
                                context.Set<T3>().Add(entityCollection3[i]);
                            }
                            for (int i = 0; i < entityCollection4.Count; i++)
                            {
                                entityCollection4[i] = GenHelperEF.Getinstance.SetColIDZero<T4>(entityCollection4[i]);
                                context.Set<T4>().Add(entityCollection4[i]);
                            }

                            hasil = await context.SaveChangesAsync();
                            result = hasil > 0 ? true : false;

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;

            return result;
        }
        /// <summary>
        /// Metode Async untuk menyimpan data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Room</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection3">List entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection4">List entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> SaveAsync<T1, T2, T3, T4, T5>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4, List<T5> entityCollection5)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null & entityCollection3 != null & entityCollection4 != null & entityCollection5 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                entityCollection1[i] = GenHelperEF.Getinstance.SetColIDZero<T1>(entityCollection1[i]);
                                context.Set<T1>().Add(entityCollection1[i]);
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                entityCollection2[i] = GenHelperEF.Getinstance.SetColIDZero<T2>(entityCollection2[i]);
                                context.Set<T2>().Add(entityCollection2[i]);
                            }
                            for (int i = 0; i < entityCollection3.Count; i++)
                            {
                                entityCollection3[i] = GenHelperEF.Getinstance.SetColIDZero<T3>(entityCollection3[i]);
                                context.Set<T3>().Add(entityCollection3[i]);
                            }
                            for (int i = 0; i < entityCollection4.Count; i++)
                            {
                                entityCollection4[i] = GenHelperEF.Getinstance.SetColIDZero<T4>(entityCollection4[i]);
                                context.Set<T4>().Add(entityCollection4[i]);
                            }
                            for (int i = 0; i < entityCollection5.Count; i++)
                            {
                                entityCollection5[i] = GenHelperEF.Getinstance.SetColIDZero<T5>(entityCollection5[i]);
                                context.Set<T5>().Add(entityCollection5[i]);
                            }
                            hasil = await context.SaveChangesAsync();
                            result = hasil > 0 ? true : false;

                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// Metode Sync untuk menyimpan dan/atau mengubah data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Update<T1, T2>(T1 entity1, T2 entity2) where T1 : class where T2 : class
        {
            //  var context = GetDbContext();
            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var colNotNull1 = FilterLinq.PropertyColNotNull<T1>(entity1);
                            //entity1 = FilterLinq.PropertyColDefaultValue<T1>(entity1);
                            entity1 = PrepareEntityForUpdateNoSelect<T1>(entity1);
                            context.Set<T1>().Attach(entity1);
                            context.Entry(entity1).State = EntityState.Unchanged;
                            PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                            foreach (var property in colNotNull1)
                            {
                                if (piID != property)
                                {
                                    object value = property.GetValue(entity1);
                                    if (value != null)
                                    {
                                        context.Entry(entity1).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull2 = FilterLinq.PropertyColNotNull<T2>(entity2);
                            //entity2 = FilterLinq.PropertyColDefaultValue<T2>(entity2);
                            entity2 = PrepareEntityForUpdateNoSelect<T2>(entity2);
                            context.Set<T2>().Attach(entity2);
                            context.Entry(entity2).State = EntityState.Unchanged;
                            PropertyInfo piID2 = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                            foreach (var property in colNotNull2)
                            {
                                if (piID2 != property)
                                {
                                    object value = property.GetValue(entity2);
                                    if (value != null)
                                    {
                                        context.Entry(entity2).Property(property.Name).IsModified = true;
                                    }
                                }

                            };

                            hasil = context.SaveChanges();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menyimpan dan/atau mengubah data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Dept</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity3">Entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Update<T1, T2, T3>(T1 entity1, T2 entity2, T3 entity3) where T1 : class where T2 : class where T3 : class
        {

            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null & entity3 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var colNotNull1 = FilterLinq.PropertyColNotNull<T1>(entity1);
                            // entity1 = FilterLinq.PropertyColDefaultValue<T1>(entity1);
                            entity1 = PrepareEntityForUpdateNoSelect<T1>(entity1);
                            context.Set<T1>().Attach(entity1);
                            context.Entry(entity1).State = EntityState.Unchanged;
                            PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                            foreach (var property in colNotNull1)
                            {
                                if (piID != property)
                                {
                                    object value = property.GetValue(entity1);
                                    if (value != null)
                                    {
                                        context.Entry(entity1).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull2 = FilterLinq.PropertyColNotNull<T2>(entity2);
                            //entity2 = FilterLinq.PropertyColDefaultValue<T2>(entity2);
                            entity2 = PrepareEntityForUpdateNoSelect<T2>(entity2);
                            context.Set<T2>().Attach(entity2);
                            context.Entry(entity2).State = EntityState.Unchanged;
                            PropertyInfo piID2 = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                            foreach (var property in colNotNull2)
                            {
                                if (piID2 != property)
                                {
                                    object value = property.GetValue(entity2);
                                    if (value != null)
                                    {
                                        context.Entry(entity2).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull3 = FilterLinq.PropertyColNotNull<T3>(entity3);
                            //entity3 = FilterLinq.PropertyColDefaultValue<T3>(entity3);
                            entity3 = PrepareEntityForUpdateNoSelect<T3>(entity3);
                            context.Set<T3>().Attach(entity3);
                            context.Entry(entity3).State = EntityState.Unchanged;
                            PropertyInfo piID3 = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                            foreach (var property in colNotNull3)
                            {
                                if (piID3 != property)
                                {
                                    object value = property.GetValue(entity3);
                                    if (value != null)
                                    {
                                        context.Entry(entity3).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            hasil = context.SaveChanges();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;

            return result;
        }
        /// <summary>
        /// Metode Sync untuk menyimpan dan/atau mengubah data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Dept</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity3">Entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity4">Entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Update<T1, T2, T3, T4>(T1 entity1, T2 entity2, T3 entity3, T4 entity4)
            where T1 : class where T2 : class where T3 : class where T4 : class
        {

            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null & entity3 != null & entity4 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var colNotNull1 = FilterLinq.PropertyColNotNull<T1>(entity1);
                            //entity1 = FilterLinq.PropertyColDefaultValue<T1>(entity1);
                            entity1 = PrepareEntityForUpdateNoSelect<T1>(entity1);
                            context.Set<T1>().Attach(entity1);
                            context.Entry(entity1).State = EntityState.Unchanged;
                            PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                            foreach (var property in colNotNull1)
                            {
                                if (piID != property)
                                {
                                    object value = property.GetValue(entity1);
                                    if (value != null)
                                    {
                                        context.Entry(entity1).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull2 = FilterLinq.PropertyColNotNull<T2>(entity2);
                            //entity2 = FilterLinq.PropertyColDefaultValue<T2>(entity2);
                            entity2 = PrepareEntityForUpdateNoSelect<T2>(entity2);
                            context.Set<T2>().Attach(entity2);
                            context.Entry(entity2).State = EntityState.Unchanged;
                            PropertyInfo piID2 = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                            foreach (var property in colNotNull2)
                            {
                                if (piID2 != property)
                                {
                                    object value = property.GetValue(entity2);
                                    if (value != null)
                                    {
                                        context.Entry(entity2).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull3 = FilterLinq.PropertyColNotNull<T3>(entity3);
                            //entity3 = FilterLinq.PropertyColDefaultValue<T3>(entity3);
                            entity3 = PrepareEntityForUpdateNoSelect<T3>(entity3);
                            context.Set<T3>().Attach(entity3);
                            context.Entry(entity3).State = EntityState.Unchanged;
                            PropertyInfo piID3 = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                            foreach (var property in colNotNull3)
                            {
                                if (piID3 != property)
                                {
                                    object value = property.GetValue(entity3);
                                    if (value != null)
                                    {
                                        context.Entry(entity3).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull4 = FilterLinq.PropertyColNotNull<T4>(entity4);
                            //entity4 = FilterLinq.PropertyColDefaultValue<T4>(entity4);
                            entity4 = PrepareEntityForUpdateNoSelect<T4>(entity4);
                            context.Set<T4>().Attach(entity4);
                            context.Entry(entity4).State = EntityState.Unchanged;
                            PropertyInfo piID4 = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                            foreach (var property in colNotNull4)
                            {
                                if (piID4 != property)
                                {
                                    object value = property.GetValue(entity4);
                                    if (value != null)
                                    {
                                        context.Entry(entity4).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            hasil = context.SaveChanges();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menyimpan dan/atau mengubah data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. 
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Dept</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Kamar</typeparam>
        ///  <typeparam name="T5">Tabel kelas entitas 5 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity3">Entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity4">Entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity5">Entitas 5 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Update<T1, T2, T3, T4, T5>(T1 entity1, T2 entity2, T3 entity3, T4 entity4, T5 entity5)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {

            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null & entity3 != null & entity4 != null & entity5 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var colNotNull1 = FilterLinq.PropertyColNotNull<T1>(entity1);
                            //entity1 = FilterLinq.PropertyColDefaultValue<T1>(entity1);
                            entity1 = PrepareEntityForUpdateNoSelect<T1>(entity1);
                            context.Set<T1>().Attach(entity1);
                            context.Entry(entity1).State = EntityState.Unchanged;
                            PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                            foreach (var property in colNotNull1)
                            {
                                if (piID != property)
                                {
                                    object value = property.GetValue(entity1);
                                    if (value != null)
                                    {
                                        context.Entry(entity1).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull2 = FilterLinq.PropertyColNotNull<T2>(entity2);
                            //entity2 = FilterLinq.PropertyColDefaultValue<T2>(entity2);
                            entity2 = PrepareEntityForUpdateNoSelect<T2>(entity2);
                            context.Set<T2>().Attach(entity2);
                            context.Entry(entity2).State = EntityState.Unchanged;
                            PropertyInfo piID2 = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                            foreach (var property in colNotNull2)
                            {
                                if (piID2 != property)
                                {
                                    object value = property.GetValue(entity2);
                                    if (value != null)
                                    {
                                        context.Entry(entity2).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull3 = FilterLinq.PropertyColNotNull<T3>(entity3);
                            // entity3 = FilterLinq.PropertyColDefaultValue<T3>(entity3);
                            entity3 = PrepareEntityForUpdateNoSelect<T3>(entity3);
                            context.Set<T3>().Attach(entity3);
                            context.Entry(entity3).State = EntityState.Unchanged;
                            PropertyInfo piID3 = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                            foreach (var property in colNotNull3)
                            {
                                if (piID3 != property)
                                {
                                    object value = property.GetValue(entity3);
                                    if (value != null)
                                    {
                                        context.Entry(entity3).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull4 = FilterLinq.PropertyColNotNull<T4>(entity4);
                            // entity4 = FilterLinq.PropertyColDefaultValue<T4>(entity4);
                            entity4 = PrepareEntityForUpdateNoSelect<T4>(entity4);
                            context.Set<T4>().Attach(entity4);
                            context.Entry(entity4).State = EntityState.Unchanged;
                            PropertyInfo piID4 = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                            foreach (var property in colNotNull4)
                            {
                                if (piID4 != property)
                                {
                                    object value = property.GetValue(entity4);
                                    if (value != null)
                                    {
                                        context.Entry(entity4).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull5 = FilterLinq.PropertyColNotNull<T5>(entity5);
                            //entity5 = FilterLinq.PropertyColDefaultValue<T5>(entity5);
                            entity5 = PrepareEntityForUpdateNoSelect<T5>(entity5);
                            context.Set<T5>().Attach(entity5);
                            context.Entry(entity5).State = EntityState.Unchanged;
                            PropertyInfo piID5 = GenHelperEF.Getinstance.GetIdentityColumnProps<T5>();
                            foreach (var property in colNotNull5)
                            {
                                if (piID5 != property)
                                {
                                    object value = property.GetValue(entity5);
                                    if (value != null)
                                    {
                                        context.Entry(entity5).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            hasil = context.SaveChanges();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// Metode Sync untuk menyimpan dan/atau mengubah data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. Pengubahan hanya untuk kolom yang valuenya NotNull aja ya
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Update<T1, T2>(List<T1> entityCollection1, List<T2> entityCollection2) where T1 : class where T2 : class
        {

            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                var myentity = entityCollection1[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T1>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T1>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T1>(myentity);

                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                var myentity = entityCollection2[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T2>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T2>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T2>(myentity);
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }

                            hasil = context.SaveChanges();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menyimpan dan/atau mengubah data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. Pengubahan hanya untuk kolom yang valuenya NotNull aja ya
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection3">List entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Update<T1, T2, T3>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3)
            where T1 : class where T2 : class where T3 : class
        {

            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null & entityCollection3 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                var myentity = entityCollection1[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T1>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T1>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T1>(myentity);
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                var myentity = entityCollection2[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T2>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T2>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T2>(myentity);
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection3.Count; i++)
                            {
                                var myentity = entityCollection3[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T3>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T3>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T3>(myentity);
                                context.Set<T3>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }


                            hasil = context.SaveChanges();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menyimpan dan/atau mengubah data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. Pengubahan hanya untuk kolom yang valuenya NotNull aja ya
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Room</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection3">List entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection4">List entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Update<T1, T2, T3, T4>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4)
            where T1 : class where T2 : class where T3 : class where T4 : class
        {

            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null & entityCollection3 != null & entityCollection4 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                var myentity = entityCollection1[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T1>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T1>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T1>(myentity);
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                var myentity = entityCollection2[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T2>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T2>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T2>(myentity);
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection3.Count; i++)
                            {
                                var myentity = entityCollection3[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T3>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T3>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T3>(myentity);
                                context.Set<T3>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection4.Count; i++)
                            {
                                var myentity = entityCollection4[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T4>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T4>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T4>(myentity);
                                context.Set<T4>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }

                            hasil = context.SaveChanges();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menyimpan dan/atau mengubah data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. Pengubahan hanya untuk kolom yang valuenya NotNull aja ya
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Room</typeparam>
        /// <typeparam name="T5">Tabel kelas entitas 5 yang akan diproses  ex.MT_Room</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection3">List entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection4">List entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection5">List entitas 5 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public bool Update<T1, T2, T3, T4, T5>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4, List<T5> entityCollection5)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null & entityCollection3 != null & entityCollection4 != null & entityCollection5 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                var myentity = entityCollection1[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T1>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T1>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T1>(myentity);
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                var myentity = entityCollection2[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T2>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T2>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T2>(myentity);
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection3.Count; i++)
                            {
                                var myentity = entityCollection3[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T3>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T3>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T3>(myentity);
                                context.Set<T3>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection4.Count; i++)
                            {
                                var myentity = entityCollection4[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T4>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T4>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T4>(myentity);
                                context.Set<T4>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection5.Count; i++)
                            {
                                var myentity = entityCollection5[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T5>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T5>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T5>(myentity);
                                context.Set<T5>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T5>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            hasil = context.SaveChanges();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// Metode Async untuk mengubah data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. Pengubahan hanya untuk kolom yang valuenya NotNull aja ya
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<T1, T2>(T1 entity1, T2 entity2) where T1 : class where T2 : class
        {

            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var colNotNull1 = FilterLinq.PropertyColNotNull<T1>(entity1);
                            //entity1 = FilterLinq.PropertyColDefaultValue<T1>(entity1);
                            entity1 = PrepareEntityForUpdateNoSelect<T1>(entity1);
                            context.Set<T1>().Attach(entity1);
                            context.Entry(entity1).State = EntityState.Unchanged;
                            PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                            foreach (var property in colNotNull1)
                            {
                                if (piID != property)
                                {
                                    object value = property.GetValue(entity1);
                                    if (value != null)
                                    {
                                        context.Entry(entity1).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull2 = FilterLinq.PropertyColNotNull<T2>(entity2);
                            //entity2 = FilterLinq.PropertyColDefaultValue<T2>(entity2);
                            entity2 = PrepareEntityForUpdateNoSelect<T2>(entity2);
                            context.Set<T2>().Attach(entity2);
                            context.Entry(entity2).State = EntityState.Unchanged;
                            PropertyInfo piID2 = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                            foreach (var property in colNotNull2)
                            {
                                if (piID2 != property)
                                {
                                    object value = property.GetValue(entity2);
                                    if (value != null)
                                    {
                                        context.Entry(entity2).Property(property.Name).IsModified = true;
                                    }
                                }

                            };

                            hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Async untuk mengubah data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. Pengubahan hanya untuk kolom yang valuenya NotNull aja ya
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Dept</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity3">Entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<T1, T2, T3>(T1 entity1, T2 entity2, T3 entity3) where T1 : class where T2 : class where T3 : class
        {

            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null & entity3 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var colNotNull1 = FilterLinq.PropertyColNotNull<T1>(entity1);
                            //entity1 = FilterLinq.PropertyColDefaultValue<T1>(entity1);
                            entity1 = PrepareEntityForUpdateNoSelect<T1>(entity1);
                            context.Set<T1>().Attach(entity1);
                            context.Entry(entity1).State = EntityState.Unchanged;
                            PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                            foreach (var property in colNotNull1)
                            {
                                if (piID != property)
                                {
                                    object value = property.GetValue(entity1);
                                    if (value != null)
                                    {
                                        context.Entry(entity1).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull2 = FilterLinq.PropertyColNotNull<T2>(entity2);
                            //entity2 = FilterLinq.PropertyColDefaultValue<T2>(entity2);
                            entity2 = PrepareEntityForUpdateNoSelect<T2>(entity2);
                            context.Set<T2>().Attach(entity2);
                            context.Entry(entity2).State = EntityState.Unchanged;
                            PropertyInfo piID2 = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                            foreach (var property in colNotNull2)
                            {
                                if (piID2 != property)
                                {
                                    object value = property.GetValue(entity2);
                                    if (value != null)
                                    {
                                        context.Entry(entity2).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull3 = FilterLinq.PropertyColNotNull<T3>(entity3);
                            //entity3 = FilterLinq.PropertyColDefaultValue<T3>(entity3);
                            entity3 = PrepareEntityForUpdateNoSelect<T3>(entity3);
                            context.Set<T3>().Attach(entity3);
                            context.Entry(entity3).State = EntityState.Unchanged;
                            PropertyInfo piID3 = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                            foreach (var property in colNotNull3)
                            {
                                if (piID3 != property)
                                {
                                    object value = property.GetValue(entity3);
                                    if (value != null)
                                    {
                                        context.Entry(entity3).Property(property.Name).IsModified = true;
                                    }
                                }

                            };

                            hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Async untuk  mengubah data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. Pengubahan hanya untuk kolom yang valuenya NotNull aja ya
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Dept</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity3">Entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity4">Entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<T1, T2, T3, T4>(T1 entity1, T2 entity2, T3 entity3, T4 entity4)
            where T1 : class where T2 : class where T3 : class where T4 : class
        {

            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null & entity3 != null & entity4 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var colNotNull1 = FilterLinq.PropertyColNotNull<T1>(entity1);
                            //entity1 = FilterLinq.PropertyColDefaultValue<T1>(entity1);
                            entity1 = PrepareEntityForUpdateNoSelect<T1>(entity1);
                            context.Set<T1>().Attach(entity1);
                            context.Entry(entity1).State = EntityState.Unchanged;
                            PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                            foreach (var property in colNotNull1)
                            {
                                if (piID != property)
                                {
                                    object value = property.GetValue(entity1);
                                    if (value != null)
                                    {
                                        context.Entry(entity1).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull2 = FilterLinq.PropertyColNotNull<T2>(entity2);
                            //entity2 = FilterLinq.PropertyColDefaultValue<T2>(entity2);
                            entity2 = PrepareEntityForUpdateNoSelect<T2>(entity2);
                            context.Set<T2>().Attach(entity2);
                            context.Entry(entity2).State = EntityState.Unchanged;
                            PropertyInfo piID2 = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                            foreach (var property in colNotNull2)
                            {
                                if (piID2 != property)
                                {
                                    object value = property.GetValue(entity2);
                                    if (value != null)
                                    {
                                        context.Entry(entity2).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull3 = FilterLinq.PropertyColNotNull<T3>(entity3);
                            //entity3 = FilterLinq.PropertyColDefaultValue<T3>(entity3);
                            entity3 = PrepareEntityForUpdateNoSelect<T3>(entity3);
                            context.Set<T3>().Attach(entity3);
                            context.Entry(entity3).State = EntityState.Unchanged;
                            PropertyInfo piID3 = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                            foreach (var property in colNotNull3)
                            {
                                if (piID3 != property)
                                {
                                    object value = property.GetValue(entity3);
                                    if (value != null)
                                    {
                                        context.Entry(entity3).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull4 = FilterLinq.PropertyColNotNull<T4>(entity4);
                            //entity4 = FilterLinq.PropertyColDefaultValue<T4>(entity4);
                            entity4 = PrepareEntityForUpdateNoSelect<T4>(entity4);
                            context.Set<T4>().Attach(entity4);
                            context.Entry(entity4).State = EntityState.Unchanged;
                            PropertyInfo piID4 = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                            foreach (var property in colNotNull4)
                            {
                                if (piID4 != property)
                                {
                                    object value = property.GetValue(entity4);
                                    if (value != null)
                                    {
                                        context.Entry(entity4).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;

        }
        /// <summary>
        /// Metode Async untuk  mengubah data. Bisa untuk melakukan proses pada entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. Pengubahan hanya untuk kolom yang valuenya NotNull aja ya
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Dept</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Kamar</typeparam>
        ///  <typeparam name="T5">Tabel kelas entitas 4 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <param name="entity1">Entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity2">Entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity3">Entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity4">Entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entity5">Entitas 5 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<T1, T2, T3, T4, T5>(T1 entity1, T2 entity2, T3 entity3, T4 entity4, T5 entity5)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {

            int hasil = 0;
            bool result = false;
            if (entity1 != null & entity2 != null & entity3 != null & entity4 != null & entity5 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var colNotNull1 = FilterLinq.PropertyColNotNull<T1>(entity1);
                            //entity1 = FilterLinq.PropertyColDefaultValue<T1>(entity1);
                            entity1 = PrepareEntityForUpdateNoSelect<T1>(entity1);
                            context.Set<T1>().Attach(entity1);
                            context.Entry(entity1).State = EntityState.Unchanged;
                            PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                            foreach (var property in colNotNull1)
                            {
                                if (piID != property)
                                {
                                    object value = property.GetValue(entity1);
                                    if (value != null)
                                    {
                                        context.Entry(entity1).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull2 = FilterLinq.PropertyColNotNull<T2>(entity2);
                            //entity2 = FilterLinq.PropertyColDefaultValue<T2>(entity2);
                            entity2 = PrepareEntityForUpdateNoSelect<T2>(entity2);
                            context.Set<T2>().Attach(entity2);
                            context.Entry(entity2).State = EntityState.Unchanged;
                            PropertyInfo piID2 = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                            foreach (var property in colNotNull2)
                            {
                                if (piID2 != property)
                                {
                                    object value = property.GetValue(entity2);
                                    if (value != null)
                                    {
                                        context.Entry(entity2).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull3 = FilterLinq.PropertyColNotNull<T3>(entity3);
                            //entity3 = FilterLinq.PropertyColDefaultValue<T3>(entity3);
                            entity3 = PrepareEntityForUpdateNoSelect<T3>(entity3);
                            context.Set<T3>().Attach(entity3);
                            context.Entry(entity3).State = EntityState.Unchanged;
                            PropertyInfo piID3 = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                            foreach (var property in colNotNull3)
                            {
                                if (piID3 != property)
                                {
                                    object value = property.GetValue(entity3);
                                    if (value != null)
                                    {
                                        context.Entry(entity3).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull4 = FilterLinq.PropertyColNotNull<T4>(entity4);
                            //entity4 = FilterLinq.PropertyColDefaultValue<T4>(entity4);
                            entity4 = PrepareEntityForUpdateNoSelect<T4>(entity4);
                            context.Set<T4>().Attach(entity4);
                            context.Entry(entity4).State = EntityState.Unchanged;
                            PropertyInfo piID4 = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                            foreach (var property in colNotNull4)
                            {
                                if (piID4 != property)
                                {
                                    object value = property.GetValue(entity4);
                                    if (value != null)
                                    {
                                        context.Entry(entity4).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            var colNotNull5 = FilterLinq.PropertyColNotNull<T5>(entity5);
                            //entity5 = FilterLinq.PropertyColDefaultValue<T5>(entity5);
                            entity5 = PrepareEntityForUpdateNoSelect<T5>(entity5);
                            context.Set<T5>().Attach(entity5);
                            context.Entry(entity5).State = EntityState.Unchanged;
                            PropertyInfo piID5 = GenHelperEF.Getinstance.GetIdentityColumnProps<T5>();
                            foreach (var property in colNotNull5)
                            {
                                if (piID5 != property)
                                {
                                    object value = property.GetValue(entity5);
                                    if (value != null)
                                    {
                                        context.Entry(entity5).Property(property.Name).IsModified = true;
                                    }
                                }

                            };
                            hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// Metode Async untuk mengubah data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. Pengubahan hanya untuk kolom yang valuenya NotNull aja ya
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>

        /// <returns></returns>
        public async Task<bool> UpdateAsync<T1, T2>(List<T1> entityCollection1, List<T2> entityCollection2) where T1 : class where T2 : class
        {

            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                var myentity = entityCollection1[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T1>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T1>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T1>(myentity);
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                var myentity = entityCollection2[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T2>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T2>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T2>(myentity);
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Async untuk mengubah data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. Pengubahan hanya untuk kolom yang valuenya NotNull aja ya
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection3">List entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<T1, T2, T3>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3)
            where T1 : class where T2 : class where T3 : class
        {
            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null & entityCollection3 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                var myentity = entityCollection1[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T1>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T1>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T1>(myentity);
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                var myentity = entityCollection2[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T2>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T2>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T2>(myentity);
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection3.Count; i++)
                            {
                                var myentity = entityCollection3[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T3>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T3>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T3>(myentity);
                                context.Set<T3>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }


                            hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Async untuk mengubah data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. Pengubahan hanya untuk kolom yang valuenya NotNull aja ya
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Room</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection3">List entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection4">List entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<T1, T2, T3, T4>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4)
            where T1 : class where T2 : class where T3 : class where T4 : class
        {
            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null & entityCollection3 != null & entityCollection4 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                var myentity = entityCollection1[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T1>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T1>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T1>(myentity);
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                var myentity = entityCollection2[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T2>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T2>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T2>(myentity);
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection3.Count; i++)
                            {
                                var myentity = entityCollection3[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T3>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T3>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T3>(myentity);
                                context.Set<T3>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection4.Count; i++)
                            {
                                var myentity = entityCollection4[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T4>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T4>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T4>(myentity);
                                context.Set<T4>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }

                            hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Async untuk  mengubah data. Bisa untuk melakukan proses pada list entitas yang berbeda-beda dari T1...Tn.
        /// ex. MT_Division dan MT_Dept akan disimpan secara bersama-sama. Pengubahan hanya untuk kolom yang valuenya NotNull aja ya
        /// </summary>
        /// <typeparam name="T1">Tabel kelas entitas 1 yang akan diproses ex.MT_Division</typeparam>
        /// <typeparam name="T2">Tabel kelas entitas 2 yang akan diproses  ex.MT_Rumah</typeparam>
        /// <typeparam name="T3">Tabel kelas entitas 3 yang akan diproses  ex.MT_Kamar</typeparam>
        /// <typeparam name="T4">Tabel kelas entitas 4 yang akan diproses  ex.MT_Room</typeparam>
        ///  <typeparam name="T5">Tabel kelas entitas 5 yang akan diproses  ex.MT_Roomz</typeparam>
        /// <param name="entityCollection1">List entitas 1 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection2">List entitas 2 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection3">List entitas 3 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection4">List entitas 4 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <param name="entityCollection5">List entitas 5 yang akan di simpan/ubah.  Untuk yang akan diubah,isi value kolom yang akan di update saja. Hemat</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<T1, T2, T3, T4, T5>(List<T1> entityCollection1, List<T2> entityCollection2, List<T3> entityCollection3, List<T4> entityCollection4, List<T5> entityCollection5)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            int hasil = 0;
            bool result = false;
            if (entityCollection1 != null & entityCollection2 != null & entityCollection3 != null & entityCollection4 != null & entityCollection5 != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection1.Count; i++)
                            {
                                var myentity = entityCollection1[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T1>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T1>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T1>(myentity);
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection2.Count; i++)
                            {
                                var myentity = entityCollection2[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T2>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T2>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T2>(myentity);
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection3.Count; i++)
                            {
                                var myentity = entityCollection3[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T3>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T3>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T3>(myentity);
                                context.Set<T3>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection4.Count; i++)
                            {
                                var myentity = entityCollection4[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T4>(myentity);
                                //myentity = FilterLinq.PropertyColDefaultValue<T4>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T4>(myentity);
                                context.Set<T4>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            for (int i = 0; i < entityCollection5.Count; i++)
                            {
                                var myentity = entityCollection5[i];
                                var colNotNull = FilterLinq.PropertyColNotNull<T5>(myentity);
                                // myentity = FilterLinq.PropertyColDefaultValue<T5>(myentity);
                                myentity = PrepareEntityForUpdateNoSelect<T5>(myentity);
                                context.Set<T5>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T5>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                };
                            }
                            hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }


        /// <summary>
        /// Metode Sync untuk menghapus data secara fisik, hard delete ya.1 record doank ya
        /// </summary>
        /// <typeparam name="T">Kelas tabel database </typeparam>
        /// <param name="entity">Instance kelas yang akan dihapus datanya</param>
        /// <returns></returns>
        public bool Delete<T>(T entity) where T : class
        {
            bool result = false;
            using (var context = GetDbContext())
            {
                if (entity != null)
                {

                    try
                    {
                        context.Set<T>().Remove(entity);
                        if (context.SaveChanges() > 0)
                            result = true;
                    }
                    catch { }
                }
            }
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menghapus list data secara fisik, hard delete ya.Note yang ini bisa banyak record doank ya
        /// Ada Jaminan, jika data sukses di hapus semua maka akan return true, jika ada salah satu yang gagal,maka digagalkan semua dan  akan return false.
        /// </summary>
        /// <typeparam name="T">Kelas tabel database </typeparam>
        /// <param name="entityCollection">List instance kelas yang akan dihapus datanya</param>
        /// <returns></returns>
        public bool Delete<T>(List<T> entityCollection) where T : class
        {
            bool result = false;
            int hasil = 0;
            using (var context = GetDbContext())
            {
                using (var contextTrans = context.Database.BeginTransaction())
                {
                    if (entityCollection != null)
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection.Count(); i++)
                            {
                                context.Set<T>().Remove(entityCollection[i]);
                            }
                            hasil = context.SaveChanges();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menghapus data secara fisik, hard delete ya.1 record doank ya
        /// </summary>
        /// <typeparam name="T">Kelas tabel database </typeparam>
        /// <param name="entity">Instance kelas yang akan dihapus datanya</param>
        /// <returns></returns>
        public bool Delete<T>(int IDIdentity) where T : class
        {
            bool result = false;
            if (IDIdentity > 0)
            {
                using (var context = GetDbContext())
                {
                    var wanttodelete = this.ListByID<T>(IDIdentity);
                    if (wanttodelete != null)
                    {
                        try
                        {
                            context.Set<T>().Remove(wanttodelete);
                            if (context.SaveChanges() > 0)
                                result = true;
                        }
                        catch { }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menghapus list data secara fisik, hard delete ya.Note yang ini bisa banyak record doank ya
        /// Ada Jaminan, jika data sukses di hapus semua maka akan return true, jika ada salah satu yang gagal,maka digagalkan semua dan  akan return false.
        /// </summary>
        /// <typeparam name="T">Kelas tabel database </typeparam>
        /// <param name="entityCollection">List instance kelas yang akan dihapus datanya</param>
        /// <returns></returns>
        public bool Delete<T>(List<int> IDIdentity) where T : class
        {
            bool result = false;
            int hasil = 0;
            if (IDIdentity != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < IDIdentity.Count(); i++)
                            {
                                var wanttodelete = this.ListByID<T>(IDIdentity[i]);
                                context.Set<T>().Remove(wanttodelete);
                            }
                            hasil = context.SaveChanges();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menghapus data secara non fisik,soft delete ya,cuma update active bool = false.1 record doank ya
        /// Ada Jaminan, jika data sukses di hapus semua maka akan return true, jika ada salah satu yang gagal,maka digagalkan semua dan  akan return false.
        /// </summary>
        /// <typeparam name="T">Kelas tabel database </typeparam>
        /// <param name="identityID">int identity ID yang akan dihapus</param>
        /// <returns></returns>
        public bool DeleteActiveBool<T>(int identityID, string pic) where T : class
        {
            bool result = false;
            var entity = GenHelperEF.Getinstance.ActiveBoolColumnFalse<T>(identityID, pic);
            if (entity != null)
            {
                result = this.Update<T>(entity);
            }

            return result;
        }
        /// <summary>
        /// Metode Sync untuk menghapus data secara non fisik,soft delete ya,cuma update active bool = false .Note yang ini bisa banyak record doank ya
        /// Ada Jaminan, jika data sukses di hapus semua maka akan return true, jika ada salah satu yang gagal,maka digagalkan semua dan  akan return false.
        /// </summary>
        /// <typeparam name="T">Kelas tabel database </typeparam>
        /// <param name="identityID">List<int></int> identity ID yang akan dihapus</param>
        /// <returns></returns>
        public bool DeleteActiveBool<T>(List<int> identityID, string pic) where T : class
        {
            bool result = false;
            var entityCollection = GenHelperEF.Getinstance.ActiveBoolColumnFalse<T>(identityID, pic);
            if (entityCollection != null)
            {
                result = this.Update<T>(entityCollection);
            }

            return result;
        }

         

        /// <summary>
        /// Metode ASync untuk menghapus data secara fisik, hard delete ya.1 record doank ya
        /// </summary>
        /// <typeparam name="T">Kelas tabel database </typeparam>
        /// <param name="entity">Instance kelas yang akan dihapus datanya</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync<T>(T entity) where T : class
        {
            bool result = false;
            using (var context = GetDbContext())
            {
                if (entity != null)
                {

                    try
                    {
                        context.Set<T>().Remove(entity);
                        if (await context.SaveChangesAsync() > 0)
                            result = true;
                    }
                    catch { }
                }
            }
            return result;
        }
        /// <summary>
        /// Metode ASync untuk menghapus list data secara fisik, hard delete ya.Note yang ini bisa banyak record doank ya
        /// Ada Jaminan, jika data sukses di hapus semua maka akan return true, jika ada salah satu yang gagal,maka digagalkan semua dan  akan return false.
        /// </summary>
        /// <typeparam name="T">Kelas tabel database </typeparam>
        /// <param name="entityCollection">List instance kelas yang akan dihapus datanya</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync<T>(List<T> entityCollection) where T : class
        {
            bool result = false;
            int hasil = 0;
            using (var context = GetDbContext())
            {
                using (var contextTrans = context.Database.BeginTransaction())
                {
                    if (entityCollection != null)
                    {
                        try
                        {
                            for (int i = 0; i < entityCollection.Count(); i++)
                            {
                                context.Set<T>().Remove(entityCollection[i]);
                            }
                            hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menghapus data secara fisik, hard delete ya.1 record doank ya
        /// </summary>
        /// <typeparam name="T">Kelas tabel database </typeparam>
        /// <param name="entity">Instance kelas yang akan dihapus datanya</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync<T>(int IDIdentity) where T : class
        {
            bool result = false;
            if (IDIdentity > 0)
            {
                using (var context = GetDbContext())
                {
                    var wanttodelete = await this.ListByIDAsync<T>(IDIdentity);
                    if (wanttodelete != null)
                    {
                        try
                        {
                            context.Set<T>().Remove(wanttodelete);
                            var cek1 = await context.SaveChangesAsync();
                            if (cek1 > 0)
                                result = true;
                        }
                        catch { }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menghapus list data secara fisik, hard delete ya.Note yang ini bisa banyak record doank ya
        /// Ada Jaminan, jika data sukses di hapus semua maka akan return true, jika ada salah satu yang gagal,maka digagalkan semua dan  akan return false.
        /// </summary>
        /// <typeparam name="T">Kelas tabel database </typeparam>
        /// <param name="entityCollection">List instance kelas yang akan dihapus datanya</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync<T>(List<int> IDIdentity) where T : class
        {
            bool result = false;
            int hasil = 0;
            if (IDIdentity != null)
            {
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < IDIdentity.Count(); i++)
                            {
                                var wanttodelete = this.ListByID<T>(IDIdentity[i]);
                                context.Set<T>().Remove(wanttodelete);
                            }
                            hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }
                    }
                }
            }
            result = hasil > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// Metode ASync untuk menghapus data secara non fisik,soft delete ya,cuma update active bool = false.1 record doank ya
        /// Ada Jaminan, jika data sukses di hapus semua maka akan return true, jika ada salah satu yang gagal,maka digagalkan semua dan  akan return false.
        /// </summary>
        /// <typeparam name="T">Kelas tabel database </typeparam>
        /// <param name="identityID">int identity ID yang akan dihapus</param>
        /// <returns></returns>
        public async Task<bool> DeleteActiveBoolAsync<T>(int identityID, int UserId,string UserByName) where T : class
        {
            bool result = false;
            var entity = GenHelperEF.Getinstance.ActiveBoolColumnFalse<T>(identityID, UserId, UserByName);
            entity = await PrepareEntityForUpdateAsync<T>(entity);//
            if (entity != null)
            {
                result = await this.UpdateAllAsync<T>(entity);
            }
            return result;
        }
        /// <summary>
        /// Metode ASync untuk menghapus data secara non fisik,soft delete ya,cuma update active bool = false .Note yang ini bisa banyak record doank ya
        /// Ada Jaminan, jika data sukses di hapus semua maka akan return true, jika ada salah satu yang gagal,maka digagalkan semua dan  akan return false.
        /// </summary>
        /// <typeparam name="T">Kelas tabel database </typeparam>
        /// <param name="identityID">List<int></int> identity ID yang akan dihapus</param>
        /// <returns></returns>
        public async Task<bool> DeleteActiveBoolAsync<T>(List<int> identityID, int UserId,string UserByName) where T : class
        {
            bool result = false;
            var entityCollection = GenHelperEF.Getinstance.ActiveBoolColumnFalse<T>(identityID, UserId, UserByName);
            entityCollection = await PrepareEntityForUpdateAsync<T>(entityCollection);
            if (entityCollection != null)
            {
                result = await this.UpdateAllAsync<T>(entityCollection);
            }

            return result;
        }

        /// <summary>
        /// Metode Sync untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <returns></returns>
        public bool SaveHeaderDetail<T, T1>(T tblHeader, string IDConnectedColName, T1 tblDetail1)
            where T : class where T1 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            object checkIDresult = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
             )
            {
                this.Save(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1, IDConnectedColName, mainID);
                    var result1 = this.Save(tblDetail1);
                    PropertyInfo piresult = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                    if (piresult != null)
                        checkIDresult = piresult.GetValue(result1);
                    if (checkIDresult != null)
                    {
                        try
                        {
                            if (Convert.ToInt32(checkIDresult) > 0)
                            {
                                result = true;
                            }
                        }
                        catch { }
                    }

                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    this.Delete<T>(mainID);
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Sync untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <returns></returns>
        public bool SaveHeaderDetail<T, T1, T2>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2)
            where T : class where T1 : class where T2 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
              & tblDetail2 != null
             )
            {
                this.Save(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2, IDConnectedColName, mainID);
                    result = this.Save<T1, T2>(tblDetail1, tblDetail2);
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    this.Delete<T>(mainID);
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Sync untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        ///  <typeparam name="T3">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <param name="tblDetail3">tabel detail</param>
        /// <returns></returns>
        public bool SaveHeaderDetail<T, T1, T2, T3>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2, T3 tblDetail3)
            where T : class where T1 : class where T2 : class where T3 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
              & tblDetail2 != null & tblDetail3 != null
             )
            {
                this.Save(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T3>(tblDetail3, IDConnectedColName, mainID);
                    result = this.Save<T1, T2, T3>(tblDetail1, tblDetail2, tblDetail3);
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    this.Delete<T>(mainID);
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Sync untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        ///  <typeparam name="T3">Kelas Details</typeparam>
        ///  <typeparam name="T4">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <param name="tblDetail3">tabel detail</param>
        /// <param name="tblDetail4">tabel detail</param>
        /// <returns></returns>
        public bool SaveHeaderDetail<T, T1, T2, T3, T4>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2, T3 tblDetail3, T4 tblDetail4)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
              & tblDetail2 != null & tblDetail3 != null & tblDetail4 != null
             )
            {
                this.Save(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T3>(tblDetail3, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T4>(tblDetail4, IDConnectedColName, mainID);
                    result = this.Save<T1, T2, T3, T4>(tblDetail1, tblDetail2, tblDetail3, tblDetail4);
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    this.Delete<T>(mainID);
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Sync untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        ///  <typeparam name="T3">Kelas Details</typeparam>
        ///  <typeparam name="T4">Kelas Details</typeparam>
        ///   <typeparam name="T5">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <param name="tblDetail3">tabel detail</param>
        /// <param name="tblDetail4">tabel detail</param>
        /// <param name="tblDetail5">tabel detail</param>
        /// <returns></returns>
        public bool SaveHeaderDetail<T, T1, T2, T3, T4, T5>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2, T3 tblDetail3, T4 tblDetail4, T5 tblDetail5)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
              & tblDetail2 != null & tblDetail3 != null & tblDetail4 != null & tblDetail5 != null
             )
            {
                this.Save(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T3>(tblDetail3, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T4>(tblDetail4, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T5>(tblDetail5, IDConnectedColName, mainID);
                    result = this.Save<T1, T2, T3, T4, T5>(tblDetail1, tblDetail2, tblDetail3, tblDetail4, tblDetail5);
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    this.Delete<T>(mainID);
                }
            }
            return result;

        }

        /// <summary>
        /// Metode Sync untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>     
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>     
        /// <returns></returns>
        public bool SaveHeaderDetail<T, T1>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1)
            where T : class where T1 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
             )
            {
                this.Save(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    if (tblDetail1 != null)
                    {
                        for (int i = 0; i < tblDetail1.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1[i], IDConnectedColName, mainID);
                        }
                    }
                    var result1 = this.Save<T1>(tblDetail1);
                    if (result1 != null)
                    {
                        result = true;
                    }
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    this.Delete<T>(mainID);
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Sync untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <returns></returns>
        public bool SaveHeaderDetail<T, T1, T2>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2)
            where T : class where T1 : class where T2 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
              & tblDetail2 != null)
            {
                this.Save(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    if (tblDetail1 != null)
                    {
                        for (int i = 0; i < tblDetail1.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1[i], IDConnectedColName, mainID);
                        }
                    }
                    if (tblDetail2 != null)
                    {
                        for (int i = 0; i < tblDetail2.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2[i], IDConnectedColName, mainID);
                        }
                    }



                    result = this.Save<T1, T2>(tblDetail1, tblDetail2);
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    this.Delete<T>(mainID);
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Sync untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        ///  <typeparam name="T3">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <param name="tblDetail3">tabel detail</param>
        /// <returns></returns>
        public bool SaveHeaderDetail<T, T1, T2, T3>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2, List<T3> tblDetail3)
            where T : class where T1 : class where T2 : class where T3 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
               & tblDetail2 != null & tblDetail3 != null)
            {
                this.Save(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    if (tblDetail1 != null)
                    {
                        for (int i = 0; i < tblDetail1.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1[i], IDConnectedColName, mainID);
                        }
                    }
                    if (tblDetail2 != null)
                    {
                        for (int i = 0; i < tblDetail2.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2[i], IDConnectedColName, mainID);
                        }
                    }
                    if (tblDetail3 != null)
                    {
                        for (int i = 0; i < tblDetail3.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T3>(tblDetail3[i], IDConnectedColName, mainID);
                        }
                    }


                    result = this.Save<T1, T2, T3>(tblDetail1, tblDetail2, tblDetail3);
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    this.Delete<T>(mainID);
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Sync untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        ///  <typeparam name="T3">Kelas Details</typeparam>
        ///  <typeparam name="T4">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <param name="tblDetail3">tabel detail</param>
        /// <param name="tblDetail4">tabel detail</param>
        /// <returns></returns>
        public bool SaveHeaderDetail<T, T1, T2, T3, T4>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2,
            List<T3> tblDetail3, List<T4> tblDetail4)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
              & tblDetail2 != null & tblDetail3 != null & tblDetail4 != null
             )
            {
                if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
               & tblDetail2 != null & tblDetail3 != null)
                {
                    this.Save(tblHeader);
                    PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                    if (pi != null) checkID = pi.GetValue(tblHeader);
                    if (checkID != null)
                    {
                        mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                    }
                    if (mainID > 0) // MainHeader sudah berhasil diinsert
                    {
                        if (tblDetail1 != null)
                        {
                            for (int i = 0; i < tblDetail1.Count; i++)
                            {
                                GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1[i], IDConnectedColName, mainID);
                            }
                        }
                        if (tblDetail2 != null)
                        {
                            for (int i = 0; i < tblDetail2.Count; i++)
                            {
                                GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2[i], IDConnectedColName, mainID);
                            }
                        }
                        if (tblDetail3 != null)
                        {
                            for (int i = 0; i < tblDetail3.Count; i++)
                            {
                                GenHelperEF.Getinstance.SetColValue<T3>(tblDetail3[i], IDConnectedColName, mainID);
                            }
                        }
                        if (tblDetail4 != null)
                        {
                            for (int i = 0; i < tblDetail4.Count; i++)
                            {
                                GenHelperEF.Getinstance.SetColValue<T4>(tblDetail4[i], IDConnectedColName, mainID);
                            }
                        }

                        result = this.Save<T1, T2, T3, T4>(tblDetail1, tblDetail2, tblDetail3, tblDetail4);
                    }
                    if (!result)
                    {
                        /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                        this.Delete<T>(mainID);
                    }
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Sync untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        ///  <typeparam name="T3">Kelas Details</typeparam>
        ///  <typeparam name="T4">Kelas Details</typeparam>
        ///   <typeparam name="T5">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <param name="tblDetail3">tabel detail</param>
        /// <param name="tblDetail4">tabel detail</param>
        /// <param name="tblDetail5">tabel detail</param>
        /// <returns></returns>
        public bool SaveHeaderDetail<T, T1, T2, T3, T4, T5>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2, List<T3> tblDetail3, List<T4> tblDetail4, List<T5> tblDetail5)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
               & tblDetail2 != null & tblDetail3 != null & tblDetail4 != null & tblDetail5 != null
              )
            {
                if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
               & tblDetail2 != null & tblDetail3 != null & tblDetail4 != null)
                {
                    this.Save(tblHeader);
                    PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                    if (pi != null) checkID = pi.GetValue(tblHeader);
                    if (checkID != null)
                    {
                        mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                    }
                    if (mainID > 0) // MainHeader sudah berhasil diinsert
                    {
                        if (tblDetail1 != null)
                        {
                            for (int i = 0; i < tblDetail1.Count; i++)
                            {
                                GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1[i], IDConnectedColName, mainID);
                            }
                        }
                        if (tblDetail2 != null)
                        {
                            for (int i = 0; i < tblDetail2.Count; i++)
                            {
                                GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2[i], IDConnectedColName, mainID);
                            }
                        }
                        if (tblDetail3 != null)
                        {
                            for (int i = 0; i < tblDetail3.Count; i++)
                            {
                                GenHelperEF.Getinstance.SetColValue<T3>(tblDetail3[i], IDConnectedColName, mainID);
                            }
                        }
                        if (tblDetail4 != null)
                        {
                            for (int i = 0; i < tblDetail4.Count; i++)
                            {
                                GenHelperEF.Getinstance.SetColValue<T4>(tblDetail4[i], IDConnectedColName, mainID);
                            }
                        }
                        if (tblDetail5 != null)
                        {
                            for (int i = 0; i < tblDetail5.Count; i++)
                            {
                                GenHelperEF.Getinstance.SetColValue<T5>(tblDetail5[i], IDConnectedColName, mainID);
                            }
                        }
                        result = this.Save<T1, T2, T3, T4, T5>(tblDetail1, tblDetail2, tblDetail3, tblDetail4, tblDetail5);
                    }
                    if (!result)
                    {
                        /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                        this.Delete<T>(mainID);
                    }
                }
            }
            return result;

        }

        /// <summary>
        /// Metode Async untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <returns></returns>
        public async Task<bool> SaveHeaderDetailAsync<T, T1>(T tblHeader, string IDConnectedColName, T1 tblDetail1)
            where T : class where T1 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
             )
            {
                var hasil1 = await this.SaveAsync(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1, IDConnectedColName, mainID);
                    var result1 = await this.SaveAsync<T1>(tblDetail1);
                    if (result1 != null)
                    {
                        result = true;
                    }
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    var hasil2 = await this.DeleteAsync<T>(mainID);
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Async untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <returns></returns>
        public async Task<bool> SaveHeaderDetailAsync<T, T1, T2>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2)
            where T : class where T1 : class where T2 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
               & tblDetail2 != null
              )
            {
                var hasil1 = await this.SaveAsync(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2, IDConnectedColName, mainID);
                    result = await this.SaveAsync<T1, T2>(tblDetail1, tblDetail2);
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    var hasil2 = await this.DeleteAsync<T>(mainID);
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Async untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        ///  <typeparam name="T3">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <param name="tblDetail3">tabel detail</param>
        /// <returns></returns>
        public async Task<bool> SaveHeaderDetailAsync<T, T1, T2, T3>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2, T3 tblDetail3)
            where T : class where T1 : class where T2 : class where T3 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
               & tblDetail2 != null & tblDetail3 != null)
            {
                var hasil1 = await this.SaveAsync(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T3>(tblDetail3, IDConnectedColName, mainID);
                    result = await this.SaveAsync<T1, T2, T3>(tblDetail1, tblDetail2, tblDetail3);
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    var hasil2 = await this.DeleteAsync<T>(mainID);
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Async untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        ///  <typeparam name="T3">Kelas Details</typeparam>
        ///  <typeparam name="T4">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <param name="tblDetail3">tabel detail</param>
        /// <param name="tblDetail4">tabel detail</param>
        /// <returns></returns>
        public async Task<bool> SaveHeaderDetailAsync<T, T1, T2, T3, T4>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2, T3 tblDetail3, T4 tblDetail4)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
                & tblDetail2 != null & tblDetail3 != null & tblDetail4 != null)
            {
                var hasil1 = await this.SaveAsync(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T3>(tblDetail3, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T4>(tblDetail4, IDConnectedColName, mainID);
                    result = await this.SaveAsync<T1, T2, T3, T4>(tblDetail1, tblDetail2, tblDetail3, tblDetail4);
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    var hasil2 = await this.DeleteAsync<T>(mainID);
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Async untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        ///  <typeparam name="T3">Kelas Details</typeparam>
        ///  <typeparam name="T4">Kelas Details</typeparam>
        ///   <typeparam name="T5">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <param name="tblDetail3">tabel detail</param>
        /// <param name="tblDetail4">tabel detail</param>
        /// <param name="tblDetail5">tabel detail</param>
        /// <returns></returns>
        public async Task<bool> SaveHeaderDetailAsync<T, T1, T2, T3, T4, T5>(T tblHeader, string IDConnectedColName, T1 tblDetail1, T2 tblDetail2, T3 tblDetail3, T4 tblDetail4, T5 tblDetail5)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
                & tblDetail2 != null & tblDetail3 != null & tblDetail4 != null & tblDetail5 != null)
            {
                var hasil1 = await this.SaveAsync(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T3>(tblDetail3, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T4>(tblDetail4, IDConnectedColName, mainID);
                    GenHelperEF.Getinstance.SetColValue<T5>(tblDetail5, IDConnectedColName, mainID);
                    result = await this.SaveAsync<T1, T2, T3, T4, T5>(tblDetail1, tblDetail2, tblDetail3, tblDetail4, tblDetail5);
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    var hasil2 = await this.DeleteAsync<T>(mainID);
                }
            }
            return result;

        }

        /// <summary>
        /// Metode Async untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>     
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>     
        /// <returns></returns>
        public async Task<bool> SaveHeaderDetailAsync<T, T1>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1)
            where T : class where T1 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
               )
            {
                var hasil1 = await this.SaveAsync(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    if (tblDetail1 != null)
                    {
                        for (int i = 0; i < tblDetail1.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1[i], IDConnectedColName, mainID);
                        }
                    }

                    var result1 = await this.SaveAsync<T1>(tblDetail1);
                    if (result1 != null)
                    {
                        result = true;
                    }
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    var hasil2 = await this.DeleteAsync<T>(mainID);
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Async untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <returns></returns>
        public async Task<bool> SaveHeaderDetailAsync<T, T1, T2>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2)
            where T : class where T1 : class where T2 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
                & tblDetail2 != null
               )
            {
                var hasil1 = await this.SaveAsync(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    if (tblDetail1 != null)
                    {
                        for (int i = 0; i < tblDetail1.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1[i], IDConnectedColName, mainID);
                        }
                    }
                    if (tblDetail2 != null)
                    {
                        for (int i = 0; i < tblDetail2.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2[i], IDConnectedColName, mainID);
                        }
                    }



                    result = await this.SaveAsync<T1, T2>(tblDetail1, tblDetail2);
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    var hasil2 = await this.DeleteAsync<T>(mainID);
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Async untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        ///  <typeparam name="T3">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <param name="tblDetail3">tabel detail</param>
        /// <returns></returns>
        public async Task<bool> SaveHeaderDetailAsync<T, T1, T2, T3>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2, List<T3> tblDetail3)
            where T : class where T1 : class where T2 : class where T3 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
                & tblDetail2 != null & tblDetail3 != null
               )
            {
                var hasil1 = await this.SaveAsync(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    if (tblDetail1 != null)
                    {
                        for (int i = 0; i < tblDetail1.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1[i], IDConnectedColName, mainID);
                        }
                    }
                    if (tblDetail2 != null)
                    {
                        for (int i = 0; i < tblDetail2.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2[i], IDConnectedColName, mainID);
                        }
                    }
                    if (tblDetail3 != null)
                    {
                        for (int i = 0; i < tblDetail3.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T3>(tblDetail3[i], IDConnectedColName, mainID);
                        }
                    }


                    result = await this.SaveAsync<T1, T2, T3>(tblDetail1, tblDetail2, tblDetail3);
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    var hasil2 = await this.DeleteAsync<T>(mainID);
                }
            }
            return result;
        }
        /// <summary>
        /// Metode Async untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        ///  <typeparam name="T3">Kelas Details</typeparam>
        ///  <typeparam name="T4">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <param name="tblDetail3">tabel detail</param>
        /// <param name="tblDetail4">tabel detail</param>
        /// <returns></returns>
        public async Task<bool> SaveHeaderDetailAsync<T, T1, T2, T3, T4>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2,
            List<T3> tblDetail3, List<T4> tblDetail4)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
                & tblDetail2 != null & tblDetail3 != null & tblDetail4 != null
               )
            {
                var hasil1 = await this.SaveAsync(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    if (tblDetail1 != null)
                    {
                        for (int i = 0; i < tblDetail1.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1[i], IDConnectedColName, mainID);
                        }
                    }
                    if (tblDetail2 != null)
                    {
                        for (int i = 0; i < tblDetail2.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2[i], IDConnectedColName, mainID);
                        }
                    }
                    if (tblDetail3 != null)
                    {
                        for (int i = 0; i < tblDetail3.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T3>(tblDetail3[i], IDConnectedColName, mainID);
                        }
                    }
                    if (tblDetail4 != null)
                    {
                        for (int i = 0; i < tblDetail4.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T4>(tblDetail4[i], IDConnectedColName, mainID);
                        }
                    }

                    result = await this.SaveAsync<T1, T2, T3, T4>(tblDetail1, tblDetail2, tblDetail3, tblDetail4);
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    var hasil2 = await this.DeleteAsync<T>(mainID);
                }
            }
            return result;

        }
        /// <summary>
        /// Metode Async untuk melakukan penyimpanan sekaligus antara tabel utama dan tabel detail dari T1...Tn. Jika semua data berhasil disimpan maka akan memberikan return true
        /// dan jika ada salah satu data yang gagal disimpan maka akan memberkan return false dan semua data yang diproses akan di rollback/dibatalkan. Koneksi antar tabel data 
        /// melalui identityID pada tabel utama. Tabel detail harus mempunyai kolom name yang merujuk pada identityID pada tabel utama
        /// </summary>
        /// <typeparam name="T">Kelas Header</typeparam>
        /// <typeparam name="T1">Kelas Details</typeparam>
        /// <typeparam name="T2">Kelas Details</typeparam>
        ///  <typeparam name="T3">Kelas Details</typeparam>
        ///  <typeparam name="T4">Kelas Details</typeparam>
        ///   <typeparam name="T5">Kelas Details</typeparam>
        /// <param name="tblHeader">Tabel header/utama</param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <param name="tblDetail1">tabel detail</param>
        /// <param name="tblDetail2">tabel detail</param>
        /// <param name="tblDetail3">tabel detail</param>
        /// <param name="tblDetail4">tabel detail</param>
        /// <param name="tblDetail5">tabel detail</param>
        /// <returns></returns>
        public async Task<bool> SaveHeaderDetailAsync<T, T1, T2, T3, T4, T5>(T tblHeader, string IDConnectedColName, List<T1> tblDetail1, List<T2> tblDetail2, List<T3> tblDetail3, List<T4> tblDetail4, List<T5> tblDetail5)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            int mainID = 0;
            bool result = false;
            object checkID = null;
            if (tblHeader != null & !string.IsNullOrEmpty(IDConnectedColName) & tblDetail1 != null
                 & tblDetail2 != null & tblDetail3 != null & tblDetail4 != null & tblDetail5 != null
                )
            {
                var hasil1 = await this.SaveAsync(tblHeader);
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null) checkID = pi.GetValue(tblHeader);
                if (checkID != null)
                {
                    mainID = Convert.ToInt32(pi.GetValue(tblHeader));
                }
                if (mainID > 0) // MainHeader sudah berhasil diinsert
                {
                    if (tblDetail1 != null)
                    {
                        for (int i = 0; i < tblDetail1.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T1>(tblDetail1[i], IDConnectedColName, mainID);
                        }
                    }
                    if (tblDetail2 != null)
                    {
                        for (int i = 0; i < tblDetail2.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T2>(tblDetail2[i], IDConnectedColName, mainID);
                        }
                    }
                    if (tblDetail3 != null)
                    {
                        for (int i = 0; i < tblDetail3.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T3>(tblDetail3[i], IDConnectedColName, mainID);
                        }
                    }
                    if (tblDetail4 != null)
                    {
                        for (int i = 0; i < tblDetail4.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T4>(tblDetail4[i], IDConnectedColName, mainID);
                        }
                    }
                    if (tblDetail5 != null)
                    {
                        for (int i = 0; i < tblDetail5.Count; i++)
                        {
                            GenHelperEF.Getinstance.SetColValue<T5>(tblDetail5[i], IDConnectedColName, mainID);
                        }
                    }
                    result = await this.SaveAsync<T1, T2, T3, T4, T5>(tblDetail1, tblDetail2, tblDetail3, tblDetail4, tblDetail5);
                }
                if (!result)
                {
                    /*Jika table details gagal , maka tabel utama akan dihapus juga/hard delete*/
                    var hasil2 = await this.DeleteAsync<T>(mainID);
                }
            }
            return result;

        }



        /// <summary>
        /// Metode Sync untuk menghapus data secara fisik, hard delete ya.1 record doank pada Header dan bisa jadi banyak record pada detail.
        /// Jika berhasil semua return true, jika ada yang gagal return false;
        /// </summary>
        /// <typeparam name="T">Tabel header/utama</typeparam>
        /// <typeparam name="T1">Tabel details</typeparam>
        /// <param name="IDIdentity">ID identity dari tabel utama yang akan digunakan untuk menghapus data pada tabel utam dan tabel detailnya
        /// </param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <returns></returns>
        public bool DeleteSaveHeader<T, T1>(int IDIdentity, string IDConnectedColName) where T : class where T1 : class
        {
            bool result = false;

            if (IDIdentity > 0 & !string.IsNullOrEmpty(IDConnectedColName))
            {
                var tblHeader = this.ListByID<T>(IDIdentity);
                List<SearchField> param = new System.Collections.Generic.List<SearchField>();
                param.Add(new SearchField { Name = IDConnectedColName, Operator = "=", Value1 = IDIdentity.ToString() });
                if (tblHeader != null)
                {

                    using (var context = GetDbContext())
                    {
                        var tblDetail1 = this.List<T1>(param);
                        if (tblDetail1 != null)
                        {
                            for (int i = 0; i < tblDetail1.Count(); i++)
                            {
                                context.Set<T1>().Remove(tblDetail1.ToList()[i]);
                            }

                        }
                        context.Set<T>().Remove(tblHeader);

                        result = true;
                    }
                }

            }
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menghapus data secara fisik, hard delete ya.1 record doank pada Header dan bisa jadi banyak record pada detail.
        /// Jika berhasil semua return true, jika ada yang gagal return false;
        /// </summary>
        /// <typeparam name="T">Tabel header/utama</typeparam>
        /// <typeparam name="T1">Tabel details</typeparam>
        ///  <typeparam name="T2">Tabel details<</typeparam>
        /// <param name="IDIdentity">ID identity dari tabel utama yang akan digunakan untuk menghapus data pada tabel utam dan tabel detailnya
        /// </param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <returns></returns>
        public bool DeleteSaveHeader<T, T1, T2>(int IDIdentity, string IDConnectedColName)
            where T : class where T1 : class where T2 : class
        {
            bool result = false;
            if (IDIdentity > 0 & !string.IsNullOrEmpty(IDConnectedColName))
            {
                var tblHeader = this.ListByID<T>(IDIdentity);
                List<SearchField> param = new System.Collections.Generic.List<SearchField>();
                param.Add(new SearchField { Name = IDConnectedColName, Operator = "=", Value1 = IDIdentity.ToString() });
                if (tblHeader != null)
                {
                    var tblDetail1 = this.List<T1>(param);
                    var tblDetail2 = this.List<T2>(param);
                    using (var context = GetDbContext())
                    {
                        if (tblDetail1 != null)
                        {
                            for (int i = 0; i < tblDetail1.Count(); i++)
                            {
                                context.Set<T1>().Remove(tblDetail1.ToList()[i]);
                            }

                        }

                        if (tblDetail2 != null)
                        {
                            for (int i = 0; i < tblDetail2.Count(); i++)
                            {
                                context.Set<T2>().Remove(tblDetail2.ToList()[i]);
                            }

                        }
                        context.Set<T>().Remove(tblHeader);
                        if (context.SaveChanges() > 0)
                            result = true;
                    }
                }

            }
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menghapus data secara fisik, hard delete ya.1 record doank pada Header dan bisa jadi banyak record pada detail.
        /// Jika berhasil semua return true, jika ada yang gagal return false;
        /// </summary>
        /// <typeparam name="T">Tabel header/utama</typeparam>
        /// <typeparam name="T1">Tabel details</typeparam>
        /// <typeparam name="T2">Tabel details</typeparam>
        ///  <typeparam name="T3">Tabel details</typeparam>
        /// <param name="IDIdentity">ID identity dari tabel utama yang akan digunakan untuk menghapus data pada tabel utam dan tabel detailnya
        /// </param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <returns></returns>
        public bool DeleteSaveHeader<T, T1, T2, T3>(int IDIdentity, string IDConnectedColName)
            where T : class where T1 : class where T2 : class where T3 : class
        {
            bool result = false;

            if (IDIdentity > 0 & !string.IsNullOrEmpty(IDConnectedColName))
            {
                var tblHeader = this.ListByID<T>(IDIdentity);
                List<SearchField> param = new System.Collections.Generic.List<SearchField>();
                param.Add(new SearchField { Name = IDConnectedColName, Operator = "=", Value1 = IDIdentity.ToString() });
                if (tblHeader != null)
                {
                    var tblDetail1 = this.List<T1>(param);
                    var tblDetail2 = this.List<T2>(param);
                    var tblDetail3 = this.List<T3>(param);
                    using (var context = GetDbContext())
                    {
                        if (tblDetail1 != null)
                        {
                            for (int i = 0; i < tblDetail1.Count(); i++)
                            {
                                context.Set<T1>().Remove(tblDetail1.ToList()[i]);
                            }

                        }

                        if (tblDetail2 != null)
                        {
                            for (int i = 0; i < tblDetail2.Count(); i++)
                            {
                                context.Set<T2>().Remove(tblDetail2.ToList()[i]);
                            }
                        }

                        if (tblDetail3 != null)
                        {
                            for (int i = 0; i < tblDetail3.Count(); i++)
                            {
                                context.Set<T3>().Remove(tblDetail3.ToList()[i]);
                            }

                        }
                        context.Set<T>().Remove(tblHeader);
                        if (context.SaveChanges() > 0)
                            result = true;
                    }
                }

            }
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menghapus data secara fisik, hard delete ya.1 record doank pada Header dan bisa jadi banyak record pada detail.
        /// Jika berhasil semua return true, jika ada yang gagal return false;
        /// </summary>
        /// <typeparam name="T">Tabel header/utama</typeparam>
        /// <typeparam name="T1">Tabel details</typeparam>
        /// <typeparam name="T2">Tabel details</typeparam>
        ///  <typeparam name="T3">Tabel details</typeparam>
        ///  <typeparam name="T4">Tabel details</typeparam>
        /// <param name="IDIdentity">ID identity dari tabel utama yang akan digunakan untuk menghapus data pada tabel utam dan tabel detailnya
        /// </param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <returns></returns>
        public bool DeleteSaveHeader<T, T1, T2, T3, T4>(int IDIdentity, string IDConnectedColName)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class
        {
            bool result = false;
            if (IDIdentity > 0 & !string.IsNullOrEmpty(IDConnectedColName))
            {
                var tblHeader = this.ListByID<T>(IDIdentity);
                List<SearchField> param = new System.Collections.Generic.List<SearchField>();
                param.Add(new SearchField { Name = IDConnectedColName, Operator = "=", Value1 = IDIdentity.ToString() });
                if (tblHeader != null)
                {
                    var tblDetail1 = this.List<T1>(param);
                    var tblDetail2 = this.List<T2>(param);
                    var tblDetail3 = this.List<T3>(param);
                    var tblDetail4 = this.List<T4>(param);
                    using (var context = GetDbContext())
                    {
                        if (tblDetail1 != null)
                        {
                            for (int i = 0; i < tblDetail1.Count(); i++)
                            {
                                context.Set<T1>().Remove(tblDetail1.ToList()[i]);
                            }

                        }
                        if (tblDetail2 != null)
                        {
                            for (int i = 0; i < tblDetail2.Count(); i++)
                            {
                                context.Set<T2>().Remove(tblDetail2.ToList()[i]);
                            }
                        }
                        if (tblDetail3 != null)
                        {
                            for (int i = 0; i < tblDetail3.Count(); i++)
                            {
                                context.Set<T3>().Remove(tblDetail3.ToList()[i]);
                            }
                        }

                        if (tblDetail4 != null)
                        {
                            for (int i = 0; i < tblDetail4.Count(); i++)
                            {
                                context.Set<T4>().Remove(tblDetail4.ToList()[i]);
                            }
                        }
                        context.Set<T>().Remove(tblHeader);
                        if (context.SaveChanges() > 0)
                            result = true;
                    }
                }

            }
            return result;
        }
        /// <summary>
        /// Metode Sync untuk menghapus data secara fisik, hard delete ya.1 record doank pada Header dan bisa jadi banyak record pada detail.
        /// Jika berhasil semua return true, jika ada yang gagal return false;
        /// </summary>
        /// <typeparam name="T">Tabel header/utama</typeparam>
        /// <typeparam name="T1">Tabel details</typeparam>
        /// <typeparam name="T2">Tabel details</typeparam>
        /// <typeparam name="T3">Tabel details</typeparam>
        /// <typeparam name="T4">Tabel details</typeparam>
        /// <typeparam name="T5">Tabel details</typeparam>
        /// <param name="IDIdentity">ID identity dari tabel utama yang akan digunakan untuk menghapus data pada tabel utam dan tabel detailnya
        /// </param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <returns></returns>
        public bool DeleteSaveHeader<T, T1, T2, T3, T4, T5>(int IDIdentity, string IDConnectedColName)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            bool result = false;

            if (IDIdentity > 0 & !string.IsNullOrEmpty(IDConnectedColName))
            {
                var tblHeader = this.ListByID<T>(IDIdentity);
                List<SearchField> param = new System.Collections.Generic.List<SearchField>();
                param.Add(new SearchField { Name = IDConnectedColName, Operator = "=", Value1 = IDIdentity.ToString() });
                if (tblHeader != null)
                {
                    var tblDetail1 = this.List<T1>(param);
                    var tblDetail2 = this.List<T2>(param);
                    var tblDetail3 = this.List<T3>(param);
                    var tblDetail4 = this.List<T4>(param);
                    var tblDetail5 = this.List<T5>(param);
                    using (var context = GetDbContext())
                    {
                        if (tblDetail1 != null)
                        {
                            for (int i = 0; i < tblDetail1.Count(); i++)
                            {
                                context.Set<T1>().Remove(tblDetail1.ToList()[i]);
                            }

                        }
                        if (tblDetail2 != null)
                        {
                            for (int i = 0; i < tblDetail2.Count(); i++)
                            {
                                context.Set<T2>().Remove(tblDetail2.ToList()[i]);
                            }
                        }
                        if (tblDetail3 != null)
                        {
                            for (int i = 0; i < tblDetail3.Count(); i++)
                            {
                                context.Set<T3>().Remove(tblDetail3.ToList()[i]);
                            }
                        }
                        if (tblDetail4 != null)
                        {
                            for (int i = 0; i < tblDetail4.Count(); i++)
                            {
                                context.Set<T4>().Remove(tblDetail4.ToList()[i]);
                            }
                        }

                        if (tblDetail5 != null)
                        {
                            for (int i = 0; i < tblDetail5.Count(); i++)
                            {
                                context.Set<T5>().Remove(tblDetail5.ToList()[i]);
                            }
                        }
                        context.Set<T>().Remove(tblHeader);
                        if (context.SaveChanges() > 0)
                            result = true;
                    }
                }

            }
            return result;
        }



        /// <summary>
        /// Metode Async untuk menghapus data secara fisik, hard delete ya.1 record doank pada Header dan bisa jadi banyak record pada detail.
        /// Jika berhasil semua return true, jika ada yang gagal return false;
        /// </summary>
        /// <typeparam name="T">Tabel header/utama</typeparam>
        /// <typeparam name="T1">Tabel details</typeparam>
        /// <param name="IDIdentity">ID identity dari tabel utama yang akan digunakan untuk menghapus data pada tabel utam dan tabel detailnya
        /// </param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <returns></returns>
        public async Task<bool> DeleteSaveHeaderAsync<T, T1>(int IDIdentity, string IDConnectedColName) where T : class where T1 : class
        {
            bool result = false;
            if (IDIdentity > 0 & !string.IsNullOrEmpty(IDConnectedColName))
            {
                var tblHeader = this.ListByID<T>(IDIdentity);
                List<SearchField> param = new System.Collections.Generic.List<SearchField>();
                param.Add(new SearchField { Name = IDConnectedColName, Operator = "=", Value1 = IDIdentity.ToString() });
                if (tblHeader != null)
                {
                    var tblDetail1 = this.List<T1>(param);
                    using (var context = GetDbContext())
                    {
                        if (tblDetail1 != null)
                        {
                            for (int i = 0; i < tblDetail1.Count(); i++)
                            {
                                context.Set<T1>().Remove(tblDetail1.ToList()[i]);
                            }

                        }
                        context.Set<T>().Remove(tblHeader);

                        if (await context.SaveChangesAsync() > 0)
                            result = true;
                    }
                }

            }
            return result;
        }
        /// <summary>
        /// Metode Async untuk menghapus data secara fisik, hard delete ya.1 record doank pada Header dan bisa jadi banyak record pada detail.
        /// Jika berhasil semua return true, jika ada yang gagal return false;
        /// </summary>
        /// <typeparam name="T">Tabel header/utama</typeparam>
        /// <typeparam name="T1">Tabel details</typeparam>
        ///  <typeparam name="T2">Tabel details<</typeparam>
        /// <param name="IDIdentity">ID identity dari tabel utama yang akan digunakan untuk menghapus data pada tabel utam dan tabel detailnya
        /// </param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <returns></returns>
        public async Task<bool> DeleteSaveHeaderAsync<T, T1, T2>(int IDIdentity, string IDConnectedColName)
            where T : class where T1 : class where T2 : class
        {
            bool result = false;

            if (IDIdentity > 0 & !string.IsNullOrEmpty(IDConnectedColName))
            {
                var tblHeader = this.ListByID<T>(IDIdentity);
                List<SearchField> param = new System.Collections.Generic.List<SearchField>();
                param.Add(new SearchField { Name = IDConnectedColName, Operator = "=", Value1 = IDIdentity.ToString() });
                if (tblHeader != null)
                {
                    var tblDetail1 = this.List<T1>(param);
                    var tblDetail2 = this.List<T2>(param);
                    using (var context = GetDbContext())
                    {
                        if (tblDetail1 != null)
                        {
                            for (int i = 0; i < tblDetail1.Count(); i++)
                            {
                                context.Set<T1>().Remove(tblDetail1.ToList()[i]);
                            }

                        }

                        if (tblDetail2 != null)
                        {
                            for (int i = 0; i < tblDetail2.Count(); i++)
                            {
                                context.Set<T2>().Remove(tblDetail2.ToList()[i]);
                            }

                        }

                        context.Set<T>().Remove(tblHeader);
                        if (await context.SaveChangesAsync() > 0)
                            result = true;
                    }
                }

            }
            return result;
        }
        /// <summary>
        /// Metode Async untuk menghapus data secara fisik, hard delete ya.1 record doank pada Header dan bisa jadi banyak record pada detail.
        /// Jika berhasil semua return true, jika ada yang gagal return false;
        /// </summary>
        /// <typeparam name="T">Tabel header/utama</typeparam>
        /// <typeparam name="T1">Tabel details</typeparam>
        /// <typeparam name="T2">Tabel details</typeparam>
        ///  <typeparam name="T3">Tabel details</typeparam>
        /// <param name="IDIdentity">ID identity dari tabel utama yang akan digunakan untuk menghapus data pada tabel utam dan tabel detailnya
        /// </param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <returns></returns>
        public async Task<bool> DeleteSaveHeaderAsync<T, T1, T2, T3>(int IDIdentity, string IDConnectedColName)
            where T : class where T1 : class where T2 : class where T3 : class
        {
            bool result = false;

            if (IDIdentity > 0 & !string.IsNullOrEmpty(IDConnectedColName))
            {
                var tblHeader = this.ListByID<T>(IDIdentity);
                List<SearchField> param = new System.Collections.Generic.List<SearchField>();
                param.Add(new SearchField { Name = IDConnectedColName, Operator = "=", Value1 = IDIdentity.ToString() });
                if (tblHeader != null)
                {
                    var tblDetail1 = this.List<T1>(param);
                    var tblDetail2 = this.List<T2>(param);
                    var tblDetail3 = this.List<T3>(param);
                    using (var context = GetDbContext())
                    {
                        if (tblDetail1 != null)
                        {
                            for (int i = 0; i < tblDetail1.Count(); i++)
                            {
                                context.Set<T1>().Remove(tblDetail1.ToList()[i]);
                            }

                        }

                        if (tblDetail2 != null)
                        {
                            for (int i = 0; i < tblDetail2.Count(); i++)
                            {
                                context.Set<T2>().Remove(tblDetail2.ToList()[i]);
                            }
                        }

                        if (tblDetail3 != null)
                        {
                            for (int i = 0; i < tblDetail3.Count(); i++)
                            {
                                context.Set<T3>().Remove(tblDetail3.ToList()[i]);
                            }

                        }
                        context.Set<T>().Remove(tblHeader);
                        if (await context.SaveChangesAsync() > 0)
                            result = true;
                    }
                }

            }
            return result;
        }
        /// <summary>
        /// Metode Async untuk menghapus data secara fisik, hard delete ya.1 record doank pada Header dan bisa jadi banyak record pada detail.
        /// Jika berhasil semua return true, jika ada yang gagal return false;
        /// </summary>
        /// <typeparam name="T">Tabel header/utama</typeparam>
        /// <typeparam name="T1">Tabel details</typeparam>
        /// <typeparam name="T2">Tabel details</typeparam>
        ///  <typeparam name="T3">Tabel details</typeparam>
        ///  <typeparam name="T4">Tabel details</typeparam>
        /// <param name="IDIdentity">ID identity dari tabel utama yang akan digunakan untuk menghapus data pada tabel utam dan tabel detailnya
        /// </param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <returns></returns>
        public async Task<bool> DeleteSaveHeaderAsync<T, T1, T2, T3, T4>(int IDIdentity, string IDConnectedColName)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class
        {
            bool result = false;

            if (IDIdentity > 0 & !string.IsNullOrEmpty(IDConnectedColName))
            {
                var tblHeader = this.ListByID<T>(IDIdentity);
                List<SearchField> param = new System.Collections.Generic.List<SearchField>();
                param.Add(new SearchField { Name = IDConnectedColName, Operator = "=", Value1 = IDIdentity.ToString() });
                if (tblHeader != null)
                {
                    var tblDetail1 = this.List<T1>(param);
                    var tblDetail2 = this.List<T2>(param);
                    var tblDetail3 = this.List<T3>(param);
                    var tblDetail4 = this.List<T4>(param);
                    using (var context = GetDbContext())
                    {
                        if (tblDetail1 != null)
                        {
                            for (int i = 0; i < tblDetail1.Count(); i++)
                            {
                                context.Set<T1>().Remove(tblDetail1.ToList()[i]);
                            }

                        }

                        if (tblDetail2 != null)
                        {
                            for (int i = 0; i < tblDetail2.Count(); i++)
                            {
                                context.Set<T2>().Remove(tblDetail2.ToList()[i]);
                            }
                        }

                        if (tblDetail3 != null)
                        {
                            for (int i = 0; i < tblDetail3.Count(); i++)
                            {
                                context.Set<T3>().Remove(tblDetail3.ToList()[i]);
                            }
                        }

                        if (tblDetail4 != null)
                        {
                            for (int i = 0; i < tblDetail4.Count(); i++)
                            {
                                context.Set<T4>().Remove(tblDetail4.ToList()[i]);
                            }
                        }
                        context.Set<T>().Remove(tblHeader);
                        if (await context.SaveChangesAsync() > 0)
                            result = true;
                    }
                }

            }
            return result;
        }
        /// <summary>
        /// Metode Async untuk menghapus data secara fisik, hard delete ya.1 record doank pada Header dan bisa jadi banyak record pada detail.
        /// Jika berhasil semua return true, jika ada yang gagal return false;
        /// </summary>
        /// <typeparam name="T">Tabel header/utama</typeparam>
        /// <typeparam name="T1">Tabel details</typeparam>
        /// <typeparam name="T2">Tabel details</typeparam>
        /// <typeparam name="T3">Tabel details</typeparam>
        /// <typeparam name="T4">Tabel details</typeparam>
        /// <typeparam name="T5">Tabel details</typeparam>
        /// <param name="IDIdentity">ID identity dari tabel utama yang akan digunakan untuk menghapus data pada tabel utam dan tabel detailnya
        /// </param>
        /// <param name="IDConnectedColName">kolom name yang merujuk pada tabel utama dan terdapta pada kolom detailnya. Value akan diisi oleh identityID tabel utama </param>
        /// <returns></returns>
        public async Task<bool> DeleteSaveHeaderAsync<T, T1, T2, T3, T4, T5>(int IDIdentity, string IDConnectedColName)
            where T : class where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            bool result = false;
            if (IDIdentity > 0 & !string.IsNullOrEmpty(IDConnectedColName))
            {
                var tblHeader = this.ListByID<T>(IDIdentity);
                List<SearchField> param = new System.Collections.Generic.List<SearchField>();
                param.Add(new SearchField { Name = IDConnectedColName, Operator = "=", Value1 = IDIdentity.ToString() });
                if (tblHeader != null)
                {
                    var tblDetail1 = this.List<T1>(param);
                    var tblDetail2 = this.List<T2>(param);
                    var tblDetail3 = this.List<T3>(param);
                    var tblDetail4 = this.List<T4>(param);
                    var tblDetail5 = this.List<T5>(param);
                    using (var context = GetDbContext())
                    {
                        if (tblDetail1 != null)
                        {
                            for (int i = 0; i < tblDetail1.Count(); i++)
                            {
                                context.Set<T1>().Remove(tblDetail1.ToList()[i]);
                            }

                        }

                        if (tblDetail2 != null)
                        {
                            for (int i = 0; i < tblDetail2.Count(); i++)
                            {
                                context.Set<T2>().Remove(tblDetail2.ToList()[i]);
                            }
                        }

                        if (tblDetail3 != null)
                        {
                            for (int i = 0; i < tblDetail3.Count(); i++)
                            {
                                context.Set<T3>().Remove(tblDetail3.ToList()[i]);
                            }
                        }

                        if (tblDetail4 != null)
                        {
                            for (int i = 0; i < tblDetail4.Count(); i++)
                            {
                                context.Set<T4>().Remove(tblDetail4.ToList()[i]);
                            }
                        }

                        if (tblDetail5 != null)
                        {
                            for (int i = 0; i < tblDetail5.Count(); i++)
                            {
                                context.Set<T5>().Remove(tblDetail5.ToList()[i]);
                            }
                        }
                        context.Set<T>().Remove(tblHeader);
                        if (await context.SaveChangesAsync() > 0)
                            result = true;
                    }
                }

            }
            return result;
        }

        /// <summary>
        /// Metode sync untuk Menambah atau mengubah data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <typeparam name="T3">Kelas 3</typeparam>
        /// <typeparam name="T4">Kelas 4</typeparam>
        /// <typeparam name="T5">Kelas 5</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="entity3">Entity Kelas 3</param>
        /// <param name="entity4">Entity Kelas 4</param>
        /// <param name="entity5">Entity Kelas 5</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        /// <param name="IsSaveT3">Is Save Kelas 3</param>
        /// <param name="IsSaveT4">Is Save Kelas 4</param>
        /// <param name="IsSaveT5">Is Save Kelas 5</param>
        /// <returns></returns>
        public bool SaveUpdate<T1, T2, T3, T4, T5>(T1 entity1, T2 entity2, T3 entity3, T4 entity4, T5 entity5
            , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4, bool IsSaveT5)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            bool result = false;
            bool goProcess = false;

            if (entity1 != null & entity2 != null & entity3 != null & entity4 != null & entity5 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new RepoGenPrepareUpdate<T1>();
                var ceklistEntity2 = new RepoGenPrepareUpdate<T2>();
                var ceklistEntity3 = new RepoGenPrepareUpdate<T3>();
                var ceklistEntity4 = new RepoGenPrepareUpdate<T4>();
                var ceklistEntity5 = new RepoGenPrepareUpdate<T5>();
                if (!IsSaveT1)
                {
                    ceklistEntity1 = PrepareEntityForSaveUpdate<T1>(entity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = PrepareEntityForSaveUpdate<T2>(entity2);
                }
                if (!IsSaveT3)
                {
                    ceklistEntity3 = PrepareEntityForSaveUpdate<T3>(entity3);
                }
                if (!IsSaveT4)
                {
                    ceklistEntity4 = PrepareEntityForSaveUpdate<T4>(entity4);
                }
                if (!IsSaveT5)
                {
                    ceklistEntity5 = PrepareEntityForSaveUpdate<T5>(entity5);
                }

                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                                context.Set<T1>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity1.Entity;
                                var colNotNull = ceklistEntity1.PropertyInfos;
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT2)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                                context.Set<T2>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity2.Entity;
                                var colNotNull = ceklistEntity2.PropertyInfos;
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT3)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T3>(entity3);
                                context.Set<T3>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity3.Entity;
                                var colNotNull = ceklistEntity3.PropertyInfos;
                                context.Set<T3>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT4)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T4>(entity4);
                                context.Set<T4>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity4.Entity;
                                var colNotNull = ceklistEntity4.PropertyInfos;
                                context.Set<T4>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT5)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T5>(entity5);
                                context.Set<T5>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity5.Entity;
                                var colNotNull = ceklistEntity5.PropertyInfos;
                                context.Set<T5>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T5>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            int hasil = context.SaveChanges();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Metode sync untuk Menambah atau mengubah data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <typeparam name="T3">Kelas 3</typeparam>
        /// <typeparam name="T4">Kelas 4</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="entity3">Entity Kelas 3</param>
        /// <param name="entity4">Entity Kelas 4</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        /// <param name="IsSaveT3">Is Save Kelas 3</param>
        /// <param name="IsSaveT4">Is Save Kelas 4</param>
        /// <returns></returns>
        public bool SaveUpdate<T1, T2, T3, T4>(T1 entity1, T2 entity2, T3 entity3, T4 entity4
          , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4)
          where T1 : class where T2 : class where T3 : class where T4 : class
        {
            bool result = false;
            bool goProcess = false;

            if (entity1 != null & entity2 != null & entity3 != null & entity4 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new RepoGenPrepareUpdate<T1>();
                var ceklistEntity2 = new RepoGenPrepareUpdate<T2>();
                var ceklistEntity3 = new RepoGenPrepareUpdate<T3>();
                var ceklistEntity4 = new RepoGenPrepareUpdate<T4>();
                if (!IsSaveT1)
                {
                    ceklistEntity1 = PrepareEntityForSaveUpdate<T1>(entity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = PrepareEntityForSaveUpdate<T2>(entity2);
                }
                if (!IsSaveT3)
                {
                    ceklistEntity3 = PrepareEntityForSaveUpdate<T3>(entity3);
                }
                if (!IsSaveT4)
                {
                    ceklistEntity4 = PrepareEntityForSaveUpdate<T4>(entity4);
                }

                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                                context.Set<T1>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity1.Entity;
                                var colNotNull = ceklistEntity1.PropertyInfos;
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT2)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                                context.Set<T2>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity2.Entity;
                                var colNotNull = ceklistEntity2.PropertyInfos;
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT3)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T3>(entity3);
                                context.Set<T3>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity3.Entity;
                                var colNotNull = ceklistEntity3.PropertyInfos;
                                context.Set<T3>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT4)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T4>(entity4);
                                context.Set<T4>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity4.Entity;
                                var colNotNull = ceklistEntity4.PropertyInfos;
                                context.Set<T4>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            int hasil = context.SaveChanges();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Metode sync untuk Menambah atau mengubah data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <typeparam name="T3">Kelas 3</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="entity3">Entity Kelas 3</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        /// <param name="IsSaveT3">Is Save Kelas 3</param>
        /// <returns></returns>
        public bool SaveUpdate<T1, T2, T3>(T1 entity1, T2 entity2, T3 entity3
          , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3)
          where T1 : class where T2 : class where T3 : class
        {
            bool result = false;
            bool goProcess = false;

            if (entity1 != null & entity2 != null & entity3 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new RepoGenPrepareUpdate<T1>();
                var ceklistEntity2 = new RepoGenPrepareUpdate<T2>();
                var ceklistEntity3 = new RepoGenPrepareUpdate<T3>();
                if (!IsSaveT1)
                {
                    ceklistEntity1 = PrepareEntityForSaveUpdate<T1>(entity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = PrepareEntityForSaveUpdate<T2>(entity2);
                }
                if (!IsSaveT3)
                {
                    ceklistEntity3 = PrepareEntityForSaveUpdate<T3>(entity3);
                }
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                                context.Set<T1>().Add(myentity);

                            }
                            else
                            {

                                var myentity = ceklistEntity1.Entity;
                                var colNotNull = ceklistEntity1.PropertyInfos;
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT2)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                                context.Set<T2>().Add(myentity);

                            }
                            else
                            {

                                var myentity = ceklistEntity2.Entity;
                                var colNotNull = ceklistEntity2.PropertyInfos;
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT3)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T3>(entity3);
                                context.Set<T3>().Add(myentity);

                            }
                            else
                            {

                                var myentity = ceklistEntity3.Entity;
                                var colNotNull = ceklistEntity3.PropertyInfos;
                                context.Set<T3>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }

                            int hasil = context.SaveChanges();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Metode sync untuk Menambah atau mengubah data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        /// <returns></returns>
        public bool SaveUpdate<T1, T2>(T1 entity1, T2 entity2
          , bool IsSaveT1, bool IsSaveT2)
          where T1 : class where T2 : class
        {
            bool result = false;
            bool goProcess = false;

            if (entity1 != null & entity2 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new RepoGenPrepareUpdate<T1>();
                var ceklistEntity2 = new RepoGenPrepareUpdate<T2>();
                if (!IsSaveT1)
                {
                    ceklistEntity1 = PrepareEntityForSaveUpdate<T1>(entity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = PrepareEntityForSaveUpdate<T2>(entity2);
                }
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                                context.Set<T1>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity1.Entity;
                                var colNotNull = ceklistEntity1.PropertyInfos;
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT2)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                                context.Set<T2>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity2.Entity;
                                var colNotNull = ceklistEntity2.PropertyInfos;
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }


                            int hasil = context.SaveChanges();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Metode sync untuk Menambah atau mengubah Sekelompok data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <typeparam name="T3">Kelas 3</typeparam>
        /// <typeparam name="T4">Kelas 4</typeparam>
        /// <typeparam name="T5">Kelas 5</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="entity3">Entity Kelas 3</param>
        /// <param name="entity4">Entity Kelas 4</param>
        /// <param name="entity5">Entity Kelas 5</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        /// <param name="IsSaveT3">Is Save Kelas 3</param>
        /// <param name="IsSaveT4">Is Save Kelas 4</param>
        /// <param name="IsSaveT5">Is Save Kelas 5</param>
        /// <returns></returns>
        public bool SaveUpdate<T1, T2, T3, T4, T5>(List<T1> listEntity1, List<T2> listEntity2, List<T3> listEntity3, List<T4> listEntity4, List<T5> listEntity5
            , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4, bool IsSaveT5)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            bool result = false;
            bool goProcess = false;

            if (listEntity1 != null & listEntity2 != null & listEntity3 != null & listEntity4 != null & listEntity5 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new List<RepoGenPrepareUpdate<T1>>();
                var ceklistEntity2 = new List<RepoGenPrepareUpdate<T2>>();
                var ceklistEntity3 = new List<RepoGenPrepareUpdate<T3>>();
                var ceklistEntity4 = new List<RepoGenPrepareUpdate<T4>>();
                var ceklistEntity5 = new List<RepoGenPrepareUpdate<T5>>();
                if (!IsSaveT1)
                {
                    ceklistEntity1 = PrepareEntityForSaveUpdate<T1>(listEntity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = PrepareEntityForSaveUpdate<T2>(listEntity2);
                }
                if (!IsSaveT3)
                {
                    ceklistEntity3 = PrepareEntityForSaveUpdate<T3>(listEntity3);
                }
                if (!IsSaveT4)
                {
                    ceklistEntity4 = PrepareEntityForSaveUpdate<T4>(listEntity4);
                }
                if (!IsSaveT5)
                {
                    ceklistEntity5 = PrepareEntityForSaveUpdate<T5>(listEntity5);
                }
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                for (int i = 0; i < listEntity1.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(listEntity1[i]);
                                    context.Set<T1>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity1 != null)
                                {
                                    for (int i = 0; i < ceklistEntity1.Count; i++)
                                    {
                                        var myentity = ceklistEntity1[i].Entity;
                                        var colNotNull = ceklistEntity1[i].PropertyInfos;
                                        context.Set<T1>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            if (IsSaveT2)
                            {
                                for (int i = 0; i < listEntity2.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(listEntity2[i]);
                                    context.Set<T2>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity2 != null)
                                {
                                    for (int i = 0; i < ceklistEntity2.Count; i++)
                                    {
                                        var myentity = ceklistEntity2[i].Entity;
                                        var colNotNull = ceklistEntity2[i].PropertyInfos;
                                        context.Set<T2>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            if (IsSaveT3)
                            {
                                for (int i = 0; i < listEntity3.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T3>(listEntity3[i]);
                                    context.Set<T3>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity3 != null)
                                {
                                    for (int i = 0; i < ceklistEntity3.Count; i++)
                                    {
                                        var myentity = ceklistEntity3[i].Entity;
                                        var colNotNull = ceklistEntity3[i].PropertyInfos;
                                        context.Set<T3>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            if (IsSaveT4)
                            {
                                for (int i = 0; i < listEntity4.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T4>(listEntity4[i]);
                                    context.Set<T4>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity4 != null)
                                {
                                    for (int i = 0; i < ceklistEntity4.Count; i++)
                                    {
                                        var myentity = ceklistEntity4[i].Entity;
                                        var colNotNull = ceklistEntity4[i].PropertyInfos;
                                        context.Set<T4>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }

                                }
                            }
                            if (IsSaveT5)
                            {
                                for (int i = 0; i < listEntity5.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T5>(listEntity5[i]);
                                    context.Set<T5>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity5 != null)
                                {
                                    for (int i = 0; i < ceklistEntity5.Count; i++)
                                    {
                                        var myentity = ceklistEntity5[i].Entity;
                                        var colNotNull = ceklistEntity5[i].PropertyInfos;
                                        context.Set<T5>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T5>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            int hasil = context.SaveChanges();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Metode sync untuk Menambah atau mengubah Sekelompok data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <typeparam name="T3">Kelas 3</typeparam>
        /// <typeparam name="T4">Kelas 4</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="entity3">Entity Kelas 3</param>
        /// <param name="entity4">Entity Kelas 4</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        /// <param name="IsSaveT3">Is Save Kelas 3</param>
        /// <param name="IsSaveT4">Is Save Kelas 4</param>
        public bool SaveUpdate<T1, T2, T3, T4>(List<T1> listEntity1, List<T2> listEntity2, List<T3> listEntity3, List<T4> listEntity4
           , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4)
           where T1 : class where T2 : class where T3 : class where T4 : class
        {
            bool result = false;
            bool goProcess = false;

            if (listEntity1 != null & listEntity2 != null & listEntity3 != null & listEntity4 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new List<RepoGenPrepareUpdate<T1>>();
                var ceklistEntity2 = new List<RepoGenPrepareUpdate<T2>>();
                var ceklistEntity3 = new List<RepoGenPrepareUpdate<T3>>();
                var ceklistEntity4 = new List<RepoGenPrepareUpdate<T4>>();
                if (!IsSaveT1)
                {
                    ceklistEntity1 = PrepareEntityForSaveUpdate<T1>(listEntity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = PrepareEntityForSaveUpdate<T2>(listEntity2);
                }
                if (!IsSaveT3)
                {
                    ceklistEntity3 = PrepareEntityForSaveUpdate<T3>(listEntity3);
                }
                if (!IsSaveT4)
                {
                    ceklistEntity4 = PrepareEntityForSaveUpdate<T4>(listEntity4);
                }
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                for (int i = 0; i < listEntity1.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(listEntity1[i]);
                                    context.Set<T1>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity1 != null)
                                {
                                    for (int i = 0; i < ceklistEntity1.Count; i++)
                                    {
                                        var myentity = ceklistEntity1[i].Entity;
                                        var colNotNull = ceklistEntity1[i].PropertyInfos;
                                        context.Set<T1>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            if (IsSaveT2)
                            {
                                for (int i = 0; i < listEntity2.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(listEntity2[i]);
                                    context.Set<T2>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity2 != null)
                                {
                                    for (int i = 0; i < ceklistEntity2.Count; i++)
                                    {
                                        var myentity = ceklistEntity2[i].Entity;
                                        var colNotNull = ceklistEntity2[i].PropertyInfos;
                                        context.Set<T2>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            if (IsSaveT3)
                            {
                                for (int i = 0; i < listEntity3.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T3>(listEntity3[i]);
                                    context.Set<T3>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity3 != null)
                                {
                                    for (int i = 0; i < ceklistEntity3.Count; i++)
                                    {
                                        var myentity = ceklistEntity3[i].Entity;
                                        var colNotNull = ceklistEntity3[i].PropertyInfos;
                                        context.Set<T3>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            if (IsSaveT4)
                            {
                                for (int i = 0; i < listEntity4.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T4>(listEntity4[i]);
                                    context.Set<T4>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity4 != null)
                                {
                                    for (int i = 0; i < ceklistEntity4.Count; i++)
                                    {
                                        var myentity = ceklistEntity4[i].Entity;
                                        var colNotNull = ceklistEntity4[i].PropertyInfos;
                                        context.Set<T4>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }

                            int hasil = context.SaveChanges();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Metode sync untuk Menambah atau mengubah Sekelompok data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <typeparam name="T3">Kelas 3</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="entity3">Entity Kelas 3</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        /// <param name="IsSaveT3">Is Save Kelas 3</param>
        public bool SaveUpdate<T1, T2, T3>(List<T1> listEntity1, List<T2> listEntity2, List<T3> listEntity3
           , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3)
           where T1 : class where T2 : class where T3 : class
        {
            bool result = false;
            bool goProcess = false;

            if (listEntity1 != null & listEntity2 != null & listEntity3 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new List<RepoGenPrepareUpdate<T1>>();
                var ceklistEntity2 = new List<RepoGenPrepareUpdate<T2>>();
                var ceklistEntity3 = new List<RepoGenPrepareUpdate<T3>>();
                if (!IsSaveT1)
                {
                    ceklistEntity1 = PrepareEntityForSaveUpdate<T1>(listEntity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = PrepareEntityForSaveUpdate<T2>(listEntity2);
                }
                if (!IsSaveT3)
                {
                    ceklistEntity3 = PrepareEntityForSaveUpdate<T3>(listEntity3);
                }
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                for (int i = 0; i < listEntity1.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(listEntity1[i]);
                                    context.Set<T1>().Add(myentity);
                                }


                            }
                            else
                            {

                                if (ceklistEntity1 != null)
                                {
                                    for (int i = 0; i < ceklistEntity1.Count; i++)
                                    {
                                        var myentity = ceklistEntity1[i].Entity;
                                        var colNotNull = ceklistEntity1[i].PropertyInfos;
                                        context.Set<T1>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            if (IsSaveT2)
                            {
                                for (int i = 0; i < listEntity2.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(listEntity2[i]);
                                    context.Set<T2>().Add(myentity);
                                }


                            }
                            else
                            {

                                if (ceklistEntity2 != null)
                                {
                                    for (int i = 0; i < ceklistEntity2.Count; i++)
                                    {
                                        var myentity = ceklistEntity2[i].Entity;
                                        var colNotNull = ceklistEntity2[i].PropertyInfos;
                                        context.Set<T2>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            if (IsSaveT3)
                            {
                                for (int i = 0; i < listEntity3.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T3>(listEntity3[i]);
                                    context.Set<T3>().Add(myentity);
                                }


                            }
                            else
                            {

                                if (ceklistEntity3 != null)
                                {
                                    for (int i = 0; i < ceklistEntity3.Count; i++)
                                    {
                                        var myentity = ceklistEntity3[i].Entity;
                                        var colNotNull = ceklistEntity3[i].PropertyInfos;
                                        context.Set<T3>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }

                            int hasil = context.SaveChanges();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Metode sync untuk Menambah atau mengubah Sekelompok data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        public bool SaveUpdate<T1, T2>(List<T1> listEntity1, List<T2> listEntity2
           , bool IsSaveT1, bool IsSaveT2)
           where T1 : class where T2 : class
        {
            bool result = false;
            bool goProcess = false;

            if (listEntity1 != null & listEntity2 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new List<RepoGenPrepareUpdate<T1>>();
                var ceklistEntity2 = new List<RepoGenPrepareUpdate<T2>>();
                if (!IsSaveT1)
                {
                    ceklistEntity1 = PrepareEntityForSaveUpdate<T1>(listEntity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = PrepareEntityForSaveUpdate<T2>(listEntity2);
                }
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                for (int i = 0; i < listEntity1.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(listEntity1[i]);
                                    context.Set<T1>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity1 != null)
                                {
                                    for (int i = 0; i < ceklistEntity1.Count; i++)
                                    {
                                        var myentity = ceklistEntity1[i].Entity;
                                        var colNotNull = ceklistEntity1[i].PropertyInfos;
                                        context.Set<T1>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            if (IsSaveT2)
                            {
                                for (int i = 0; i < listEntity2.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(listEntity2[i]);
                                    context.Set<T2>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity2 != null)
                                {
                                    for (int i = 0; i < ceklistEntity2.Count; i++)
                                    {
                                        var myentity = ceklistEntity2[i].Entity;
                                        var colNotNull = ceklistEntity2[i].PropertyInfos;
                                        context.Set<T2>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }
                            }


                            int hasil = context.SaveChanges();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Metode Async untuk Menambah atau mengubah data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <typeparam name="T3">Kelas 3</typeparam>
        /// <typeparam name="T4">Kelas 4</typeparam>
        /// <typeparam name="T5">Kelas 5</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="entity3">Entity Kelas 3</param>
        /// <param name="entity4">Entity Kelas 4</param>
        /// <param name="entity5">Entity Kelas 5</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        /// <param name="IsSaveT3">Is Save Kelas 3</param>
        /// <param name="IsSaveT4">Is Save Kelas 4</param>
        /// <param name="IsSaveT5">Is Save Kelas 5</param>
        /// <returns></returns>
        public async Task<bool> SaveUpdateAsync<T1, T2, T3, T4, T5>(T1 entity1, T2 entity2, T3 entity3, T4 entity4, T5 entity5
            , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4, bool IsSaveT5)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            bool result = false;
            bool goProcess = false;

            if (entity1 != null & entity2 != null & entity3 != null & entity4 != null & entity5 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new RepoGenPrepareUpdate<T1>();
                var ceklistEntity2 = new RepoGenPrepareUpdate<T2>();
                var ceklistEntity3 = new RepoGenPrepareUpdate<T3>();
                var ceklistEntity4 = new RepoGenPrepareUpdate<T4>();
                var ceklistEntity5 = new RepoGenPrepareUpdate<T5>();
                if (!IsSaveT1)
                {
                    ceklistEntity1 = await PrepareEntityForSaveUpdateAsync<T1>(entity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = await PrepareEntityForSaveUpdateAsync<T2>(entity2);
                }
                if (!IsSaveT3)
                {
                    ceklistEntity3 = await PrepareEntityForSaveUpdateAsync<T3>(entity3);
                }
                if (!IsSaveT4)
                {
                    ceklistEntity4 = await PrepareEntityForSaveUpdateAsync<T4>(entity4);
                }
                if (!IsSaveT5)
                {
                    ceklistEntity5 = await PrepareEntityForSaveUpdateAsync<T5>(entity5);
                }


                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                                context.Set<T1>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity1.Entity;
                                var colNotNull = ceklistEntity1.PropertyInfos;
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT2)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                                context.Set<T2>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity2.Entity;
                                var colNotNull = ceklistEntity2.PropertyInfos;
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT3)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T3>(entity3);
                                context.Set<T3>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity3.Entity;
                                var colNotNull = ceklistEntity3.PropertyInfos;
                                context.Set<T3>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT4)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T4>(entity4);
                                context.Set<T4>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity4.Entity;
                                var colNotNull = ceklistEntity4.PropertyInfos;
                                context.Set<T4>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT5)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T5>(entity5);
                                context.Set<T5>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity5.Entity;
                                var colNotNull = ceklistEntity5.PropertyInfos;
                                context.Set<T5>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T5>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            int hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Metode Async untuk Menambah atau mengubah data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <typeparam name="T3">Kelas 3</typeparam>
        /// <typeparam name="T4">Kelas 4</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="entity3">Entity Kelas 3</param>
        /// <param name="entity4">Entity Kelas 4</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        /// <param name="IsSaveT3">Is Save Kelas 3</param>
        /// <param name="IsSaveT4">Is Save Kelas 4</param>
        /// <returns></returns>
        public async Task<bool> SaveUpdateAsync<T1, T2, T3, T4>(T1 entity1, T2 entity2, T3 entity3, T4 entity4
          , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4)
          where T1 : class where T2 : class where T3 : class where T4 : class
        {
            bool result = false;
            bool goProcess = false;

            if (entity1 != null & entity2 != null & entity3 != null & entity4 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new RepoGenPrepareUpdate<T1>();
                var ceklistEntity2 = new RepoGenPrepareUpdate<T2>();
                var ceklistEntity3 = new RepoGenPrepareUpdate<T3>();
                var ceklistEntity4 = new RepoGenPrepareUpdate<T4>();
                if (!IsSaveT1)
                {
                    ceklistEntity1 = PrepareEntityForSaveUpdate<T1>(entity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = PrepareEntityForSaveUpdate<T2>(entity2);
                }
                if (!IsSaveT3)
                {
                    ceklistEntity3 = PrepareEntityForSaveUpdate<T3>(entity3);
                }
                if (!IsSaveT4)
                {
                    ceklistEntity4 = PrepareEntityForSaveUpdate<T4>(entity4);
                }
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                                context.Set<T1>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity1.Entity;
                                var colNotNull = ceklistEntity1.PropertyInfos;
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT2)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                                context.Set<T2>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity2.Entity;
                                var colNotNull = ceklistEntity2.PropertyInfos;
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT3)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T3>(entity3);
                                context.Set<T3>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity3.Entity;
                                var colNotNull = ceklistEntity3.PropertyInfos;
                                context.Set<T3>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT4)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T4>(entity4);
                                context.Set<T4>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity4.Entity;
                                var colNotNull = ceklistEntity4.PropertyInfos;
                                context.Set<T4>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            int hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Metode Async untuk Menambah atau mengubah data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <typeparam name="T3">Kelas 3</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="entity3">Entity Kelas 3</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        /// <param name="IsSaveT3">Is Save Kelas 3</param>
        /// <returns></returns>
        public async Task<bool> SaveUpdateAsync<T1, T2, T3>(T1 entity1, T2 entity2, T3 entity3
          , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3)
          where T1 : class where T2 : class where T3 : class
        {
            bool result = false;
            bool goProcess = false;

            if (entity1 != null & entity2 != null & entity3 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new RepoGenPrepareUpdate<T1>();
                var ceklistEntity2 = new RepoGenPrepareUpdate<T2>();
                var ceklistEntity3 = new RepoGenPrepareUpdate<T3>();
                if (!IsSaveT1)
                {
                    ceklistEntity1 = PrepareEntityForSaveUpdate<T1>(entity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = PrepareEntityForSaveUpdate<T2>(entity2);
                }
                if (!IsSaveT3)
                {
                    ceklistEntity3 = PrepareEntityForSaveUpdate<T3>(entity3);
                }
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                                context.Set<T1>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity1.Entity;
                                var colNotNull = ceklistEntity1.PropertyInfos;
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT2)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                                context.Set<T2>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity2.Entity;
                                var colNotNull = ceklistEntity2.PropertyInfos;
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT3)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T3>(entity3);
                                context.Set<T3>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity3.Entity;
                                var colNotNull = ceklistEntity3.PropertyInfos;
                                context.Set<T3>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }

                            int hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Metode Async untuk Menambah atau mengubah data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        /// <returns></returns>
        public async Task<bool> SaveUpdateAsync<T1, T2>(T1 entity1, T2 entity2
          , bool IsSaveT1, bool IsSaveT2)
          where T1 : class where T2 : class
        {
            bool result = false;
            bool goProcess = false;

            if (entity1 != null & entity2 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new RepoGenPrepareUpdate<T1>();
                var ceklistEntity2 = new RepoGenPrepareUpdate<T2>();

                if (!IsSaveT1)
                {
                    ceklistEntity1 = PrepareEntityForSaveUpdate<T1>(entity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = PrepareEntityForSaveUpdate<T2>(entity2);
                }
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(entity1);
                                context.Set<T1>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity1.Entity;
                                var colNotNull = ceklistEntity1.PropertyInfos;
                                context.Set<T1>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }
                            if (IsSaveT2)
                            {
                                var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(entity2);
                                context.Set<T2>().Add(myentity);

                            }
                            else
                            {
                                var myentity = ceklistEntity2.Entity;
                                var colNotNull = ceklistEntity2.PropertyInfos;
                                context.Set<T2>().Attach(myentity);
                                context.Entry(myentity).State = EntityState.Unchanged;
                                PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                foreach (var property in colNotNull)
                                {
                                    if (piID != property)
                                    {
                                        object value = property.GetValue(myentity);
                                        if (value != null)
                                        {
                                            context.Entry(myentity).Property(property.Name).IsModified = true;
                                        }
                                    }

                                }
                            }


                            int hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Metode Async untuk Menambah atau mengubah Sekelompok data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <typeparam name="T3">Kelas 3</typeparam>
        /// <typeparam name="T4">Kelas 4</typeparam>
        /// <typeparam name="T5">Kelas 5</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="entity3">Entity Kelas 3</param>
        /// <param name="entity4">Entity Kelas 4</param>
        /// <param name="entity5">Entity Kelas 5</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        /// <param name="IsSaveT3">Is Save Kelas 3</param>
        /// <param name="IsSaveT4">Is Save Kelas 4</param>
        /// <param name="IsSaveT5">Is Save Kelas 5</param>
        /// <returns></returns>
        public async Task<bool> SaveUpdateAsync<T1, T2, T3, T4, T5>(List<T1> listEntity1, List<T2> listEntity2, List<T3> listEntity3, List<T4> listEntity4, List<T5> listEntity5
            , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4, bool IsSaveT5)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            bool result = false;
            bool goProcess = false;

            if (listEntity1 != null & listEntity2 != null & listEntity3 != null & listEntity4 != null & listEntity5 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new List<RepoGenPrepareUpdate<T1>>();
                var ceklistEntity2 = new List<RepoGenPrepareUpdate<T2>>();
                var ceklistEntity3 = new List<RepoGenPrepareUpdate<T3>>();
                var ceklistEntity4 = new List<RepoGenPrepareUpdate<T4>>();
                var ceklistEntity5 = new List<RepoGenPrepareUpdate<T5>>();
                if (!IsSaveT1)
                {
                    ceklistEntity1 = await PrepareEntityForSaveUpdateAsync<T1>(listEntity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = await PrepareEntityForSaveUpdateAsync<T2>(listEntity2);
                }
                if (!IsSaveT3)
                {
                    ceklistEntity3 = await PrepareEntityForSaveUpdateAsync<T3>(listEntity3);
                }
                if (!IsSaveT4)
                {
                    ceklistEntity4 = await PrepareEntityForSaveUpdateAsync<T4>(listEntity4);
                }
                if (!IsSaveT5)
                {
                    ceklistEntity5 = await PrepareEntityForSaveUpdateAsync<T5>(listEntity5);
                }

                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                for (int i = 0; i < listEntity1.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(listEntity1[i]);
                                    context.Set<T1>().Add(myentity);
                                }


                            }
                            else
                            {

                                if (ceklistEntity1 != null)
                                {
                                    for (int i = 0; i < ceklistEntity1.Count; i++)
                                    {
                                        var myentity = ceklistEntity1[i].Entity;
                                        var colNotNull = ceklistEntity1[i].PropertyInfos;
                                        context.Set<T1>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            if (IsSaveT2)
                            {
                                for (int i = 0; i < listEntity2.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(listEntity2[i]);
                                    context.Set<T2>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity2 != null)
                                {
                                    for (int i = 0; i < ceklistEntity2.Count; i++)
                                    {
                                        var myentity = ceklistEntity2[i].Entity;
                                        var colNotNull = ceklistEntity2[i].PropertyInfos;
                                        context.Set<T2>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            if (IsSaveT3)
                            {
                                for (int i = 0; i < listEntity3.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T3>(listEntity3[i]);
                                    context.Set<T3>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity3 != null)
                                {
                                    for (int i = 0; i < ceklistEntity3.Count; i++)
                                    {
                                        var myentity = ceklistEntity3[i].Entity;
                                        var colNotNull = ceklistEntity3[i].PropertyInfos;
                                        context.Set<T3>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            if (IsSaveT4)
                            {
                                for (int i = 0; i < listEntity4.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T4>(listEntity4[i]);
                                    context.Set<T4>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity4 != null)
                                {
                                    for (int i = 0; i < ceklistEntity4.Count; i++)
                                    {
                                        var myentity = ceklistEntity4[i].Entity;
                                        var colNotNull = ceklistEntity4[i].PropertyInfos;
                                        context.Set<T4>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            if (IsSaveT5)
                            {
                                for (int i = 0; i < listEntity5.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T5>(listEntity5[i]);
                                    context.Set<T5>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity5 != null)
                                {
                                    for (int i = 0; i < ceklistEntity5.Count; i++)
                                    {
                                        var myentity = ceklistEntity5[i].Entity;
                                        var colNotNull = ceklistEntity5[i].PropertyInfos;
                                        context.Set<T5>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T5>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            int hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Metode Async untuk Menambah atau mengubah Sekelompok data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <typeparam name="T3">Kelas 3</typeparam>
        /// <typeparam name="T4">Kelas 4</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="entity3">Entity Kelas 3</param>
        /// <param name="entity4">Entity Kelas 4</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        /// <param name="IsSaveT3">Is Save Kelas 3</param>
        /// <param name="IsSaveT4">Is Save Kelas 4</param>
        public async Task<bool> SaveUpdateAsync<T1, T2, T3, T4>(List<T1> listEntity1, List<T2> listEntity2, List<T3> listEntity3, List<T4> listEntity4
           , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3, bool IsSaveT4)
           where T1 : class where T2 : class where T3 : class where T4 : class
        {
            bool result = false;
            bool goProcess = false;

            if (listEntity1 != null & listEntity2 != null & listEntity3 != null & listEntity4 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new List<RepoGenPrepareUpdate<T1>>();
                var ceklistEntity2 = new List<RepoGenPrepareUpdate<T2>>();
                var ceklistEntity3 = new List<RepoGenPrepareUpdate<T3>>();
                var ceklistEntity4 = new List<RepoGenPrepareUpdate<T4>>();
                if (!IsSaveT1)
                {
                    ceklistEntity1 = await PrepareEntityForSaveUpdateAsync<T1>(listEntity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = await PrepareEntityForSaveUpdateAsync<T2>(listEntity2);
                }
                if (!IsSaveT3)
                {
                    ceklistEntity3 = await PrepareEntityForSaveUpdateAsync<T3>(listEntity3);
                }
                if (!IsSaveT4)
                {
                    ceklistEntity4 = await PrepareEntityForSaveUpdateAsync<T4>(listEntity4);
                }
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                for (int i = 0; i < listEntity1.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(listEntity1[i]);
                                    context.Set<T1>().Add(myentity);
                                }


                            }
                            else
                            {

                                if (ceklistEntity1 != null)
                                {
                                    for (int i = 0; i < ceklistEntity1.Count; i++)
                                    {
                                        var myentity = ceklistEntity1[i].Entity;
                                        var colNotNull = ceklistEntity1[i].PropertyInfos;
                                        context.Set<T1>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            if (IsSaveT2)
                            {
                                for (int i = 0; i < listEntity2.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(listEntity2[i]);
                                    context.Set<T2>().Add(myentity);
                                }


                            }
                            else
                            {

                                if (ceklistEntity2 != null)
                                {
                                    for (int i = 0; i < ceklistEntity2.Count; i++)
                                    {
                                        var myentity = ceklistEntity2[i].Entity;
                                        var colNotNull = ceklistEntity2[i].PropertyInfos;
                                        context.Set<T2>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            if (IsSaveT3)
                            {
                                for (int i = 0; i < listEntity3.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T3>(listEntity3[i]);
                                    context.Set<T3>().Add(myentity);
                                }


                            }
                            else
                            {

                                if (ceklistEntity3 != null)
                                {
                                    for (int i = 0; i < ceklistEntity3.Count; i++)
                                    {
                                        var myentity = ceklistEntity3[i].Entity;
                                        var colNotNull = ceklistEntity3[i].PropertyInfos;
                                        context.Set<T3>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            if (IsSaveT4)
                            {
                                for (int i = 0; i < listEntity4.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T4>(listEntity4[i]);
                                    context.Set<T4>().Add(myentity);
                                }


                            }
                            else
                            {

                                if (ceklistEntity4 != null)
                                {
                                    for (int i = 0; i < ceklistEntity4.Count; i++)
                                    {
                                        var myentity = ceklistEntity4[i].Entity;
                                        var colNotNull = ceklistEntity4[i].PropertyInfos;
                                        context.Set<T4>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T4>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                            int hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch
                        {
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Metode Async untuk Menambah atau mengubah Sekelompok data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <typeparam name="T3">Kelas 3</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="entity3">Entity Kelas 3</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        /// <param name="IsSaveT3">Is Save Kelas 3</param>
        public async Task<bool> SaveUpdateAsync<T1, T2, T3>(List<T1> listEntity1, List<T2> listEntity2, List<T3> listEntity3
           , bool IsSaveT1, bool IsSaveT2, bool IsSaveT3)
           where T1 : class where T2 : class where T3 : class
        {
            bool result = false;
            bool goProcess = false;

            if (listEntity1 != null & listEntity2 != null & listEntity3 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new List<RepoGenPrepareUpdate<T1>>();
                var ceklistEntity2 = new List<RepoGenPrepareUpdate<T2>>();
                var ceklistEntity3 = new List<RepoGenPrepareUpdate<T3>>();

                if (!IsSaveT1)
                {
                    ceklistEntity1 = await PrepareEntityForSaveUpdateAsync<T1>(listEntity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = await PrepareEntityForSaveUpdateAsync<T2>(listEntity2);
                }
                if (!IsSaveT3)
                {
                    ceklistEntity3 = await PrepareEntityForSaveUpdateAsync<T3>(listEntity3);
                }
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                for (int i = 0; i < listEntity1.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(listEntity1[i]);
                                    context.Set<T1>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity1 != null)
                                {
                                    for (int i = 0; i < ceklistEntity1.Count; i++)
                                    {
                                        var myentity = ceklistEntity1[i].Entity;
                                        var colNotNull = ceklistEntity1[i].PropertyInfos;
                                        context.Set<T1>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            if (IsSaveT2)
                            {
                                for (int i = 0; i < listEntity2.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(listEntity2[i]);
                                    context.Set<T2>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity2 != null)
                                {
                                    for (int i = 0; i < ceklistEntity2.Count; i++)
                                    {
                                        var myentity = ceklistEntity2[i].Entity;
                                        var colNotNull = ceklistEntity2[i].PropertyInfos;
                                        context.Set<T2>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            if (IsSaveT3)
                            {
                                for (int i = 0; i < listEntity3.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T3>(listEntity3[i]);
                                    context.Set<T3>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity3 != null)
                                {
                                    for (int i = 0; i < ceklistEntity3.Count; i++)
                                    {
                                        var myentity = ceklistEntity3[i].Entity;
                                        var colNotNull = ceklistEntity3[i].PropertyInfos;
                                        myentity = this.PrepareEntityForUpdate<T3>(myentity);
                                        context.Set<T3>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T3>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }

                            int hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Metode Async untuk Menambah atau mengubah Sekelompok data.Data yang diubah adalah data yang tidak null saja. Untuk data yang diupdate memerlukan 1 operasi select ke db untuk verifikasi data
        /// </summary>
        /// <typeparam name="T1">Kelas 1</typeparam>
        /// <typeparam name="T2">Kelas 2</typeparam>
        /// <param name="entity1">Entity Kelas 1</param>
        /// <param name="entity2">Entity Kelas 2</param>
        /// <param name="IsSaveT1">Is Save Kelas 1</param>
        /// <param name="IsSaveT2">Is Save Kelas 2</param>
        public async Task<bool> SaveUpdateAsync<T1, T2>(List<T1> listEntity1, List<T2> listEntity2
           , bool IsSaveT1, bool IsSaveT2)
           where T1 : class where T2 : class
        {
            bool result = false;
            bool goProcess = false;

            if (listEntity1 != null & listEntity2 != null)
                goProcess = true;
            if (goProcess)
            {
                var ceklistEntity1 = new List<RepoGenPrepareUpdate<T1>>();
                var ceklistEntity2 = new List<RepoGenPrepareUpdate<T2>>();
                if (!IsSaveT1)
                {
                    ceklistEntity1 = await PrepareEntityForSaveUpdateAsync<T1>(listEntity1);
                }
                if (!IsSaveT2)
                {
                    ceklistEntity2 = await PrepareEntityForSaveUpdateAsync<T2>(listEntity2);
                }
                using (var context = GetDbContext())
                {
                    using (var contextTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (IsSaveT1)
                            {
                                for (int i = 0; i < listEntity1.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T1>(listEntity1[i]);
                                    context.Set<T1>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity1 != null)
                                {
                                    for (int i = 0; i < ceklistEntity1.Count; i++)
                                    {
                                        var myentity = ceklistEntity1[i].Entity;
                                        var colNotNull = ceklistEntity1[i].PropertyInfos;
                                        context.Set<T1>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T1>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            if (IsSaveT2)
                            {
                                for (int i = 0; i < listEntity2.Count; i++)
                                {
                                    var myentity = GenHelperEF.Getinstance.SetColIDZero<T2>(listEntity2[i]);
                                    context.Set<T2>().Add(myentity);
                                }


                            }
                            else
                            {
                                if (ceklistEntity2 != null)
                                {
                                    for (int i = 0; i < ceklistEntity2.Count; i++)
                                    {
                                        var myentity = ceklistEntity2[i].Entity;
                                        var colNotNull = ceklistEntity2[i].PropertyInfos;
                                        context.Set<T2>().Attach(myentity);
                                        context.Entry(myentity).State = EntityState.Unchanged;
                                        PropertyInfo piID = GenHelperEF.Getinstance.GetIdentityColumnProps<T2>();
                                        foreach (var property in colNotNull)
                                        {
                                            if (piID != property)
                                            {
                                                object value = property.GetValue(myentity);
                                                if (value != null)
                                                {
                                                    context.Entry(myentity).Property(property.Name).IsModified = true;
                                                }
                                            }

                                        }
                                    }
                                }
                            }


                            int hasil = await context.SaveChangesAsync();
                            contextTrans.Commit();
                            if (hasil > 0)
                                result = true;
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                            contextTrans.Rollback();
                        }


                    }
                }
            }

            return result;
        }

        public async Task<bool> DeleteActiveBoolAsync<T>(int identityID, string pic) where T : class
        {
            bool result = false;
            var entity = GenHelperEF.Getinstance.ActiveBoolColumnFalse<T>(identityID, pic);
            entity = await PrepareEntityForUpdateAsync<T>(entity);//
            if (entity != null)
            {
                result = await this.UpdateAllAsync<T>(entity);
            }
            return result;
        }

        public async Task<bool> DeleteActiveBoolAsync<T>(List<int> identityID, string pic) where T : class
        {
            bool result = false;
            var entityCollection = GenHelperEF.Getinstance.ActiveBoolColumnFalse<T>(identityID, pic);
            entityCollection = await PrepareEntityForUpdateAsync<T>(entityCollection);
            if (entityCollection != null)
            {
                result = await this.UpdateAllAsync<T>(entityCollection);
            }

            return result;
        }
    }
    public class RepoGenPrepareUpdate<T> where T : class
    {
        public List<PropertyInfo> PropertyInfos { get; set; }
        public T Entity { get; set; }
    }
}
