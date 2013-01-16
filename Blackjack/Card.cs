using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Resources;
using System.Collections;
using System.Globalization;
using System.Reflection;
using Nancy;

namespace Blackjack
{
    public class Card : ICloneable
    {
        private CardType cardType;
        private int value;
        private int trueValue;

        public enum CardType
        {
            Ace = 0,
            Two = 1,
            Three = 2,
            Four = 3,
            Five = 4,
            Six = 5,
            Seven = 6,
            Eight = 7,
            Nine = 8,
            Ten = 9,
            Jack = 10,
            Queen = 11,
            King = 12
        }
        public int Value
        {
            get { return value; }
        }

        public CardType FaceValue
        {
            get { return cardType; }
        }

        public int TrueValue
        {
            get { return trueValue; }
        }

        public Card Clone()
        {
            return new Card(cardType);
        }

        public static Card Clone(Card card)
        {
            return new Card(card.FaceValue);
        }
    }
}