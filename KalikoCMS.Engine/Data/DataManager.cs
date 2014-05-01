#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * http://www.gnu.org/licenses/lgpl-3.0.html
 */
#endregion

namespace KalikoCMS.Data {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using IQToolkit;
    using IQToolkit.Data;
    using Configuration;
    using Kaliko;
    using EntityProvider;

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
            get { return new ContentDatabase(EntityProvider); }
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


        public static void BatchUpdate<T>(IEntityTable<T> entityTable, IEnumerable<T> items, Expression<Func<T, int>> idSelector) {
            OpenConnection();

            try {
                var entityType = typeof(T);
                var primaryKeyName = GetPrimaryKeyName(entityType);

                foreach (var item in items) {
                    int identity = entityTable.InsertOrUpdate(item, null, idSelector);
                    entityType.GetProperty(primaryKeyName).SetValue(item, identity);
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


        private static string GetPrimaryKeyName(Type entityType) {
            var mappingEntity = EntityProvider.Mapping.GetEntity(entityType);
            var primaryKey = EntityProvider.Mapping.GetPrimaryKeyMembers(mappingEntity).SingleOrDefault() as PropertyInfo;

            if (primaryKey == null) {
                Utils.Throw<Exception>(string.Format("Cannot find primary key for type {0}", entityType.Name), Logger.Severity.Critical);
            }

            return primaryKey.Name;
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


        public static void InsertOrUpdate<T>(IEntityTable<T> entityTable, T item, Expression<Func<T, int>> idSelector) {
            OpenConnection();

            try {
                var entityType = typeof(T);
                var primaryKeyName = GetPrimaryKeyName(entityType);
                var identity = entityTable.InsertOrUpdate(item, null, idSelector);
                entityType.GetProperty(primaryKeyName).SetValue(item, identity);
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


        internal static int DeleteAll<T>(IEntityTable<T> entityTable) {
            int affectedRows;

            OpenConnection();

            try {
                affectedRows = entityTable.Delete(p => true);
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                CloseConnection();
            }

            return affectedRows;
        }

        public static int Delete<T>(IEntityTable<T> entityTable, Expression<Func<T, bool>> whereClause) {
            int affectedRows;

            OpenConnection();

            try {
                affectedRows = ((IUpdatable<T>)entityTable).Delete(whereClause);
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                CloseConnection();
            }

            return affectedRows;
        }
    }
}

