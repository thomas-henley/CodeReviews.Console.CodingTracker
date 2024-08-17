using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace CodingTracker;

public class DapperHelper
{
    private readonly SqliteConnection _connection;
    private readonly IConfiguration _config;
    private readonly string _table;

    public DapperHelper(IConfiguration config)
    {
        _config = config;
        _table = config["TableName"] ?? "CodingSessions";
        _connection = new SqliteConnection(_config.GetConnectionString("SQLite"));
    }

    public void InitializeDb()
    {
        var sql = $@"CREATE TABLE IF NOT EXISTS {_table} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT,
                        EndTime TEXT,
                        Duration INTEGER
                    );";

        _connection.Execute(sql);
    }

    public bool IsTableCreated()
    {
        var sql = $@"SELECT COUNT() FROM (
                        SELECT name FROM sqlite_master 
                        WHERE type='table' AND name=@TableName)";

        var parameters = new { TableName = _table };

        var res = _connection.ExecuteScalar<int>(sql, parameters);
        return res > 0;
    }

    public void TeardownDB()
    {
        var sql = $@"DROP TABLE {_table}";
        _connection.Execute(sql);
    }

    // CREATE
    public int Insert(CodingSession sesh)
    {
        var sql = $"INSERT INTO {_table} (StartTime, EndTime, Duration) values (@StartTime, @EndTime, @Duration);";
        var parameters = new { sesh.StartTime, sesh.EndTime, sesh.Duration };
        var affectedRows = _connection.Execute(sql, parameters);
        return affectedRows;
    }

    // RETRIEVE
    public bool TryGetSession(int id, out CodingSession session)
    {
        var sql = $"SELECT * FROM {_table} WHERE id = @Id";
        var parameters = new { Id = id };

        session = _connection.QueryFirstOrDefault<CodingSession>(sql, parameters)!;
        
        return session is not null;
    }

    public List<CodingSession> GetAllSessions()
    {
        var sql = $"SELECT * FROM {_table}";

        return _connection.Query<CodingSession>(sql).ToList();
    }

    // UPDATE
    public bool UpdateSession(CodingSession session)
    {
        if (TryGetSession(session.Id, out var _))
        {
            session.SaveDuration();
            var sql = $@"UPDATE {_table} 
                         SET StartTime = @StartTime,
                             EndTime = @EndTime,
                             Duration = @Duration
                         WHERE Id = @Id;";
            var parameters = new {session.StartTime, session.EndTime, session.Duration, session.Id};
            int rowsAffected = _connection.Execute(sql, parameters);
            return rowsAffected == 1;
        }
        return false;
    }

    // DELETE
    public bool DeleteSessionById(int id)
    {
        var sql = $"DELETE FROM {_table} WHERE Id = @Id;";
        var parameters = new { Id = id };
        return _connection.Execute(sql, parameters) == 1;
    }
}
