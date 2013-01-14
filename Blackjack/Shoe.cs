using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
namespace Blackjack
{
    public class Shoe
    {
        private const int CardsPerDeck = 52;

        private Card[] cards;
        private int numberOfDecks = 4;
        private int shoeLocation;

        public delegate void ShoeEventHandler(object sender, EventArgs e);

        public event ShoeEventHandler EndofShoeEvent;
        public event ShoeEventHandler ShuffleEvent;

        public int NumberOfDecks
        {
            get
            {
                return numberOfDecks;
            }
            set
            {

                if (numberOfDecks != value)
                    numberOfDecks = value;
            }
        }

        public int ShoeLocation
        {
            get { return shoeLocation; }
        }

        public void Init()
    {
        shoeLocation = 0;

        cards = new Card[numberOfDecks * CardsPerDeck];
        int current = 0;

        for ( int j=0; j < 4; j++)
        {
             for (int y = 0; y < 4; y++)
        {
                 for (int x = 0; x < 13; x++)
                 {
                     cards[current++] = new Card (Card.CardType)x, (Card.Suits)y );
                 }
             }
        }
    }

        //End-of-Deck property
        public bool EoD
        {
            get { return (shoeLocation >= cards.Length * .66); }
        }

        //Beginning-of-Deck property
        public bool BoD
        {
            get { return (shoeLocation == 0); }
        }

        public void Shuffle()
        {
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < rand.Next(10, 100); i++)
            {
                shoeLocation = 0;

                Card[] shuffledCards = new Card[numberOfDecks * CardsPerDeck];

                Random index = new Random(unchecked((int)DateTime.Now.Ticks));
                int upperLimit = cards.GetUpperBound(0);

                for (int j = 0; j < numberOfDecks * CardsPerDeck; j++)
                {
                    int k = index.Next(0, upperLimit);
                    shuffledCards[j] = cards[k];
                    cards[k] = cards[upperLimit--];
                }

                cards = shuffledCards;
            }

            ShuffleEvent(this, EventArgs.Empty);
        }

        //Indexer
        public Card this[int index]
        {
            get
            {
                if (index < 0 || index > 52)
                    throw new ArgumentOutOfRangeException();
                else
                    return cards[index];
            }
        }

        public Card Next()
        {
            shoeLocation++;

            if (shoeLocation < cards.GetUpperBound(0))
            {
                if(EoD)
                    EndofShoeEvent(this, EventArgs.Empty);

                return cards[shoeLocation];
            }
            else
            {
            //Shuffles cards of shoes, if cards run out
                Shuffle();
                return cards[shoeLocation];
            }
        }
    }
}