using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Net;


namespace GiapDepTrai.Helpers 
{
public class VnPayLibrary
{
    public SortedList<string, string> requestData = new SortedList<string, string>(StringComparer.Ordinal);
    public SortedList<string, string> responseData = new SortedList<string, string>(StringComparer.Ordinal);

    public void AddRequestData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            requestData.Add(key, value);
        }
    }

    public void AddResponseData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            responseData.Add(key, value);
        }
    }

    public string GetResponseData()
    {
        return string.Join("&", responseData.OrderBy(x => x.Key).Select(x => $"{x.Key}={x.Value}"));
    }

    public string HmacSHA512(string key, string inputData)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(key)))
        {
            byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
            return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        }
    }

    public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
    {
        string data = string.Join("&", requestData.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));
        string signData = string.Join("&", requestData.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        string secureHash = HmacSHA512(vnp_HashSecret, signData);

        return $"{baseUrl}?{data}&vnp_SecureHash={secureHash}";
    }
}
}
