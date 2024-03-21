using System.Collections.Generic;
using System.Threading.Tasks;

namespace OwlCore.AI.Phirmware;

/// <summary>
/// A base class for using the Phi model for menus.
/// </summary>
public abstract class PhirmwareMenuBase : MenuBase
{
    /// <inheritdoc />
    public override abstract string MenuTitle { get; }
    
    /// <inheritdoc />
    public override Task<string> InferAsync(string systemMessage, string input)
    {
        throw new System.NotImplementedException();
    }
}