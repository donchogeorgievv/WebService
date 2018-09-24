namespace MF.Classes
{
    public interface IContext
    {
        int ResolveVariable(string name);
        int CallFunction(string name, int[] arguments);
    }
}
