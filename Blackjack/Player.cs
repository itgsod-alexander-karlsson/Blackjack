using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace Blackjack
{
    public class Player
    {
        private Point location;
        private double bank;
        private string backgroundImage;
        private Strategy plyrStrategy;
        private CountMethod plyrMethod;
        private int currentHandIndex = 0;
        private NumericUpDown plyrBet;
        private Hand[] hands;
        private int numberOfHands = 1;
        private playerType plyrType;

        public enum LabelType
        {
            none = 0,
            drawToHand = 1,
            bothHands = 2,
            outcome = 3
        }

        public enum playerType
        {
            computer = 0,
            human = 1
        }

        public Player(Point pntlocation, double dblBank, NumericUpDown betControl, playerType type, Strategy strategy, CountMethod method)
        {
            // Since the game is limited to one split (two hands), just set them up now
            hands = new Hand[2];
            hands[0] = new Hand(new Point(0, 0));
            hands[1] = new Hand(new Point(50, 0));

            // Player specific variables
            location = pntlocation;
            bank = dblBank;
            plyrStrategy = strategy;
            plyrMethod = method;
            plyrType = type;
            plyrBet = betControl;

            // Start out with one hand, they may split pairs to get two
            numberOfHands = 1;
        }

        ~Player()
        {
            try
            {
                // When we destroy the player object, get rid of the betting control too
                // But first make it invisible in case the garbage collector is slow.
                plyrBet.Visible = false;
                plyrBet.Refresh();
                plyrBet.Dispose();
            }
            catch { }

        }

        public void MoveControls(float scaleX, float scaleY)
        {
            plyrBet.Left = (int)((location.X + 110) * scaleX);
            plyrBet.Top = (int)((location.Y - 20) * scaleY);
        }

        public void DrawBackground(Graphics drawingSurface, Card dealerCard)
        {
            // Draw different colored backgrounds to match the advice
            switch (GetAdvice(dealerCard))
            {
                case Strategy.AdviceType.Hit:
                    drawingSurface.DrawImage(Resources.GetImage(backgroundImage + "_hit"), location);
                    break;
                case Strategy.AdviceType.Stand:
                    drawingSurface.DrawImage(Resources.GetImage(backgroundImage + "_stand"), location);
                    break;
                case Strategy.AdviceType.Split:
                    drawingSurface.DrawImage(Resources.GetImage(backgroundImage + "_split"), location);
                    break;
                case Strategy.AdviceType.DoubleDown:
                    drawingSurface.DrawImage(Resources.GetImage(backgroundImage + "_ddown"), location);
                    break;
                case Strategy.AdviceType.None:
                    drawingSurface.DrawImage(Resources.GetImage(backgroundImage + "_noadvice"), location);
                    break;
                default:
                    break;
            }
        }

        public Hand[] GetHands()
        {
            return hands;
        }

        public int NumberOfHands
        {
            get { return numberOfHands; }
        }

        public Hand CurrentHand
        {
            // This is just a shortcut to get to the current hand being played
            get { return hands[currentHandIndex]; }
        }

        public void Won(Hand hand)
        {
            // Since the bet is taken from the bank at the beginning of play,
            // give back twice as much on a win.
            bank += hand.Wager * 2;
        }

        public void Blackjack(Hand hand)
        {
            // Since the bet is taken from the bank at the beginning of play,
            // give it back plus 1.5 times the wager.
            bank += hand.Wager + hand.Wager * 1.5;
        }

        public void Push(Hand hand)
        {
            // Since the bet is taken from the bank at the beginning of play,
            // give it back on a push.
            bank += hand.Wager;
        }

        public void Reset()
        {
            // This method is called at the beginning of a new hand.
            hands = new Hand[2];
            hands[0] = new Hand(new Point(0, 0));
            hands[1] = new Hand(new Point(50, 0));

            // Take away the betting control during play
            plyrBet.Visible = false;

            // Reset variables that keep track of play
            numberOfHands = 1;
            currentHandIndex = 0;
        }

        public void GetWager()
        {
            // Reset the wager to match what the user entered.
            if (plyrType == playerType.computer && plyrMethod != null)
                hands[0].Wager = plyrMethod.GetWager((double)plyrBet.Value);
            else
                hands[0].Wager = (double)plyrBet.Value;

            // Reduce the bank up front
            bank -= hands[0].Wager;
        }

        public bool DoubleDown(Hand hand)
        {
            // This method determines whether a double-down is possible and, 
            // if so, doubling the bet.
            int handTotal = hand.Total();

            if (((handTotal >= 7 && handTotal <= 11) || hand.IsSoft) && hand.Count == 2)
            {
                // Reduce the bank
                bank -= hand.Wager;

                // Double the bet
                hand.Wager *= 2;

                // Mark the hand as doubled so the last card is drawn at an angle
                hand.Doubled = true;

                // Tell the form that we doubled.  The form then moves on to the next player.
                return true;
            }
            return false;
        }

        public bool Split()
        {
            // This method determines if a split is possible and,
            // if so, create the new hand and double the bet
            if (hands[0].Count == 2 && numberOfHands == 1)
            {
                if (hands[0][0].FaceValue == hands[0][1].FaceValue)
                {
                    // Move the first hand to the left 50 pixels.
                    hands[0].HandLocation = new Point(hands[0].HandLocation.X - 50, hands[0].HandLocation.Y);
                    // Put the second card of the first hand into the first card of the second hand
                    hands[1].Add(hands[0][1]);
                    // Remove the card from the first hand
                    hands[0].RemoveAt(hands[0].Count);
                    // Make the second hand's wager equal to the first
                    hands[1].Wager = hands[0].Wager;
                    // Decrement the bank accordingly
                    bank -= hands[1].Wager;
                    // increase the number of hands
                    numberOfHands++;
                    // Tell the form that the split was successful
                    return true;
                }
            }
            return false;
        }

        public void NextHand()
        {
            // This is just a shortcut to incrementing the hand index
            currentHandIndex++;
        }

        public bool LastHand()
        {
            // This is a convenient way to determine if the player has any more
            // hands to draw to.
            return currentHandIndex + 1 == numberOfHands;
        }

        public double TotalWager()
        {
            // The total wager for the player is the sum of all the hand's wagers
            double wager = 0;

            foreach (Hand hand in hands)
            {
                wager += hand.Wager;
            }

            return wager;
        }

        public CountMethod Method
        {
            get { return plyrMethod; }
            set { plyrMethod = (CountMethod)value; }
        }

        public Strategy CardStrategy
        {
            get { return plyrStrategy; }
            set { plyrStrategy = (Strategy)value; }
        }

        public playerType Type
        {
            get { return plyrType; }
            set { plyrType = value; }
        }

        public NumericUpDown Bet
        {
            get { return plyrBet; }
        }

        public void DrawHands(Graphics drawingSurface, Player.LabelType labelType, Hand dealerHand, bool currentPlayer)
        {
            // This routine is responsible for drawing the player's cards and the appropriate label
            foreach( Hand hand in hands )
            {
            // Increment the drawing position
            int x = location.X + hand.HandLocation.X;
            int y = location.Y + hand.HandLocation.Y;

            // Make sure there are cards in the hand to draw.
            if( hand.Count > 0 )
            {
                // Draw the appropriate label type in the upper left corner
                switch( labelType )
                {
                    case LabelType.none:
                        break;
                    case LabelType.bothHands:
                        drawingSurface.DrawString( hand.Label(numberOfHands==1), new Font("Arial",8,FontStyle.Bold), new SolidBrush(Color.Yellow), x-10, y-20 );
                        break;
                    case LabelType.drawToHand:
                        if( hand == CurrentHand || !currentPlayer )
                            drawingSurface.DrawString( hand.Label(numberOfHands==1), new Font("Arial",8,FontStyle.Bold), new SolidBrush(Color.Yellow), x-10, y-20 );
                        break;
                    case LabelType.outcome:
                       switch( hand.Outcome( dealerHand, numberOfHands ))
                       {
                           case Hand.OutcomeType.Won:
                           case Hand.OutcomeType.Blackjack:
                               drawingSurface.DrawString("WON", new Font("Arial",8,FontStyle.Bold), new SolidBrush(Color.LimeGreen), x-10, y-20 );
                               break;
                           case Hand.OutcomeType.Lost:
                               drawingSurface.DrawString("LOST", new Font("Arial",8,FontStyle.Bold), new SolidBrush(Color.Crimson), x-10, y-20 );
                               break;
                           case Hand.OutcomeType.Push:
                               drawingSurface.DrawString("PUSH", new Font("Arial",8,FontStyle.Bold), new SolidBrush(Color.Yellow), x-10, y-20 );
                               break;
                       }
                       break;
                }

                // Increment the drawing position
                x += (int)Card.cardSpacing.Width;
                y += (int)Card.cardSpacing.Height;

                // Draw the cards.
                int cardNumber = 0;
                foreach( Card card in hand )
                {
                    if( card != null )
                    {
                        cardNumber++;
                        card.Draw( drawingSurface, new Point(x, y), true, currentPlayer && hand!=CurrentHand, hand.Doubled && cardNumber==3 );
                        x += (int)Card.cardSpacing.Width;
                        y += (int)Card.cardSpacing.Height;
                    }
                }
            }

                // Draw the bet
                drawingSurface.DrawString( "$" + TotalWager().ToString(CultureInfo.InvariantCulture), new Font("Arial",8,FontStyle.Bold), new SolidBrush(Color.DarkKhaki), location.X+110, location.Y-20 );

                // Draw the bank
                drawingSurface.DrawString( "$" + bank.ToString(CultureInfo.InvariantCulture), new Font("Arial",8,FontStyle.Bold), new SolidBrush(Color.DarkKhaki), location.X+130, location.Y );

                // Draw the running card count
                if( plyrMethod != null )
                    {
                        //drawingSurface.DrawString( plyrMethod.MethodName, new Font("Arial",6,FontStyle.Bold), new SolidBrush(Color.DarkKhaki), location.X+140, location.Y+20 );
                        drawingSurface.DrawString( plyrMethod.GetWager((double)plyrBet.Value).ToString("F0"), new Font("Arial",6,FontStyle.Bold), new SolidBrush(Color.DarkKhaki), location.X+140, location.Y+20 );
                        drawingSurface.DrawString( plyrMethod.Count.ToString("F1",CultureInfo.InvariantCulture), new Font("Arial",6,FontStyle.Bold), new SolidBrush(Color.DarkKhaki), location.X+145, location.Y+40 );
                }

            }

            public Strategy.AdviceType GetAdvice( Card dealerCard )
            {
                // Get the advice for this player's chosen strategy
                return hands[currentHandIndex].GetAdvice( dealerCard, plyrStrategy, !(numberOfHands == 2), plyrMethod!=null ? plyrMethod.Count : 0 );
            }

            public void ResetCount( int decks )
            {
                if( plyrMethod != null )
                plyrMethod.Reset( decks );
            }

            public void CountCard( Card newCard )
            {
                if( plyrMethod != null )
                plyrMethod.CountCard( newCard );
            }
        }
    }
}
