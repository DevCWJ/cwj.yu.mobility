using CWJ;
using CWJ.Singleton;

public class SingletonExample : SingletonBehaviour<SingletonExample>
{
    [InvokeButton]
    private void GetInstance()
    {
        UpdateInstance();
    }


    protected override void OnAfterInstanceAssigned()
    {
        //Debug.LogWarning($"name : {gameObject.name}\n" + CWJ.ReflectionExtension.GetPrevMethodInfo());
    }

    protected override void _Awake()
    {
        //Debug.LogWarning($"name : {gameObject.name}\n" + CWJ.ReflectionExtension.GetPrevMethodInfo());
    }
}