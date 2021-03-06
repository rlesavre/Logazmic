using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Logazmic.Core.Log;
using NuGet;

namespace Logazmic.Core.Filters
{
    public class FilterLogic
    {
        private readonly FiltersProfile filtersProfile;

        private readonly HashSet<string> logSourceLeaves = new HashSet<string>();

        public FilterLogic(FiltersProfile filtersProfile)
        {
            this.filtersProfile = filtersProfile;
        }

        public bool IsFiltered(LogMessage logMessage)
        {
            if (logMessage == null)
            {
                return true;
            }

            if (logMessage.LogLevel < filtersProfile.MinLogLevel)
            {
                return true;
            }

            if (!filtersProfile.LogLevels.First(l => l.LogLevel == logMessage.LogLevel).IsEnabled)
            {
                return true;
            }

            foreach (var messageFilter in filtersProfile.MessageFilters.Where(mf => mf.IsEnabled))
            {
                if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(logMessage.Message, messageFilter.Message, CompareOptions.IgnoreCase) >= 0)
                {
                    return true;
                }
            }

            if (!string.IsNullOrEmpty(filtersProfile.FilterText))
            {
                if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(logMessage.Message, filtersProfile.FilterText, CompareOptions.IgnoreCase) < 0)
                {
                    return true;
                }
            }

            if (!logSourceLeaves.Contains(logMessage.LoggerName))
            {
                return true;
            }
            return false;
        }

        public void RebuildLeaves()
        {
            logSourceLeaves.Clear();
            logSourceLeaves.AddRange(filtersProfile.SourceFilterRoot.Leaves().Where(l => l.IsChecked).Select(c => c.FullName));
        }
    }
}