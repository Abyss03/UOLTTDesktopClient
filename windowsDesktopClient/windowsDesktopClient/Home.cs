﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using windowsDesktopClient.Classes;
using DataDelivery;

namespace windowsDesktopClient
{
    public partial class Home : Form
    {
        
        public Home()
        {
            InitializeComponent();
            PopulateLists();
        }
        
        protected void PopulateLists()
        {

            string shipListJson = Common.LoadData(GetCalls.ListOfShips);
            Global.listOfShips = JsonConvert.DeserializeObject<List<Ship>>(shipListJson);
            foreach (var item in Global.listOfShips)
            {
                ShipDropDown.Items.Add(item);
            }
            ShipDropDown.DisplayMember = "ShipName";

            string orgListJson = Common.LoadData(GetCalls.ListOfOrgs);
            Global.listOfOrgs = JsonConvert.DeserializeObject<List<Organization>>(orgListJson);
            foreach (var item in Global.listOfOrgs)
            {
                OrgDropDown.Items.Add(item);
            }
            OrgDropDown.DisplayMember = "Name";

            string userListJson = Common.LoadData(GetCalls.ListOfUsers);
            Global.listOfUsers = JsonConvert.DeserializeObject<List<User>>(userListJson);
        }

        private void ShipButton_Click(object sender, EventArgs e)
        {
            PopulateLists();
        }

        private void ShipDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ship selected = (Ship)ShipDropDown.SelectedItem;
            ShipId.Text = Convert.ToString(selected.Id);
            ShipName.Text = Convert.ToString(selected.ShipName);
            ShipHeight.Text = Convert.ToString(selected.Height + "m");
            ShipLength.Text = Convert.ToString(selected.Length + "m");
            ShipBeam.Text = Convert.ToString(selected.Beam + "m");
            ShipCargoCapacity.Text = Convert.ToString(selected.CargoCapacity);
            ShipPowerPlant.Text = Convert.ToString(selected.PowerPlant);
            ShipPowerCount.Text = Convert.ToString(selected.PowerCount);
            ShipClass.Text = Convert.ToString(selected.Class);
        }

        private void OrgDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            Organization selected = (Organization)OrgDropDown.SelectedItem;
            OrgId.Text = Convert.ToString(selected.Id);
            OrgAdminUserId.Text = Convert.ToString(selected.Admin_User_Id);
            OrgStatusId.Text = Convert.ToString(selected.Status_Id);
            OrgUserCount.Text = Convert.ToString(selected.User_Count);
            OrgDomain.Text = Convert.ToString(selected.Domain);
            try
            {
                string jsonUserRaw = Common.LoadData(GetCalls.IndividualUser, selected.Admin_User_Id);
                User adminUser = JsonConvert.DeserializeObject<User>(jsonUserRaw);
                OrgAdminUser.Text = adminUser.UserName;
            }
            catch (WebException)
            {
                string messageBoxText = "Error fetching data from database";
                string caption = "UOLTT Desktop Application";
                MessageBoxButtons button = MessageBoxButtons.RetryCancel;

                MessageBox.Show(messageBoxText, caption, button);
            }
        }
    }

    /// <summary>
    /// Internally shared variables able to be populated elsewhere
    /// </summary>
    internal static class Global
    {
        private static List<Ship> _listOfShips = new List<Ship>();
        private static List<Organization> _listOfOrgs = new List<Organization>();
        private static List<User> _listOfUsers = new List<User>();

        internal static List<Ship> listOfShips
        {
            get { return _listOfShips; }
            set { _listOfShips = value; }
        }

        internal static List<Organization> listOfOrgs
        {
            get { return _listOfOrgs; }
            set { _listOfOrgs = value; }
        }

        internal static List<User> listOfUsers
        {
            get { return _listOfUsers; }
            set { _listOfUsers = value; }
        }
    }
}
