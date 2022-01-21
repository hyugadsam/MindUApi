using DBService.Entities;
using DBService.Interfaces;
using Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace DBService.Services
{
    public class CollaboratorsService: ICollaboratorsService
    {
        private readonly MindUContext context;
        private readonly ILogger<CollaboratorsService> logger;

        public CollaboratorsService(MindUContext context, ILogger<CollaboratorsService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<BasicResponse> Create(Collaborators obj)
        {
            try
            {
                context.Collaborators.Add(obj);
                await context.SaveChangesAsync();
                return new BasicResponse { Code = 200, Message = "Create Success" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.CollaboratorsService.Create");
                return new BasicResponse { Code = 500, Message = "Internal eroror creating new Collaborator" };
            }
        }

        public async Task<BasicResponse> Delete(int CollaboratorId)
        {
            try
            {
                var data = await ExistsCollaborator(CollaboratorId);
                if (data == null)
                {
                    return new BasicResponse { Code = 400, Message = "Collaborator dosen't exists" };
                }
                context.Collaborators.Remove(data);
                await context.SaveChangesAsync();

                return new BasicResponse { Code = 200, Message = "Delete Success" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.CollaboratorsService.Delete");
                return new BasicResponse { Code = 500, Message = "Internal eroror deleting" };
            }
        }

        public async Task<List<Collaborators>> GetList()
        {
            try
            {
                return await context.Collaborators.Include(l => l.Level).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.CollaboratorsService.GetList");
                return null;
            }
        }

        public async Task<Collaborators> Get(int CollaboratorId)
        {
            try
            {
                return await context.Collaborators  //Colaborador
                    .Include(l => l.Level)  //Descripcion del nivel de expertis
                    .Include(t => t.CollaboratorsTechnologies)  //Lista de tecnologias
                    .ThenInclude(tch => tch.Technology) //Obtener los detalles de las tecnologias
                    .FirstOrDefaultAsync(c => c.CollaboratorId == CollaboratorId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.CollaboratorsService.Get");
                return null;
            }
        }

        public async Task<BasicResponse> Update(Collaborators obj)
        {
            try
            {
                var data = await ExistsCollaborator(obj.CollaboratorId);
                if (data == null)
                {
                    return new BasicResponse { Code = 400, Message = "Collaborator dosen't exists" };
                }
                data.Levelid = obj.Levelid;
                data.FullName = obj.FullName;
                data.IsActive = obj.IsActive;
                data.TimeZone = obj.TimeZone;
                data.CollaboratorsTechnologies = obj.CollaboratorsTechnologies;

                context.Entry(data).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return new BasicResponse { Code = 200, Message = "Update Success" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.CollaboratorsService.Update");
                return new BasicResponse { Code = 500, Message = "Internal eroror updating info" };
            }
        }

        private async Task<Collaborators> ExistsCollaborator(int CollaboratorId)
        {
            return await context.Collaborators.FirstOrDefaultAsync(c => c.CollaboratorId == CollaboratorId);
        }


    }
}
