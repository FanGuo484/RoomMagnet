﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class WebPages_ManageSearchProperties : System.Web.UI.Page
{
    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ToString());
    protected void Page_Load(object sender, EventArgs e)
    {

        propertyImage5.Visible = false;
        propertyImage6.Visible = false;
        propertyImage3.Visible = false;
        propertyImage4.Visible = false;

        String sql = "Select Title, [Property].City, [Property].HomeState, [Property].ZipCode, AvailableBedrooms, [Property].RentPrice, [Property].StartDate, [Property].EndDate, " +
      "[ImagePath].ImagePath, AvailableBathrooms,[Users].ImagePath, AirConditioning, Heating, OnSiteLaundry,Parking,Furnished,PetFriendly,CarbonMonoxideDetector, SmokeDetector,SeperateEntrance," +
    "Wifi, TV, SeperateBathroom, [Rating].Descriptions, NumStars, [Rating].LastUpdated, [Rating].LastUpdatedBy from [Property] inner join [ImagePath]" +
    "on [Property].PropertyID = [ImagePath].PropertyID INNER JOIN [PropertyRoom] ON [Property].PropertyID = [PropertyRoom].PropertyID" +
    " INNER JOIN [Users] ON [Property].HostID = [Users].UserID INNER JOIN Amenities ON [Amenities].PropertyID = [Property].PropertyID INNER JOIN [Rating] ON [Property].PropertyID= " +
    "[Rating].PropertyID WHERE ([Property].PropertyID = " + 1003 + ")";


        //Session["ResultPropertyID"]
        connection.Open();
        string title;
        string city;
        string homeState;
        string zipCode;
        int AvailableBedrooms;
        string rentPrice;
        DateTime startDate;
        DateTime endDate;
        int availableBathrooms;
        string airConditioning;
        string heating;
        string onSiteLaundry;
        string parking;
        string furnished;
        string petFriendly;
        string carbonMonoxideDetector;
        string smokeDetector;
        string seperateEntrance;
        string wifi;
        string tv;
        string seperateBathroom;
        string descriptions;
        int numStars;
        DateTime lastUpdated;
        string lastUpdatedBy;
        int rating;

        SqlCommand search = new SqlCommand(sql, connection);
        SqlDataReader reader = search.ExecuteReader();


        int counter = 0;
        if (reader.HasRows)
        {
            while (reader.Read())
            {

                byte[] propertyImage = (byte[])reader[8];


                byte[] hostImages = (byte[])reader[10];

                string propertyImageURL = "data:image;base64," + Convert.ToBase64String(propertyImage);

                if (counter == 0)
                {
                    propertyImage5.ImageUrl = propertyImageURL;
                }

                else if (counter == 1)
                {
                    propertyImage6.ImageUrl = propertyImageURL;
                }

                else if (counter == 2)
                {
                    propertyImage3.ImageUrl = propertyImageURL;

                }
                else if (counter == 3)
                {
                    propertyImage4.ImageUrl = propertyImageURL;

                }

                string hostImageURL = "data:image;base64," + Convert.ToBase64String(hostImages);

                hostImage.ImageUrl = hostImageURL;
                counter++;


                if (counter == 0)
                {
                    Title = reader.GetString(0);
                    city = reader.GetString(1);
                    homeState = reader.GetString(2);
                    zipCode = reader.GetString(3);
                    AvailableBedrooms = reader.GetInt32(4);
                    endDate = reader.GetDateTime(5);
                    decimal x = reader.GetDecimal(6);
                    rentPrice = String.Format("{0:0.##}", x);
                    RentPriceTxtBx.Text = "$" + rentPrice + "/Month";
                    startDate = reader.GetDateTime(7);
                    endDate = reader.GetDateTime(8);
                    availableBathrooms = reader.GetInt32(9);
                    airConditioning = reader.GetString(10);
                    heating = reader.GetString(11);
                    onSiteLaundry = reader.GetString(12);
                    parking = reader.GetString(13);
                    furnished = reader.GetString(14);
                    petFriendly = reader.GetString(15);
                    carbonMonoxideDetector = reader.GetString(16);
                    smokeDetector = reader.GetString(17);
                    seperateEntrance = reader.GetString(18);
                    wifi = reader.GetString(19);
                    tv = reader.GetString(20);
                    seperateBathroom = reader.GetString(21);
                    descriptions = reader.GetString(22);
                    numStars = reader.GetInt32(23);
                    lastUpdated = reader.GetDateTime(24);
                    lastUpdatedBy = reader.GetString(25);
                    rating = reader.GetInt32(26);


                }

            }
            connection.Close();
        }
    }
}