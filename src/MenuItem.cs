using System;
using System.Threading.Tasks;

namespace OwlCore.AI.Phirmware;

/// <summary>
/// Represents a single menu item that can be selected.
/// </summary>
/// <param name="Label">The label for the menu item.</param>
/// <param name="OnSelectDelegate">The delegate that is invoked when the menu item is selected.</param>
public record MenuItem(string Label, Func<MenuItem, Task> OnSelectDelegate);