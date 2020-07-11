using APP.Repository.IRepo;
using System;
using System.Threading.Tasks;

namespace APP.Repository.EFRepo.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICountryRepository Countries { get; }
        IStateRepository States { get; }
        ILGARepository LGAs { get; }
        IProfileRepository Profiles { get; }
        Task<int> CompleteAsync();
    }
}