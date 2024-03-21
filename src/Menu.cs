using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace OwlCore.AI.Phirmware;

public class PhirmwareMenuTestRoot : PhirmwareMenuBase
{
    /// <inheritdoc />
    public override string MenuTitle => "";

    /// <summary>
    /// Display the lighting menu.
    /// </summary>
    [Description("Lighting")]
    public async Task ShowLightingMenuAsync()
    {

    }

    [Description("Media control")]
    public async Task ShowMediaMenuAsync()
    {
        await InferNextMenuAsync(new MediaMenu { ParentMenu = null, UserRequest = UserRequest });
    }
}


public class MediaDeviceMenu : PhirmwareMenuBase
{
    /// <inheritdoc />
    public override string MenuTitle => "Select a device";

    public override List<MenuItem> MenuItems { get; set; } = new List<MenuItem>
        {
        new MenuItem("TV", x => ContinueWithDevice(x)),
        new MenuItem("Sound system", ContinueWithDevice),
        new MenuItem("Projector", ContinueWithDevice)
    };

    [Description("Lighting control")]
    public async Task ContinueWithDevice(MenuItem menuItem)
    {

    }
}


public class MediaMenu : PhirmwareMenuBase
{
    /// <inheritdoc />
    public override string MenuTitle => "Media";

    [Description("Lighting control")]
    public async Task ShowLightingMenuAsync()
    {

    }
}