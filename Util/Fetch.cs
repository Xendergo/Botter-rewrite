using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

public static class Util { 
  // reference: https://alialhaddad.medium.com/how-to-fetch-data-in-c-net-core-ea1ab720e3f9
  private static async Task<Object> fetch(string url) {
    try {
      using (HttpClient client = new HttpClient()) {
        using (HttpResponseMessage res = await client.GetAsync(url)) {
          using (HttpContent content = res.Content)
            {
                //Retrieve the data from the content of the response, have the await keyword since it is asynchronous.
                string data = await content.ReadAsStringAsync();
                //If the data is not null, parse the data to a C# object, then create a new instance of PokeItem.
                if (data is not null) // using is or is not null is preferable since == and != can be overloaded
                {
                    //Parse your data into a object.
                    var dataObj = JObject.Parse(data);
                    return dataObj;
                }
                else
                {
                    //If data is null log it into console.
                    Console.WriteLine("Data is null!");
                }
            }
        }
      }
    } catch (Exception) {
      
    }
  }
  public static async Task<Object> fetchStockData(string ticker) {
    string baseURL = $"https://query1.finance.yahoo.com/v8/finance/chart/{ticker}";
    fetch(baseURL);
  }
}