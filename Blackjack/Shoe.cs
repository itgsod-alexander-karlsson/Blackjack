using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
namespace Blackjack
{
    public class Shoe
    {
        private Card[] cards;
    }

    for(int k=0; k<2; k++)
    {
        foreach(Player player in players)
        {
            player.GetHands()[0].Add(shoe.Next());
        }
        dealer.Hand.Add(shoe.Next());
    }
    
}