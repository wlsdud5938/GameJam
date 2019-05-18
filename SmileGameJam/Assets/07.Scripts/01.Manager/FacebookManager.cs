using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacebookManager : MonoBehaviour {

    const string FACEBOOK_APP_ID = "2309172562499415";
    const string FACEBOOK_URL = "http://www.facebook.com/dialog/feed";
    const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";

    public static void ShareToFacebook(
        string linkParameter, string nameParameter, string captionParameter,
        string descriptionParameter, string pictureParameter, string redirectParameter
        )
    {
        Application.OpenURL(FACEBOOK_URL + "?app_id=" + FACEBOOK_APP_ID +
        "&link=" + WWW.EscapeURL(linkParameter) +
        "&name=" + WWW.EscapeURL(nameParameter) +
        "&caption=" + WWW.EscapeURL(captionParameter) +
        "&description=" + WWW.EscapeURL(descriptionParameter) +
        "&picture=" + WWW.EscapeURL(pictureParameter) +
        "&redirect_uri=" + WWW.EscapeURL(redirectParameter));
    }

    public static void ShareToTwitter(string textToDisplay, string language)
    {// language: "en", "kor", "jap"
        Application.OpenURL(TWITTER_ADDRESS +
                    "?text=" + WWW.EscapeURL(textToDisplay) +
                    "&amp;lang=" + WWW.EscapeURL(language));
    }

    public void OnBtnFaceBook()
    {
        ShareToFacebook(
            "http://naver.com",            // linkParameter
            "Royal Rumble",                // nameParameter
            "Dev",           // captionParameter
            "Be the King",       // descriptionParameter
            "https://www.google.co.kr/images/srpr/logo11w.png", //pictureParameter
            "http://www.facebook.com"       // redirectParamter
            );
    }

    public void OnBtnTwitter()
    {
        ShareToTwitter("http://sunbug.net", "kor");
    }
}
