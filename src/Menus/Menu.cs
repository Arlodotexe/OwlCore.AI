using System.Collections.Generic;

namespace OwlCore.AI.Menus;

/// <summary>
/// Represents a menu item with items.
/// </summary>
/// <param name="Title">The menu title.</param>
/// <param name="Items">The items in the menu.</param>
/// <param name="Parent">The parent menu, if any.</param>
public record Menu<TTitle, TMenuItem, TMenuParent>(TTitle Title, List<TMenuItem> Items, TMenuParent? Parent);
