﻿using PKISharp.WACS.DomainObjects;
using PKISharp.WACS.Extensions;
using PKISharp.WACS.Plugins.Base.Factories;
using PKISharp.WACS.Services;
using System;

namespace PKISharp.WACS.Plugins.InstallationPlugins
{
    internal class ScriptInstallerFactory : BaseInstallationPluginFactory<ScriptInstaller, ScriptInstallerOptions>
    {
        public const string PluginName = "Manual";
        public ScriptInstallerFactory(ILogService log) : base(log, PluginName, "Run a custom script") { }

        public override ScriptInstallerOptions Aquire(ScheduledRenewal renewal, IOptionsService optionsService, IInputService inputService, RunLevel runLevel)
        {
            inputService.Show("Full instructions", "https://github.com/PKISharp/win-acme/wiki/Install-Script");
            do
            {
                renewal.Script = optionsService.TryGetOption(optionsService.Options.Script, inputService, "Enter the path to the script that you want to run after renewal");
            }
            while (!renewal.Script.ValidFile(_log));
            inputService.Show("{0}", "Hostname");
            inputService.Show("{1}", ".pfx password");
            inputService.Show("{2}", ".pfx path");
            inputService.Show("{3}", "Certificate store name");
            inputService.Show("{4}", "Certificate friendly name");
            inputService.Show("{5}", "Certificate thumbprint");
            inputService.Show("{6}", "Central SSL store path");
            renewal.ScriptParameters = optionsService.TryGetOption(optionsService.Options.ScriptParameters, inputService, "Enter the parameter format string for the script, e.g. \"--hostname {0}\"");
            return null;
        }

        public override ScriptInstallerOptions Default(ScheduledRenewal renewal, IOptionsService optionsService)
        {
            renewal.Script = optionsService.TryGetRequiredOption(nameof(optionsService.Options.Script), optionsService.Options.Script);
            if (!renewal.Script.ValidFile(_log))
            {
                throw new ArgumentException(nameof(optionsService.Options.Script));
            }
            renewal.ScriptParameters = optionsService.Options.ScriptParameters;
            return null;
        }
    }
}