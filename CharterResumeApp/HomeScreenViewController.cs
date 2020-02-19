using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json.Linq;
using UIKit;

namespace CharterResumeApp
{
    public class HomeScreenViewController : UIViewController
    {
        public UITableView table;
        private LoginViewController loginpage;
        private UILabel titleLabel, descriptionLabel;
        private UIImageView logoImage;
        public List<User> users = new List<User>();


        public HomeScreenViewController()
        {
            View.BackgroundColor = UIColor.SystemBackgroundColor;
            table.ReloadData();
        }

        public override void ViewDidLoad()
        {
            //creates the view of the home screen
            base.ViewDidLoad();


            var width = View.Bounds.Width;
            var height = View.Bounds.Height;
            var margins = View.LayoutMarginsGuide;

            nfloat h = 30;
            //Charter logo
            logoImage = new UIImageView(new CGRect(0, 50, width, 150));
            logoImage.Image = UIImage.FromBundle("CharterLogo");

            //Charter Title Label
            titleLabel = new UILabel()
            {
                Text = "Customer Search",

                TextColor = UIColor.FromRGB(22, 98, 156),
                BackgroundColor = UIColor.SystemBackgroundColor,
                TextAlignment = UITextAlignment.Center,
                Frame = new CGRect(10, 220, width - 20, 50)
            };
            titleLabel.Font = titleLabel.Font.WithSize(36);

            //Description label
            descriptionLabel = new UILabel()
            {
                Text = "Select a user",
                MinimumFontSize = 24,
                TextColor = UIColor.Gray,
                TextAlignment = UITextAlignment.Center,
                Frame = new CGRect(10, 275, width - 20, h)
            };

            //creates table
            table = new UITableView(new CGRect(0, 300, width, height));
            table.AutoresizingMask = UIViewAutoresizing.All;
            CreateTableItems();

            //loads onto page
            View.Add(logoImage);
            View.Add(table);
            View.Add(titleLabel);
            View.Add(descriptionLabel);

            //adds the Add user button
            UIBarButtonItem btn = new UIBarButtonItem();
            btn.Title = "+";
            btn.TintColor = UIColor.SystemBlueColor;
            //makes the '+' symbol more visible
            var fontAttribute = new UITextAttributes();
            fontAttribute.Font = UIFont.SystemFontOfSize(40);
            btn.SetTitleTextAttributes(fontAttribute, UIControlState.Normal);
            //opens the add user page
            btn.Clicked += (sender, e) =>
            {
                loginpage = new LoginViewController(this);
                NavigationController.PushViewController(loginpage, true);
            };
            NavigationItem.RightBarButtonItem= btn;
            table.ReloadData();
        }

        protected void CreateTableItems()
        {
            //populates items in table based on json file in folder structure
            //checks to see if file exists
            string filename =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "CharterApp.UserList.json");
            bool doesExist = File.Exists(filename);
            if (doesExist)
            {
                //if file exists, it reads the file
                string json = File.ReadAllText(filename);
                //file converts json into objects
                var Jitems = JArray.Parse(json);
                //objects are stored in list to be read into table
                users = Jitems.ToObject<User[]>().ToList();
            }

            //read into table
            table.Source = new TableSource(users, this);
        }

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }
    }
}

#region Maybe Reusable code

//Add(table);
//UIButton myButton = new UIButton(UIButtonType.System);
//myButton.Frame = new CGRect(25, 25, 50, 50);
//myButton.SetTitle("Hello, World!", UIControlState.Normal);
//myButton.SetImage(UIImage.FromBundle("plusbutton.png"), UIControlState.Normal );
//myButton.BackgroundColor = UIColor.Green;
////table.BottomAnchor.ConstraintEqualTo(margins.TopAnchor,10).Active = true;
//myButton.TouchUpInside += (sender, e) => {

//};
//View.Add(myButton);

#endregion