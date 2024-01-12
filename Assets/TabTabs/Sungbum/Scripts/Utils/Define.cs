using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public const string SPLIT_PATTERN = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    public const string LINE_SPLIT_PATTERN = @"\r\n|\n\r|\n|\r";
    public static char[] TRIM_CHARS = { '\"' };

    public enum TileType//타일 넘버링
    {
        Non,            //0 빈 타일
        Wood,           //1 숲 타일 (자연)
        Mountain,       //2 산 타일 (자연)
        Sea,            //3 바다 (자연)
        Gold,           //4 돈
        PlayerEconomy,  //5 경제 시설(아군)
        PlayerScience,  //6 과학 시설(아군)
        PlayerMilitary, //7 군사 시설(아군)
        PlayerMagic,    //8 마법 시설(아군)
        EnemyEconomy,   //9 경제 시설(적군)
        EnemyScience,   //10 과학 시설(적군)
        EnemyMilitary,  //11 군사 시설(적군)
        EnemyMagic,     //12 마법 시설(적군)
        Lava,           //13 용암
        Desert,         //14 사막
        ThornyVine,     //15 가시넝쿨
        LightningRiver,  //16 번개 치는 호수
        End
    };

    public const int TOUCH_HANDLE_STAGE = 0;
    public const int TOUCH_HANDLE_BOTTOM = 1;

    public enum TouchHandleType
    {
        Stage,
        Bottom
    }

    public enum Scene
    {
        Unknown,
        Title,
        MainMenu,
        Game,
        Gache
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }
}
