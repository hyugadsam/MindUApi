using AutoMapper;
using DBService.Entities;
using Dtos.Dtos;
using Dtos.Request;
using Dtos.Responses;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ApplicationServices.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            #region RolesMaps
            CreateMap<Roles, RoleDto>();
            #endregion

            #region UserMaps
            CreateMap<Users, UserDto>();
            CreateMap<NewBasicUserRequest, NewUserRequest>();
            CreateMap<NewUserRequest, Users>();
            CreateMap<UpdateUserRequest, Users>();
            CreateMap<DBService.Models.LoginResponse, UserValidationResponse>();
            #endregion

            #region CollaboratorsMaps
            CreateMap<NewCollaboratorRequest, Collaborators>().ForMember(
                collaborator => collaborator.CollaboratorsTechnologies,
                member =>  member.MapFrom(MapCollaboratorTechnologiesRequest)
            );

            CreateMap<Collaborators, CollaboratorDto>()
                .ForMember(
                destino => destino.Level,
                option => option.MapFrom(r => r.Level.Description));

            CreateMap<Collaborators, CollaboratorFullDto>()
                .ForMember(
                destino => destino.Level,
                option => option.MapFrom(r => r.Level.Description))
                .ForMember(
                destino => destino.Technologies,
                option => option.MapFrom(MapCollaboratorTechnologiesDescription));

            CreateMap<UpdateCollaboratorRequest, Collaborators>().ForMember(
                collaborator => collaborator.CollaboratorsTechnologies,
                member => member.MapFrom(MapCollaboratorTechnologiesUpdate)
            );

            #endregion

            #region TechnologiesMaps
            CreateMap<TechnologyDto, Technologies>();
            CreateMap<Technologies, TechnologyDto>();
            #endregion

            #region LevelsMaps
            CreateMap<LevelDto, Levels>();
            CreateMap<Levels, LevelDto>();
            #endregion

        }

        #region Methods
        private List<CollaboratorsTechnologies> MapCollaboratorTechnologiesUpdate(UpdateCollaboratorRequest request, Collaborators collaborators)
        {
            var result = new List<CollaboratorsTechnologies>();
            if (request.Technologies?.Count == 0)
                return result;

            foreach (var item in request.Technologies)
            {
                string xmlCertificates = item.Certificates?.Count > 0 ?
                    new XElement("Certificates", item.Certificates.Select(i => new XElement("Certificate", i))).ToString()
                    : string.Empty;

                string xmlProyectsUrl = item.Certificates?.Count > 0 ?
                    new XElement("ProyectsUrl", item.ProyectsUrl.Select(i => new XElement("ProyectUrl", i))).ToString()
                    : string.Empty;

                result.Add(new CollaboratorsTechnologies
                {
                    Certificates = xmlCertificates,
                    ProyectsUrl = xmlProyectsUrl,
                    TechnologyId = item.TechnologyId,
                    YearsExperience = item.YearsExperience
                });
            }

            return result;
        }
        private List<CollaboratorTechnologiesDescriptionDto> MapCollaboratorTechnologiesDescription(Collaborators collaborators, CollaboratorFullDto dto)
        {
            var result = new List<CollaboratorTechnologiesDescriptionDto>();
            if (collaborators.CollaboratorsTechnologies?.Count == 0)
                return result;

            foreach (var item in collaborators.CollaboratorsTechnologies)
            {
                List<string> xmlCertificates = new List<string>();
                List<string> xmlProyectsUrl = new List<string>();

                if (!string.IsNullOrEmpty(item.Certificates))
                {
                    using (var stream = GenerateStreamFromString(item.Certificates))
                    {
                        XDocument xmlDoc = XDocument.Load(stream);
                        xmlCertificates = xmlDoc.Root.Elements("Certificate")
                           .Select(element => element.Value)
                           .ToList();
                    }
                }

                if (!string.IsNullOrEmpty(item.ProyectsUrl))
                {
                    using (var stream = GenerateStreamFromString(item.ProyectsUrl))
                    {
                        XDocument xmlDoc = XDocument.Load(stream);
                        xmlProyectsUrl = xmlDoc.Root.Elements("ProyectUrl")
                           .Select(element => element.Value)
                           .ToList();
                    }

                }

                result.Add(new CollaboratorTechnologiesDescriptionDto
                {
                    Certificates = xmlCertificates,
                    ProyectsUrl = xmlProyectsUrl,
                    TechnologyId = item.TechnologyId,
                    YearsExperience = item.YearsExperience,
                    Technology = item.Technology.Description
                });
            }

            return result;
        }

        private List<CollaboratorsTechnologies> MapCollaboratorTechnologiesRequest(NewCollaboratorRequest request, Collaborators collaborators)
        {
            var result = new List<CollaboratorsTechnologies>();
            if (request.Technologies?.Count == 0)
                return result;

            foreach (var item in request.Technologies)
            {
                string xmlCertificates = item.Certificates?.Count > 0 ?
                    new XElement("Certificates", item.Certificates.Select(i => new XElement("Certificate", i))).ToString()
                    : string.Empty;

                string xmlProyectsUrl = item.Certificates?.Count > 0 ?
                    new XElement("ProyectsUrl", item.ProyectsUrl.Select(i => new XElement("ProyectUrl", i))).ToString()
                    : string.Empty;

                result.Add(new CollaboratorsTechnologies
                {
                    Certificates = xmlCertificates,
                    ProyectsUrl = xmlProyectsUrl,
                    TechnologyId = item.TechnologyId,
                    YearsExperience = item.YearsExperience
                });
            }

            return result;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        #endregion


    }
}
