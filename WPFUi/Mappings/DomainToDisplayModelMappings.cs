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

            CreateMap<Reservation, AppointmentViewReservationModel>()
                .ForMember(model =>
                    model.Time, x => x.MapFrom(y => $"{ y.Hour } - { y.Hour + y.Schedule.MaxTimePerPatient }"));

            CreateMap<Schedule, AppointmentViewScheduleModel>()
                .ForMember(model =>
                    model.DoctorFullName, x => x.MapFrom(y => $"{ y.Doctor.LastName} { y.Doctor.FirstName }"));
          
        }
    }
}
