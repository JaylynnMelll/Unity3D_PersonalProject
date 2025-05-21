using UnityEngine;

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
