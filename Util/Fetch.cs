using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

public static class Util { 
  // reference: https://alialhaddad.medium.com/how-to-fetch-data-in-c-net-core-ea1ab720e3f9
  private static async Task<JObject> fetch(string url) {
    try {
      using (HttpClient client = new HttpClient()) {
        using (HttpResponseMessage res = await client.GetAsync(url)) {
          using (HttpContent content = res.Content) {
            //Retrieve the data from the content of the response, have the await keyword since it is asynchronous.
            string data = await content.ReadAsStringAsync();
            //If the data is not null, parse the data to a C# object, then create a new instance of PokeItem.
            if (data is not null) {// using is or is not null is preferable since == and != can be overloaded
              //Parse your data into a object.
              var dataObj = JObject.Parse(data);
              return dataObj;
            }
            
            throw new NullReferenceException("Data is null");
          }
        }
      }
    } catch (Exception exception) {
      Console.WriteLine("Ooooooof, the fetch request failed");
      Console.WriteLine("~~~~~~~~~~~~~");
      throw exception;
      // You can't just return nothing here
    }
  }

  public static async Task<float> fetchStockData(string ticker) {
    string baseUrl = $"https://query1.finance.yahoo.com/v8/finance/chart/{ticker}";
    try {
      JObject res = (JObject)(await fetch(baseUrl))["chart"];
      if (res["error"].Type != JTokenType.Null) return -1;
      return (float) (((res["result"] as JArray)[0] as JObject)["meta"] as JObject)["regularMarketPrice"];
    } catch (Exception exception) {
      Console.WriteLine("Something went wrong with the parsing of stock data");
      Console.WriteLine("~~~~~~~~~~~~~");
      throw exception;
    }
  }
}