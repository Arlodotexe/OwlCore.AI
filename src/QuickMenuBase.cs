using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace OwlCore.AI.Phirmware;

/// <summary>
/// A base class for building menus quickly using the <see cref="LabelAttribute"/> on async methods.
/// </summary>
public abstract class QuickMenuBase : MenuBase
{
    /// <inheritdoc/>
    public override Task PopulateMenuItemsAsync(CancellationToken cancellationToken = default)
    {
        Type asyncAttributeType = typeof(AsyncStateMachineAttribute);
        Type labelAttributeType = typeof(LabelAttribute);

        var asyncMethods = GetType().GetMethods().Where(x => CustomAttributeExtensions.GetCustomAttribute((MemberInfo)x, asyncAttributeType) is AsyncStateMachineAttribute);
        var asyncMethodsWithLabel = asyncMethods.Where(x => x.GetCustomAttribute(labelAttributeType) is LabelAttribute);

        foreach (var methodInfo in asyncMethodsWithLabel)
        {
            var label = (methodInfo.GetCustomAttribute(labelAttributeType) as LabelAttribute)?.Label;

            if (label is null || string.IsNullOrWhiteSpace(label))
                throw new ArgumentException("A required label is missing.");

            var menuItem = new MenuItem(label, x => (Task)methodInfo.Invoke(this, [x]));
            MenuItems.Add(menuItem);
        }

        return Task.CompletedTask;
    }
}