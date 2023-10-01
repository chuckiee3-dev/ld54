public class BaseEvents : IBaseEvents
{
    public IBaseEvents.MineWorkerRequested OnMineWorkerRequested { get; set; }
    public IBaseEvents.SpaceEarned OnSpaceEarned { get; set; }
    public IBaseEvents.SpaceSpent OnSpaceSpent { get; set; }
    public IBaseEvents.SpaceRequested OnSpaceRequested { get; set; }
    public IBaseEvents.SpaceGranted OnSpaceGranted { get; set; }
    public IBaseEvents.SpaceRejected OnSpaceRejected { get; set; }
}
