using System;

using R5T.NetStandard.IO.Paths;
using R5T.NetStandard.IO.Paths.Extensions;


namespace R5T.Rivet.Configuration
{
    public static class Constants
    {
        public const string SecretsDirectoryNameValue = "Secrets";
        public static readonly DirectoryName SecretsDirectoryName = Constants.SecretsDirectoryNameValue.AsDirectoryName();
        public const string DefaultDevelopmentMachinesListTextFileNameValue = "Development Machines.txt";
        public static readonly FileName DefaultDevelopmentMachinesListTextFileName = Constants.DefaultDevelopmentMachinesListTextFileNameValue.AsFileName();
    }
}
