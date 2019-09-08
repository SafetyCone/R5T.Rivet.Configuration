using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using R5T.NetStandard;
using R5T.NetStandard.IO.Paths;
using R5T.NetStandard.IO.Paths.Extensions;

using BasePathUtilities = R5T.NetStandard.IO.Paths.Base.Utilities;
using PathUtilities = R5T.NetStandard.IO.Paths.Utilities;
using RivetDropboxUtilities = R5T.Rivet.Dropbox.Utilities;


namespace R5T.Rivet.Configuration
{
    public static class Utilities
    {
        /// <summary>
        /// Allows overridding the secrets directory path.
        /// </summary>
        public static OverridableValue<string> SecretsDirectoryPathValueOverride { get; set; }

        /// <summary>
        /// Returns the secrets directory path using either the overridden path value, or the development or non-development path value depending on whether the current machine is a development machine.
        /// </summary>
        public static string SecretsDirectoryPathValue
        {
            get
            {
                if(Utilities.SecretsDirectoryPathValueOverride.IsOverridden)
                {
                    return Utilities.SecretsDirectoryPathValueOverride.Value;
                }

                var isDevelopmentMachine = Utilities.IsDevelopmentMachine();
                if(isDevelopmentMachine)
                {
                    return Utilities.DevelopmentSecretsDirectoryPathValue;
                }
                else
                {
                    return Utilities.NonDevelopmentSecretsDirectoryPathValue;
                }
            }
        }

        /// <summary>
        /// The development secrets directory is in the Dropbox/Organizations/Rivet/Data directory.
        /// </summary>
        public static string DevelopmentSecretsDirectoryPathValue
        {
            get
            {
                var dropboxRivetDataDirectoryPathValue = RivetDropboxUtilities.DataDirectoryPathValue;

                var output = Utilities.AppendSecretsDirectory(dropboxRivetDataDirectoryPathValue);
                return output;
            }
        }

        /// <summary>
        /// The non-development secrets directory is in the executable directory.
        /// </summary>
        public static string NonDevelopmentSecretsDirectoryPathValue
        {
            get
            {
                var executableDirectoryPath = PathUtilities.ExecutableDirectoryPathValue;

                var output = Utilities.GetNonDevelopmentSecretsDirectoryPathValue(executableDirectoryPath);
                return output;
            }
        }

        /// <summary>
        /// Returns machine name based on <see cref="Environment.MachineName"/>.
        /// Allows for customization of machine name via indirection on machine name.
        /// </summary>
        public static string MachineName
        {
            get
            {
                var output = Environment.MachineName;
                return output;
            }
        }

        private static bool? IsDevelopmentMachineResult { get; set; }


        static Utilities()
        {
            Utilities.SecretsDirectoryPathValueOverride = new OverridableValue<string>(BasePathUtilities.UnsetPathValue);
        }

        public static string AppendSecretsDirectory(string directoryPath)
        {
            var output = PathUtilities.Combine(directoryPath, Constants.SecretsDirectoryNameValue);
            return output;
        }

        public static DirectoryPath AppendSecretsDirectory(DirectoryPath directoryPath)
        {
            var output = Utilities.AppendSecretsDirectory(directoryPath.Value).AsDirectoryPath();
            return output;
        }

        public static string GetNonDevelopmentSecretsDirectoryPathValue(string executableDirectoryPath)
        {
            var output = Utilities.AppendSecretsDirectory(executableDirectoryPath);
            return output;
        }

        /// <summary>
        /// If the <paramref name="developmentMachineNames"/> enumerable contains the <paramref name="machineName"/> value, then the <paramref name="machineName"/> is a development machine.
        /// </summary>
        public static bool IsDevelopmentMachine(IEnumerable<string> developmentMachineNames, string machineName)
        {
            var output = developmentMachineNames.Contains(machineName);
            return output;
        }

        public static bool IsDevelopmentMachine(IEnumerable<string> developmentMachineNames)
        {
            var machineName = Utilities.MachineName;

            var output = Utilities.IsDevelopmentMachine(developmentMachineNames, machineName);
            return output;
        }

        /// <summary>
        /// Loads the names of development machines from a text file.
        /// Machine names are the values provided by <see cref="Utilities.MachineName"/>, and are one to a line in a regular text file.
        /// </summary>
        public static IEnumerable<string> LoadDevelopmentMachineNamesFromTextFile(string developmentMachineNamesTextFilePath)
        {
            var machineNames = File.ReadAllLines(developmentMachineNamesTextFilePath);
            return machineNames;
        }

