using System;
using Celeste.Mod.SuperDashCollabHelper.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.SuperDashCollabHelper;

public class SuperDashCollabHelperModule : EverestModule
{

    public const string NAME = "SuperDashCollabHelper";
    public static SuperDashCollabHelperModule Instance { get; private set; }

    public override Type SettingsType => typeof(SuperDashCollabHelperModuleSettings);
    public static SuperDashCollabHelperModuleSettings Settings => (SuperDashCollabHelperModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(SuperDashCollabHelperModuleSession);
    public static SuperDashCollabHelperModuleSession Session => (SuperDashCollabHelperModuleSession) Instance._Session;

    public SuperDashCollabHelperModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(SuperDashCollabHelperModule), LogLevel.Verbose);
#else
            // release builds use info logging to reduce spam in log files
            Logger.SetLogLevel(nameof(SuperDashCollabHelperModule), LogLevel.Info);
#endif
    }

    public override void Load() {
    }

    public override void Unload() {
    }
}