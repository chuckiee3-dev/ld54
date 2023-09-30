
public class UpgradeProvider : IUpgradeProvider
{
    private readonly int[] MineWordModifier= new []{0, 1 ,3};
    private readonly int[] MineSpaceRequirement= new []{1, 3};
    private readonly int[] TowerSpaceRequirement= new []{3, 5};

    public int GetMineLengthModifier()
    {
        return MineWordModifier[0];
    }

    public int GetMineSpaceRequirement()
    {
        return MineSpaceRequirement[0];
    }

    public int GetTowerSpaceRequirement()
    {
        return TowerSpaceRequirement[0];
    }
}
