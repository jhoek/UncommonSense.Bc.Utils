using System.Collections.Generic;
using System.Linq;

namespace UncommonSense.Bc.Utils
{
    public static class FormattingHelper
    {
        private static bool HeaderPrinted;

        public static IEnumerable<string> FormatAvailabilitySummary(this ObjectIdAvailabilitySummary summary, bool supportsColor)
        {
            const int columnWidth = 12;

            if (!supportsColor && !HeaderPrinted)
            {
                yield return string.Join(" ", summary.Properties.Select(p => p.Name.PadRight(columnWidth)));
                HeaderPrinted = true;
            }

            if (supportsColor)
            {
                yield return string.Join(" ", summary.Properties.Select(p=> p.Name.ToString().PadRight(columnWidth)));
            }
            else
            {
                yield return string.Join(" ", summary.Properties.Select(p=> p.Value.ToString().PadRight(columnWidth)));
            }
        }
    }
}