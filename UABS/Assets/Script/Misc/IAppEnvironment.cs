namespace UABS.Assets.Script.Misc
{
    public interface IAppEnvironment
    {
        AppEnvironment AppEnvironment { get; }

        void Initialize(AppEnvironment appEnvironment);
    }
}