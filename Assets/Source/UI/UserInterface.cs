using Racing.UI.InGame;

namespace Racing.UI
{
    public static class UserInterface
    {
        public static IUserInterfaceController CurrentController { get; set; }
        public static IUserInterfaceView CurrentView { get; set; }

        public static T GetControllerAsType<T>()
        {
            return (T)CurrentController;
        }

        public static T GetViewAsType<T>()
        {
            return (T)CurrentView;
        }

    }
}
