﻿using System;
using System.Collections.Generic;

namespace Shared
{
    [Serializable]
    public class ServerState
    {
        public string Id { get; set; }
        public List<Player> players = new List<Player>();

        public ServerState() { }
        public ServerState(string id)
        {
            Id = id;
        }
        public ServerState(string id, Player player)
        {
            Id = id;
            players.Add(player);
        }
    }

    [Serializable]
    public class Player
    {
        public String Id { get; set; }
        public Player(String id)
        {
            Id = id;
        }
    }
}
