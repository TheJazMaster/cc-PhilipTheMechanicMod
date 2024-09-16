namespace clay.PhilipTheMechanic.Actions;

public class AAddCardUpgraded : AAddCard
{
    public override Icon? GetIcon(State s)
    {
        if (card.upgrade != Upgrade.None) return new Icon(ModEntry.Instance.sprites["icon_addUpgradedCard"], amount, Colors.textBold);
        return base.GetIcon(s);
    }
}