#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz and Contributors
 * 
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 * 
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
 * EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */
#endregion

namespace KalikoCMS.Data {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using IQToolkit;
    using IQToolkit.Data;
    using Configuration;
    using Kaliko;
    using KalikoCMS.Data.EntityProvider;

    public static class DataManager {
        private static readonly DbEntityProvider EntityProvider = GetDbEntityProvider();

        private static DbEntityProvider GetDbEntityProvider() {
            return DbEntityProvider.From(SiteSettings.Instance.DataProvider,
                                         SiteSettings.Instance.ConnectionString,
                                         "KalikoCMS.Data.EntityProvider.ContentDatabaseWithAttributes");
        }

        public static DbEntityProvider Provider {
            get { return EntityProvider; }
        }

        public static ContentDatabase Instance {
            get {
                return new ContentDatabase(EntityProvider);
            }
        }

        public static void OpenConnection() {
            if (EntityProvider.Connection.State == ConnectionState.Open) {
                EntityProvider.Connection.Close();
            }
            Provider.Connection.Open();
        }

        public static void CloseConnection() {
            EntityProvider.Connection.Close();
        }


        public static void BatchUpdate<T>(IEntityTable<T> entityTable, IEnumerable<T> items) {
            OpenConnection();

            try {
                foreach (var item in items) {
                    entityTable.InsertOrUpdate(item);
                }
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                CloseConnection();
            }
        }


        public static T GetById<T>(IEntityTable<T> entityTable, int id) {
            T item;

            OpenConnection();

            try {
                item = entityTable.GetById(id);
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                CloseConnection();
            }

            return item;
        }


        public static T GetById<T>(IEntityTable<T> entityTable, Guid id) {
            T item;

            OpenConnection();

            try {
                item = entityTable.GetById(id);
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                CloseConnection();
            }

            return item;
        }


        public static void Insert<T>(IEntityTable<T> entityTable, T item) {
            OpenConnection();

            try {
                entityTable.Insert(item);
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                CloseConnection();
            }
        }


        public static void InsertOrUpdate<T>(IEntityTable<T> entityTable, T item) {
            OpenConnection();

            try {
                entityTable.InsertOrUpdate(item);
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                CloseConnection();
            }
        }


        public static List<T> Select<T>(IEntityTable<T> entityTable, Expression<Func<T, bool>> whereClause) {
            List<T> items;

            OpenConnection();

            try {
                items = entityTable.Where(whereClause).ToList();
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                CloseConnection();
            }

            return items;
        }


        public static List<T> Select<T, TKey>(IEntityTable<T> entityTable, Expression<Func<T, bool>> whereClause, Expression<Func<T, TKey>> orderBy) {
            List<T> items;

            OpenConnection();

            try {
                items = entityTable.Where(whereClause).OrderBy(orderBy).ToList();
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                CloseConnection();
            }

            return items;
        }


        public static List<T> SelectAll<T>(IEntityTable<T> entityTable) {
            List<T> items;

            OpenConnection();

            try {
                items = entityTable.Select(p => p).ToList();
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                CloseConnection();
            }

            return items;
        }

        public static T Single<T>(IEntityTable<T> entityTable, Expression<Func<T, bool>> whereClause) {
            T item;

            OpenConnection();

            try {
                item = entityTable.SingleOrDefault(whereClause);
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                CloseConnection();
            }

            return item;
        }
    }
}

