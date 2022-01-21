using DBService.Entities;
using DBService.Interfaces;
using DBService.Services;
using Dtos.Responses;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dtos.Dtos;
using AutoMapper;

namespace ApplicationServices.Services
{
    public class AppServiceLevels
    {
        IGenericService<Levels> service;
        private readonly IMapper mapper;

        public AppServiceLevels(MindUContext context, ILogger<LevelsService> logger, IMapper mapper)
        {
            service = new LevelsService(context, logger);
            this.mapper = mapper;
        }

        public async Task<BasicResponse> CreateLevel(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return new BasicResponse { Code= 400, Message = "Description is required" };
            }

            var data = new Levels() { Description = description };
            return await service.Create(data);
        }

        public async Task<BasicResponse> UpdateLevel(LevelDto obj)
        {
            var data = mapper.Map<Levels>(obj);
            return await service.Update(data);
        }

        public async Task<BasicResponse> Delete(int LevelId)
        {
            if (LevelId <= 0)
            {
                return new BasicResponse { Code = 400, Message = "LevelId is required" };
            }

            return await service.Delete(LevelId);
        }

        public async Task<List<LevelDto>> GetList()
        {
            var data = await service.GetList();
            return mapper.Map<List<LevelDto>>(data);
        }


    }
}
