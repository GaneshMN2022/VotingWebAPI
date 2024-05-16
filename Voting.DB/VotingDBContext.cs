using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voting.DB.Entities;

namespace Voting.DB {
    public class VotingDBContext : DbContext, IVotingDBContext {
        public VotingDBContext(DbContextOptions<VotingDBContext> options) : base(options) {
        }

        public DbSet<CandidateEntity> Candidate { get; set; }

        public DbSet<VotersEntity> Voters { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task<IDbContextTransaction> BeginTransaction() {
            return await this.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction() {
            await this.Database.CommitTransactionAsync();
        }
    }
}
