using Microsoft.EntityFrameworkCore;
using NodaTime;
using Shared.Features.Webhooks;
using SyncService.Data;

namespace SyncService.Features.Games.Webhook;

public class WebhookDatabaseService(SyncDbContext context)
{
    public async Task AddActiveWebhookAsync(WebhookResponse response)
    {
        var active = new ActiveWebhook
        {
            Id = response.Id,
            Url = response.Url,
            Category = response.Category,
            SubCategory = response.SubCategory,
            Active = response.Active,
            CreatedAt = response.CreatedAt,
            UpdatedAt = response.UpdatedAt,
            LastProcessedAt = SystemClock.Instance.GetCurrentInstant()
        };

        context.ActiveWebhooks.Add(active);
    }
    public async Task<List<ActiveWebhook>> GetWebhooksAsync()
    {
        var webhooks = await context.ActiveWebhooks.ToListAsync();
        return webhooks;
    }

    // TODO: ? Add endpoints, and in that way make this "more flexible"
    public async Task UpdateWebhookStatusAsync(
        WebhookMethods method)
    {
        var hook = await context.ActiveWebhooks.FirstOrDefaultAsync(g => g.SubCategory == (int)method);
        if (hook is not null)
        {
            hook.LastProcessedAt = SystemClock.Instance.GetCurrentInstant();
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteWebhooksAsync()
    {
        await context.ActiveWebhooks.ExecuteDeleteAsync();
    }

}