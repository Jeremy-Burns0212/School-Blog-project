using Microsoft.Data.SqlClient;
using System.Text.Json;

// Reads connection string from web project's appsettings.json
string appSettingsPath = Path.Combine("..", "..", "School Blog project", "appsettings.json");
if (!File.Exists(appSettingsPath))
{
	DirectoryInfo? dir = new(Directory.GetCurrentDirectory());
	FileInfo? found = null;
	while (dir != null)
	{
		string candidate = Path.Combine(dir.FullName, "School Blog project", "appsettings.json");
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

JsonDocument json = JsonDocument.Parse(File.ReadAllText(appSettingsPath));
if (!json.RootElement.TryGetProperty("ConnectionStrings", out JsonElement cs) || !cs.TryGetProperty("DefaultConnection", out JsonElement connElem))
{
	Console.Error.WriteLine("DefaultConnection not found in appsettings.json");
	return 1;
}

string connectionString = connElem.GetString() ?? throw new InvalidOperationException("Connection string empty");

string targetUserId = "c455b6ba-8259-4307-a7bb-a5a6ee5e6ac6"; // jburns3222

using SqlConnection conn = new(connectionString);
await conn.OpenAsync();

string? adminRoleId = null;
using (SqlCommand cmd = conn.CreateCommand())
{
	cmd.CommandText = "SELECT Id FROM AspNetRoles WHERE Name = 'Admin'";
	object? o = await cmd.ExecuteScalarAsync();
	adminRoleId = o as string;
}

if (adminRoleId is null)
{
	using SqlCommand c2 = conn.CreateCommand();
	c2.CommandText = "INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) VALUES (NEWID(), 'Admin', 'ADMIN', NEWID()); SELECT Id FROM AspNetRoles WHERE Name='Admin'";
	adminRoleId = (string?)await c2.ExecuteScalarAsync() ?? throw new InvalidOperationException("Failed to create or retrieve Admin role");
}

using (SqlCommand ins = conn.CreateCommand())
{
	ins.CommandText = "IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId=@uid AND RoleId=@rid) INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@uid, @rid)";
	_ = ins.Parameters.AddWithValue("@uid", targetUserId);
	_ = ins.Parameters.AddWithValue("@rid", adminRoleId);
	int rows = await ins.ExecuteNonQueryAsync();
	Console.WriteLine($"Ensured Admin role mapping for user {targetUserId} (rows affected: {rows}).");
}

// Verify current roles for the user
using SqlCommand q = conn.CreateCommand();
q.CommandText = "SELECT r.Name FROM AspNetUserRoles ur JOIN AspNetRoles r ON ur.RoleId = r.Id WHERE ur.UserId = @uid";
_ = q.Parameters.AddWithValue("@uid", targetUserId);
using SqlDataReader rdr = await q.ExecuteReaderAsync();
List<string> list = [];
while (await rdr.ReadAsync())
{
	list.Add(rdr.GetString(0));
}
Console.WriteLine($"Current roles for {targetUserId}: {string.Join(',', list)}");

return 0;