        /// <summary>
        /// Saves the names of development machines to a text file.
        /// Name values are the values provided by <see cref="Utilities.MachineName"/>, and are one to a line in a regular text file.
        /// </summary>
        /// <param name="developmentMachineNames"></param>
        public static void SaveDevelopmentMachineNamesToTextFile(string developmentMachineNamesTextFilePath, IEnumerable<string> developmentMachineNames)
        {
            File.WriteAllLines(developmentMachineNamesTextFilePath, developmentMachineNames);
        }

        /// <summary>
        /// Tests both the development and non-development secrets directory locations for whether they contain a development machine names text file with the given file name.
        /// </summary>
        public static bool DevelopmentMachineNamesTextFileExists(string developmentMachineNamesTextFileName, out string developmentMachineNamesTextFilePath)
        {
            // Order of testing locations is biased towards non-development.
            // Start with the non-development secrets location.
            var nonDevelopmentLocationForDevelopmentMachineNamesTextFile = PathUtilities.Combine(Utilities.NonDevelopmentSecretsDirectoryPathValue, developmentMachineNamesTextFileName);
            if(PathUtilities.ExistsFilePath(nonDevelopmentLocationForDevelopmentMachineNamesTextFile))
            {
                developmentMachineNamesTextFilePath = nonDevelopmentLocationForDevelopmentMachineNamesTextFile;
                return true;
            }

            // Then try the development secrets location.
            var developmentLocationForDevelopmentMachineNamesTextFile = PathUtilities.Combine(Utilities.DevelopmentSecretsDirectoryPathValue, developmentMachineNamesTextFileName);
            if(PathUtilities.ExistsFilePath(developmentLocationForDevelopmentMachineNamesTextFile))
            {
                developmentMachineNamesTextFilePath = developmentLocationForDevelopmentMachineNamesTextFile;
                return true;
            }

            // Else, the development machine names text file does not exist.
            developmentMachineNamesTextFilePath = BasePathUtilities.UnsetPathValue;
            return false;
        }

        public static bool IsDevelopmentMachine()
        {
            var output = Utilities.IsDevelopmentMachine(Constants.DefaultDevelopmentMachinesListTextFileNameValue);
            return output;
        }

        public static bool IsDevelopmentMachine(string developmentMachinesListTextFileName)
        {
            // Determining if the current machine is a development machine is done by searching a list of development machines for the name of the current machine.
            // The list is loaded from a text file, the development machines list text file.
            // The location of the text file is hard to know... It is stored in the custom secrets directory, the location of which depends on if the current machine is a development machine!
            // Thus the difficulty arises that IsDevelopmentMachine() would like to use IsDevelopmentMachine()...
            // This is resolved by testing if each development machines list text file location (the development location and the non-development location) exists.
            // The order of testing is biased towards non-development.
            // If a development machines list text file is found, the file is loaded, parsed, and searched for the name of the current machine.
            // If the name of the current machine is found, then the current machine is a development machine.
            // Store the result (since whether the current machine is development or non-development is assumed to not change over the course of a program).
            // Use the result if available.

            // Use a previously determine value if available.
            if(Utilities.IsDevelopmentMachineResult.HasValue)
            {
                return Utilities.IsDevelopmentMachineResult.Value;
            }

            // Determine if a development machine names text file exists, and if so, its file path in the secrets directory (the path of which differs based on whether the current machine is a development machine...).
            // Note: cannot just use the secrets directory path property since it uses the IsDevelopmentMachine() method!
            var developmentMachineNamesTextFileExists = Utilities.DevelopmentMachineNamesTextFileExists(developmentMachinesListTextFileName, out var developmentMachineNamesTextFilePath);
            if(!developmentMachineNamesTextFileExists)
            {
                // Save the value for future tests, assuming a program restart is required after creating a development machine names list text file.
                Utilities.IsDevelopmentMachineResult = false;

                return false;
            }

            // If the development machine names text file exists, load it.
            var developmentMachineNames = Utilities.LoadDevelopmentMachineNamesFromTextFile(developmentMachineNamesTextFilePath);

            // Test if the current machine name is in the list of development machine names.
            var isDevelopmentMachine = Utilities.IsDevelopmentMachine(developmentMachineNames);

            // Save the value for future tests.
            Utilities.IsDevelopmentMachineResult = isDevelopmentMachine;

            return isDevelopmentMachine;
        }

        /// <summary>
        /// Since the answer to whether the current machine is a development machine is cached, allow resetting the answer.
        /// (Mostly used in testing the functionality for determining whether the current machine is a development machine.)
        /// </summary>
        public static void ResetIsDevelopmentMachine()
        {
            Utilities.IsDevelopmentMachineResult = null;
        }

        public static void OverrideIsDevelopmentMachine(bool isDevelopmentMachine)
        {
            Utilities.IsDevelopmentMachineResult = isDevelopmentMachine;
        }
    }
}
