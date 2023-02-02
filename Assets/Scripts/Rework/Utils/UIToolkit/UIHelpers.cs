namespace CodesmithWorkshop.UIToolkit
{
    using UnityEngine.UIElements;

    public static class UIHelpers
    {
        public static void DelayAddToClassList(
            VisualElement ui,
            string classToAdd = "animate",
            int delay = 100
        )
        {
            ui.schedule.Execute(() => ui.ToggleInClassList(classToAdd)).StartingIn(delay);
        }
    }
}
