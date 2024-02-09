using Nickel;

namespace clay.PhilipTheMechanic;

internal interface IRegisterableCard
{
    static abstract Rarity GetRarity();
}

internal interface IRegisterableArtifact
{
    static abstract void Register(IModHelper helper);
}
