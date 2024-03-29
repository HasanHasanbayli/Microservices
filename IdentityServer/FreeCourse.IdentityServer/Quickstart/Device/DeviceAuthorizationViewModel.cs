using FreeCourse.IdentityServer.Quickstart.Consent;
using IdentityServerHost.Quickstart.UI;

namespace FreeCourse.IdentityServer.Quickstart.Device;

public class DeviceAuthorizationViewModel : ConsentViewModel
{
    public string UserCode { get; set; }
    public bool ConfirmUserCode { get; set; }
}