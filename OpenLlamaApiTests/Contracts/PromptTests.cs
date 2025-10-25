using Microsoft.Extensions.AI;
using OpenLlamaAPI.Contracts;

namespace OpenLlamaApiTests.Contracts;

public class PromptTests
{
    public static IEnumerable<object[]> GetRoles()
    {
        yield return ["user", ChatRole.User];
        yield return ["User", ChatRole.User];
        yield return ["system", ChatRole.System];
        yield return ["System", ChatRole.System];
        yield return ["assistant", ChatRole.Assistant];
        yield return ["Assistant", ChatRole.Assistant];
        yield return ["tool", ChatRole.Tool];
        yield return ["Tool", ChatRole.Tool];
    }
    
    [Theory]
    [MemberData(nameof(GetRoles))]
    public void ToChatMessage_WithValidRoles_MapsExpectedChatRoleAndMessage(string role, ChatRole expected)
    {
        const string message = "Take that role";
        var prompt = new Prompt { Role = role, Message = message };
        var chatMessage = prompt.ToChatMessage();
        
        Assert.Equal(expected, chatMessage.Role);
        Assert.Equal(message, chatMessage.Text);
    }
    
    [Fact]
    public void ToChatMessage_WithInvalidRole_ThrowsException()
    {
        var prompt = new Prompt { Role = "invalid", Message = "Invalis role!" };
        
        Assert.Throws<ArgumentOutOfRangeException>(() => prompt.ToChatMessage());
    }
}