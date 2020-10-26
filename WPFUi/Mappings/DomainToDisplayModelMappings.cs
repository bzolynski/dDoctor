using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using WPFUi.Models;

namespace WPFUi.Mappings
{
    public class DomainToDisplayModelMappings : Profile
    {
        public DomainToDisplayModelMappings()
        {
            CreateMap<Patient, PatientDisplayModel>();
        }
    }
}
