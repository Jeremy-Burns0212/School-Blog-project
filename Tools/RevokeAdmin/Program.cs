using Microsoft.Data.SqlClient;
using System.Globalization;
using System.Text.Json;

// Reads connection string from web project's appsettings.json
// Try a few strategies to locate the web project's appsettings.json
string appSettingsPath = Path.Combine("..", "..", "School Blog project", "appsettings.json");
if (!File.Exists(appSettingsPath))
{
	// Walk up from current dir looking for the folder
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

// Get role ids
string? adminRoleId = null;
string? userRoleId = null;
using (SqlCommand cmd = conn.CreateCommand())
{
	cmd.CommandText = "SELECT Id, Name FROM AspNetRoles WHERE Name IN ('Admin','User')";
	using SqlDataReader rdr = await cmd.ExecuteReaderAsync();
	while (await rdr.ReadAsync())
	{
		string id = rdr.GetString(0);
		string name = rdr.GetString(1);
		if (name == "Admin")
		{
			adminRoleId = id;
		}

		if (name == "User")
		{
			userRoleId = id;
		}
	}
}

if (adminRoleId is null)
{
	Console.WriteLine("Admin role not found. Creating it.");
	using SqlCommand c2 = conn.CreateCommand();
	c2.CommandText = "INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) VALUES (NEWID(), 'Admin', 'ADMIN', NEWID()); SELECT Id FROM AspNetRoles WHERE Name='Admin'";
	object? adminObj = await c2.ExecuteScalarAsync();
	adminRoleId = Convert.ToString(adminObj, CultureInfo.InvariantCulture) ?? throw new InvalidOperationException("Failed to retrieve Admin role Id");
}

if (userRoleId is null)
{
	Console.WriteLine("User role not found. Creating it.");
	using SqlCommand c3 = conn.CreateCommand();
	c3.CommandText = "INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) VALUES (NEWID(), 'User', 'USER', NEWID()); SELECT Id FROM AspNetRoles WHERE Name='User'";
	object? userObj = await c3.ExecuteScalarAsync();
	userRoleId = Convert.ToString(userObj, CultureInfo.InvariantCulture) ?? throw new InvalidOperationException("Failed to retrieve User role Id");
}

// Remove Admin role mapping for target user
using (SqlCommand rem = conn.CreateCommand())
{
	rem.CommandText = "DELETE FROM AspNetUserRoles WHERE UserId = @uid AND RoleId = @rid";
	_ = rem.Parameters.AddWithValue("@uid", targetUserId);
	_ = rem.Parameters.AddWithValue("@rid", adminRoleId);
	int rows = await rem.ExecuteNonQueryAsync();
	Console.WriteLine($"Removed {rows} admin role mappings for user {targetUserId}.");
}

// Ensure User role mapping exists
using (SqlCommand ins = conn.CreateCommand())
{
	ins.CommandText = "IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId=@uid AND RoleId=@urid) INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@uid, @urid)";
	_ = ins.Parameters.AddWithValue("@uid", targetUserId);
	_ = ins.Parameters.AddWithValue("@urid", userRoleId);
	int rows = await ins.ExecuteNonQueryAsync();
	Console.WriteLine($"Ensured User role mapping for user {targetUserId} (rows affected: {rows}).");
}

Console.WriteLine("Done.");
// Verify current roles for the user
using (SqlCommand q = conn.CreateCommand())
{
	q.CommandText = "SELECT r.Name FROM AspNetUserRoles ur JOIN AspNetRoles r ON ur.RoleId = r.Id WHERE ur.UserId = @uid";
	_ = q.Parameters.AddWithValue("@uid", targetUserId);
	using SqlDataReader rdr = await q.ExecuteReaderAsync();
	List<string> list = [];
	while (await rdr.ReadAsync())
	{
		list.Add(rdr.GetString(0));
	}
	Console.WriteLine($"Current roles for {targetUserId}: {string.Join(',', list)}");
}

return 0;
