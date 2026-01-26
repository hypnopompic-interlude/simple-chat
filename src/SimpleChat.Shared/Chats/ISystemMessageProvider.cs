using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.Shared.Chats;

public interface ISystemMessageProvider
{
    public string InvalidUserNameMessage {  get; }
    public string SystemName { get; }
    public string UserJoinedChatMessage { get; }
}
