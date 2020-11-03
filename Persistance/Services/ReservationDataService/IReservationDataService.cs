using Domain.Entities;
using Persistance.Services.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.Services.ReservationDataService
{
    public interface IReservationDataService : IDataService<Reservation>
    {
    }
}
