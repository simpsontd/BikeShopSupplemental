using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Security.Cryptography;
using System.ComponentModel.Design;

namespace MetricsSampleDataGenerator
{
    class UserGenSample
    {
        public  int UserNum { get; set; }
        public  int NumDays { get; set; }
        public UserGenSample(int usernum, int numdays)
        {
            UserNum = usernum;
            NumDays = numdays;
        }
        /// <summary>
        /// Hold data from response as dictionary object
        /// </summary>
        public  List<IDictionary<string, dynamic>> ResponseData { get; set; }
        public  bool DebugOutput { get; set; } = true;
        /// <summary>
        /// Store primary key for Posted record
        /// </summary>
        private  string POSTID { get; set; } = "";
        /// <summary>
        /// response in string format
        /// </summary>
        public  string ResponseString { get; set; }
        /// <summary>
        /// Our client to use for the program
        /// </summary>
         readonly HttpClient client = new HttpClient();
        private  Random rand = new Random();
        /// <summary>
        /// Hold shuffled items for sale
        /// </summary>
        public  List<String[]> Items { get; set; } = new List<string[]>();

        private  string BikeShopApiUrl { get; set; } = "https://segfault.asuscomm.com:9200/api/BikeShop/";
        private  string MetricsApiUrl { get; set; } = "https://segfault.asuscomm.com:9300/api/Metrics/";
        public  bool ItemsPopulated { get; set; } = false;




