using Microsoft.Data.SqlClient;
using System.Text.Json;

// Reads connection string from web project's appsettings.json
// Try a few strategies to locate the web project's appsettings.json
string appSettingsPath = Path.Combine("..", "..", "School Blog project", "appsettings.json");
if (!File.Exists(appSettingsPath))
{
    // Walk up from current dir looking for the folder
    var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
    FileInfo? found = null;
    while (dir != null)
    {
        var candidate = Path.Combine(dir.FullName, "School Blog project", "appsettings.json");
        if (File.Exists(candidate))
        {
            found = new FileInfo(candidate);
            break;
        }
        dir = dir.Parent;
    }
    if (found is null)
    {
        Console.Error.WriteLine($"appsettings.json not found (looked at {appSettingsPath} and parent folders)");
        return 1;
    }
    appSettingsPath = found.FullName;
}

var json = JsonDocument.Parse(File.ReadAllText(appSettingsPath));
if (!json.RootElement.TryGetProperty("ConnectionStrings", out var cs) || !cs.TryGetProperty("DefaultConnection", out var connElem))
{
    Console.Error.WriteLine("DefaultConnection not found in appsettings.json");
    return 1;
}

string connectionString = connElem.GetString() ?? throw new Exception("Connection string empty");

string targetUserId = "c455b6ba-8259-4307-a7bb-a5a6ee5e6ac6"; // jburns3222

using var conn = new SqlConnection(connectionString);
await conn.OpenAsync();

// Get role ids
string adminRoleId = null;
string userRoleId = null;
using (var cmd = conn.CreateCommand())
{
    cmd.CommandText = "SELECT Id, Name FROM AspNetRoles WHERE Name IN ('Admin','User')";
    using var rdr = await cmd.ExecuteReaderAsync();
    while (await rdr.ReadAsync())
    {
        var id = rdr.GetString(0);
        var name = rdr.GetString(1);
        if (name == "Admin") adminRoleId = id;
        if (name == "User") userRoleId = id;
    }
}

if (adminRoleId is null)
{
    Console.WriteLine("Admin role not found. Creating it.");
    using var c2 = conn.CreateCommand();
    c2.CommandText = "INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) VALUES (NEWID(), 'Admin', 'ADMIN', NEWID()); SELECT Id FROM AspNetRoles WHERE Name='Admin'";
    adminRoleId = (string)await c2.ExecuteScalarAsync();
}

if (userRoleId is null)
{
    Console.WriteLine("User role not found. Creating it.");
    using var c3 = conn.CreateCommand();
    c3.CommandText = "INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) VALUES (NEWID(), 'User', 'USER', NEWID()); SELECT Id FROM AspNetRoles WHERE Name='User'";
    userRoleId = (string)await c3.ExecuteScalarAsync();
}

// Remove Admin role mapping for target user
using (var rem = conn.CreateCommand())
{
    rem.CommandText = "DELETE FROM AspNetUserRoles WHERE UserId = @uid AND RoleId = @rid";
    rem.Parameters.AddWithValue("@uid", targetUserId);
    rem.Parameters.AddWithValue("@rid", adminRoleId);
    int rows = await rem.ExecuteNonQueryAsync();
    Console.WriteLine($"Removed {rows} admin role mappings for user {targetUserId}.");
}

// Ensure User role mapping exists
using (var ins = conn.CreateCommand())
{
    ins.CommandText = "IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId=@uid AND RoleId=@urid) INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@uid, @urid)";
    ins.Parameters.AddWithValue("@uid", targetUserId);
    ins.Parameters.AddWithValue("@urid", userRoleId);
    int rows = await ins.ExecuteNonQueryAsync();
    Console.WriteLine($"Ensured User role mapping for user {targetUserId} (rows affected: {rows}).");
}

Console.WriteLine("Done.");
// Verify current roles for the user
using (var q = conn.CreateCommand())
{
    q.CommandText = "SELECT r.Name FROM AspNetUserRoles ur JOIN AspNetRoles r ON ur.RoleId = r.Id WHERE ur.UserId = @uid";
    q.Parameters.AddWithValue("@uid", targetUserId);
    using var rdr = await q.ExecuteReaderAsync();
    var list = new System.Collections.Generic.List<string>();
    while (await rdr.ReadAsync())
    {
        list.Add(rdr.GetString(0));
    }
    Console.WriteLine($"Current roles for {targetUserId}: {string.Join(',', list)}");
}

return 0;
