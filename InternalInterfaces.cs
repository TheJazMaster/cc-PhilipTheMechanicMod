
namespace clay.PhilipTheMechanic;

internal interface IRegisterableCard
{
    static abstract Rarity GetRarity();
    static abstract Spr GetArt();
}

internal interface IRegisterableArtifact
{
    static abstract Spr GetSpriteForRegistering();
    static abstract ArtifactPool[] GetPools();
}
