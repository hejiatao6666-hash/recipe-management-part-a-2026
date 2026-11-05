using System.Collections.Generic;
using RecipeManagement.Core;

namespace RecipeManagement.Tests;

public sealed class RecipeManagerTests
{
    [Fact]
    public void Constructor_RejectsNullCollection()
    {
        Assert.Throws<ArgumentNullException>(() => new RecipeManager(null!));
    }

    [Fact]
    public void Constructor_AcceptsEmptyCatalogue()
    {
        IRecipeManager manager = new RecipeManager(Array.Empty<Recipe>());

        Assert.Equal(0, manager.RecipeCount);
        Assert.Equal(0, manager.ShoppingItemCount);
        Assert.Equal(0, manager.CookingPlanCount);
        Assert.Equal(0, manager.RemovedRecipeCount);
        Assert.Equal(0, manager.PendingInstructionCount);
    }

    [Theory]
    [InlineData(0, "Recipe")]
    [InlineData(-1, "Recipe")]
    [InlineData(1, "")]
    [InlineData(1, " ")]
    [InlineData(1, null)]
    public void Constructor_RejectsInvalidRecipe(int recipeId, string? title)
    {
        var recipes = new[] { new Recipe { Id = recipeId, Title = title! } };

        Assert.Throws<ArgumentException>(() => new RecipeManager(recipes));
    }

    [Fact]
    public void Constructor_RejectsDuplicateIds()
    {
        var recipes = new[]
        {
            new Recipe { Id = 10, Title = "First recipe" },
            new Recipe { Id = 10, Title = "Duplicate recipe" }
        };

        Assert.Throws<ArgumentException>(() => new RecipeManager(recipes));
    }

    [Fact]
    public void AddRecipe_RejectsNull()
    {
        var manager = CreateManager();

        Assert.Throws<ArgumentNullException>(() => manager.AddRecipe(null!));
        Assert.Equal(2, manager.RecipeCount);
    }

    [Theory]
    [InlineData(0, "Recipe")]
    [InlineData(-1, "Recipe")]
    [InlineData(30, "")]
    [InlineData(30, " ")]
    [InlineData(30, null)]
    public void AddRecipe_RejectsInvalidRecipe(int recipeId, string? title)
    {
        var manager = CreateManager();

        Assert.False(manager.AddRecipe(new Recipe { Id = recipeId, Title = title! }));
        Assert.Equal(2, manager.RecipeCount);
    }

    [Fact]
    public void AddRecipe_StoresRecipeAndRejectsDuplicateWithoutReplacingIt()
    {
        var manager = CreateManager();
        var recipe = new Recipe { Id = 30, Title = "New recipe" };

        Assert.True(manager.AddRecipe(recipe));
        Assert.Same(recipe, manager.FindRecipe(30));
        Assert.False(manager.AddRecipe(new Recipe { Id = 30, Title = "Replacement" }));
        Assert.Same(recipe, manager.FindRecipe(30));
        Assert.Equal(3, manager.RecipeCount);
    }

    [Fact]
    public void FindRecipe_ReturnsNullForMissingId()
    {
        Assert.Null(CreateManager().FindRecipe(999));
    }

    [Fact]
    public void RemoveRecipe_RemovesExistingRecipeAndHandlesMissingIds()
    {
        var manager = CreateManager();

        Assert.True(manager.RemoveRecipe(10));
        Assert.Null(manager.FindRecipe(10));
        Assert.False(manager.RemoveRecipe(10));
        Assert.False(manager.RemoveRecipe(999));
        Assert.Equal(1, manager.RecipeCount);
    }

    [Fact]
    public void Constructor_BuildsRecipeDictionary()
    {
        var manager = CreateManager();
        Assert.Equal(2, manager.RecipeCount);
        Assert.Equal("Recipe A", manager.FindRecipe(10)?.Title);
    }

    [Fact]
    public void InstructionsAreCompletedInFileOrder()
    {
        var manager = CreateManager();
        Assert.True(manager.StartCooking(10));
        Assert.Equal("First step", manager.PeekNextInstruction());
        Assert.Equal("First step", manager.CompleteNextInstruction());
        Assert.Equal("Second step", manager.PeekNextInstruction());
    }

    [Fact]
    public void RemovedRecipesAreRestoredLastInFirstOut()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);
        manager.RemoveRecipeFromCookingPlan(10);
        manager.RemoveRecipeFromCookingPlan(20);
        Assert.Equal(20, manager.PeekLastRemovedRecipe());
        Assert.True(manager.RestoreLastRemovedRecipe());
        Assert.Equal(new[] { 20 }, manager.GetCookingPlan());
    }

    private static RecipeManager CreateManager()
    {
        return new RecipeManager(new[]
        {
            new Recipe
            {
                Id = 10,
                Title = "Recipe A",
                Ingredients = new() { "1 apple" },
                Instructions = new() { "First step", "Second step" }
            },
            new Recipe
            {
                Id = 20,
                Title = "Recipe B"
            }
        });
    }
}
