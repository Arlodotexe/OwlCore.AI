using OwlCore.AI.Inference;
using OwlCore.AI.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace OwlCore.AI;

/// <summary>
/// Extensions for <see cref="TextMenu"/> and <see cref="TextMenuItem"/>.
/// </summary>
public static class TextMenuExtensions
{
    /// <summary>
    /// Builds the string containing all available menu items for providing to Phi.
    /// </summary>
    /// <param name="menuItems"></param>
    /// <returns></returns>
    public static string Build(this IReadOnlyList<TextMenuItem> menuItems) => Build((IReadOnlyList<MenuItem<string>>)menuItems);

    /// <summary>
    /// Builds the string containing all available menu items for providing to Phi.
    /// </summary>
    /// <param name="menuItems"></param>
    /// <returns></returns>
    public static string Build(this IReadOnlyList<MenuItem<string>> menuItems)
    {
        var menuLines = new List<string>();

        for (var index = 0; index < menuItems.Count; index++)
        {
            var menuItem = menuItems[index];

            menuLines.Add($"{index + 1}) {menuItem.Label}");
        }

        return string.Join("\n", menuLines);
    }

    /// <summary>
    /// Builds the full menu path for this menu from the root.
    /// </summary>
    /// <returns>The full menu path.</returns>
    public static string BuildMenuPath(this TextMenu menu)
    {
        var menuParts = new List<string>();
        var current = menu;

        do
        {
            menuParts.Add(current.Title);
            current = menu.Parent;
        }
        while (current?.Parent is not null);

        return string.Join(" -> ", menuParts);
    }

    /// <summary>
    /// Performs menu inference and selects an item.
    /// </summary>
    /// <returns>A Task containing the selected menu item.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the AI doesn't select a valid option.</exception>
    public static async Task<MenuItem<string>> InferAsync(this TextMenu menu, string userRequest, IModelInference<string, string> modelInference, CancellationToken cancellationToken)
    {
        if (menu.Items.Count == 0)
            throw new ArgumentException("Menu has no items to infer");

        if (menu.Items.Count == 1)
            throw new ArgumentException("Menu only has one item to infer. Skip this inference call instead.");

        var menuPath = BuildMenuPath(menu);
        var menuItems = Build(menu.Items);

        var menuPrompt = $"""
                          ## Menu
                          {menuPath}

                          {menuItems}

                          ## User Request
                          {userRequest}

                          Select a Menu Item:
                          """;

    // Infer
    infer:
        var input = menuPrompt;
        var result = await modelInference.InferAsync(input, cancellationToken).AggregateAsync((x, y) => x + y, cancellationToken);

        // AI should be restricted to output 4 tokens.
        // The output tokens could be:
        // - The number.
        // - The number in parentheses.
        // - If the label is short, it may output the label instead of the number.
        // - The label in parentheses.
        // only take the first integer output
        result = Regex.Match(result, @"\d+").Value;

        if (result.Trim() == string.Empty)
            goto infer;

        // We need to find the menu item that corresponds to the result.
        var selectedMenuItem = menu.Items
            .Select((item, index) => (item, index))
            .FirstOrDefault(x => result == ($"{(x.index + 1)}"));

        if (selectedMenuItem.item is not null)
        {
            return selectedMenuItem.item;
        }

        throw new InvalidOperationException($"Phi did not select a valid menu item. Returned result: {result}");
    }
}