using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataScript {
	private static int scorePlayer1, scorePlayer2, numberOfGamesToWin, p1selection, p2selection;
	private static float buffPlayer1 = 1;
	private static float buffPlayer2 = 1;

	public static int ScorePlayer1 { get { return scorePlayer1; } set { scorePlayer1 = value; } }

	public static int ScorePlayer2 { get { return scorePlayer2; } set { scorePlayer2 = value; } }

	public static int NumberOfGamesToWin { get { return numberOfGamesToWin; } set { numberOfGamesToWin = value; } }

	public static int P1selection { get { return p1selection; } set { p1selection = value; } }

	public static int P2selection { get { return p2selection; } set { p2selection = value; } }

	public static float BuffPlayer1 { get { return buffPlayer1; } set { buffPlayer1 = value; } }

	public static float BuffPlayer2 { get { return buffPlayer2; } set { buffPlayer2 = value; } }
}
