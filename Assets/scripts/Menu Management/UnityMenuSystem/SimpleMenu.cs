/// <summary>
/// A base menu class that implements parameterless Show and Hide methods
/// </summary>

namespace menusystem {

    public abstract class SimpleMenu<T> : MenuView<T> where T : SimpleMenu<T> {

        public static void Show() {
            Open();
        }

        public static void Hide() {
            Close();
        }

    }

}