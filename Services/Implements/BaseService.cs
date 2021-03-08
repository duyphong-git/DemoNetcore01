using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Implements
{
    public class BaseService
    {
        protected readonly DataContext context;

        public BaseService(DataContext context)
        {
            this.context = context;
        }

        protected (bool isCreated, TResult result) GetOrCreateEntity<TResult>(
            DbSet<TResult> sourceCollection,
            Expression<Func<TResult, bool>> whereConditions = null)
            where TResult : class, new()
        {
            var isCreated = false;
            TResult result = null;

            if (whereConditions != null)
                result = sourceCollection.FirstOrDefault(whereConditions); 

            if (result == null)
            {
                isCreated = true;
                result = new TResult();
                sourceCollection.Add(result);
            }

            var creatorId = typeof(TResult).GetProperty("CreatorId");
            if (creatorId != null && isCreated)
                creatorId.SetValue(result, "Admin");//CurrentUser.Id);

            var modifierId = typeof(TResult).GetProperty("ModifierId");
            if (modifierId != null)
                modifierId.SetValue(result, "Admin");//CurrentUser.Id);

            var createDate = typeof(TResult).GetProperty("CreateDate");
            if (createDate != null && isCreated)
                createDate.SetValue(result, DateTime.UtcNow);

            var modifyDate = typeof(TResult).GetProperty("ModifyDate");
            if (modifyDate != null)
                modifyDate.SetValue(result, DateTime.UtcNow);

            //var hversion = typeof(TResult).GetProperty("HVersion");
            //if (hversion != null)
            //    hversion.SetValue(result, (int)hversion.GetValue(result) + 1);

            return (isCreated, result);
        }

        protected async Task<(bool isCreated, TResult result)> GetOrCreateEntityAsync<TResult>(
            DbSet<TResult> sourceCollection,
            Expression<Func<TResult, bool>> whereConditions = null)
            where TResult : class, new()
        {
            var isCreated = false;
            TResult result = null;

            if (whereConditions != null)
                result = await sourceCollection.FirstOrDefaultAsync(whereConditions);

            if (result == null)
            {
                isCreated = true;
                result = new TResult();
                await sourceCollection.AddAsync(result);
            }

            var creatorId = typeof(TResult).GetProperty("CreatorId");
            if (creatorId != null && isCreated)
                creatorId.SetValue(result, "Admin");//CurrentUser.Id);

            var modifierId = typeof(TResult).GetProperty("ModifierId");
            if (modifierId != null)
                modifierId.SetValue(result, "Admin");//CurrentUser.Id);

            var createDate = typeof(TResult).GetProperty("CreateDate");
            if (createDate != null && isCreated)
                createDate.SetValue(result, DateTime.UtcNow);

            var modifyDate = typeof(TResult).GetProperty("ModifyDate");
            if (modifyDate != null)
                modifyDate.SetValue(result, DateTime.UtcNow);

            var hversion = typeof(TResult).GetProperty("HVersion");
            if (hversion != null)
                hversion.SetValue(result, (int)hversion.GetValue(result) + 1);

            return (isCreated, result);
        }
    }
}
