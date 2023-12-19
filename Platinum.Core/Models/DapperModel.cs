using Dapper;
using System.Data.Common;

namespace Platinum.Core.Models
{
    public class DapperModel
    {
        public DbConnection DbConnection { get; set; }
        public CommandDefinition CommandDefinition { get; set; }
    }
}
