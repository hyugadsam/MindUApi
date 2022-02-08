using AutoMapper;
using DBService.Entities;
using DBService.Interfaces;
using DBService.Services;
using Dtos.Dtos;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationServices.Services
{
    public class AppServiceCollaborators
    {
        ICollaboratorsService service;
        private readonly IMapper mapper;

        public AppServiceCollaborators(MindUContext Context, ILogger<CollaboratorsService> logger, IMapper mapper)
        {
            service = new CollaboratorsService(Context, logger);
            this.mapper = mapper;
        }


        public async Task<BasicCreateResponse> Create(NewCollaboratorRequest obj)
        {
            var data = mapper.Map<Collaborators>(obj);
            return await service.Create(data);
        }

        public async Task<BasicResponse> Delete(int CollaboratorId)
        {
            return await service.Delete(CollaboratorId);
        }

        public async Task<List<CollaboratorDto>> GetList(PaginacionDTO paginacion)
        {
            var data = await service.GetList(paginacion);
            return mapper.Map<List<CollaboratorDto>>(data);
        }

        public async Task<CollaboratorFullDto> Get(int CollaboratorId)
        {
            var data = await service.Get(CollaboratorId);
            return mapper.Map<CollaboratorFullDto>(data);
        }

        public async Task<BasicResponse> Update(UpdateCollaboratorRequest obj)
        {
            var data = mapper.Map<Collaborators>(obj);
            return await service.Update(data);
        }

        public async Task<BasicResponse> DeactivateCollaborator(int CollaboratorId)
        {
            return await service.DeactivateCollaborator(CollaboratorId);
        }

        public async Task<BasicResponse> GraduateCollaborator(int CollaboratorId)
        {
            return await service.GraduateCollaborator(CollaboratorId);
        }


    }
}
