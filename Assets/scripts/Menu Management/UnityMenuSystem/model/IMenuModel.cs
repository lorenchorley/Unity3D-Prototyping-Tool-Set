
using System;

namespace menusystem {

    public interface IMenuModel {

        OnMenuReadySignal OnMenuReadySignal { get; set; }
        OpenMenuSignal OpenMenuSignal { get; set; }
        CloseMenuSignal CloseMenuSignal { get; set; }
        CloseTopMenuSignal CloseTopMenuSignal { get; set; }
        MenuBackSignal MenuBackSignal { get; set; }

        void Init();
        bool IsMenuOpen();
        MenuView GetTopMenu();
        void CloseTopMenu();
        void OpenMenu(Type type);
        void OpenMenu<T>() where T : MenuView;

    }

}

