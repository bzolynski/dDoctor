using AutoMapper;
using Domain.Entities;
using WPFUi.Models;

namespace WPFUi.Mappings
{
    public class DomainToDisplayModelMappings : Profile
    {
        public DomainToDisplayModelMappings()
        {
            CreateMap<Patient, PatientDisplayModel>();

            CreateMap<Doctor, ManageScheduleDoctorModel>();
            CreateMap<Doctor, DoctorPickerModel>();
        }
    }
}
