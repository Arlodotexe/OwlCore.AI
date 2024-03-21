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
    public abstract string MenuTitle { get; }

    /// <summary>
    /// Menu items that will be used during inference.
    /// </summary>
    public virtual List<MenuItem> MenuItems { get; set; } = new();

    /// <summary>
    /// The parent menu of this menu. If this is the root menu, this will be null.
    /// </summary>
    public required MenuBase? ParentMenu { get; init; }

    /// <summary>
    /// The request from the user to be fulfilled by the AI.
    /// </summary>
    public required string UserRequest { get; init; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="menu"></param>
    /// <returns></returns>
    public virtual async Task InferNextMenuAsync(MenuBase menu)
    {
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

        var menuPath = BuildMenuPath();
        var menuItems = BuildMenuItems();

        var menuPrompt = $"""
                          ## Menu
                          {menuPath}
                          
                          {menuItems}

                          Menu Item:
                          """;

        var result = await InferAsync(systemPrompt, menuPrompt);

        // AI should be restricted to output 4 tokens.
        // These could be:
        // - The number.
        // - The number in parentheses.
        // - If the label is short, it may output the label instead of the number.
        // - Not observed, but handling label in parentheses.
        result = result.Trim().TrimStart('(').TrimEnd(')').Trim();

        // We need to find the menu item that corresponds to the result.
        var selectedMenuItem = MenuItems.Select((item, index) => (item, index)).FirstOrDefault(x => x.index.ToString() == result || x.item.Label.StartsWith(result) || x.item.Label.EndsWith(result) || x.item.Label.Contains(result));

        if (selectedMenuItem.item is not null)
        {
            await selectedMenuItem.item.OnSelectDelegate(selectedMenuItem.item);
        }
        else
        {

        }
    }

    protected virtual string BuildMenuItems()
    {
        var menuLines = new List<string>();

        for (var index = 0; index < MenuItems.Count; index++)
        {
            var menuItem = MenuItems[index];

            menuLines.Add($"({index + 1}) {menuItem.Label}");
        }

        return string.Join(" -> ", menuLines);
    }

    /// <summary>
    /// Builds the full menu path for this menu from the root.
    /// </summary>
    /// <returns></returns>
    protected virtual string BuildMenuPath()
    {
        var menuParts = new List<string>();
        var current = this;

        do
        {
            menuParts.Add(current.MenuTitle);
            current = ParentMenu;
        }
        while (current?.ParentMenu is not null);

        return string.Join(" -> ", menuParts);
    }

    public abstract Task<string> InferAsync(string systemMessage, string input);

    /// <summary>
    /// Populate the menu items for this menu.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the ongoing operation.</param>
    /// <returns></returns>
    public abstract Task PopulateMenuItemsAsync(CancellationToken cancellationToken = default);
}