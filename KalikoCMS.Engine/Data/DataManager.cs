//#region License and copyright notice
///* 
// * Kaliko Content Management System
// * 
// * Copyright (c) Fredrik Schultz
// * 
// * This library is free software; you can redistribute it and/or
// * modify it under the terms of the GNU Lesser General Public
// * License as published by the Free Software Foundation; either
// * version 3.0 of the License, or (at your option) any later version.
// * 
// * This library is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// * Lesser General Public License for more details.
// * http://www.gnu.org/licenses/lgpl-3.0.html
// */
//#endregion

namespace KalikoCMS.Data {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using AutoMapper;
    using Kaliko;
    using Entities;

    public static class DataManager {
        public static void Insert<T>(T item) where T : class {
            var context = new DataContext();

            try {
                context.Set<T>().Add(item);
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

        internal static void BatchUpdate(List<PropertyTypeEntity> items) {
            var context = new DataContext();

            try {
                var dbSet = context.Set<PropertyTypeEntity>();
                foreach (var item in items) {
                    if (item.PropertyTypeId == Guid.Empty) {
                        dbSet.Add(item);
                    }
                    else {
                        dbSet.Update(item);
                    }
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

        public static List<TCast> SelectAll<T, TCast>() where T : class {
            using (var context = new DataContext()) {
                try {
                    var entities = context.Set<T>().ToList();
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
        }

        public static T FirstOrDefault<T>(Func<T, bool> predicate) where T : class {
            var context = new DataContext();

            try {
                return context.Set<T>().FirstOrDefault(predicate);
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                context.Dispose();
            }
        }

        public static void InsertOrUpdate(KeyValuePair keyValuePair) {
            throw new NotImplementedException();
        }

        public static T Single<T>(Func<T, bool> predicate) where T : class {
            var context = new DataContext();

            try {
                return context.Set<T>().SingleOrDefault(predicate);
            }
            catch (Exception e) {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally {
                context.Dispose();
            }
        }

        public static void Delete<T>(Func<T, bool> predicate) where T : class {
            var context = new DataContext();

            try {
                var dbSet = context.Set<T>();
                var entities = dbSet.Where(predicate);
                dbSet.RemoveRange(entities);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.Write(e, Logger.Severity.Major);
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public static IEnumerable<T> Select<T>(Func<T, bool> predicate) where T : class {
            var context = new DataContext();

            try {
                return context.Set<T>().Where(predicate);
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

