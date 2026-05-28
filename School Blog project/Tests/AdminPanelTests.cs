using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace School_Blog_project.Tests
{
    public class AdminPanelTests
    {
        [Fact]
        public void ModelStateCleanup_RemovesNonRoleFormKeys()
        {
            var ms = new ModelStateDictionary();
            ms.SetModelValue("SettingsForm.SchoolName", new StringValues("X"), null);
            ms.SetModelValue("NewLink.Url", new StringValues("http://a"), null);
            ms.SetModelValue("RoleForm.UserId", new StringValues("u"), null);
            ms.SetModelValue("RoleForm.TargetRole", new StringValues("Writer"), null);

            var keysToKeepPrefix = "RoleForm.";
            var keysToRemove = ms.Keys.Where(k => !k.StartsWith(keysToKeepPrefix)).ToList();
            foreach (var k in keysToRemove) ms.Remove(k);

            Assert.True(ms.ContainsKey("RoleForm.UserId"));
            Assert.True(ms.ContainsKey("RoleForm.TargetRole"));
            Assert.False(ms.ContainsKey("SettingsForm.SchoolName"));
            Assert.False(ms.ContainsKey("NewLink.Url"));
        }
    }
}
