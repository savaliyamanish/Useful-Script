using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
namespace MS
{
public class WebSeviceData : MonoBehaviour
{
	public enum LogType
	{
		Log_Complete,
		Log_Error_Only,
		Log_Responce_Only,
		Not_Log
	}

	[Header ("Debug Setting")]
	public LogType logType;
	public Color URLColor, ErrorColor, TimeColor;
	public bool ignoreImageLog;
	public static WebSeviceData intance;
	public static string baseURL = "";
	
	string loginURL{ get { return baseURL + "NewLogin.php"; } }

	public static JSONObject dashboardData;


	public static bool networkError;

	void Awake ()
	{ 
		if (intance != null) {
			DestroyImmediate (this.gameObject);
		} else {
			intance = this;
			DontDestroyOnLoad (this.gameObject);			
		}
	}
	
	public IEnumerator loginWebService ()
	{
		WWWForm form = new WWWForm ();
		form.AddField ("Email", "");
		
		WWW request = new WWW (loginURL, form);
		networkError = false;

		float dif = Time.time;
		yield return request;
		dif = Time.time - dif;
		PrintWWWLog (request, form, dif);
		if (request.error == null) {
			if (CheckSuccess (request.text)) {			
				dashboardData = new JSONObject (request.text);
			} else {

			}
		} else {
			dashboardData = null;
			networkError = true;
			print ("Net Nathi Baka : " + request.error);
		}
		request.Dispose ();

	}

	public static bool CheckSuccess (string responce)
	{
		try {
			JSONObject temp = new JSONObject (responce);
			if (temp.GetField ("success").IsString && temp.GetField ("success").MyStr.Equals ("true"))
				return true;
			else if (temp.GetField ("success").IsBool && temp.GetField ("success").b)
				return true;
			else
				return false;
		} catch (Exception e) {
			print (e);
			return false;
		}
	}
	void PrintWWWLog (WWW request, WWWForm form, float loadtime = 0)
	{
		string url;
		string error;
		string time;
		#if UNITY_EDITOR
		url = "<color=\"#" + ColorUtility.ToHtmlStringRGBA (URLColor) + "\">  " + request.url + "?" + System.Text.Encoding.ASCII.GetString (form.data) + "  </color>";
		error = "<color=\"#" + ColorUtility.ToHtmlStringRGBA (ErrorColor) + "\">" + request.error + "?" + System.Text.Encoding.ASCII.GetString (form.data) + "</color>";
		time = "<color=\"#" + ColorUtility.ToHtmlStringRGBA (TimeColor) + "\">" + loadtime + "</color>";
		#else
		url =  request.url + "?"+System.Text.Encoding.ASCII.GetString (form.data);
		error =  request.error + "?"+System.Text.Encoding.ASCII.GetString (form.data);
		time=  loadtime +"";
		#endif
		if (ignoreImageLog && request.text.Equals ("")) {
			if (request.error != null)
				Debug.Log(url + "\n" + "Time : " + time + "\n\n" + error);
		
			return;
		}
		if (logType == LogType.Log_Complete) {
			if (request.error == null)
				Debug.Log(url + "\n" + "Time : " + time + "\n\n" + request.text);
			else
				Debug.Log(url + "\n" + "Time : " + time + "\n\n" + error);
		} else if (logType == LogType.Log_Responce_Only) {
			if (request.error == null)
				Debug.Log(request.text);
			else
				Debug.Log(url + "\n" + "Time : " + time + "\n\n" + error);
		} else if (logType == LogType.Log_Error_Only) {
			if (request.error != null)
				Debug.Log(url + "\n" + "Time : " + time + "\n\n" + error);
		}
	}

}
}