using APP.Repository.EFRepo.Repositories;
using APP.Repository.IRepo;
using System.Threading.Tasks;

namespace APP.Repository.EFRepo.UnitOfWork
{
    /// <summary>
    /// Unit of work base
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;

        public UnitOfWork(AppDBContext context)
        {
            _context = context;
            Countries = new CountryRepository(_context);
            States = new StateRepository(_context);
            LGAs = new LGARepository(_context);
            Profiles = new ProfileRepository(_context);
        }

        public ICountryRepository Countries { get; private set; }
        public IStateRepository States { get; private set; }
        public ILGARepository LGAs { get; private set; }
        public IProfileRepository Profiles { get; private set; }



        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}