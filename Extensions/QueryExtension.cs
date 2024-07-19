using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PreCompiledQuery.Data;
using PreCompiledQuery.Entities;

namespace PreCompiledQuery.Extensions;
public static class QueryExtensions
{
    // Method with TParam
    public static Func<TContext, TParam, IAsyncEnumerable<TEntity>> CompileAsyncQuery<TContext, TEntity, TParam>(
        this TContext context,
        Expression<Func<TContext, TParam, IQueryable<TEntity>>> queryExpression)
        where TContext : DbContext
    {
        return EF.CompileAsyncQuery(queryExpression);
    }

    // Method without TParam
    public static Func<TContext, IAsyncEnumerable<TEntity>> CompileAsyncQuery<TContext, TEntity>(
        this TContext context,
        Expression<Func<TContext, IQueryable<TEntity>>> queryExpression)
        where TContext : DbContext
    {
        return EF.CompileAsyncQuery(queryExpression);
    }
}
