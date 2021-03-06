﻿using System.Runtime.Remoting.Messaging;

namespace GameEngine.Util.Mediator
{
    public interface ISystemsMediator
    {
        void sendMessage(MediatorMessage message);
    }
}