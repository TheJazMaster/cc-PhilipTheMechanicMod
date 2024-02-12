using Nickel;

namespace clay.PhilipTheMechanic;

internal interface IRegisterableCard
{
    static abstract Rarity GetRarity();
    static abstract Spr GetArt();
}

internal interface IRegisterableArtifact
{
    static abstract void Register(IModHelper helper);
}
