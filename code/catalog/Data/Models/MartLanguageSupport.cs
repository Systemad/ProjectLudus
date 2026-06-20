using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class MartLanguageSupport
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// FK to mart_games.id — the game this language support belongs to.
    /// </summary>
    public long? Game { get; set; }

    /// <summary>
    /// FK to languages.id — the supported language.
    /// </summary>
    public long? Language { get; set; }

    /// <summary>
    /// FK to language_support_types.id — type of support (interface, subtitles, audio).
    /// </summary>
    public long? LanguageSupportType { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Checksum { get; set; } = null!;
}
