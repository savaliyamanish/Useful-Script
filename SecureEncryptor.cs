using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;
using System.IO;
public class SecureEncryptor 
{
	private byte[] Key;
	private Aes encryptor;
	public SecureEncryptor(string pwd,byte[] iv)
	{
		// Create sha256 hash
		SHA256 mySHA256 = SHA256Managed.Create();
		byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(pwd));
		byte[] aesKey = new byte[32];

		encryptor = Aes.Create();
		Array.Copy(key, 0, aesKey, 0, 32);
		encryptor.Mode = CipherMode.CBC;

		encryptor.Key = aesKey;
		encryptor.IV = iv;
	}
	public string Encrypt(string plainText)
	{
		MemoryStream memoryStream = new MemoryStream();
		ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();
		CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);
		byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);
		cryptoStream.Write(plainBytes, 0, plainBytes . Length);
		cryptoStream.FlushFinalBlock();
		byte[] cipherBytes = memoryStream.ToArray();
		memoryStream.Close();
		cryptoStream.Close();
		string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);
		return cipherText;
	}
	public string Decrypt(string cipherText,string defultValue="")
	{
		MemoryStream memoryStream = new MemoryStream();
		ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();
		CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);
		string plainText = defultValue;
		try {
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			cryptoStream.Write(cipherBytes, 0, cipherBytes . Length);
			cryptoStream.FlushFinalBlock();
			byte[] plainBytes = memoryStream.ToArray();
			plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
		} finally {
			memoryStream.Close();
			cryptoStream.Close();
		}
		return plainText;
	}

	public string Encrypt(int num)
	{
		return Encrypt (num.ToString ());
	}
	public int DecryptInt(string cipherText,int defultValue=0)
	{
		int num = defultValue;
		int.TryParse (Decrypt (cipherText), out num);
		return num;
	}

	public string Encrypt(long num)
	{
		return Encrypt (num.ToString ());
	}
	public long DecryptLong(string cipherText,long defultValue=0)
	{
		long num = defultValue;
		long.TryParse (Decrypt (cipherText), out num);
		return num;
	}

	public string Encrypt(float num)
	{
		return Encrypt (num.ToString ());
	}
	public float DecryptFloat(string cipherText,float defultValue=0)
	{
		float num = defultValue;
		float.TryParse (Decrypt (cipherText), out num);
		return num;
	}

	public string Encrypt(double num)
	{
		return Encrypt (num.ToString ());
	}
	public double DecryptDouble(string cipherText,double defultValue=0)
	{
		double num = defultValue;
		double.TryParse (Decrypt (cipherText), out num);
		return num;
	}



	public string GetString(string key,string defultValue)
	{
		key = "String_" + key;
		return Decrypt(PlayerPrefs.GetString (Encrypt (key), Encrypt (defultValue)));
	}
	public void SetString(string key,string value)
	{
		key = "String_" + key;
		PlayerPrefs.SetString (Encrypt (key), Encrypt (value));
	}

	public int GetInt(string key,int defultValue)
	{
		key = "Int_" + key;
		return DecryptInt(PlayerPrefs.GetString (Encrypt (key), Encrypt (defultValue)));
	}
	public void SetInt(string key,int value)
	{
		key = "Int_" + key;
		PlayerPrefs.SetString (Encrypt (key), Encrypt (value));
	}

	public long GetLong(string key,float defultValue)
	{
		key = "Long_" + key;
		return DecryptLong(PlayerPrefs.GetString (Encrypt (key), Encrypt (defultValue)));
	}
	public void SetLong(string key,float value)
	{
		key = "Long_" + key;
		PlayerPrefs.SetString (Encrypt (key), Encrypt (value));
	}

	public float GetFloat(string key,float defultValue)
	{
		key = "Float_" + key;
		return DecryptLong(PlayerPrefs.GetString (Encrypt (key), Encrypt (defultValue)));
	}
	public void SetFloat(string key,float value)
	{
		key = "Float_" + key;
		PlayerPrefs.SetString (Encrypt (key), Encrypt (value));
	}

	public double GetDouble(string key,float defultValue)
	{
		key = "Double_" + key;
		return DecryptDouble(PlayerPrefs.GetString (Encrypt (key), Encrypt (defultValue)));
	}
	public void SetDouble(string key,float value)
	{
		key = "Double_" + key;
		PlayerPrefs.SetString (Encrypt (key), Encrypt (value));
	}
}


