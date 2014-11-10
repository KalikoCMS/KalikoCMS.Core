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
    using System.Linq;
    using System.Linq.Expressions;
    using AutoMapper;
    using Kaliko;
    using Telerik.OpenAccess;

    public static class DataManager {
        public static void BatchUpdate<T>(IEnumerable<T> items) {
            var context = new DataContext();

            try {
                foreach (var item in items) {
                    context.AttachCopy(item);
                }
                context.SaveChanges();
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                context.Dispose();
            }
        }

        
        public static void Insert<T>(T item) {
            var context = new DataContext();

            try {
                context.Add(item);
                context.SaveChanges();
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                context.Dispose();
            }
        }


        public static T InsertOrUpdate<T>(T item) {
            var context = new DataContext();

            try {
                item = context.AttachCopy(item);
                context.SaveChanges();
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                context.Dispose();
            }
            return item;
        }


        public static List<T> Select<T>(Expression<Func<T, bool>> predicate) {
            var context = new DataContext();

            try {
                return context.GetAll<T>().Where(predicate).ToList();
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                context.Dispose();
            }
        }

        
        public static List<TCast> Select<T,TCast>(Expression<Func<T, bool>> predicate) {
            var context = new DataContext();

            try {
                var entities = context.GetAll<T>().Where(predicate).ToList();
                return Mapper.Map<List<T>, List<TCast>>(entities);
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                context.Dispose();
            }
        }


        public static List<TCast> SelectAll<T,TCast>() {
            var context = new DataContext();

            try {
                var entities = context.GetAll<T>().ToList();
                var mappedEntities = Mapper.Map<List<T>, List<TCast>>(entities);
                return mappedEntities;
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                context.Dispose();
            }
        }


        internal static int DeleteAll<T>() {
            var context = new DataContext();

            try {
                var affectedRows = context.GetAll<T>().DeleteAll();
                return affectedRows;
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                context.Dispose();
            }
        }


        public static int Delete<T>(Expression<Func<T, bool>> predicate) {
            var affectedRows = 0;
            var context = new DataContext();

            try {
                affectedRows = context.GetAll<T>().Where(predicate).DeleteAll();
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                context.Dispose();
            }

            return affectedRows;
        }


        public static TCast Single<T, TCast>(Func<T, bool> predicate) {
            var context = new DataContext();

            try {
                var entity = context.GetAll<T>().SingleOrDefault(predicate);
                return Mapper.Map<T, TCast>(entity);
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                context.Dispose();
            }
        }


        public static T Single<T>(Func<T, bool> predicate) {
            var context = new DataContext();

            try {
                return context.GetAll<T>().SingleOrDefault(predicate);
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                context.Dispose();
            }
        }


        public static T FirstOrDefault<T>(Func<T, bool> predicate) {
            var context = new DataContext();

            try {
                return context.GetAll<T>().FirstOrDefault(predicate);
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                context.Dispose();
            }
        }
    }
}

