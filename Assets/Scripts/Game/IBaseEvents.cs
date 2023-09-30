public interface IBaseEvents 
{
    public delegate void MineWorkerRequested();
    public MineWorkerRequested OnMineWorkerRequested { get; set; }
}
