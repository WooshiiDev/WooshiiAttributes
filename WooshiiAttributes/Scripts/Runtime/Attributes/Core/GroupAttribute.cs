using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class GroupAttribute : GUIElementAttribute
    {
        public string GroupName { get; set; } = null;
        public bool TitleGrouped { get; private set; }
        public bool TitleUpper { get; private set; }
        public bool TitleUnderlined { get; private set; }
       
        public GroupAttribute(bool groupedTitle = false, bool upperTitle = false, bool underlineTitle = false)
        {
            TitleGrouped = groupedTitle;
            TitleUpper = upperTitle;
            TitleUnderlined = underlineTitle;
        }

        public GroupAttribute(string groupName, bool groupedTitle = false, bool upperTitle = false, bool underlineTitle = false)
        {
            GroupName = groupName;

            TitleGrouped = groupedTitle;
            TitleUpper = upperTitle;
            TitleUnderlined = underlineTitle;
        }
    }
}