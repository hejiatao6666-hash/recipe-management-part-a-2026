# Recipe Management System: Part A

Target framework: .NET 10. Test framework: xUnit.

Repository: https://github.com/hejiatao6666-hash/recipe-management-part-a-2026

## Implementation Progress

### 5 November 2026: Recipe Catalogue

- Store recipes in a `Dictionary<int, Recipe>`.
- Validate null collection arguments, non-positive IDs, blank titles and duplicate IDs during construction.
- Add, find and remove recipes, with safe handling of invalid input and missing IDs.
- Create private fields for the five required collections and expose their counts.
- Pass 18 catalogue tests.

### 6 November 2026: Shopping List

- Append ingredients to a `List<string>` in their original order, preserving the original strings and duplicates.
- Return 0 for a missing recipe; recipes without ingredients leave the shopping list unchanged.
- Return a read-only view to protect the internal list from modification by callers.
- Clear the shopping list and update its count without changing the recipe's ingredients.
- Add 9 shopping list tests, bringing the passing total to 27.
- Fix a build error caused by extra blank lines before the solution file header.

### 7 November 2026: Cooking Plan and Removal History

- Store the cooking plan in a `LinkedList<int>` in insertion order, rejecting missing and duplicate recipe IDs.
- Remove a planned recipe by ID and push the ID onto a `Stack<int>` only when removal succeeds.
- Peek at the most recently removed ID without changing the history; restore by popping the ID and appending it to the end of the plan.
- Handle an empty stack safely. A deleted or already-planned recipe cannot be restored, but its history entry is still consumed.
- Return a read-only copy of the plan and prevent catalogue deletion of recipes currently in the plan.
- Add 15 linked-list, stack and interaction tests. Together with the supplied history example, 43 tests pass.

### 8 November 2026: Instruction Queue and Final Verification

- Load instructions into a `Queue<string>` in their original order. Peek leaves the queue unchanged; completion removes exactly one instruction from the front.
- Return `null` for empty queue operations. A valid new session replaces the previous instructions; an unsuccessful start leaves the current session unchanged.
- Add 12 queue and workflow tests. All 56 tests pass, including the supplied queue example.
- Test interactions across the catalogue, shopping list, cooking plan, removal history and instruction queue.
- Build all three projects with zero warnings and errors. Verify the console workflow using the supplied dataset of 670 recipes.

All Part A requirements are implemented. Part B methods retain the supplied placeholders, and `Design.md` remains the template for Part B.

## Development Checkpoints

| Date | Work completed |
| --- | --- |
| 2026-11-05 | Starter projects, Git setup, recipe catalogue and catalogue tests |
| 2026-11-06 | Shopping list and list tests |
| 2026-11-07 | Cooking plan, removal history, linked-list and stack tests |
| 2026-11-08 | Instruction queue, workflow tests, full verification and submission packaging |

Earlier commit messages are retained in their original language. The summaries above describe each checkpoint in English.

## Project Structure

- `RecipeManagement.Application`: the supplied console application; references Core.
- `RecipeManagement.Core`: the supplied models, loader and interface, together with the Part A implementation of `RecipeManager`.
- `RecipeManagement.Tests`: xUnit tests; references Core.
- `data/recipes.json`: the supplied recipe dataset.
- `Design.md`: the Part B design template.

## Build, Test and Run

Run these commands from the repository root:

```powershell
dotnet build
dotnet test
dotnet run --project RecipeManagement.Application -- data/recipes.json
```

If the default Windows `dotnet` command cannot find an SDK installed in the user directory, use:

```powershell
& "$env:USERPROFILE\.dotnet\dotnet.exe" build
```

If a restricted local terminal reports access denied when the test host monitors its parent process, use the following temporary environment workaround. All tests still run:

```powershell
dotnet test -p:_MSTestEnableParentProcessQuery=false
```

Generated `bin`, `obj`, test results and temporary IDE files are excluded from Git.

## Submission Version and Git History

The English submission version is identified by the `part-a-final-en` tag. The repository retains the development commits from 5 to 8 November 2026. After extracting the submission ZIP, run these commands from the project root:

```powershell
git log --oneline --date=iso-strict --format="%h %ad %s"
git show part-a-final-en --stat
```

The ZIP includes the hidden `.git` directory, source files, project files, tests and data. It excludes `bin` and `obj`. Commit history is also available through the Commits link on the GitHub repository page.

