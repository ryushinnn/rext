namespace RExt.Patterns.ObjectPool {
    public interface IPoolable {
        void Activate();
        void Deactivate();
    }
}