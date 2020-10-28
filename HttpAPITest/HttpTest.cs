using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;

namespace HttpAPITest
{
    public partial class HttpTest : Form
    {
        /// <summary>
        /// Store the types of http requests such as POST, PUT, etc
        /// </summary>
        public List<String> httpMethods { get; set; }
        /// <summary>
        /// Our client to use for the program
        /// </summary>
        static readonly HttpClient client = new HttpClient();
        /// <summary>
        /// Dictionary object to contain data received from GET request
        /// </summary>
        public IDictionary<string, dynamic> dict {get; set;}
        /// <summary>
        /// Default Constructor
        /// </summary>
        public HttpTest()
        {
            InitializeComponent();
            InitializeProperties();
        }
        /// <summary>
        /// Initialize custom data
        /// </summary>
        private void InitializeProperties()
        {
            httpMethods = new List<String> { "POST", "GET", "PUT", "DELETE" };
            httpComboBox.DataSource = httpMethods;
        }
        /// <summary>
        /// Get requested data and change associated values of controls when data is received
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        async Task HTTPGET(string uri)
        {
            List<IDictionary<string, dynamic>> output = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //halt method and wait on response from api
                response.EnsureSuccessStatusCode(); //make sure we have 200 OK
                string responseBody = await response.Content.ReadAsStringAsync(); //halt method and wait on string read
                responseBodyTextBox.Text = responseBody; //update control
                output = JsonConvert.DeserializeObject<List<IDictionary<string, dynamic>>>(responseBody); //Convert from json text to dictionary
                //application specific code below
                dict = output[0];
                List<string> keys = dict.Keys.ToList();
                keysListBox.DataSource = keys; //populate listbox with keys
                requestBodyTextBox.Text = JsonConvert.SerializeObject(dict);
            }
            catch (HttpRequestException e)
            {
                responseBodyTextBox.Text = e.Message;
            }
        }

        async Task HTTPPOST(string uri, string body)
        {
            try
            {
                //List<IDictionary<string, dynamic>> tempdict = null;
                //tempdict = JsonConvert.DeserializeObject<List<IDictionary<string, dynamic>>>(body);
                //var json = JsonConvert.SerializeObject(tempdict);
                //requestBodyTextBox.Text = json.ToString();
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, data);
                response.EnsureSuccessStatusCode(); //make sure we have 200 OK
                responseBodyTextBox.Text = response.ToString();
            }
            catch (HttpRequestException e)
            {
                responseBodyTextBox.Text = e.Message;
            }
            
        }

        /// <summary>
        /// Send a HTTP request based on request type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private void SendRequest(string type, string url, string body = "")
        {
            if (type == "GET")
            {
                HTTPGET(url); //Async method will make all changes when it gets to it.
            }
            else if (type == "POST")
            {
                HTTPPOST(url, body);
            }
            
        }
        /// <summary>
        /// Listener for the send button. Sends an http request using data in the input boxes. Converts Get request data to a dictionary object and binds data to listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendButton_Click(object sender, EventArgs e)
        {
            SendRequest((string)httpComboBox.SelectedItem, urlTextBox.Text, requestBodyTextBox.Text); //Send get request and return the response to the text box
            
        }
        /// <summary>
        /// Listener for keys listbox. Updates the text of the value text box with the value corresponding to the selected key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keysListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dict[(string)keysListBox.SelectedItem] != null)
            {
                valueTextBox.Text = dict[(string)keysListBox.SelectedItem].ToString();
            }
            else
            {
                valueTextBox.Text = "{NULL}";
            }
            
        }
    }
}
