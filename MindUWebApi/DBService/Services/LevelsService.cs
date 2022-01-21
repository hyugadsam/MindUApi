using DBService.Entities;
using DBService.Interfaces;
using Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBService.Services
{
    public class LevelsService : IGenericService<Levels>
    {
        private readonly MindUContext context;
        private readonly ILogger<LevelsService> logger;

        public LevelsService(MindUContext context, ILogger<LevelsService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<BasicResponse> Create(Levels obj)
        {
            try
            {
                context.Levels.Add(obj);
                await context.SaveChangesAsync();
                return new BasicResponse { Code = 200, Message = "Save Success" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.LevelsService.Create");
                return new BasicResponse { Code = 500, Message = "Internal eroror creating new Level" };
            }
        }

        public async Task<BasicResponse> Delete(int LevelId)
        {
            try
            {
                var data = await ExistsLevelId(LevelId);
                if (data == null)
                {
                    return new BasicResponse { Code = 400, Message = "Level dosent exists" };
                }
                context.Levels.Remove(data);
                await context.SaveChangesAsync();

                return new BasicResponse { Code = 200, Message = "Delete Success" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.LevelsService.Delete");
                return new BasicResponse { Code = 500, Message = "Internal eroror deleting" };
            }
        }

        public async Task<List<Levels>> GetList()
        {
            try
            {
                return await context.Levels.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.LevelsService.GetList");
                return null;
            }
        }

        public async Task<BasicResponse> Update(Levels obj)
        {
            try
            {
                var data = await ExistsLevelId(obj.LevelId);
                if (data == null)
                {
                    return new BasicResponse { Code = 400, Message = "Level dosent exists" };
                }
                data.Description = obj.Description;

                context.Entry(data).State=EntityState.Modified;
                await context.SaveChangesAsync();

                return new BasicResponse { Code = 200, Message = "Update Success" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.LevelsService.Update");
                return new BasicResponse { Code = 500, Message = "Internal eroror updating info" };
            }
        }

        private async Task<Levels> ExistsLevelId(int LevelId)
        {
            return await context.Levels.FirstOrDefaultAsync(t => t.LevelId == LevelId);
        }

    }
}
