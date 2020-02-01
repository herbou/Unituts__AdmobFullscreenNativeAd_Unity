using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;

//Native ad
public class Admob : MonoBehaviour
{
	private UnifiedNativeAd adNative;
	private bool nativeLoaded = false;

	private string idApp, idNative;

	[SerializeField] Button btnShowNative;

	[SerializeField] GameObject nativeAdPanel;

	[SerializeField] RawImage adChoices;
	[SerializeField] RawImage adImage;
	[SerializeField] Text adHeadline;
	[SerializeField] Text adBodyText;
	[SerializeField] Text adAdvertiser;
	[SerializeField] Text adCallToAction;

	void Start ()
	{
		idApp = "ca-app-pub-3940256099942544~3347511713";
		idNative = "ca-app-pub-3940256099942544/2247696110";

		MobileAds.Initialize (idApp);
	}

	void Update ()
	{
		if (nativeLoaded) {
			nativeLoaded = false;

			//get data
			Texture2D image = this.adNative.GetImageTextures () [0];
			Texture2D adchoice = this.adNative.GetAdChoicesLogoTexture ();
			string headline = this.adNative.GetHeadlineText ();
			string bodyText = this.adNative.GetBodyText ();
			string advertiser = this.adNative.GetAdvertiserText ();
			string cta = this.adNative.GetCallToActionText ();

			//assign values
			adImage.texture = image;
			adChoices.texture = adchoice;
			adHeadline.text = headline;
			adBodyText.text = bodyText;
			adAdvertiser.text = advertiser;
			adCallToAction.text = cta;

			//register gameobjects to recieve events (click..)
			adNative.RegisterAdChoicesLogoGameObject (adChoices.gameObject);
			adNative.RegisterHeadlineTextGameObject (adHeadline.gameObject);
			adNative.RegisterBodyTextGameObject (adBodyText.gameObject);
			adNative.RegisterAdvertiserTextGameObject (adAdvertiser.gameObject);
			adNative.RegisterCallToActionGameObject (adCallToAction.gameObject);


			nativeAdPanel.SetActive (true);
		}
	}

	public void OnShowNativeClicked ()
	{
		RequestNativeAd ();
		btnShowNative.interactable = false;
		btnShowNative.GetComponentInChildren <Text> ().text = "Loading...";
	}

	public void OnCloseNativeClicked ()
	{
		nativeAdPanel.SetActive (false);
		btnShowNative.interactable = true;
		btnShowNative.GetComponentInChildren <Text> ().text = "Show Native Ad";
	}


	#region Native Ad Mehods ------------------------------------------------

	private void RequestNativeAd ()
	{
		AdLoader adLoader = new AdLoader.Builder (idNative).ForUnifiedNativeAd ().Build ();
		adLoader.OnUnifiedNativeAdLoaded += this.HandleOnUnifiedNativeAdLoaded;
		adLoader.OnNativeAdClosed += this.HandleOnNativeAdClosed;
		adLoader.LoadAd (AdRequestBuild ());
	}

	//events
	private void HandleOnUnifiedNativeAdLoaded (object sender, UnifiedNativeAdEventArgs args)
	{
		this.adNative = args.nativeAd;
		nativeLoaded = true;
	}

	private void HandleOnNativeAdClosed (object sender, EventArgs args)
	{
		nativeAdPanel.SetActive (false);
		btnShowNative.interactable = true;
		btnShowNative.GetComponentInChildren <Text> ().text = "Show Native Ad";
	}

	#endregion

	//------------------------------------------------------------------------
	AdRequest AdRequestBuild ()
	{
		return new AdRequest.Builder ().Build ();
	}


}

