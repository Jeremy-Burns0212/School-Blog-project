using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace SchoolBlogProject.Tests
{
	public class AdminPanelTests
	{
		[Fact]
		public void ModelStateCleanupRemovesNonRoleFormKeys()
		{
			ModelStateDictionary ms = new();
			ms.SetModelValue("SettingsForm.SchoolName", new StringValues("X"), null);
			ms.SetModelValue("NewLink.Url", new StringValues("http://a"), null);
			ms.SetModelValue("RoleForm.UserId", new StringValues("u"), null);
			ms.SetModelValue("RoleForm.TargetRole", new StringValues("Writer"), null);

			string keysToKeepPrefix = "RoleForm.";
			List<string> keysToRemove = [.. ms.Keys.Where(k => !k.StartsWith(keysToKeepPrefix, StringComparison.Ordinal))];
			foreach (string? k in keysToRemove)
			{
				_ = ms.Remove(k);
			}

			Assert.True(ms.ContainsKey("RoleForm.UserId"));
			Assert.True(ms.ContainsKey("RoleForm.TargetRole"));
			Assert.False(ms.ContainsKey("SettingsForm.SchoolName"));
			Assert.False(ms.ContainsKey("NewLink.Url"));
		}
	}
}
