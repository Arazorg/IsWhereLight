using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _gameAssets;

    public static GameAssets gameAssets
    {
        get
        {
            if (_gameAssets == null)
            {
                _gameAssets = Instantiate(Resources.Load<GameAssets>("Prefabs/GameAssets"));
            }

            return _gameAssets;
        }
    }

    public Transform pfDamagePopup;
}
