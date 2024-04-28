namespace OwlCore.AI.Menus;

/// <summary>
/// Represents a single menu item that can be selected.
/// </summary>
/// <param name="Label">The label for the menu item.</param>
public record TextMenuItem(string Label) : MenuItem<string>(Label);