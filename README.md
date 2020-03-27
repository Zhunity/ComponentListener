# ComponentListener
添加Inspector上的Add Compinent回调功能

介绍地址：
https://zhuanlan.zhihu.com/p/109791861


 public static class ObjectFactory
    {
        const int kInvalidSceneHandle = 0;

        [FreeFunction(ThrowsException = true)]
        static extern Object CreateDefaultInstance([NotNull] Type type);

        [FreeFunction(ThrowsException = true)]
        static extern Component AddDefaultComponent([NotNull] GameObject gameObject, [NotNull] Type type);

        [FreeFunction]
        static extern GameObject CreateDefaultGameObject(string name);

public static event Action<Component> componentWasAdded;


添加Inspector上的Add Component回调功能之后日谈
https://zhuanlan.zhihu.com/p/118605767?just_published=2
