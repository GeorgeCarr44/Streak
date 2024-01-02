using Streak.Models;

namespace Streak
{
    public class GoalTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UncheckedTemplate { get; set; }
        public DataTemplate CheckedTemplate { get; set; }
        public DataTemplate CreateButtonTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // If the ID of the Goal is 0 then this is the special case for the new button
            // I want the create new button to be at the end of the flex grid, this is being
            // used to add it to the databound grid to avoid it getting overwritten when the
            // data refreshes.
            // Im also using this same thing to draw the buttons checked and unchecked states.
            // This is for custom holding logic which will still allow clicking.
            Goal goal = (Goal)item;

            if(goal.ID == 0)
                return CreateButtonTemplate;
            else if (goal.Checked)
                return CheckedTemplate;
            else
                return UncheckedTemplate;
        }
    }
}
