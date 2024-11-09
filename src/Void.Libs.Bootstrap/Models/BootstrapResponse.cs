using System.Text.Json.Serialization;
using Void.Libs.Bootstrap.Enums;

namespace Void.Libs.Bootstrap.Models;

/// <summary>
/// Response model that is returned as JSON when service is still in bootstrap (or failed) mode.
/// </summary>
public class BootstrapResponse
{
    [JsonPropertyName("state")]
    public BootstrapState State { get; set; }
    
    [JsonPropertyName("currentTask")]
    public string? CurrentTask { get; set; }
}