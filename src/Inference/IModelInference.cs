using System;
using System.Threading;
using System.Threading.Tasks;

namespace OwlCore.AI.Inference;

/// <summary>
/// Indicates that the derived member is capable of AI model inference.
/// </summary>
/// <typeparam name="TIn">The input type for the model.</typeparam>
/// <typeparam name="TOut">The output result type for the model.</typeparam>
public interface IModelInference<in TIn, TOut>
{
    /// <summary>
    /// Performs inference for this model.
    /// </summary>
    /// <param name="input">The input to the model.</param>
    /// <param name="progress">If the inference can be streamed, partial progress is reported via this parameter.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the ongoing task.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation. The result is a completed output-typed result.</returns>
    public Task<TOut> InferAsync(TIn input, IProgress<TOut> progress, CancellationToken cancellationToken);
}