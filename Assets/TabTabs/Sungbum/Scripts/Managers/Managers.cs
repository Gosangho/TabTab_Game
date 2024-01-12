using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성이 보장된다
    static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

    #region Contents
    readonly GameManagerEx game = new();

    public static GameManagerEx Game => Instance.game;
    #endregion

    #region Core
    readonly DataManager data = new();
    readonly InputManager input = new();
    readonly PoolManager pool = new();
    readonly ResourceManager resource = new();
    readonly SceneManagerEx scene = new();
    readonly SoundManager sound = new();
    readonly UIManager ui = new();

    public static DataManager Data => Instance.data;
    public static InputManager Input => Instance.input;
    public static PoolManager Pool => Instance.pool;
    public static ResourceManager Resource => Instance.resource;
    public static SceneManagerEx Scene => Instance.scene;
    public static SoundManager Sound => Instance.sound;
    public static UIManager UI => Instance.ui;
	#endregion

	void Start()
    {
        Init();
	}

    void Update()
    {
        input.OnUpdate();
    }

    public static void Init()
    {
        if (s_instance == null)
        {
			GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance.data.Init();
            s_instance.pool.Init();
            s_instance.sound.Init();
        }		
	}

    public static void Clear()
    {
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
