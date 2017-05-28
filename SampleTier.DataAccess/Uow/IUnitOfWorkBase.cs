using SampleTier.API.Models;
using SampleTier.DataAccess.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace SampleTier.DataAccess.Uow
{
    public interface IUnitOfWorkBase : IDisposable
    {
        IRepository<Staff> StaffRepository { get; }
       
        void Save();

    }
}