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
    public void GetShoppingList_IsInitiallyEmpty()
    {
        var manager = CreateManager();

        Assert.Empty(manager.GetShoppingList());
        Assert.Equal(0, manager.ShoppingItemCount);
    }

    [Fact]
    public void AddIngredientsToShoppingList_PreservesOriginalStringsAndOrder()
    {
        var recipe = new Recipe
        {
            Id = 30,
            Title = "Shopping recipe",
            Ingredients = new() { " 1 apple ", "2 tbsp sugar", "1 apple" }
        };
        IRecipeManager manager = new RecipeManager(new[] { recipe });

        Assert.Equal(3, manager.AddIngredientsToShoppingList(30));
        Assert.Equal(new[] { " 1 apple ", "2 tbsp sugar", "1 apple" }, manager.GetShoppingList());
        Assert.Equal(3, manager.ShoppingItemCount);
        Assert.Equal(new[] { " 1 apple ", "2 tbsp sugar", "1 apple" }, recipe.Ingredients);
    }

    [Fact]
    public void AddIngredientsToShoppingList_AppendsRecipesAndKeepsRepeatedIngredients()
    {
        var manager = CreateManager();
        manager.AddRecipe(new Recipe
        {
            Id = 30,
            Title = "Another recipe",
            Ingredients = new() { "2 eggs", "1 apple" }
        });

        Assert.Equal(1, manager.AddIngredientsToShoppingList(10));
        Assert.Equal(2, manager.AddIngredientsToShoppingList(30));
        Assert.Equal(1, manager.AddIngredientsToShoppingList(10));
        Assert.Equal(new[] { "1 apple", "2 eggs", "1 apple", "1 apple" }, manager.GetShoppingList());
        Assert.Equal(4, manager.ShoppingItemCount);
    }

    [Fact]
    public void AddIngredientsToShoppingList_MissingRecipeDoesNotChangeExistingItems()
    {
        var manager = CreateManager();
        manager.AddIngredientsToShoppingList(10);

        Assert.Equal(0, manager.AddIngredientsToShoppingList(999));
        Assert.Equal(new[] { "1 apple" }, manager.GetShoppingList());
        Assert.Equal(1, manager.ShoppingItemCount);
    }

    [Fact]
    public void AddIngredientsToShoppingList_RecipeWithoutIngredientsAddsNothing()
    {
        var manager = CreateManager();
        manager.AddIngredientsToShoppingList(10);

        Assert.Equal(0, manager.AddIngredientsToShoppingList(20));
        Assert.Equal(new[] { "1 apple" }, manager.GetShoppingList());
        Assert.Equal(1, manager.ShoppingItemCount);
    }

    [Fact]
    public void AddIngredientsToShoppingList_CopiesStringsIndependentlyOfRecipeList()
    {
        var manager = CreateManager();
        manager.AddIngredientsToShoppingList(10);

        manager.FindRecipe(10)!.Ingredients[0] = "changed ingredient";

        Assert.Equal(new[] { "1 apple" }, manager.GetShoppingList());
        Assert.True(manager.RemoveRecipe(10));
        Assert.Equal(new[] { "1 apple" }, manager.GetShoppingList());
    }

    [Fact]
    public void GetShoppingList_PreventsMutationOfInternalList()
    {
        var manager = CreateManager();
        manager.AddIngredientsToShoppingList(10);
        var returnedItems = Assert.IsAssignableFrom<IList<string>>(manager.GetShoppingList());

        Assert.True(returnedItems.IsReadOnly);
        Assert.Throws<NotSupportedException>(() => returnedItems[0] = "changed ingredient");
        Assert.Throws<NotSupportedException>(() => returnedItems.Add("extra ingredient"));
        Assert.Throws<NotSupportedException>(() => returnedItems.Clear());
        Assert.Equal(new[] { "1 apple" }, manager.GetShoppingList());
        Assert.Equal(1, manager.ShoppingItemCount);
    }

    [Fact]
    public void ClearShoppingList_RemovesAllItemsWithoutChangingRecipeIngredients()
    {
        var manager = CreateManager();
        manager.AddIngredientsToShoppingList(10);
        manager.AddIngredientsToShoppingList(10);

        manager.ClearShoppingList();

        Assert.Empty(manager.GetShoppingList());
        Assert.Equal(0, manager.ShoppingItemCount);
        Assert.Equal(new[] { "1 apple" }, manager.FindRecipe(10)!.Ingredients);
        Assert.Equal(1, manager.AddIngredientsToShoppingList(10));
        Assert.Equal(new[] { "1 apple" }, manager.GetShoppingList());
        Assert.Equal(1, manager.ShoppingItemCount);
    }

    [Fact]
    public void ClearShoppingList_HandlesAnAlreadyEmptyList()
    {
        var manager = CreateManager();

        manager.ClearShoppingList();
        manager.ClearShoppingList();

        Assert.Empty(manager.GetShoppingList());
        Assert.Equal(0, manager.ShoppingItemCount);
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
