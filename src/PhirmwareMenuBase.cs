using LLama;
using System.Linq;
using System.Threading.Tasks;

namespace OwlCore.AI.Phirmware;

/// <summary>
/// A base class for using the Phi model for menus.
/// </summary>
public abstract class PhirmwareMenuBase : MenuBase
{
    /// <summary>
    /// The <see cref="StatelessExecutor"/> used for inference.
    /// </summary>
    protected StatelessExecutor Executor { get; }

    /// <summary>
    /// Creates a new instance of <see cref="PhirmwareMenuBase"/>.
    /// </summary>
    /// <param name="executor"></param>
    public PhirmwareMenuBase(StatelessExecutor executor)
    {
        Executor = executor;
    }

    /// <inheritdoc />
    public override abstract string MenuTitle { get; set; }
    
    /// <inheritdoc />
    public override async Task<string> InferCompletionAsync(string systemMessage, string input)
    {
        var results = await Executor.InferAsync(string.Join("\n", systemMessage, input)).ToListAsync();

        return string.Join("\n", results);
    }
}