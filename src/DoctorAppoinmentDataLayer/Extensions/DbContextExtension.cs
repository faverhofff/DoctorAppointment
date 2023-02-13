using DoctorAppointmentDataLayer;
using DoctorAppointmentDataLayer.Models;
using MongoDB.Driver;
using ParkBee.MongoDb;
using System;
using System.Linq.Expressions;

namespace DoctorAppointmentDataLayer.Extensions
{
    public static class DbContextExtension
    {
        #region issue mongodb context
        public static FilterDefinition<TEntity> GetFindByKeyFilterDefinition<TEntity>(this MongoDbContext dbContext, object keyValue) where TEntity : class
        {
            var memberExpression = GetFilterByKeyExpression<TEntity>(dbContext.Database);
            var parameterExpression = Expression.Parameter(typeof(TEntity), "e");
            var left = Expression.Property(parameterExpression, memberExpression.Member.Name);
            var right = Expression.Constant(keyValue, keyValue.GetType());

            var predicate = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(left, right), parameterExpression
            );
            return new ExpressionFilterDefinition<TEntity>(predicate);
        }

        private static MemberExpression GetFilterByKeyExpression<TEntity>(IMongoDatabase database) where TEntity : class 
        {
            var entity = new EntityTypeBuilder<TEntity>(database);
            entity.HasKey(p => (p as BaseModel).Id);
            var builder = entity as EntityTypeBuilder<TEntity>;

            if (builder.KeyPropertyExpression == null)
            {
                throw new InvalidOperationException("HasKey invalid");
            }

            return ((builder.KeyPropertyExpression as LambdaExpression).Body as Expression) as MemberExpression;
        }
        #endregion 
    }
}
