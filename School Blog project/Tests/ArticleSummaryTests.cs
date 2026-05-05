using School_Blog_project.Models;
using Xunit;

namespace School_Blog_project.Tests
{
	public class ArticleSummaryTests
	{
		[Fact]
		public void CanSetAndGetProperties()
		{
			ArticleSummary article = new()
			{
				Title = "Test Title",
				Date = new DateTime(2024, 1, 1),
				Description = "Test Description",
				ImageUrl = "http://example.com/image.jpg",
				Categories = ["News", "Events"]
			};

			Assert.Equal("Test Title", article.Title);
			Assert.Equal(new DateTime(2024, 1, 1), article.Date);
			Assert.Equal("Test Description", article.Description);
			Assert.Equal("http://example.com/image.jpg", article.ImageUrl);
			Assert.Equal(["News", "Events"], article.Categories);
		}

		[Fact]
		public void CategoriesListCanAddRemoveClear()
		{
			ArticleSummary article = new() { Title = "T", Date = DateTime.Now };
			article.Categories.Add("A");
			article.Categories.Add("B");
			Assert.Contains("A", article.Categories);
			Assert.Contains("B", article.Categories);
			_ = article.Categories.Remove("A");
			Assert.DoesNotContain("A", article.Categories);
			article.Categories.Clear();
			Assert.Empty(article.Categories);
		}

		[Fact]
		public void DescriptionAndImageUrlCanBeNull()
		{
			ArticleSummary article = new() { Title = "T", Date = DateTime.Now };
			Assert.Null(article.Description);
			Assert.Null(article.ImageUrl);
		}

		[Fact]
		public void TitleAndDateAreRequired(ArticleSummary article)
		{
			Assert.Null(article.Title);
			Assert.Equal(default, article.Date);
		}

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("Special!@#$%^&*()_+-=")]
		public void TitleAllowsEdgeCases(string title)
		{
			ArticleSummary article = new() { Title = title, Date = DateTime.Now };
			Assert.Equal(title, article.Title);
		}

		[Fact]
		public void CategoriesCanHandleLargeList()
		{
			ArticleSummary article = new() { Title = "T", Date = DateTime.Now };
			for (int i = 0; i < 1000; i++)
			{
				article.Categories.Add($"Cat{i}");
			}

			Assert.Equal(1000, article.Categories.Count);
		}
	}
}
