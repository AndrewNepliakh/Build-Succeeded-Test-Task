namespace Entities
{
    public interface IHitReceiver
    {
        void ReceiveTap();
        public void SetCanReceiveTap(bool value);
    }
}