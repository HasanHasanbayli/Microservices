﻿using IdentityServerHost.Quickstart.UI;

namespace FreeCourse.IdentityServer.Quickstart.Account;

public class LogoutViewModel : LogoutInputModel
{
    public bool ShowLogoutPrompt { get; set; } = true;
}