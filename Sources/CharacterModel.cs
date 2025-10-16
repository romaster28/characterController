using System;

public class CharacterModel
{
    private readonly TransformModel _transform;
    private readonly ICharacterConfig _config;

    public CharacterModel(ICharacterConfig config)
    {
        _config = config ?? throw new ArgumentOutOfRangeException(nameof(config));
    }
}