using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {
	class Program {
		static void Main(string[] args) {
			bool quit = false;

			while (!quit) {

				Console.WriteLine("Start Game\n");

				// generate a new whole deck
				WholeDeckCards wholeDeckcards = new WholeDeckCards();
				wholeDeckcards.generateDeck();

				// shuffle deck
				wholeDeckcards.shuffleCards();
				Console.WriteLine("the shuffled deck are showing below: ");
				wholeDeckcards.printCards();
				Console.WriteLine("\n");

				// deal cards to 4 players
				Console.WriteLine("deal cards to 4 players");

				CardGroup player1Deck = wholeDeckcards.popCards(0, 5);
				CardGroup player2Deck = wholeDeckcards.popCards(0, 5);
				CardGroup player3Deck = wholeDeckcards.popCards(0, 5);
				CardGroup player4Deck = wholeDeckcards.popCards(0, 5);

				HandCards player1HandCards = HandCards.CreateInstance(player1Deck);
				HandCards player2HandCards = HandCards.CreateInstance(player2Deck);
				HandCards player3HandCards = HandCards.CreateInstance(player3Deck);
				HandCards player4HandCards = HandCards.CreateInstance(player4Deck);

				Console.WriteLine("\n");

				if (player1HandCards != null && player2HandCards != null && player3HandCards != null && player4HandCards != null) {
					List<HandCards> handCardsList = new List<HandCards> {
					player1HandCards,
					player2HandCards,
					player3HandCards,
					player4HandCards,
				};

					int numOfPlayer = 1;
					foreach (HandCards handCards in handCardsList) {
						numOfPlayer += 1;
						Console.WriteLine($"player{numOfPlayer}'s hand:");
						handCards.printCards();
						Console.Write($"player{numOfPlayer}'s hand Categoery: ");
						Console.WriteLine(handCards.handCategory);
						Console.WriteLine("-------------------------------------------------------------");
					}

					// compare category
					HandCards.HandCategory largesetCategory = getLargestCategory(handCardsList);
					Console.WriteLine($"the largest category is {largesetCategory}");
					Console.WriteLine("\n");

					// get player's who have largest category
					List<HandCards> lCHandCardsList = new List<HandCards>();

					foreach (HandCards handCards in handCardsList) {
						if (handCards.handCategory == largesetCategory) {
							lCHandCardsList.Add(handCards);
						}
					}

					HandCards winner = compareSameCategory(lCHandCardsList);
					Console.WriteLine($"Winner has a {winner.handCategory}");
					Console.WriteLine("Winner's Cards are:");
					winner.printCards();

					Console.WriteLine("game finished");
					Console.WriteLine("\n");

					// Wait for the user to respond before closing.
					Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
					if (Console.ReadLine() == "n") quit = true;

					Console.WriteLine("\n"); // Friendly linespacing.
				}
			}
		}

		static HandCards.HandCategory getLargestCategory(List<HandCards> cardGroupsList) {
			HandCards.HandCategory largestCategory = 0;
			foreach (HandCards handCards in cardGroupsList) {
				if (handCards.handCategory > largestCategory) {
					largestCategory = handCards.handCategory;
				}
			}
			return largestCategory;
		}

		static HandCards compareSameCategory(List<HandCards> cardGroupsList) {
			HandCards result = cardGroupsList[0];
			int maxRank = 0;
			foreach (HandCards handCards in cardGroupsList) {
				if (handCards.getRank() > maxRank) {
					result = handCards;
					maxRank = handCards.getRank();
				}
			}
			return result;
		}


			/*		static HandCards compareSameCategory(List<HandCards> cardGroupsList, HandCards.HandCategory category) {

						HandCards result = cardGroupsList[0];
						if (cardGroupsList.Count == 1) {
							return result;
						}

						if (category == HandCards.HandCategory.flush || category == HandCards.HandCategory.straightFlush || category == HandCards.HandCategory.royalFlush) {

							int largestNumber = 0;
							foreach (HandCards handCards in cardGroupsList) {
								if ((int)handCards.getCardIn(0).face > largestNumber) {
									result = handCards;
									largestNumber = (int)handCards.getCardIn(0).face;
								}
							}
							return result;

						}

						if (category == HandCards.HandCategory.onePair) {

							foreach (HandCards handCards in cardGroupsList) {
								int commonNumber = getMostCommonNumber(handCards);
							}
							return result;

						}


						return result;


					}*/

		}
	public class Card {
		public enum Suit {Spade, Heart, Club, Diamond};
		public enum Face {Ace=1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King}

		public Suit suit;
		public Face face;

		public Card(Suit s, Face f) {
			this.suit = s;
			this.face = f;
		}
		public int getCardFaceNumber() {
			return ((int)face);
		}
	}

	public class CardGroup {
		protected List<Card> cards;

		public int numOfCards;
		public CardGroup() {
			this.cards = new List<Card>();
			this.numOfCards = this.cards.Count;
		}

		public CardGroup(List<Card> _cards) {
			this.cards = _cards;
			this.numOfCards = this.cards.Count;

		}

		protected CardGroup(CardGroup other) {
			this.cards = other.cards;
			this.numOfCards = this.cards.Count;
		}

		public Card getCardIn(int index) {
			return cards[index];	
		}
		public void addCard(Card newCard) {
			cards.Add(newCard);
		}

		public void addCards(List<Card> _cards) {
			cards.AddRange(_cards);
		}

		public Card PopFirstCard() {
			if (cards.Any()) {
				Card popedCard = cards[0];
				cards.RemoveAt(0);
				return popedCard;
			}
			return null;
		}

		public CardGroup popCards(int index, int count) {
			List<Card> popedCards = cards.GetRange(index, count);
			cards.RemoveRange(index, count);
			CardGroup popedCardGroup = new CardGroup(popedCards);
			return popedCardGroup;
		}

		/*(
		Card drawCard() { 
			// get one random card
		}
		*/

		public bool isEmpty() {
			return !cards.Any();
		}

		public void shuffleCards() {
			Random rng = new Random();
			int n = cards.Count;
			
			while (n > 1) {
				n--;
				int k = rng.Next(n + 1);
				Card _card = cards[k];
				cards[k] = cards[n];
				cards[n] = _card;
			}
		}

		public void sortCards() {
			cards = cards.OrderByDescending(o => o.suit).OrderByDescending(o => o.face).ToList();
		}

		public void printCards() {
			for (int i = 0; i < cards.Count; i++) {
				// Creation of the individual card
				Card _card = cards[i];
				Console.WriteLine($"Card: {_card.suit} of {_card.face} ");
			}
		}
	}

	public class WholeDeckCards: CardGroup {
		public void generateDeck() {
			// Creation of each card suite deck chunk
			for (int k = 0; k < 4; k++) {
				// Creation of the individual card
				for (int i = 1; i < 14; i++) {
					cards.Add(new Card((Card.Suit) k, (Card.Face) i));
				}
			}
		}
	}

	public class HandCards : CardGroup {
		public enum HandCategory { noPair = 0, onePair, twoPair, threeKind, straight, flush, fullhouse, fourKind, straightFlush, royalFlush };
		private int rank = 0;
		public HandCategory handCategory;

		private HandCards(CardGroup _cardGroup): base(_cardGroup) {
			this.handCategory = GetCategory();
		}

		public static HandCards CreateInstance(CardGroup _cardGroup) {
			if (_cardGroup.numOfCards == 5) {
				_cardGroup.sortCards();
				return new HandCards(_cardGroup);
			}
			return null;
		}

		public int getRank() {
			return this.rank;
		}

		private int calTotalNumber() {
			int _totalRank = 0;
			foreach (Card card in cards) {
				_totalRank += (int)card.face;
			}
			return _totalRank;
		}


		private HandCategory GetCategory() {
			if (this.isRoyalFlush()) {
				return HandCategory.royalFlush;
			}

			if (this.isStraightFlush()) {
				return HandCategory.straightFlush;
			}

			if (this.isfourKind()) {
				return HandCategory.fourKind;
			}

			if (this.isFullhouse()) {
				return HandCategory.fullhouse;
			}

			if (this.isFlush()) {
				return HandCategory.flush;
			}

			if (this.isStraignt()) {
				return HandCategory.straight;
			}

			if (this.threeKind()) {
				return HandCategory.threeKind;
			}

			if (this.twoPair()) {
				return HandCategory.twoPair;
			}

			if (this.onePair()) {
				return HandCategory.onePair;
			}

			return HandCategory.noPair;
		}


		private bool isRoyalFlush() {
			if (this.isStraightFlush()) {
				if (calTotalNumber() == 47) {
					this.rank = 47;

					return true;
				}
			}
			return false;
		}

		private bool isStraightFlush() {
			if (this.isStraignt() && this.isFlush()) {
				this.rank = calTotalNumber();

				return true;
			}
			return false;

		}

		private bool isStraignt() {
			int min = (int)cards[4].face;
			int max = (int)cards[0].face;

			if (min == 1) {
				int _min = (int)cards[3].face;

				if (_min == 10) {
					this.rank = 47;
					return true;
				}
			}

			if (max - min == 4) {
				this.rank = calTotalNumber();
				return true;
			}
			return false;
		}

		private bool isFlush() {
			if (cards[0].suit == cards[1].suit && cards[0].suit == cards[2].suit && cards[0].suit == cards[3].suit && cards[0].suit == cards[4].suit) {

				for (int i = 0; i < 5; i++) {
					this.rank += (int)((int)cards[i].face * Math.Pow(10, 4 - i));
				}
				return true;
			}

			return false;
		}

		private bool isfourKind() {
			// case 2,2,2,2,1
			if (cards[0].face == cards[1].face && cards[0].face == cards[2].face && cards[0].face == cards[3].face) {
				for (int i = 0; i < 5; i++) {
					this.rank += (int)((int)cards[i].face * Math.Pow(10, 4 - i));
				}
				return true;
			}

			// case 2,1,1,1,1
			if (cards[4].face == cards[1].face && cards[4].face == cards[2].face && cards[4].face == cards[3].face) {
				for (int i = 0; i < 5; i++) {
					this.rank += (int)((int)cards[4 - i].face * Math.Pow(10, 4 - i));
				}
				return true;
			}

			return false;
		}

		private bool isFullhouse() {
			// case 3,3,3,2,2
			if (cards[0].face == cards[1].face && cards[0].face == cards[2].face) {
				if (cards[3].face == cards[4].face) {
					for (int i = 0; i < 5; i++) {
						this.rank += (int)((int)cards[i].face * Math.Pow(10, 4 - i));
					}
					return true;
				}
			}

			// case 3,3,2,2,2
			if (cards[4].face == cards[2].face && cards[4].face == cards[3].face) {
				if (cards[0].face == cards[1].face) {
					for (int i = 0; i < 5; i++) {
						this.rank += (int)((int)cards[4-i].face * Math.Pow(10, 4 - i));
					}
					return true;
				}
			}

			return false;
		}

		private bool threeKind() {
			// case 3,3,3,x,y
			if (cards[0].face == cards[1].face && cards[0].face == cards[2].face) {
				for (int i = 0; i < 5; i++) {
					this.rank += (int)((int)cards[i].face * Math.Pow(10, 4 - i));
				}
				return true;
			}

			// case x,y,2,2,2
			if (cards[4].face == cards[2].face && cards[4].face == cards[3].face) {
				for (int i = 0; i < 5; i++) {
					this.rank += (int)((int)cards[4-i].face * Math.Pow(10, 4 - i));
				}
				return true;
			}

			return false;
		}
		private bool twoPair() {
			// case x,x,y,y,z
			if (cards[0].face == cards[1].face && cards[2].face == cards[3].face) {
				for (int i = 0; i < 5; i++) {
					this.rank += (int)((int)cards[4 - i].face * Math.Pow(10, 4 - i));
				}
				return true;
			}

			// case x,x,y,z,z
			if (cards[0].face == cards[1].face && cards[3].face == cards[4].face) {
				for (int i = 0; i < 2; i++) {
					this.rank += (int)((int)cards[i].face * Math.Pow(10, 4 - i));
				}

				this.rank += (int)((int)cards[2].face * Math.Pow(10, 2));

				for (int i = 3; i < 5; i++) {
					this.rank += (int)((int)cards[i].face * Math.Pow(10, 4 - i));
				}
				return true;
			}

			// case x,y,y,z,z
			if (cards[1].face == cards[2].face && cards[3].face == cards[4].face) {
				for (int i = 1; i < 5; i++) {
					this.rank += (int)((int)cards[i].face * Math.Pow(10, 4 - i));
				}
				this.rank += (int)cards[0].face;
				return true;
			}

			return false;
		}

		private bool onePair() {
			// case 5,5,x,y,z
			if (cards[0].face == cards[1].face) {
				for (int i = 0; i < 5; i++) {
					this.rank += (int)((int)cards[i].face * Math.Pow(10, 4 - i));
				}
				return true;
			}

			// case x,4,4,y,z
			if (cards[1].face == cards[2].face) {

				this.rank += (int)((int)cards[1].face * Math.Pow(10, 4));
				this.rank += (int)((int)cards[2].face * Math.Pow(10, 3));
				this.rank += (int)((int)cards[0].face * Math.Pow(10, 2));
				this.rank += (int)((int)cards[3].face * Math.Pow(10, 1));
				this.rank += (int)((int)cards[4].face * Math.Pow(10, 0));
				return true;
			}

			// case x,y,3,3,z
			if (cards[2].face == cards[3].face) {

				this.rank += (int)((int)cards[2].face * Math.Pow(10, 4));
				this.rank += (int)((int)cards[3].face * Math.Pow(10, 3));
				this.rank += (int)((int)cards[0].face * Math.Pow(10, 2));
				this.rank += (int)((int)cards[1].face * Math.Pow(10, 1));
				this.rank += (int)((int)cards[4].face * Math.Pow(10, 0));
				return true;
			}

			// case x,y,z,2,2
			if (cards[3].face == cards[4].face) {

				this.rank += (int)((int)cards[3].face * Math.Pow(10, 4));
				this.rank += (int)((int)cards[4].face * Math.Pow(10, 3));
				this.rank += (int)((int)cards[0].face * Math.Pow(10, 2));
				this.rank += (int)((int)cards[1].face * Math.Pow(10, 1));
				this.rank += (int)((int)cards[2].face * Math.Pow(10, 0));
				return true;
			}

			return false;
		}

	}
}
	