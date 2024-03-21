using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OwlCore.AI.Phirmware;

/// <summary>
/// Invoking a menu item can either return information to the caller (like getting a status), do a thing (like controlling a device) or showing another menu.
/// </summary>
public abstract class MenuBase
{
    /// <summary>
    /// The title for this menu. Should be a single word, ideally.
    /// </summary>
    public abstract string MenuTitle { get; set; }

    /// <summary>
    /// Menu items that will be used during inference.
    /// </summary>
    public virtual List<MenuItem> MenuItems { get; set; } = new();

    /// <summary>
    /// The parent menu of this menu. If this is the root menu, this will be null.
    /// </summary>
    public required MenuBase? ParentMenu { get; init; }

    /// <summary>
    /// The currently selected menu item, if any.
    /// </summary>
    public MenuItem? SelectedMenuItem { get; set; }

    /// <summary>
    /// The request from the user to be fulfilled by the AI.
    /// </summary>
    public required string UserRequest { get; init; }

    /// <summary>
    /// Performs menu inference and selects an item.
    /// </summary>
    /// <returns>A Task containing the selected menu item.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the AI doesn't select a valid option.</exception>
    public virtual async Task<MenuItem> InferMenuAsync()
    {
        var menu = this;

        // AI selects a menu item number
        // We invoke the corresponding async method.
        var systemPrompt = $"""
You are a menu-based AI assistant firmware. Use the menu to complete the user request. 
                           
Using the firmware menu:
- Respond with ONLY a menu item number in parenthesis. For example, output (x) to select menu item x).
- Select the menu item that will complete the user request.

User request:
{UserRequest}
""";

        var menuPath = BuildMenuPath(menu);
        var menuItems = BuildMenuItems(menu);

        var menuPrompt = $"""
                          ## Menu
                          {menuPath}
                          
                          {menuItems}

                          Menu Item:
                          """;

        var result = await InferCompletionAsync(systemPrompt, menuPrompt);

        // AI should be restricted to output 4 tokens.
        // These could be:
        // - The number.
        // - The number in parentheses.
        // - If the label is short, it may output the label instead of the number.
        // - Not observed, but handling label in parentheses.
        result = result.Trim().TrimStart('(').TrimEnd(')').Trim();

        // We need to find the menu item that corresponds to the result.
        var selectedMenuItem = menu.MenuItems.Select((item, index) => (item, index)).FirstOrDefault(x => x.index.ToString() == result || x.item.Label.StartsWith(result) || x.item.Label.EndsWith(result) || x.item.Label.Contains(result));

        if (selectedMenuItem.item is not null)
        {
            await selectedMenuItem.item.OnSelectDelegate(selectedMenuItem.item);
            SelectedMenuItem = selectedMenuItem.item;
            return SelectedMenuItem;
        }
        else
            throw new InvalidOperationException($"Phi did not select a valid menu item. Returned result: {result}");
    }

    /// <summary>
    /// Builds the string containing all available menu items for providing to Phi.
    /// </summary>
    /// <param name="menu"></param>
    /// <returns></returns>
    protected virtual string BuildMenuItems(MenuBase menu)
    {
        var menuLines = new List<string>();

        for (var index = 0; index < MenuItems.Count; index++)
        {
            var menuItem = menu.MenuItems[index];

            menuLines.Add($"({index + 1}) {menuItem.Label}");
        }

        return string.Join(" -> ", menuLines);
    }

    /// <summary>
    /// Builds the full menu path for this menu from the root.
    /// </summary>
    /// <returns>The full menu path.</returns>
    protected virtual string BuildMenuPath(MenuBase menu)
    {
        var menuParts = new List<string>();
        var current = menu;

        do
        {
            menuParts.Add(current.MenuTitle);
            current = ParentMenu;
        }
        while (current?.ParentMenu is not null);

        return string.Join(" -> ", menuParts);
    }

    /// <summary>
    /// Performs inference completion.
    /// </summary>
    /// <param name="systemMessage">The system message.</param>
    /// <param name="input">The text to infer completion.</param>
    /// <returns></returns>
    public abstract Task<string> InferCompletionAsync(string systemMessage, string input);

    /// <summary>
    /// Populate the menu items for this menu.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the ongoing operation.</param>
    /// <returns></returns>
    public abstract Task PopulateMenuItemsAsync(CancellationToken cancellationToken = default);
}