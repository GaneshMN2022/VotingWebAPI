using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voting.DB.Entities;

namespace Voting.DB {
    public interface IVotingDBContext {
        DbSet<CandidateEntity> Candidate { get; set; }
        DbSet<VotersEntity> Voters { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransaction();
        Task CommitTransaction();

    }
}
