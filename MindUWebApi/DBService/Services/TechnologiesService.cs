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
    public class TechnologiesService: IGenericService<Technologies>
    {
        private readonly MindUContext context;
        private readonly ILogger<TechnologiesService> logger;

        public TechnologiesService(MindUContext context, ILogger<TechnologiesService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<BasicResponse> Create(Technologies obj)
        {
            try
            {
                context.Technologies.Add(obj);
                await context.SaveChangesAsync();
                return new BasicResponse { Code = 200, Message = "Save Success" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.TechnologiesService.CreateTechnology");
                return new BasicResponse { Code = 500, Message = "Internal eroror creating new Technology" };
            }
        }

        public async Task<BasicResponse> Delete(int TechnologyId)
        {
            try
            {
                var data = await ExistsTechnologyId(TechnologyId);
                if (data == null)
                {
                    return new BasicResponse { Code = 400, Message = "Technology dosent exists" };
                }
                context.Technologies.Remove(data);
                await context.SaveChangesAsync();

                return new BasicResponse { Code = 200, Message = "Delete Success" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.TechnologiesService.Delete");
                return new BasicResponse { Code = 500, Message = "Internal eroror deleting" };
            }
        }

        public async Task<List<Technologies>> GetList()
        {
            try
            {
                return await context.Technologies.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.TechnologiesService.GetList");
                return null;
            }
        }

        public async Task<BasicResponse> Update(Technologies obj)
        {
            try
            {
                var data = await ExistsTechnologyId(obj.TechnologyId);
                if (data == null)
                {
                    return new BasicResponse { Code = 400, Message = "Technology dosent exists" };
                }
                data.Description = obj.Description;

                context.Entry(data).State=EntityState.Modified;
                await context.SaveChangesAsync();

                return new BasicResponse { Code = 200, Message = "Update Success" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.TechnologiesService.UpdateTechnology");
                return new BasicResponse { Code = 500, Message = "Internal eroror updating info" };
            }
        }

        private async Task<Technologies> ExistsTechnologyId(int TechnologyId)
        {
            return await context.Technologies.FirstOrDefaultAsync(t => t.TechnologyId == TechnologyId);
        }

    }
}
