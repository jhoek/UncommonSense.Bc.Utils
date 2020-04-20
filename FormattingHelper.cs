using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UncommonSense.Bc.Utils
{
    public static class FormattingHelper
    {
        public static string FormatObjectIdAvailabilitySummary(ObjectIdAvailabilitySummary summary, bool supportsVirtualTerminal)
        {
            var stringBuilder = new StringBuilder();

            foreach (var item in summary.Items)
            {
                stringBuilder.AppendLine(
                    GetObjectIdAvailabilitySummaryItemColumn(item, summary.ConsideredObjectTypes, supportsVirtualTerminal)
                        .Join("  "));
            }

            return stringBuilder.ToString();
        }

        public static IEnumerable<string> GetObjectIdAvailabilitySummaryItemColumn(ObjectIdAvailabilitySummaryItem item, ObjectType[] objectTypes, bool supportsVirtualTerminal)
        {
            yield return item.ObjectID.ToString();

            foreach (var objectType in objectTypes)
            {
                yield return objectType.ToString().Colorize(supportsVirtualTerminal, item.GetAvailability(objectType));
            }
        }

        public static string Colorize(this string value, bool supportsVirtualTerminal, Availability availability)
        {
            if (!supportsVirtualTerminal)
                return value;

            switch (availability)
            {
                case Availability.Available: return $"\x1B[32m{value}\x1B[0m";
                case Availability.Reserved: return $"\x1B[34m{value}\x1B[0m";
                case Availability.InUse: return $"\x1B[31m{value}\x1B[0m";
                default: return value;
            }
        }

        public static void AppendIf(this StringBuilder stringBuilder, bool condition, string value)
        {
            if (condition)
                stringBuilder.Append(value);
        }
    }
}