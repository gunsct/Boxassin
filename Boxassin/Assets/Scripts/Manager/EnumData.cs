using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumData
{
    //지형지물
    public enum EnvironmentType { Forest, Sea }
    public enum EnviViewType { Forest, Sea }
    public enum OBJType { Obstacle, Item, Prob }
    public enum OBJState { Idle, Appear, Collision, Touch, Drag, Hold }
    public enum OBJCollisonType { Appear, Interactive, Touch, Drag, Hold }
    public enum OBJRateSet { Pass, Item, Obstacle }
    public enum BackOBJType { Close, Far, Particle, BackGround }
    //캐릭터
    public enum CharacterState { Fly, FlyDown, Run, Jump, Interactive, Out, Spawn, CantTouch }
    public enum FlyPath { FlyPath1, FlyPath2, FlyPath3 }
    public enum BackInteractOBJMovePath { MovePath1, MovePath2, MovePath3, MovePath4, MovePath5 }

    public enum BackInteractOBJType { Bird, Fish }
    public enum BackInteractOBJState { Idle, Spawn, Move, Collision }
}
