using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMapper;
using BoxOffice.Api.Bootstrapping.Modules;
using BoxOffice.DAL;
using BoxOffice.DAL.Interfaces;
using System.Web.Http;
using System.Web.Mvc;
using DataContracts = BoxOffice.Api.DataContracts;
using DataLayer = BoxOffice.Model;

namespace BoxOffice.Api
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new BoxOfficeProfile()));
        }
    }

    public class BoxOfficeProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DataContracts.Theatre, DataLayer.Theatres>().ReverseMap();
            Mapper.CreateMap<DataContracts.Screen, DataLayer.Screen>().ReverseMap();
            Mapper.CreateMap<DataContracts.Screenclasses, DataLayer.Screenclasses>().ReverseMap();
            Mapper.CreateMap<DataContracts.Movie, DataLayer.Movie>().ReverseMap();
            Mapper.CreateMap<DataContracts.MovieTimings, DataLayer.MovieTimings>().ReverseMap();
            Mapper.CreateMap<DataContracts.Seat, DataLayer.Seat>().ReverseMap();
        }
    }
}