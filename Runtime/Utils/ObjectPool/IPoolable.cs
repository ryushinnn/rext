namespace RExt.Utils.ObjectPool {
    public interface IPoolable {
        void Activate();
        void Deactivate();
    }
}