using System;

using R5T.Rivet.Organizational;

using BasePathUtilities = R5T.NetStandard.IO.Paths.Base.Utilities;
using DropboxUtilities = R5T.Dropbox.Base.Utilities;
using OrganizationalUtilities = R5T.NetStandard.Organizational.Utilities;
using RivetConfigurationUtilities = R5T.Rivet.Configuration.Utilities;
using RivetDropboxUtilities = R5T.Rivet.Dropbox.Utilities;


namespace R5T.Rivet.Configuration.Construction
{
    public static class Construction
    {
        public static void SubMain()
        {
            //Construction.Scratch();
            //Construction.TestCustomSecretsDirectoryPath();
            //Construction.TestRivetOrganizationStaticNewNameHiding();
            Construction.TestIsDevelopmentMachine();
        }

        private static void TestIsDevelopmentMachine()
        {
            var isDevelopmentMachineDefault = Utilities.IsDevelopmentMachine();

            Console.WriteLine($"Is development machine (default): {isDevelopmentMachineDefault}");

            Utilities.ResetIsDevelopmentMachine();

            Utilities.OverrideIsDevelopmentMachine(true);

            isDevelopmentMachineDefault = Utilities.IsDevelopmentMachine();

            Console.WriteLine($"Is development machine (override true): {isDevelopmentMachineDefault}");

            Utilities.OverrideIsDevelopmentMachine(false);

            isDevelopmentMachineDefault = Utilities.IsDevelopmentMachine();

            Console.WriteLine($"Is development machine (override false): {isDevelopmentMachineDefault}");

            Utilities.ResetIsDevelopmentMachine();

            isDevelopmentMachineDefault = Utilities.IsDevelopmentMachine("Development Machines-BAD-Development.txt");

            Console.WriteLine($"Is development machine (override false): {isDevelopmentMachineDefault}");
        }

        /// <summary>
        /// Shows that the point at which a derived-class static new "Name" field hides a base-class "Name" property is at an attempt to access the instance-level "Name" property.
        /// </summary>
        private static void TestRivetOrganizationStaticNewNameHiding()
        {
            var writer = Console.Out;

            //writer.WriteLine($"{nameof(RivetOrganization)}.{nameof(RivetOrganization.Name)}: {RivetOrganization.Name}");

            var rivetOrganizationInstance = RivetOrganization.Instance;

            //writer.WriteLine($"{nameof(rivetOrganizationInstance)}.{nameof(rivetOrganizationInstance.Name)}: {RivetOrganization.Name}"); // Error accessing "Name" at instance level.
        }

        private static void TestCustomSecretsDirectoryPath()
        {
            var writer = Console.Out;

            var userProfileDirectoryPathValue = BasePathUtilities.UserProfileDirectoryPathValue;

            writer.WriteLine($"User Profile Directory:\n{userProfileDirectoryPathValue}\n");

            var dropboxDirectoryPathValue = DropboxUtilities.DropboxDirectoryPathValue;

            writer.WriteLine($"Dropbox Directory:\n{dropboxDirectoryPathValue}\n");

            var dropboxOrganizationsDirectoryPathValue = DropboxUtilities.OrganizationsDirectoryPathValue;

            writer.WriteLine($"Dropbox Organizations Directory:\n{dropboxOrganizationsDirectoryPathValue}\n");

            var rivetOrganization = RivetOrganization.Instance;

            var dropboxRivetOrganizationDirectoryPathValue1 = OrganizationalUtilities.AppendDefaultOrganizationDirectory(dropboxOrganizationsDirectoryPathValue, rivetOrganization);

            writer.WriteLine($"Dropbox Rivet Organization Directory (1):\n{dropboxRivetOrganizationDirectoryPathValue1}\n");

            var dropboxRivetOrganizationDirectoryPathValue2 = OrganizationalUtilities.AppendOrganizationsAndDefaultOrganizationDirectories(dropboxDirectoryPathValue, rivetOrganization);

            writer.WriteLine($"Dropbox Rivet Organization Directory (2):\n{dropboxRivetOrganizationDirectoryPathValue2}\n");

            var dropboxRivetOrganizationDirectoryPathValue3 = RivetDropboxUtilities.RivetDirectoryPathValue;

            writer.WriteLine($"Dropbox Rivet Organization Directory (3):\n{dropboxRivetOrganizationDirectoryPathValue3}\n");

            var dropboxRivetDataDirectoryPathValue = RivetDropboxUtilities.DataDirectoryPathValue;

            writer.WriteLine($"Dropbox Rivet Data Directory:\n{dropboxRivetDataDirectoryPathValue}\n");

            var rivetSecretsDirectoryPathValue = RivetConfigurationUtilities.SecretsDirectoryPathValue;

            writer.WriteLine($"Rivet Secrets Directory:\n{rivetSecretsDirectoryPathValue}\n");

            var secretsDirectoryOverridePathValue = @"C:\Temp\Secrets";

            RivetConfigurationUtilities.SecretsDirectoryPathValueOverride.Override(secretsDirectoryOverridePathValue);

            var rivetSecretsDirectoryPathValueOverride = RivetConfigurationUtilities.SecretsDirectoryPathValue;

            writer.WriteLine($"Rivet Secrets Directory (override):\n{rivetSecretsDirectoryPathValueOverride}\n");

            RivetConfigurationUtilities.SecretsDirectoryPathValueOverride.Reset();

            var rivetSecretsDirectoryPathValueOverrideReset = RivetConfigurationUtilities.SecretsDirectoryPathValue;

            writer.WriteLine($"Rivet Secrets Directory (override reset):\n{rivetSecretsDirectoryPathValueOverrideReset}\n");
        }

        private static void Scratch()
        {
            //// Test of nullable type reset behavior.
            //bool? value = new bool?();

            //var hasValue = value.HasValue;

            //value = false;

            //hasValue = value.HasValue;

            //value = null;

            //hasValue = value.HasValue;

            var machineName = Utilities.MachineName;
        }
    }
}
