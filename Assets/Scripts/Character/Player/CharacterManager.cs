using UnityEngine;

/* [ClassINFO : CharacterManager]
   @ Description : - This class is used to manage the player game object and its components.
                   - The class is a singleton, meaning there will only be one instance of it in the game.
                   - The class holds a player instance and since the class is a singleton, it can always provide access to the player instance we need.
                   - And through the player property here, we can access all the components attached to the player game object.
   @ Attached at : gameObject(DontDestroyOnLoad) 
*/

public class CharacterManager : MonoBehaviour
{
    #region [CharacterManager Singleton instance]
    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }

            return _instance;
        }
    }
    #endregion

    #region [Player instance]
    public Player _player;
    public Player Player
    {
        get { return _player;  }
        set { _player = value; }
    }
    #endregion

    // ========================== // 
    //     [Unity LifeCycle]
    // ========================== //
    #region [Unity LifeCycle]
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }   
        }
    }
    #endregion
}
