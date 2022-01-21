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
    public class AppServiceTechnologies
    {
        IGenericService<Technologies> service;
        private readonly IMapper mapper;

        public AppServiceTechnologies(MindUContext context, ILogger<TechnologiesService> logger, IMapper mapper)
        {
            service = new TechnologiesService(context, logger);
            this.mapper = mapper;
        }

        public async Task<BasicResponse> CreateTechnology(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return new BasicResponse { Code= 400, Message = "Description is required" };
            }

            var data = new Technologies() { Description = description };
            return await service.Create(data);
        }

        public async Task<BasicResponse> UpdateTechnology(TechnologyDto obj)
        {
            var tech = mapper.Map<Technologies>(obj);
            return await service.Update(tech);
        }

        public async Task<BasicResponse> Delete(int TechnologyId)
        {
            if (TechnologyId <=0)
            {
                return new BasicResponse { Code = 400, Message = "TechnologyId is required" };
            }

            return await service.Delete(TechnologyId);
        }

        public async Task<List<TechnologyDto>> GetList()
        {
            var data = await service.GetList();
            return mapper.Map<List<TechnologyDto>>(data);
        }


    }
}
