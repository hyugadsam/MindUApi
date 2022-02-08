using AutoMapper;
using DBService.Entities;
using Dtos.Dtos;
using Dtos.Models;
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

            #region ApiMaps
            CreateMap<Feature, CityInfoDto>()
                .ForMember(destino => destino.Name, option => option.MapFrom(x => string.IsNullOrEmpty(x.place_name_es) ? x.place_name : x.place_name_es))
                .ForMember(destino => destino.Longitude, option => option.MapFrom(y => y.geometry.coordinates[0]))
                .ForMember(destino => destino.Latitude, option => option.MapFrom(z => z.geometry.coordinates[1]))
                .ForMember(destino => destino.PlaceId, option => option.MapFrom(a => a.id ));

            CreateMap<MapBoxResponse, CityInfoDto>()
                .ForMember(destino => destino.Name, option => option.MapFrom(x => string.IsNullOrEmpty(x.features[0].place_name_es) ? x.features[0].place_name : x.features[0].place_name_es))
                .ForMember(destino => destino.Longitude, option => option.MapFrom(y => y.features[0].geometry.coordinates[0]))
                .ForMember(destino => destino.Latitude, option => option.MapFrom(z => z.features[0].geometry.coordinates[1]));

            CreateMap<OpenWatherResponse, WeatherInfoDto>()
                .ForMember(destino => destino.Weather, opt => opt.MapFrom(x => x.current.weather[0].description))
                .ForMember(destino => destino.Currtemp, opt => opt.MapFrom(y => KelvinToCelcius(y.current.temp)))
                .ForMember(destino => destino.Maxtemp, opt => opt.MapFrom(z => KelvinToCelcius(z.daily[0].temp.max)))
                .ForMember(destino => destino.Mintemp, opt => opt.MapFrom(a => KelvinToCelcius(a.daily[0].temp.min)));
            #endregion

        }

        #region Methods
        private string KelvinToCelcius(double value)
        {
            return (value - 273.15).ToString("N2");
        }

        private List<CollaboratorsTechnologies> MapCollaboratorTechnologiesUpdate(UpdateCollaboratorRequest request, Collaborators collaborators)
        {
            var result = new List<CollaboratorsTechnologies>();
            if (request.Technologies == null || request.Technologies?.Count == 0)
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
            if (request.Technologies == null || request.Technologies?.Count == 0)
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
