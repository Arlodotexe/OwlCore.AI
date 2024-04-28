using System;
using System.Collections.Generic;
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
    /// <param name="progress"></param>
    /// <param name="cancellationToken">A token that can be used to cancel the ongoing task.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation. The result is a completed output-typed result.</returns>
    public IAsyncEnumerable<TOut> InferAsync(TIn input, Progress<string> progress, CancellationToken cancellationToken);
}