using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace Blackjack
{
    public class Player
    {
        private Strategy plyrStrategy;
        private Hand[] hands;
    }
}