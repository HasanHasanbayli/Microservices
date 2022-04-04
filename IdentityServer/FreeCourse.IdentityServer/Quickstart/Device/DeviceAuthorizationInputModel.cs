using FreeCourse.IdentityServer.Quickstart.Consent;
using IdentityServerHost.Quickstart.UI;

namespace FreeCourse.IdentityServer.Quickstart.Device;

public class DeviceAuthorizationInputModel : ConsentInputModel
{
    public string UserCode { get; set; }
}