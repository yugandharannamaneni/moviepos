using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using AutoMapper;
using BoxOffice.DAL;
using BoxOffice.DAL.Interfaces;

namespace BoxOffice.Api.Bootstrapping.Modules
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.Contains("BoxOffice"))
                {
                    builder.RegisterAssemblyTypes(assembly)
                   .AsImplementedInterfaces();
                }
            }
            base.Load(builder);
        }
    }
}