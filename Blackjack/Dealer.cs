using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace Blackjack
{
    /// The dealer object is fairly simple since he has only one hand of cards
    /// and no bank.  The bank in this game is unlimited and money comes out of 
    /// thin air to pay the players (if they win).
    public class Dealer
    {
        private Hand dealerHand;
        private Point location;

        public Dealer(Point pntlocation)
        {
            // Tell the dealer where his origin point is
            location = pntlocation;

            // Create a hand for the dealer and locate its origin at the dealer's origin
            dealerHand = new Hand(new Point(0, 0));
        }

        public void Reset()
        {
            // Get a new hand for each round
            dealerHand = new Hand(new Point(0, 0));
        }

        public void AddCard(Card card)
        {
            // Take a card
            dealerHand.Add(card);
        }
        public int Total()
        {
            // The dealer's total is his hand's total.  This is just a shortcut to it.
            return dealerHand.Total();
        }

        public Hand Hand
        {
            get { return dealerHand; }
        }
    }
}