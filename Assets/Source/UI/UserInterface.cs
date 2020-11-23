namespace Racing.UI
{
    public static class UserInterface
    {
        public static IUserInterfaceController CurrentController { get; set; }
        public static IUserInterfaceView CurrentView { get; set; }

        /// <summary>
        /// Returns the current UI Controller as type T
        /// </summary>
        public static T GetControllerAsType<T>()
        {
            return (T)CurrentController;
        }

        /// <summary>
        /// Returns the current UI View as type T
        /// </summary>
        public static T GetViewAsType<T>()
        {
            return (T)CurrentView;
        }

    }
}
