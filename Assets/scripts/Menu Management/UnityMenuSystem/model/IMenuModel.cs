
using System;

namespace menusystem {

    public interface IMenuModel {

        MenuSystemReadySignal OnMenuReadySignal { get; set; }
        OpenMenuSignal OpenMenuSignal { get; set; }
        CloseMenuSignal CloseMenuSignal { get; set; }
        CloseTopMenuSignal CloseTopMenuSignal { get; set; }
        MenuBackSignal MenuBackSignal { get; set; }

        void Init();
        bool IsMenuOpen();
        MenuView GetTopMenu();
        void CloseTopMenu();
        void OpenMenu(Type type);
        void OpenMenu(Type type, Action<MenuView> menuReady);
        void OpenMenu<T>() where T : MenuView;
        void OpenMenu<T>(Action<T> menuReady) where T : MenuView;

    }

}

