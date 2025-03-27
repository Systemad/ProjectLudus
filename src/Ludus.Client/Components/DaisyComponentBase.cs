using Microsoft.AspNetCore.Components;
using TailwindMerge;

namespace Ludus.Client.Components;

public abstract class DaisyComponentBase : ComponentBase
{
    [Inject]
    internal TwMerge TwMerge { get; set; } = default!;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
