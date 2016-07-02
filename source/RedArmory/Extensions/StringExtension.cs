using System;
using System.Text.RegularExpressions;

namespace Ouranos.RedArmory.Extensions
{

    internal static class StringExtension
    {

        public static Version ToVersion(this string source)
        {
            var regex = new Regex("(?<major>[0-9]+)\\.(?<minor>[0-9]+)\\.(?<build>[0-9]+)-(?<revision>[0-9]+)", RegexOptions.Compiled);
            var match = regex.Match(source);
            if (!match.Success)
            {
                return new Version(0, 0, 0, 0);
            }

            var major = int.Parse(match.Groups["major"].Value);
            var minor = int.Parse(match.Groups["minor"].Value);
            var build = int.Parse(match.Groups["build"].Value);
            var revision = int.Parse(match.Groups["revision"].Value);

            return new Version(major, minor, build, revision);
        }

    }

}
