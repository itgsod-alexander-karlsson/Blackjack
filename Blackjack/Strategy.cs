using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace Blackjack
{
    public class Strategy
    {
        public const int S = 100;  // Stand
        public const int H = 200;  // Hit
        public const int D = 300;  // Double Down
        public const int P = 400;  // Split
        public const int N = -100;  // None
        public const int L = -200;  // Null

        public enum AdviceType    
    {
        None = N,
        Hit = H,
        Stand = S,
        DoubleDown = D,
        Split = P,
        Null = L
    }

    protected string name = "";
    protected int fontSize = 8;
    protected string fontName = "Arial";
    protected int[,] Pairs;
    protected string[] PairsLabels;
    protected int[,] Aces;
    protected string[] DoubleLabels;
    protected int[,] DoubleH;
    protected int[,] DoubleS;
    protected string[] AcesLabels;
    protected int[,] Hand;
    protected string[] HandLabels;

    protected Strategy() {}
    public abstract AdviceType GetAdvice( Hand h, Card c, bool b, double cc );
    public virtual string StrategyName{ get{ return ""; }
    {
       SizeF labelSize;

       // Get largest string and move the table over to make room for the labels
       try
       {
        labelSize = drawingSurface.MeasureString("Seven",new Font(fontName,fontSize));
       }
       catch
       {
        labelSize = drawingSurface.MeasureString("Seven",new Font("Arial",fontSize));
       }

       // Draw the Player's header column
       //   StringFormat myVerticalFormat = new StringFormat();
       //   myVerticalFormat.FormatFlags = StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft;
       //   myVerticalFormat.Alignment = StringAlignment.Center;
       //   g.DrawString("Player's Hand",new Font(fontName,fontSize),Brushes.Black,origin.X + (int)labelSize.Width+ (int)labelSize.Height + size.Width*10 + space.Width*10,origin.Y + dealerRowHeight + (size.Height*10 + space.Height*10)/2,myVerticalFormat);
   
       // Draw the actual Advice values, Total | Aces | Pairs
       myFormat.Alignment = StringAlignment.Center;
       myFormat.LineAlignment = StringAlignment.Center;

       for(int j=0; j<=Hand.GetUpperBound(0); j++)
}