        /// <summary>
        /// Get requested data and change associated values of controls when data is received
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
         async Task HTTPGET(string uri)
        {
            string responseBody = "";
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //halt method and wait on response from api
                responseBody = await response.Content.ReadAsStringAsync(); //halt method and wait on string read
                response.EnsureSuccessStatusCode(); //make sure we have 200 OK
                ResponseString = responseBody; //update control
                ResponseData = JsonConvert.DeserializeObject<List<IDictionary<string, dynamic>>>(responseBody); //Convert from json text to dictionary
            }
            catch (HttpRequestException e)
            {
                if (DebugOutput) Console.WriteLine(e.Message);
                if (DebugOutput) Console.WriteLine(responseBody);
            }
        }
        /// <summary>
        /// send a HTTP POST request using the url and bodytext(json format)
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="body"></param>
        /// <returns></returns>
         async Task HTTPPOST(string uri, string jsonBody)
        {
            string responseBody = "";
            try
            {
                var data = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, data);
                responseBody = await response.Content.ReadAsStringAsync(); //halt method and wait on string read
                response.EnsureSuccessStatusCode(); //make sure we have 200 OK
                ResponseString = response.ToString();
                POSTID = responseBody;
            }
            catch (HttpRequestException e)
            {
                if (DebugOutput) Console.WriteLine(e.Message);
                if (DebugOutput) Console.WriteLine(responseBody);
            }

        }
        /// <summary>
        /// Update fields in a record using HTTP PUT request with url and bodytext(json format with fields to be changed)
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="body"></param>
        /// <returns></returns>
         async Task HTTPPUT(string uri, string jsonBody)
        {
            string responseBody = "";
            try
            {
                var data = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(uri, data);
                responseBody = await response.Content.ReadAsStringAsync(); //halt method and wait on string read
                response.EnsureSuccessStatusCode(); //make sure we have 200 OK
                ResponseString = response.ToString();
            }
            catch (HttpRequestException e)
            {
                if (DebugOutput) Console.WriteLine(e.Message);
                if (DebugOutput) Console.WriteLine(responseBody);
            }

        }
        /// <summary>
        /// Send a HTTP DELETE request to delete the record specified by the url
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
         async Task HTTPDELETE(string uri)
        {
            string responseBody = "";
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(uri); //halt method and wait on response from api
                response.EnsureSuccessStatusCode(); //make sure we have 200 OK
                responseBody = await response.Content.ReadAsStringAsync(); //halt method and wait on string read
                ResponseString = response.ToString(); //update control

                //application specific code below
            }
            catch (HttpRequestException e)
            {
                if (DebugOutput) Console.WriteLine(e.Message);
                if (DebugOutput) Console.WriteLine(responseBody);
            }
        }

        /// <summary>
        /// Send a HTTP request based on request type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private  void SendRequest(string type, string url, string body = "")
        {
            if (type == "GET")
            {
                HTTPGET(url); //Async method will make all changes when it gets to it.
            }
            else if (type == "POST")
            {
                HTTPPOST(url, body);
            }
            else if (type == "PUT")
            {
                HTTPPUT(url, body);
            }
            else if (type == "DELETE")
            {
                HTTPDELETE(url);
            }
        }

        /// <summary>
        /// Send a HTTP request based on request type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private  async Task SendRequestAsync(string type, string url, string body = "")
        {
            if (type == "GET")
            {
                await HTTPGET(url); //Async method will make all changes when it gets to it.
            }
            else if (type == "POST")
            {
                await HTTPPOST(url, body);
            }
            else if (type == "PUT")
            {
                await HTTPPUT(url, body);
            }
            else if (type == "DELETE")
            {
                await HTTPDELETE(url);
            }
            if (DebugOutput) Console.WriteLine("HTTP " + type + " " + url + " Request finished");
            return;
        }

        private  async Task PopulateItems()
        {
            await SendRequestAsync("GET", BikeShopApiUrl + "Component");
            string[] tempArray = new string[3];
            foreach (IDictionary<string, dynamic> dict in ResponseData)
            {
                try
                {
                    tempArray = new string[3];
                    tempArray[0] = dict["COMPONENTID"].ToString();
                    tempArray[1] = "Component";
                    tempArray[2] = dict["CATEGORY"];
                    Items.Add(tempArray);
                }
                catch (Exception e)
                {
                    if (DebugOutput) Console.WriteLine(dict.ToString());
                    if (DebugOutput) Console.WriteLine(e.Message);
                }

            }
            ItemsPopulated = true;

        }

        private  void ShuffleList()
        {
            for (int i = 0; i < 3; i++)
            {
                int n = Items.Count;
                while (n > 1)
                {
                    n--;
                    int k = rand.Next(n + 1);
                    string[] value = Items[k];
                    Items[k] = Items[n];
                    Items[n] = value;
                }
            }

        }
        /// <summary>
        /// Generate a random value based on normal distribution
        /// https://stackoverflow.com/questions/218060/random-gaussian-variables
        /// </summary>
        /// <param name="stdDev"></param>
        /// <param name="mid"></param>
        /// <returns></returns>
        private  double NormalRand(double mean, double stdDev)
        {
            double output = 0;
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            output = mean + stdDev * randStdNormal;
            return output;
        }
        /// <summary>
        /// Get hexadecimal hash of input string
        /// Courtesy of turgay @ http://csharpexamples.com/c-create-md5-hash-string/
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public  string HashSlingingHasher(string input)
        {
            // Step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// Convert an integer to word representation
        /// Coutesy of Amit Mohanty @ https://www.c-sharpcorner.com/blogs/convert-number-to-words-in-c-sharp
        /// </summary>
        private  String[] units = { "Zero", "One", "Two", "Three",
        "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
        "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
        "Seventeen", "Eighteen", "Nineteen" };
        /// <summary>
        /// Convert an integer to word representation
        /// Coutesy of Amit Mohanty @ https://www.c-sharpcorner.com/blogs/convert-number-to-words-in-c-sharp
        /// </summary>
        private  String[] tens = { "", "", "Twenty", "Thirty", "Forty",
        "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        /// <summary>
        /// Convert an integer to word representation
        /// Coutesy of Amit Mohanty @ https://www.c-sharpcorner.com/blogs/convert-number-to-words-in-c-sharp
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public  String Convert(Int64 i)
        {
            if (i < 20)
            {
                return units[i];
            }
            if (i < 100)
            {
                return tens[i / 10] + ((i % 10 > 0) ? " " + Convert(i % 10) : "");
            }
            if (i < 1000)
            {
                return units[i / 100] + " Hundred"
                        + ((i % 100 > 0) ? " And " + Convert(i % 100) : "");
            }
            if (i < 100000)
            {
                return Convert(i / 1000) + " Thousand "
                + ((i % 1000 > 0) ? " " + Convert(i % 1000) : "");
            }
            if (i < 10000000)
            {
                return Convert(i / 100000) + " Lakh "
                        + ((i % 100000 > 0) ? " " + Convert(i % 100000) : "");
            }
            if (i < 1000000000)
            {
                return Convert(i / 10000000) + " Crore "
                        + ((i % 10000000 > 0) ? " " + Convert(i % 10000000) : "");
            }
            return Convert(i / 1000000000) + " Arab "
                    + ((i % 1000000000 > 0) ? " " + Convert(i % 1000000000) : "");
        }

        private  async Task CreateUsers(int user)
        {
            if (DebugOutput) Console.WriteLine("Waking both APIs");
            await SendRequestAsync("GET", BikeShopApiUrl + "Component"); //wake the bike shop api
            await SendRequestAsync("GET", MetricsApiUrl + "CUSTOMER"); //wake the metrics api
            IDictionary<string, dynamic> currentUser;

            if (DebugOutput) Console.WriteLine("Creating User");
            currentUser = new Dictionary<string, dynamic>();
            currentUser.Add("USERNAME", "USER" + user);
            currentUser.Add("CUSTOMERID", 0); //We will not reference any actual customers in bikeshop table
            currentUser.Add("EMAIL", "USER" + user + "@Gmail.Com");
            currentUser.Add("FIRSTNAME", "USER");
            currentUser.Add("LASTNAME", Convert(user)); //use word number as last name just for fun
            currentUser.Add("ADDRESS", "101 Stuck_With_Tyler St");
            currentUser.Add("ZIP", "37601");
            currentUser.Add("STATE", "TN");
            currentUser.Add("PAYMENTMETHOD", "Monopoly Money");
            currentUser.Add("PASSWORDHASH", HashSlingingHasher("PASSWORD" + user));
            if (DebugOutput) Console.WriteLine("Posting new user to metrics db.");
            SendRequestAsync("POST", MetricsApiUrl + "CUSTOMER", JsonConvert.SerializeObject(currentUser, Formatting.Indented));
            Console.WriteLine("USER" + user + " Created.");
            
            return;
        }

        private  async Task GenerateLogs(int user, int numDays)
        {
            int randint = 0;
            if (DebugOutput) Console.WriteLine("Waking both APIs");
            await SendRequestAsync("GET", BikeShopApiUrl + "Component"); //wake the bike shop api
            await SendRequestAsync("GET", MetricsApiUrl + "CUSTOMER"); //wake the metrics api
            await PopulateItems();
            if (DebugOutput) Console.WriteLine("Populating Items");
            //Thread.Sleep(1000);
            if (DebugOutput) Console.WriteLine("Items Populated");
            ShuffleList();
            if (DebugOutput) Console.WriteLine("Items shuffled");
            double[] dailyModifiers = { 1.037, 1.0735, 1.0217, 0.988, 0.988, 0.932, 0.962 };
            DateTime BaseDate = new DateTime(2019, 1, 1, 0, 0, 0);
            IDictionary<string, dynamic> currentUser;
            
            BaseDate = new DateTime(2019, 1, 1, 0, 0, 0);
            currentUser = new Dictionary<string, dynamic>();
            currentUser.Add("USERNAME", "USER" + user);

            int inclinationToShop = rand.Next(0, 100);
            for (int day = 1; day <= numDays; day++) //day loop
            {
                if (DebugOutput) Console.WriteLine("Starting Day " + day);
                Console.WriteLine("User: " + user + " Day: " + day);
                double probabilityShopToday = dailyModifiers[day % 7] * 1.5323 * Math.Pow(100 - inclinationToShop, -0.913);
                randint = rand.Next(100);
                if (randint < (probabilityShopToday * 100)) //shopping today == true 
                {
                    if (DebugOutput) Console.WriteLine("User will shop today.");
                    int x = rand.Next(0, 100);
                    int numSessions = (int)Math.Round(0.0004 * Math.Pow(x, 2) - (0.0072 * x) + 1.0139);
                    DateTime[] sessionStart = new DateTime[numSessions];
                    DateTime[] sessionEnd = new DateTime[numSessions];
                    if (DebugOutput) Console.WriteLine("User will shop for " + numSessions + " sessions today.");
                    for (int i = 0; i < numSessions; i++)
                    {
                        sessionStart[i] = new DateTime(BaseDate.Year, BaseDate.Month, BaseDate.Day, 23, 59, 59);
                        sessionEnd[i] = BaseDate;
                    }
                    for (int i = 0; i < numSessions; i++) //loop for each session
                    {
                        double sessionTime = NormalRand(616, 154); //mean 616 seconds, stdDev 154 seconds
                        if (DebugOutput) Console.WriteLine("SessionTime: " + sessionTime);
                        double loginTimeHour;
                        bool isLoginTimeGood = false;
                        DateTime possibleLoginTime = BaseDate;
                        if (DebugOutput) Console.WriteLine("Trying to login");
                        int LoopCount = 0;
                        while (isLoginTimeGood == false)
                        {
                            LoopCount++;
                            if (LoopCount > 100)
                            {
                                if (DebugOutput) Console.WriteLine("EndlessLoop Created.");
                            }
                            isLoginTimeGood = true;
                            while (true)
                            {
                                if (DebugOutput) Console.Write(".");
                                loginTimeHour = NormalRand(17, 4); //mean 1700 military time, std dev 4 hours
                                if (DebugOutput) Console.Write(loginTimeHour);
                                if (loginTimeHour >= 4 && loginTimeHour <= 28) //test if within desired parameters
                                {
                                    if (loginTimeHour >= 24)//rollover probability curve to morning
                                    {
                                        loginTimeHour -= 24;
                                    }
                                    break;
                                }
                            }
                            possibleLoginTime = BaseDate;
                            possibleLoginTime = possibleLoginTime.AddHours(loginTimeHour);

                            for (int j = 0; j < numSessions; j++)
                            {
                                TimeSpan start = sessionStart[j] - possibleLoginTime;
                                if (j != i && start.TotalSeconds < 3 * sessionTime && sessionStart[j] > possibleLoginTime)
                                {
                                    isLoginTimeGood = false;
                                    break;
                                }
                                TimeSpan end = possibleLoginTime - sessionEnd[j];
                                if (j != i && end.TotalSeconds < sessionTime && possibleLoginTime > sessionEnd[j])
                                {
                                    isLoginTimeGood = false;
                                    break;
                                }
                            }
                        }
                        if (DebugOutput) Console.WriteLine("User is trying to log in.");
                        sessionStart[i] = possibleLoginTime; //Login time is valid so add times to session start and end
                        sessionEnd[i] = possibleLoginTime.AddSeconds(sessionTime);
                        int attempt = 1;
                        bool isSuccessfulLogin = false;
                        while (!isSuccessfulLogin)
                        {
                            int loginSuccess = rand.Next(100);
                            if (loginSuccess < 60) //successful login
                            {
                                if (DebugOutput) Console.WriteLine("Login Successful");
                                IDictionary<string, dynamic> UserLoginLog = new Dictionary<string, dynamic>();
                                UserLoginLog.Add("USERNAME", currentUser["USERNAME"]);
                                UserLoginLog.Add("EVENTTIMESTAMP", possibleLoginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                UserLoginLog.Add("DESCRIPTION", "SUCCESS");
                                UserLoginLog.Add("ATTEMPT", attempt);
                                SendRequestAsync("POST", MetricsApiUrl + "USERLOGINLOG", JsonConvert.SerializeObject(UserLoginLog, Formatting.Indented));
                                isSuccessfulLogin = true;
                            }
                            else //wrong username or password
                            {
                                if (DebugOutput) Console.WriteLine("Login Failed");
                                IDictionary<string, dynamic> UserLoginLog = new Dictionary<string, dynamic>();
                                UserLoginLog.Add("USERNAME", currentUser["USERNAME"]);
                                UserLoginLog.Add("EVENTTIMESTAMP", possibleLoginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                UserLoginLog.Add("DESCRIPTION", "FAILURE - WRONG PASSWORD");
                                UserLoginLog.Add("ATTEMPT", attempt);
                                SendRequestAsync("POST", MetricsApiUrl + "USERLOGINLOG", JsonConvert.SerializeObject(UserLoginLog, Formatting.Indented));
                                attempt++;
                                possibleLoginTime = possibleLoginTime.AddSeconds(rand.Next(5, 45)); //add time it takes to try to login again
                            }
                        }
                        //login is successful
                        if (DebugOutput) Console.WriteLine("Starting Search decision");
                        DateTime eventTime = possibleLoginTime.AddSeconds(rand.Next(5, 60));
                        if (eventTime < sessionEnd[i])
                        {
                            string searchText = Items[(int)(Math.Abs(Math.Round(NormalRand(0, Items.Count / 4))))][2];
                            int randint2 = rand.Next(100);
                            if (randint2 < 80) //80% chance we start with a search
                            {
                                if (DebugOutput) Console.WriteLine("Starting Search");
                                IDictionary<string, dynamic> SearchLog = new Dictionary<string, dynamic>();
                                SearchLog.Add("USERNAME", currentUser["USERNAME"]);
                                SearchLog.Add("SEARCHTEXT", searchText);
                                SearchLog.Add("EVENTTIMESTAMP", eventTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                SendRequestAsync("POST", MetricsApiUrl + "SEARCHLOG", JsonConvert.SerializeObject(SearchLog, Formatting.Indented));
                                eventTime = eventTime.AddSeconds(rand.Next(5, 60));
                            }
                            if (DebugOutput) Console.WriteLine("EventTime: " + eventTime + "\nSessionEnd: " + sessionEnd[i]);
                            if (eventTime < sessionEnd[i])
                            {
                                int numViews = rand.Next(1, 4);
                                if (DebugOutput) Console.WriteLine("Number of views will be: " + numViews);
                                for (int k = 0; k < numViews; k++)
                                {
                                    int index = 0;
                                    int limit = 100; //limit how many times we can try to get the search text
                                    int count = 0;
                                    while (Items[index][2] != searchText) //spin until we get an item that matches the search text
                                    {
                                        index = (int)(Math.Abs(Math.Round(NormalRand(0, Items.Count / 4))));
                                        if (index >= Items.Count)
                                        {
                                            index = Items.Count - 1;
                                        }
                                        if (count > limit)
                                        {
                                            break;
                                        }
                                    }
                                    if (DebugOutput) if (DebugOutput) Console.WriteLine("Viewing item");
                                    IDictionary<string, dynamic> ItemViewLog = new Dictionary<string, dynamic>();
                                    ItemViewLog.Add("USERNAME", currentUser["USERNAME"]);
                                    ItemViewLog.Add("EVENTTIMESTAMP", eventTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                    ItemViewLog.Add("ITEMID", int.Parse(Items[index][0]));
                                    ItemViewLog.Add("ITEMTABLE", "COMPONENT");
                                    SendRequestAsync("POST", MetricsApiUrl + "ITEMVIEWLOG", JsonConvert.SerializeObject(ItemViewLog, Formatting.Indented));
                                    eventTime = eventTime.AddSeconds(rand.Next(5, 60));
                                    if (eventTime > sessionEnd[i])
                                    {
                                        break;
                                    }
                                    int randomNumber = rand.Next(100);
                                    if (randomNumber < 10) //10% chance we will add item to cart
                                    {

                                        if (DebugOutput) Console.WriteLine("Adding item to cart");
                                        int quantity = (int)(Math.Abs(Math.Round(NormalRand(0, 0.25)))) + 1;
                                        IDictionary<string, dynamic> CartLog = new Dictionary<string, dynamic>();
                                        CartLog.Add("USERNAME", currentUser["USERNAME"]);
                                        CartLog.Add("EVENTTIMESTAMP", eventTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                        CartLog.Add("DESCRIPTION", "ADD");
                                        CartLog.Add("QUANTITY", quantity);
                                        CartLog.Add("ITEMID", int.Parse(Items[index][0]));
                                        CartLog.Add("ITEMTABLE", "COMPONENT");
                                        SendRequestAsync("POST", MetricsApiUrl + "CARTLOG", JsonConvert.SerializeObject(CartLog, Formatting.Indented));

                                        //check if the item is already in the cart and add to quantity if it is.


                                        IDictionary<string, dynamic> CartItem = new Dictionary<string, dynamic>();
                                        CartItem.Add("USERNAME", currentUser["USERNAME"]);
                                        CartItem.Add("ITEMID", int.Parse(Items[index][0]));
                                        CartItem.Add("ITEMTABLE", "COMPONENT");
                                        CartItem.Add("ITEMPRICE", 10.00);

                                        await SendRequestAsync("GET", MetricsApiUrl + @"CartItem/'" + CartItem["USERNAME"] + "'&" + CartItem["ITEMID"] + "&'" + CartItem["ITEMTABLE"] + @"'");
                                        if (ResponseData.Count == 0)
                                        {//the cart does not contain this item
                                            CartItem.Add("QUANTITY", quantity);
                                            await SendRequestAsync("POST", MetricsApiUrl + "CARTITEM", JsonConvert.SerializeObject(CartItem, Formatting.Indented));
                                        }
                                        else
                                        {//cart contains item
                                            quantity += ResponseData[0]["QUANTITY"];
                                            CartItem.Add("QUANTITY", quantity);
                                            await SendRequestAsync("PUT", MetricsApiUrl + @"CartItem/'" + CartItem["USERNAME"] + "'&" + CartItem["ITEMID"] + "&'" + CartItem["ITEMTABLE"] + @"'", JsonConvert.SerializeObject(CartItem, Formatting.Indented));
                                        }
                                        randint = rand.Next(100);
                                        if (randint < 20)//20% chance to remove item from cart
                                        {
                                            if (DebugOutput) Console.WriteLine("Removing item from cart");
                                            eventTime = eventTime.AddSeconds(rand.Next(10, 30));
                                            if (eventTime > sessionEnd[i])
                                            {
                                                break;
                                            }
                                            await SendRequestAsync("DELETE", MetricsApiUrl + @"CARTITEM/'" + currentUser["USERNAME"] + @"'&" + Items[index][0] + @"&" + @"'COMPONENT'");

                                            IDictionary<string, dynamic> CartDeleteLog = new Dictionary<string, dynamic>();
                                            CartDeleteLog.Add("USERNAME", currentUser["USERNAME"]);
                                            CartDeleteLog.Add("EVENTTIMESTAMP", eventTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                            CartDeleteLog.Add("DESCRIPTION", "REMOVE");
                                            CartDeleteLog.Add("QUANTITY", quantity);
                                            CartDeleteLog.Add("ITEMID", int.Parse(Items[index][0]));
                                            CartDeleteLog.Add("ITEMTABLE", "COMPONENT");
                                            SendRequestAsync("POST", MetricsApiUrl + "CARTLOG", JsonConvert.SerializeObject(CartDeleteLog, Formatting.Indented));
                                        }
                                    }
                                    else if (randomNumber >= 10 && randomNumber < 16) //6% chance user will add item to wishlist
                                    {
                                        //check if any wishlist exists
                                        if (DebugOutput) Console.WriteLine("Entering wishlist code");
                                        await SendRequestAsync("GET", MetricsApiUrl + "WISHLIST");
                                        List<IDictionary<string, dynamic>> UserWishlists = new List<IDictionary<string, dynamic>>();
                                        foreach (IDictionary<string, dynamic> record in ResponseData)
                                        {
                                            if (record["USERNAME"] == currentUser["USERNAME"])
                                            {
                                                UserWishlists.Add(record);
                                            }
                                        }
                                        bool wishlistExists = true;
                                        if (UserWishlists.Count == 0)
                                        {
                                            wishlistExists = false;
                                        }
                                        int listid;
                                        randint = rand.Next(100);
                                        if (randint < 5 || !wishlistExists) //5% chance to create a new wishlist 
                                        {
                                            if (DebugOutput) Console.WriteLine("Creating a new wishlist");
                                            //create wishlist
                                            IDictionary<string, dynamic> wishlist = new Dictionary<string, dynamic>();
                                            wishlist.Add("LISTNAME", "FUN STUFF");
                                            wishlist.Add("USERNAME", currentUser["USERNAME"]);
                                            await SendRequestAsync("POST", MetricsApiUrl + "WISHLIST", JsonConvert.SerializeObject(wishlist, Formatting.Indented));
                                            listid = int.Parse(POSTID);

                                            //add item to wishlist
                                            IDictionary<string, dynamic> wishitem = new Dictionary<string, dynamic>();
                                            wishitem.Add("LISTID", listid);
                                            wishitem.Add("ITEMID", int.Parse(Items[index][0]));
                                            wishitem.Add("ITEMTABLE", "COMPONENT");
                                            SendRequestAsync("POST", MetricsApiUrl + "WISHLISTITEM", JsonConvert.SerializeObject(wishitem, Formatting.Indented));
                                            //send log for wishlist creation with new object
                                            if (DebugOutput) Console.WriteLine("Sending Wishlist creation log");
                                            IDictionary<string, dynamic> wishlistlog = new Dictionary<string, dynamic>();
                                            wishlistlog.Add("LISTID", listid);
                                            wishlistlog.Add("USERNAME", currentUser["USERNAME"]);
                                            wishlistlog.Add("DESCRIPTION", "ADD TO NEW LIST");
                                            wishlistlog.Add("ITEMID", int.Parse(Items[index][0]));
                                            wishlistlog.Add("ITEMTABLE", "COMPONENT");
                                            wishlistlog.Add("EVENTTIMESTAMP", eventTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                            SendRequestAsync("POST", MetricsApiUrl + "WISHLISTLOG", JsonConvert.SerializeObject(wishlistlog, Formatting.Indented));
                                        }
                                        else
                                        {
                                            //select a random wishlist to add item to then add item and log
                                            if (DebugOutput) Console.WriteLine("Adding item to wishlist");

                                            listid = (int)UserWishlists[rand.Next(UserWishlists.Count)]["LISTID"];

                                            //check if wishlist already cotains item. If yes then ignore add.
                                            await SendRequestAsync("GET", MetricsApiUrl + "WISHLISTITEM/" + listid + "&" + int.Parse(Items[index][0]) + @"&'COMPONENT'");
                                            if (ResponseData.Count == 0)
                                            {// item does not exist
                                                //add item to wishlist
                                                IDictionary<string, dynamic> wishitem = new Dictionary<string, dynamic>();
                                                wishitem.Add("LISTID", listid);
                                                wishitem.Add("ITEMID", int.Parse(Items[index][0]));
                                                wishitem.Add("ITEMTABLE", "COMPONENT");
                                                SendRequestAsync("POST", MetricsApiUrl + "WISHLISTITEM", JsonConvert.SerializeObject(wishitem, Formatting.Indented));

                                                IDictionary<string, dynamic> wishlistlog = new Dictionary<string, dynamic>();
                                                wishlistlog.Add("LISTID", listid);
                                                wishlistlog.Add("DESCRIPTION", "ADD TO EXISTING LIST");
                                                wishlistlog.Add("ITEMID", int.Parse(Items[index][0]));
                                                wishlistlog.Add("ITEMTABLE", "COMPONENT");
                                                wishlistlog.Add("EVENTTIMESTAMP", eventTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                                SendRequestAsync("POST", MetricsApiUrl + "WISHLISTLOG", JsonConvert.SerializeObject(wishlistlog, Formatting.Indented));
                                            }
                                        }


                                        //5% chance to remove item from wishlist
                                        randint = rand.Next(100);
                                        if (randint < 5)
                                        {
                                            if (DebugOutput) Console.WriteLine("Removing item from wishlist");
                                            //remove item from wishlist
                                            eventTime = eventTime.AddSeconds(rand.Next(10, 30));
                                            if (eventTime > sessionEnd[i])
                                            {
                                                break;
                                            }
                                            await SendRequestAsync("DELETE", MetricsApiUrl + @"WISHLISTITEM/'" + Items[index][0] + @"&" + @"COMPONENT'");
                                            //send log
                                            IDictionary<string, dynamic> wishlistlog = new Dictionary<string, dynamic>();
                                            wishlistlog.Add("LISTID", listid);
                                            wishlistlog.Add("DESCRIPTION", "REMOVE ITEM");
                                            wishlistlog.Add("ITEMID", int.Parse(Items[index][0]));
                                            wishlistlog.Add("ITEMTABLE", "COMPONENT");
                                            wishlistlog.Add("EVENTTIMESTAMP", eventTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                            SendRequestAsync("POST", MetricsApiUrl + "WISHLISTLOG", JsonConvert.SerializeObject(wishlistlog, Formatting.Indented));
                                        }
                                    }

                                } //end looping over views

                            }
                        }
                        // end of session
                        eventTime = sessionEnd[i];
                        await SendRequestAsync("GET", MetricsApiUrl + "CARTITEM");
                        List<IDictionary<string, dynamic>> CartItems = new List<IDictionary<string, dynamic>>();
                        foreach (IDictionary<string, dynamic> record in ResponseData)
                        {
                            if (record["USERNAME"] == currentUser["USERNAME"])
                            {
                                CartItems.Add(record);
                            }
                        }
                        randint = rand.Next(100);
                        if (randint < 30)//30% chance checkout, 70% chance abandon cart 
                        {
                            //send checkout log
                            if (DebugOutput) Console.WriteLine("Checking out");
                            IDictionary<string, dynamic> checkoutlog = new Dictionary<string, dynamic>();
                            checkoutlog.Add("USERNAME", currentUser["USERNAME"]);
                            checkoutlog.Add("PURCHASEID", 0);
                            checkoutlog.Add("EVENTTIMESTAMP", eventTime.ToString("yyyy-MM-dd HH:mm:ss"));
                            SendRequestAsync("POST", MetricsApiUrl + "CHECKOUTLOG", JsonConvert.SerializeObject(checkoutlog, Formatting.Indented));
                            //remove items from cart without sending log of removal

                            foreach (IDictionary<string, dynamic> record in CartItems)
                            {
                                SendRequestAsync("DELETE", MetricsApiUrl + @"CARTITEM/'" + currentUser["USERNAME"] + @"'&" + record["ITEMID"] + @"&" + @"'COMPONENT'");
                            }
                        }
                        else
                        {
                            //remove all items from cart and send cart log with description as "abandoned"
                            if (DebugOutput) Console.WriteLine("Abandoning Cart");
                            foreach (IDictionary<string, dynamic> record in CartItems)
                            {
                                if (DebugOutput) Console.WriteLine("Deleting cart item because it is abandoned");
                                SendRequestAsync("DELETE", MetricsApiUrl + @"CARTITEM/'" + currentUser["USERNAME"] + @"'&" + record["ITEMID"] + @"&" + @"'COMPONENT'");
                                IDictionary<string, dynamic> CartDeleteLog = new Dictionary<string, dynamic>();
                                CartDeleteLog.Add("USERNAME", currentUser["USERNAME"]);
                                CartDeleteLog.Add("EVENTTIMESTAMP", eventTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                CartDeleteLog.Add("DESCRIPTION", "CART ABANDONED");
                                CartDeleteLog.Add("QUANTITY", record["QUANTITY"]);
                                CartDeleteLog.Add("ITEMID", record["ITEMID"]);
                                CartDeleteLog.Add("ITEMTABLE", "COMPONENT");
                                SendRequestAsync("POST", MetricsApiUrl + "CARTLOG", JsonConvert.SerializeObject(CartDeleteLog, Formatting.Indented));
                            }
                        }
                    }

                }
                else
                {
                    if (DebugOutput) Console.WriteLine("User will not shop today");
                }
                BaseDate = BaseDate.AddDays(1);
            }
            
        }

        /// <summary>
        /// Main method to generate metrics data
        /// </summary>
        /// <param name="args"></param>
        public  async Task Run()
        {
            DebugOutput = false;
            await CreateUsers(UserNum);
            await GenerateLogs(UserNum, NumDays);
            if (DebugOutput) Console.WriteLine("End is reached");

        }
    }
}
