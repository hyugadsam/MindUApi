using DBService.Entities;
using Dtos.Dtos;
using Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBService.Interfaces
{
    public interface ICollaboratorsService
    {
        Task<BasicCreateResponse> Create(Collaborators obj);
        Task<BasicResponse> Delete(int CollaboratorId);
        Task<List<Collaborators>> GetList(PaginacionDTO paginacion);
        Task<Collaborators> Get(int CollaboratorId);
        Task<BasicResponse> Update(Collaborators obj);
        Task<BasicResponse> DeactivateCollaborator(int CollaboratorId);
        Task<BasicResponse> GraduateCollaborator(int CollaboratorId);


    }
}
