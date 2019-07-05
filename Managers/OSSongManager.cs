using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenSongWeb.Data;
using Microsoft.Extensions.Logging;
using OpenSongWeb.Data.Repos;
using OpenSongWeb.Models;

namespace OpenSongWeb.Managers
{
    public class OSSongManager : IOSSongManager
    {
        private readonly ILogger _logger;
        private IOSSongRepo _OSSongRepo;
        private IRepoUnitOfWork _repoUnitOfWork;

        public OSSongManager(ILogger<OSSongManager> logger, IOSSongRepo OSSongRepo, IRepoUnitOfWork repoUnitOfWork)
        {
            _logger = logger;
            _OSSongRepo = OSSongRepo;
            _repoUnitOfWork = repoUnitOfWork;
        }

        public async Task<Either<OSSong, ErrorInfo>> Get(int Id)
        {
            var song = await _OSSongRepo.Get(Id);

            return song;
        }


        public async Task<bool> Exists(int id)
        {
            return await _OSSongRepo.Exists(id);
        }
        public async Task<Either<PaginatedList<SongBrief>, ErrorInfo>> Page(SongFilterParameter queryParameter, PagingParameter pagingParameter)
        {
            return await _OSSongRepo.Page(queryParameter, pagingParameter);
        }

        public async Task<Either<List<SongBrief>, ErrorInfo>> All(SongFilterParameter queryParameter)
        {
            var results = await _OSSongRepo.All(queryParameter);
            return results.Select(s => new SongBrief(s)).ToList();
        }

    }
}
