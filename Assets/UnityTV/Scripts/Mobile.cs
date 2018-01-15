using UnityEngine;

public class Mobile : MonoBehaviour
{
	void Start ()
    {
        Handheld.PlayFullScreenMovie("https://ia802503.us.archive.org/10/items/1977GentlemenPreferHanesPantyhose/1977%20Gentlemen%20Prefer%20Hanes%20Pantyhose.ogv", Color.black, FullScreenMovieControlMode.CancelOnInput);
    }
}
