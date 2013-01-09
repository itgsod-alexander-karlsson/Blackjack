using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
namespace Blackjack
{
    public class Shoe
    {
        private const int   CardsPerDeck = 52;

        private Card[]  cards;
        private int     numberOfDecks = 2;
        private int     shoeLocation;

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
        get{ return shoeLocation; }
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

    
}