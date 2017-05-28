using System;
using System.Threading;
using System.Threading.Tasks;
using SampleTier.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using SampleTier.DataAccess.Exceptions;
using SampleTier.API.Models;

namespace SampleTier.DataAccess.Uow
{
    public  class UnitOfWorkBase : IUnitOfWorkBase
    {
        protected  AngularContext _AngularContext;
        private IRepository<Staff> _StaffRepository;


        public UnitOfWorkBase(AngularContext AngContext)
        {
            _AngularContext = AngContext;
        }

        public IRepository<Staff> StaffRepository
        {
            get
            {
                return _StaffRepository = _StaffRepository ?? new GenericEntityRepository<Staff>(_AngularContext);
            }
        }



        public void Save()
        {
            _AngularContext.SaveChanges();
        }
        #region IDisposable Implementation

        protected bool _isDisposed;

        protected void CheckDisposed()
        {
            if (_isDisposed) throw new ObjectDisposedException("The UnitOfWork is already disposed and cannot be used anymore.");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (_AngularContext != null)
                    {
                        _AngularContext.Dispose();
                        _AngularContext = null;
                    }
                }
            }
            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWorkBase()
        {
            Dispose(false);
        }

        #endregion
    }
}