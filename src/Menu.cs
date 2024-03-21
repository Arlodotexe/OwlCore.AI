using LLama;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace OwlCore.AI.Phirmware;

public class PhirmwareMenuTestRoot : QuickMenuBase
{
    public PhirmwareMenuTestRoot(StatelessExecutor executor) : base(executor)
    {
    }

    /// <inheritdoc />
    public override string MenuTitle => "Menu";

    /// <summary>
    /// Display the lighting menu.
    /// </summary>
    [Label("Lighting")]
    public async Task ShowLightingMenuAsync()
    {
        var nextMenu = new LightingDeviceMenu(Executor) { ParentMenu = this, UserRequest = UserRequest };
        await nextMenu.InferMenuAsync();
    }

    [Label("Media control")]
    public async Task ShowMediaMenuAsync()
    {
        var nextMenu = new MediaDeviceMenu(Executor) { ParentMenu = this, UserRequest = UserRequest };
        await nextMenu.InferMenuAsync();
    }
}

public class LightingDeviceMenu : PhirmwareMenuBase
{
    public LightingDeviceMenu(StatelessExecutor statelessExecutor) : base(statelessExecutor) { }

    public override Task PopulateMenuItemsAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}

public class MediaDeviceMenu : PhirmwareMenuBase
{
    public MediaDeviceMenu(StatelessExecutor executor) : base(executor)
    {
    }

    /// <inheritdoc />
    public override string MenuTitle { get; set; } = "Select a device";

    private async Task ContinueWithMenuItem(MenuItem menuItem)
    {
        MenuTitle = menuItem.Label;
        var nextMenu = new MediaMenu(Executor) { ParentMenu = this, UserRequest = UserRequest };
        await nextMenu.InferMenuAsync();
    }

    /// <inheritdoc />
    public override Task PopulateMenuItemsAsync(CancellationToken cancellationToken = default)
    {
        MenuItems.Add(new MenuItem("Device 1", ContinueWithMenuItem));
        MenuItems.Add(new MenuItem("Device 1", ContinueWithMenuItem));
        MenuItems.Add(new MenuItem("Device 1", ContinueWithMenuItem));
        MenuItems.Add(new MenuItem("Device 1", ContinueWithMenuItem));

        return Task.CompletedTask;
    }
}


public class MediaMenu : QuickMenuBase
{
    public MediaMenu(StatelessExecutor executor) : base(executor)
    {
    }

    /// <inheritdoc />
    public override string MenuTitle => "Media";

    [Label("Pause")]
    public async Task PauseAsync()
    {
        var deviceSelector = (MediaDeviceMenu?)ParentMenu;
        if (deviceSelector is null)
            throw new ArgumentNullException();

        // Get device, perform pause.
        deviceSelector.SelectedMenuItem.Label
    }
}