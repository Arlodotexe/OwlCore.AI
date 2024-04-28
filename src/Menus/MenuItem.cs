namespace OwlCore.AI.Menus;

/// <summary>
/// Represents a single menu item.
/// </summary>
/// <typeparam name="TLabel">The label type for the menu item.</typeparam>
/// <param name="Label">The label for the menu item.</param>
public record MenuItem<TLabel>(TLabel Label);