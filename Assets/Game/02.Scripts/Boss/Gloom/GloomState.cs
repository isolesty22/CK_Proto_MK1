using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomState : BossState
{
    protected GloomController gloom;
}

public class GloomState_Idle : GloomState
{
    public GloomState_Idle(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = true;
    }
}

public class GloomState_Threat : GloomState
{
    public GloomState_Threat(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;

        gloom.SetTrigger("Threat_Start");
    }
}

public class GloomState_Obstruct : GloomState
{
    /// <summary>
    /// 사용 가능한 인덱스들이 담긴 리스트
    /// </summary>
    private List<int> usableIndex;

    /// <summary>
    /// 이미 사용한 인덱스들이 담긴 리스트
    /// </summary>
    private List<int> usedIndex;

    private int currentIndex;
    public GloomState_Obstruct(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;
        currentIndex = -1;
        usableIndex = new List<int> { 0, 1, 2 };
        usedIndex = new List<int>();
        gloom.SetTrigger("Obstruct_Start");
        gloom.StartCoroutine(ProcessSkill());
    }

    private IEnumerator ProcessSkill()
    {
        yield return null;

        for (int i = 0; i < 9; i++)
        {
            Debug.Log("방해 인덱스 : " + GetUsablePositionIndex());
            yield return null;
        }
    }

    /// <summary>
    /// 사용 가능한 인덱스를 반환하고, 해당 인덱스를 '이미 사용한 인덱스 리스트'에 넣습니다.
    /// </summary>
    /// <returns></returns>
    private int GetUsablePositionIndex()
    {
        int currentPosIndex = -1;
        if (usableIndex.Count > 1)
        {
            //랜덤한 인덱스를 가져옴
            currentIndex = Random.Range(0, usableIndex.Count);

            //usableIndex 속의 값을 PosIndex에 넣음
            currentPosIndex = usableIndex[currentIndex];

            usedIndex.Add(currentPosIndex);
            usableIndex.RemoveAt(currentIndex);
        }
        else // 사용할 수 있는 인덱스가 하나밖에 없으면
        {

            //일단 남은 하나를 할당시켜줌 
            currentPosIndex = usableIndex[0];
            currentIndex = 0;

            //사용한 인덱스에 추가
            usedIndex.Add(currentPosIndex);

            //사용 불가능하게 설정
            usableIndex.RemoveAt(0);

            //usedIndex에 먼저 담겨있던 두 개의 인덱스를 사용 가능하게 변경
            usableIndex.Add(usedIndex[0]);
            usableIndex.Add(usedIndex[1]);

            //usedIndex에 먼저 담겨있던 두 개의 인덱스를 삭제
            usedIndex.RemoveRange(0, 2);
        }

        return currentPosIndex;
    }

}

public class GloomState_Chase : GloomState
{
    public GloomState_Chase(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;
    }
}

public class GloomState_Berserk : GloomState
{
    public GloomState_Berserk(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;
    }
}

public class GloomState_Die : GloomState
{
    public GloomState_Die(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;
    }
}

