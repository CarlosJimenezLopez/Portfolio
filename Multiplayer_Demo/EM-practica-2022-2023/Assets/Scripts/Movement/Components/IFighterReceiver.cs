namespace Movement.Components
{
    public interface IFighterReceiver : IRecevier
    {
        public void Attack1();
        public void Attack2();
        public void TakeHit();
        public void Die();

        //public void Destroy();
        
        //Health GetComponent<T>();
        //public void TakeHitServerRpc();
        //public void DieServerRpc();
    }
}