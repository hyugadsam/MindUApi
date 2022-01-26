using DBService.Entities;
using DBService.Interfaces;
using Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Linq;

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

        public async Task<BasicCreateResponse> Create(Collaborators obj)
        {
            try
            {
                var validations = await ValidateCollaboratorsReferences(obj);
                if (validations.Code != 200)
                    return new BasicCreateResponse { Code = validations.Code, Message = validations.Message };

                context.Collaborators.Add(obj);
                await context.SaveChangesAsync();
                return new BasicCreateResponse { Code = 200, Message = "Create Success", Id = obj.CollaboratorId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.CollaboratorsService.Create");
                return new BasicCreateResponse { Code = 500, Message = "Internal eroror creating new Collaborator" };
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
                var validations = await ValidateCollaboratorsReferences(obj);
                if (validations.Code != 200)
                    return validations;

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

        public async Task<BasicResponse> DeactivateCollaborator(int CollaboratorId)
        {
            try
            {
                var data = await ExistsCollaborator(CollaboratorId);
                if (data == null)
                {
                    return new BasicResponse { Code = 400, Message = "Collaborator dosen't exists" };
                }
                data.IsActive = false;
                context.Entry(data).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return new BasicResponse { Code = 200, Message = "Deactivate Success" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.CollaboratorsService.DeactivateCollaborator");
                return new BasicResponse { Code = 500, Message = "Internal eroror updating info" };
            }
        }

        public async Task<BasicResponse> GraduateCollaborator(int CollaboratorId)
        {
            try
            {
                var data = await ExistsCollaborator(CollaboratorId);
                string message = string.Empty;
                if (data == null || data?.IsActive.GetValueOrDefault() == false)
                {
                    message = "Collaborator dosen't exists or is inactive";
                }
                else if (data?.IsGraduated == true)
                {
                    message += "Collaborator dosen't exists or is already graduated";
                }

                if (message.Length > 0)
                {
                    return new BasicResponse { Code = 400, Message = message };
                }

                data.IsGraduated = true;
                context.Entry(data).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return new BasicResponse { Code = 200, Message = "Graduated Success" };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.CollaboratorsService.GraduateCollaborator");
                return new BasicResponse { Code = 500, Message = "Internal eroror updating info" };
            }
        }

        private async Task<Collaborators> ExistsCollaborator(int CollaboratorId)
        {
            return await context.Collaborators.FirstOrDefaultAsync(c => c.CollaboratorId == CollaboratorId);
        }
        
        private async Task<BasicResponse> ValidateCollaboratorsReferences(Collaborators obj)
        {
            string message = string.Empty;

            var ExistLevel = await context.Levels.Where(l => l.LevelId == obj.Levelid).AnyAsync();
            message += ExistLevel ? string.Empty : $"El Levelid {obj.Levelid} no existe; ";

            if (obj.CollaboratorsTechnologies.Count > 0)
            {
                var techlist = await context.Technologies.ToListAsync();
                foreach (var item in obj.CollaboratorsTechnologies)
                {
                    var exists = techlist.Where(t => t.TechnologyId == item.TechnologyId).Any();
                    message += exists ? string.Empty : $"El TechnologyId {item.TechnologyId} no existe; ";
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                return new BasicResponse
                {
                    Code = 400,
                    Message = message
                };
            }

            return new BasicResponse
            {
                Code = 200,
                Message = "OK"
            };
        }


    }
}
