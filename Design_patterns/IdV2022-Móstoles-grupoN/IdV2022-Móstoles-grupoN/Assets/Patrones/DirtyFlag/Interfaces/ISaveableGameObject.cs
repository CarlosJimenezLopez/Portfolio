namespace Patrones.DirtyFlag.Interfaces
{
    public interface ISaveableGameObject
    {
        public bool NeedsSaving();

        public ISaveableGameObjectData GetSavedObject();
        public void ToGameObject(ISaveableGameObjectData data);
    }
}