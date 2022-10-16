using FreeCourse.IdentityServer.Quickstart.Consent;

namespace FreeCourse.IdentityServer.Quickstart.Device;

public class DeviceAuthorizationInputModel : ConsentInputModel
{
    public string UserCode { get; set; }
}