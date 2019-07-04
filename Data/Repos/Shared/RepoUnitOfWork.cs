using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenSongWeb.Data.Repos
{
    /// See https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    /// This is placed outside any repo because all repos should not call SaveChanges.
    /// So managers should use this helper to commit changes performed in the repositories,
    /// Which is a more testable pattern.
    /// </summary>
    public class RepoUnitOfWork : IRepoUnitOfWork
    {
        private readonly Data.SongDbContext _context;

        public RepoUnitOfWork(Data.SongDbContext dbContext)
        {
            _context = dbContext;
        }
    
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
