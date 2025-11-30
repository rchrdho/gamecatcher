using System.Security.Claims;
using Bunit;
using GameCatcher.Components.Review;
using GameCatcher.DatabaseService;
using GameCatcher.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace GameCatcher.Tests;

[Obsolete]
public class ReviewFormTests : TestContext
{
    [Fact]
    public void ReviewForm_ShouldRenderCorrectly_AndSetDefaultRating()
    {
        // Arrange
        Services.AddAuthorization();
        Services.AddScoped<AuthenticationStateProvider, TestAuthStateProvider>();
        var mockReviewService = new Mock<IReviewService>();
        Services.AddSingleton(mockReviewService.Object);

        // Act
        var cut = Render<ReviewForm>(parameters => parameters.Add(p => p.GameId, 12345));

        // Assert
        // Verify default rating logic in OnInitialized
        cut.Find("span.text-muted")
            .MarkupMatches("<span class=\"ms-2 text-muted\">(0 / 5)</span>");
        // Verify button exists
        cut.Find("button[type='submit']");
    }

    [Fact]
    public void ClickingStar_ShouldUpdateRating()
    {
        // Arrange
        Services.AddAuthorization();
        Services.AddScoped<AuthenticationStateProvider, TestAuthStateProvider>();
        var mockReviewService = new Mock<IReviewService>();
        Services.AddSingleton(mockReviewService.Object);

        var cut = Render<ReviewForm>(parameters => parameters.Add(p => p.GameId, 1));

        // Act - Click the 3rd star button
        // The loop in razor generates buttons. We find the buttons and click the 3rd one (index 2).
        var starButtons = cut.FindAll("button.p-0");
        starButtons[2].Click(); // Click star 3

        // Assert
        cut.Find("span.text-muted")
            .MarkupMatches("<span class=\"ms-2 text-muted\">(3 / 5)</span>");
    }

    [Fact]
    public async Task SubmittingForm_ShouldCallService_AndShowSuccessMessage()
    {
        // Arrange
        Services.AddAuthorization();

        // Mock Auth Provider to return a specific user
        var authMock = new Mock<AuthenticationStateProvider>();
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.NameIdentifier, "test-user-id") },
                "mock"
            )
        );
        authMock
            .Setup(a => a.GetAuthenticationStateAsync())
            .ReturnsAsync(new AuthenticationState(user));
        Services.AddSingleton(authMock.Object);

        // Mock Review Service
        var mockReviewService = new Mock<IReviewService>();
        mockReviewService
            .Setup(s => s.AddReview(It.IsAny<Review>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        Services.AddSingleton(mockReviewService.Object);

        var cut = Render<ReviewForm>(parameters => parameters.Add(p => p.GameId, 99));

        // Act
        // 1. Fill out the text area
        cut.Find("textarea").Change("This is a test review.");

        // 2. Click submit
        cut.Find("button[type='submit']").Click();

        // Assert
        // Verify the service was called with the correct data
        mockReviewService.Verify(
            s =>
                s.AddReview(
                    It.Is<Review>(r =>
                        r.GameId == 99
                        && r.UserId == "test-user-id"
                        && r.Comment == "This is a test review."
                    )
                ),
            Times.Once
        );

        // Verify success message appears
        cut.WaitForState(() => cut.FindAll(".alert-success").Count > 0);
        var alert = cut.Find(".alert-success");
        Assert.Contains("Your review has been submitted successfully!", alert.TextContent);
    }
}

// helper for simple unauthenticated state if needed,
// though the test above mocks the provider directly with Moq for more control.
public class TestAuthStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.NameIdentifier, "TestUser") },
            "Test authentication type"
        );

        var user = new ClaimsPrincipal(identity);
        return Task.FromResult(new AuthenticationState(user));
    }
}
