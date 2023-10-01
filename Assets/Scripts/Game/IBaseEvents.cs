public interface IBaseEvents 
{
    public delegate void MineWorkerRequested();
    public MineWorkerRequested OnMineWorkerRequested { get; set; }
    public delegate void SpaceEarned(int amount);
    public SpaceEarned OnSpaceEarned { get; set; }
    public delegate void SpaceSpent(int amount);
    public SpaceSpent OnSpaceSpent { get; set; }
    public delegate void SpaceRequested(int amount);
    public SpaceRequested OnSpaceRequested { get; set; }
    public delegate void SpaceGranted(int amount);
    public SpaceGranted OnSpaceGranted { get; set; }
    public delegate void SpaceRejected(int count,int amount);
    public SpaceRejected OnSpaceRejected { get; set; }
}
