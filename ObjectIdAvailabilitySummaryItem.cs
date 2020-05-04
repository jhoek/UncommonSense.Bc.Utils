using System;
using System.Linq;
using System.Management.Automation;

namespace UncommonSense.Bc.Utils
{
    public class ObjectIdAvailabilitySummaryItem
    {
        internal ObjectIdAvailabilitySummaryItem(int objectID)
        {
            ObjectID = objectID;
            Table = Availability.NotInRange;
        }

        public int ObjectID { get; }
        public Availability Table { get; internal set; }
        public Availability TableExtension { get; internal set; }
        public Availability Page { get; internal set; }
        public Availability PageExtension { get; internal set; }
        public Availability PageCustomization { get; internal set; }
        public Availability Report { get; internal set; }
        public Availability Codeunit { get; internal set; }
        public Availability XmlPort { get; internal set; }
        public Availability Query { get; internal set; }
        public Availability Profile { get; internal set; }
        public Availability Enum { get; internal set; }
        public Availability EnumExtension { get; internal set; }

        public Availability GetAvailability(ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.Table: return Table;
                case ObjectType.TableExtension: return TableExtension;
                case ObjectType.Page: return Page;
                case ObjectType.PageExtension: return PageExtension;
                case ObjectType.PageCustomization: return PageCustomization;
                case ObjectType.Report: return Report;
                case ObjectType.Codeunit: return Codeunit;
                case ObjectType.XmlPort: return XmlPort;
                case ObjectType.Query: return Query;
                case ObjectType.Profile: return Profile;
                case ObjectType.Enum: return Enum;
                case ObjectType.EnumExtension: return EnumExtension;
                default: throw new ArgumentOutOfRangeException(nameof(objectType));
            }
        }

        public ObjectIdAvailabilitySummaryItem SetAvailability(ObjectType objectType, Availability availability)
        {
            switch (objectType)
            {
                case ObjectType.Table: Table = availability; break;
                case ObjectType.TableExtension: TableExtension = availability; break;
                case ObjectType.Page: Page = availability; break;
                case ObjectType.PageExtension: PageExtension = availability; break;
                case ObjectType.PageCustomization: PageCustomization = availability; break;
                case ObjectType.Report: Report = availability; break;
                case ObjectType.Codeunit: Codeunit = availability; break;
                case ObjectType.XmlPort: XmlPort = availability; break;
                case ObjectType.Query: Query = availability; break;
                case ObjectType.Profile: Profile = availability; break;
                case ObjectType.Enum: Enum = availability; break;
                case ObjectType.EnumExtension: EnumExtension = availability; break;
            }


            return this;
        }
    }
}