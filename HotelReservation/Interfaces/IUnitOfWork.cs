using System;
using System.Threading.Tasks;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Reservation> Reservations { get; }
    Task<int> Complete();
}
