using CWJ;
using CWJ.Singleton.SwapSingleton;

//예시입니다
public class SingletonBehaviour_Swap_Example_Parent : SingletonBehaviourDontDestroy_Swap<SingletonBehaviour_Swap_Example_Parent>
{
    [ReadonlyConditional(EPlayMode.PlayMode)] public bool isChildA;
    public string value = "default";

    protected override sealed System.Type GetSwapType() => isChildA ? typeof(SingletonBehaviour_Swap_Example_ChildA) : typeof(SingletonBehaviour_Swap_Example_ChildB);

    protected override sealed void SwapSetting(SingletonBehaviour_Swap_Example_Parent swapObject)
    {
        swapObject.isChildA = isChildA;
        swapObject.value = value;
    }

}