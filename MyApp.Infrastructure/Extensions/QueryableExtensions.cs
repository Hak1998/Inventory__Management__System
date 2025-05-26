using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WithUpdLockNoWait<T>(this DbSet<T> dbSet, string whereClause, params object[] parameters) where T : class
        {
            var tableName = dbSet.EntityType.GetTableName();
            var sql = $"SELECT * FROM \"{tableName}\" WHERE {whereClause} AND \"IsActive\" = true FOR UPDATE";
            return dbSet.FromSqlRaw(sql);
        }
        public static IQueryable<T> WithUpdLockNoWapit<T>(this DbSet<T> dbSet, string whereClause, params object[] parameters) where T : class
        {
            var tableName = typeof(T).Name;
            var sql = $"SELECT * FROM {tableName} WITH (UPDLOCK, ROWLOCK, NOWAIT) WHERE {whereClause}";
            return dbSet.FromSqlRaw(sql, parameters);
        }


        public static IQueryable<T> WithUpdLockNoWait<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> predicate) where T : class
        {
            var tableName = dbSet.EntityType.GetTableName();
            var whereClause = predicate.Body.ToString(); // Requires parsing logic
            var sql = $"SELECT * FROM \"{tableName}\" WHERE \"{whereClause}\" FOR UPDATE";
            return dbSet.FromSqlRaw(sql);
        }
    }
}
