using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenSongWeb.Data.Repos
{
    /// See https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    public interface IRepoUnitOfWork
    {
        /// <summary>
        /// Commit changes performed in all repositories
        /// </summary>
        /// <returns>Task with a result containing the number of objects written to storage</returns>
        Task<int> SaveAsync();
    }
}
