using System;
using Ouranos.RedArmory.Models.Services;

namespace Ouranos.RedArmory.Models.Helpers
{

    internal static class Utility
    {

        public static string GetSanitizedDirectoryPath(BitnamiRedmineStack stack, string directory)
        {
            var keywords = new[]
            {
                "%VERSION%",
                "%LONGDATE%",
                "%SHORTDATE%",
            };

            var name = directory ?? "";
            var datetime = DateTime.Now;

            foreach (var keyword in keywords)
            {
                do
                {
                    if (name.IndexOf(keyword, StringComparison.InvariantCulture) == -1)
                    {
                        break;
                    }

                    switch (keyword)
                    {
                        case "%VERSION%":
                            name = name.Replace(keyword, stack.DisplayVersion);
                            break;
                        case "%LONGDATE%":
                            name = name.Replace(keyword, datetime.ToString("yyyyMMdd hhmmss"));
                            break;
                        case "%SHORTDATE%":
                            name = name.Replace(keyword, datetime.ToString("yyyyMMdd"));
                            break;
                    }
                } while (true);
            }

            return name;
        }

    }

}
