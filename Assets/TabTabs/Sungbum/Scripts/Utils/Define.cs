using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public const string SPLIT_PATTERN = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    public const string LINE_SPLIT_PATTERN = @"\r\n|\n\r|\n|\r";
    public static char[] TRIM_CHARS = { '\"' };

    public enum TileType//Ÿ�� �ѹ���
    {
        Non,            //0 �� Ÿ��
        Wood,           //1 �� Ÿ�� (�ڿ�)
        Mountain,       //2 �� Ÿ�� (�ڿ�)
        Sea,            //3 �ٴ� (�ڿ�)
        Gold,           //4 ��
        PlayerEconomy,  //5 ���� �ü�(�Ʊ�)
        PlayerScience,  //6 ���� �ü�(�Ʊ�)
        PlayerMilitary, //7 ���� �ü�(�Ʊ�)
        PlayerMagic,    //8 ���� �ü�(�Ʊ�)
        EnemyEconomy,   //9 ���� �ü�(����)
        EnemyScience,   //10 ���� �ü�(����)
        EnemyMilitary,  //11 ���� �ü�(����)
        EnemyMagic,     //12 ���� �ü�(����)
        Lava,           //13 ���
        Desert,         //14 �縷
        ThornyVine,     //15 ���ó���
        LightningRiver,  //16 ���� ġ�� ȣ��
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
