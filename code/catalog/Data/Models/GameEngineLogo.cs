using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class GameEngineLogo
{
    public long Id { get; set; }

    public long GameEngineId { get; set; }

    public bool? AlphaChannel { get; set; }

    public bool? Animated { get; set; }

    public long? Height { get; set; }

    public string? ImageId { get; set; }

    public string Url { get; set; } = null!;

    public long? Width { get; set; }

    public string Checksum { get; set; } = null!;

    public virtual ICollection<GameEngine> GameEngines { get; set; } = new List<GameEngine>();
}
