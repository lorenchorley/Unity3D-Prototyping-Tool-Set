
using UnityEngine;
/// <summary>
/// A base menu class that implements parameterless Show and Hide methods
/// </summary>
namespace menusystem {

    [CreateAssetMenu(fileName = "MenuOptions", menuName = "Context/MenuOptions", order = 1)]
    public class MenuOptions : ScriptableObject {

        public GameObject[] MenuPrefabs;

    }

}