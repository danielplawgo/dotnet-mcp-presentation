using ModelContextProtocol;
using ModelContextProtocol.Client;
using Xunit;

namespace McpDemo.Tests;

public sealed class ToolContractTests(McpDemoApplication app) : IClassFixture<McpDemoApplication>
{
    private const string UserKey = "demo-user-key";
    private const string AdminKey = "demo-admin-key";

    [Fact]
    public async Task User_role_does_not_see_the_destructive_tool()
    {
        await using var client = await app.ConnectAsync(UserKey);

        var names = (await client.ListToolsAsync()).Select(tool => tool.Name).ToArray();

        Assert.Contains("GetOrder", names);
        Assert.DoesNotContain("CancelOrder", names);
    }

    [Fact]
    public async Task Admin_role_sees_the_destructive_tool()
    {
        await using var client = await app.ConnectAsync(AdminKey);

        var names = (await client.ListToolsAsync()).Select(tool => tool.Name).ToArray();

        Assert.Contains("CancelOrder", names);
    }

    [Fact]
    public async Task Hiding_a_tool_is_not_the_same_as_protecting_it()
    {
        await using var client = await app.ConnectAsync(UserKey);

        // Narzędzia nie ma na liście dla tej roli, ale klient i tak może spróbować je wywołać.
        // Odmowa autoryzacji wraca jako błąd protokołu, a nie jako wynik narzędzia z isError.
        var error = await Assert.ThrowsAnyAsync<McpException>(
            async () => await client.CallToolAsync("CancelOrder", new Dictionary<string, object?> { ["number"] = "456" }));

        Assert.Contains("authorization", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Read_tools_are_annotated_as_read_only()
    {
        await using var client = await app.ConnectAsync(AdminKey);
        var tools = await client.ListToolsAsync();

        var getOrder = tools.Single(tool => tool.Name == "GetOrder");

        Assert.True(getOrder.ProtocolTool.Annotations?.ReadOnlyHint);
    }

    [Fact]
    public async Task Destructive_tool_declares_its_intent_to_the_host()
    {
        await using var client = await app.ConnectAsync(AdminKey);
        var tools = await client.ListToolsAsync();

        var cancelOrder = tools.Single(tool => tool.Name == "CancelOrder").ProtocolTool;

        Assert.False(cancelOrder.Annotations?.ReadOnlyHint);
        Assert.True(cancelOrder.Annotations?.DestructiveHint);
        Assert.True(cancelOrder.Annotations?.IdempotentHint);
    }

    [Fact]
    public async Task Confirmation_is_not_an_argument_the_model_can_fill()
    {
        await using var client = await app.ConnectAsync(UserKey);
        var tools = await client.ListToolsAsync();

        var createTicket = tools.Single(tool => tool.Name == "CreateSupportTicket");

        Assert.DoesNotContain("confirmed", createTicket.JsonSchema.ToString());
    }

    [Fact]
    public async Task Failures_tell_the_model_how_to_fix_the_call()
    {
        await using var client = await app.ConnectAsync(UserKey);

        var result = await client.CallToolAsync("GetOrder", new Dictionary<string, object?> { ["number"] = "999" });

        Assert.True(result.IsError);
        Assert.Contains("123 and 456", string.Concat(result.Content.OfType<ModelContextProtocol.Protocol.TextContentBlock>().Select(block => block.Text)));
    }
}
