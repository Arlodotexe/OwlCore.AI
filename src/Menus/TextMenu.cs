using System.Collections.Generic;

namespace OwlCore.AI.Menus;

/// <summary>
/// Represents a text menu item with items.
/// </summary>
/// <param name="Title">The menu title.</param>
/// <param name="Items">The items in the menu.</param>
/// <param name="Parent">The parent menu, if any.</param>
public record TextMenu(string Title, List<TextMenuItem> Items, TextMenu? Parent = null) : Menu<string, TextMenuItem, TextMenu>(Title, Items, Parent);
