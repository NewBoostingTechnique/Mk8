using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.Core.ProofTypes;
using MySql.Data.MySqlClient;
using System.Collections.Immutable;
using System.Data;
using System.Data.Common;

namespace Mk8.MySql.ProofTypes;

internal class MySqlProofTypeData(IOptions<Mk8Settings> mk8Options) : IProofTypeData
{
    public async Task<IImmutableList<ProofType>> ListAsync()
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("ProofTypeList", connection);
        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync().ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
        ImmutableList<ProofType>.Builder builder = ImmutableList.CreateBuilder<ProofType>();
        while (await reader.ReadAsync())
        {
            builder.Add(new ProofType
            {
                Description = reader.GetString(0)
            });
        }
        return builder.ToImmutable();
    }

    public async Task<bool> ExistsAsync(string description)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("ProofTypeExists", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("ProofTypeDescription", description);

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteBoolAsync().ConfigureAwait(false);
    }

    public async Task<Ulid?> IdentifyAsync(string description)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("ProofTypeIdentify", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("ProofTypeDescription", description);

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteUlidAsync().ConfigureAwait(false);
    }

    public async Task InsertAsync(ProofType proofType)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("ProofTypeInsert", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", proofType.Id);
        command.AddParameter("Description", proofType.Description);

        await connection.OpenAsync().ConfigureAwait(false);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }
}
