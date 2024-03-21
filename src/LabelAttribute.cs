using System;

namespace OwlCore.AI.Phirmware;

/// <summary>
/// An attribute that carries a label.
/// </summary>
public class LabelAttribute : Attribute
{
    /// <summary>
    /// Gets the label for this attribute decoration.
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// Creates a new instance of <see cref="LabelAttribute"/>.
    /// </summary>
    /// <param name="label"></param>
    public LabelAttribute(string label)
    {
        Label = label;
    }
}