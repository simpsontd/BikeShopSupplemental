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

namespace HttpAPITest
{
    public partial class HttpTest : Form
    {
        /// <summary>
        /// Store the types of http requests such as POST, PUT, etc
        /// </summary>
        public List<String> httpMethods { get; set; }
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
        /// Send a HTTP request based on request type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private string SendRequest(string type, string url, string body = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url); //Create a request object initialized with the URL. No request is made at this point.
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate; //Modern servers usually compress data before sending
            if (type == "GET")
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) //GET request
                using (Stream stream = response.GetResponseStream()) //Get stream object for the response
                using (StreamReader reader = new StreamReader(stream)) //Create a reader to read response stream
                {
                    return reader.ReadToEnd(); //return string of all data received
                }
            }
            return "No Data returned"; //return if request type has not been implemented
            
        }
        /// <summary>
        /// Convert Json Text to a Dictionary Object. Since Json may contain a list of objects (in our case a list of records) we will represent this as a list of dictionaries
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private List<IDictionary<String, dynamic>> JsonTextToDictionary(string input)
        {
            List<IDictionary<string, dynamic>> output = JsonConvert.DeserializeObject<List<IDictionary<string, dynamic>>>((string)input);


            return output;
        }
        /// <summary>
        /// Listener for the send button. Sends an http request using data in the input boxes. Converts Get request data to a dictionary object and binds data to listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendButton_Click(object sender, EventArgs e)
        {
            responseBodyTextBox.Text = SendRequest((string)httpComboBox.SelectedItem, urlTextBox.Text); //Send get request and return the response to the text box
            if ((string)httpComboBox.SelectedItem == "GET")
            {
                dict = JsonTextToDictionary(responseBodyTextBox.Text)[0]; //convert text to IDictionary
                List<string> keys = dict.Keys.ToList();
                keysListBox.DataSource = keys; //populate listbox with keys
            }
            
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
