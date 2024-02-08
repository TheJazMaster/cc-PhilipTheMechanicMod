using Nickel;

namespace clay.PhilipTheMechanic;

internal interface IRegisterableCard
{
    static abstract void Register(IModHelper helper);
}

internal interface IRegisterableArtifact
{
    static abstract void Register(IModHelper helper);
}
