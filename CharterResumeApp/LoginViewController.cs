using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UIKit;

namespace CharterResumeApp
{
    public class LoginViewController : UIViewController
    {
        UITextField usernameField,passwordField;
        private HomeScreenViewController view;
        public LoginViewController(HomeScreenViewController view)
        {
            this.view = view;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //creates view of page
            Title = "Add New User";

            View.BackgroundColor = UIColor.SystemBackgroundColor;

            nfloat h = 40f;
            nfloat w = View.Bounds.Width;
           //user name text field
            usernameField = new UITextField
            {
                Placeholder = "Enter your username",
                BorderStyle = UITextBorderStyle.RoundedRect,
                Frame = new CGRect(10, 100, w - 20, h)
            };
            //password text field
            passwordField = new UITextField
            {
                Placeholder = "Enter your password",
                BorderStyle = UITextBorderStyle.RoundedRect,
                Frame = new CGRect(10, 150, w - 20, h),
                SecureTextEntry = true
            };

            //submit button
            var submitButton = UIButton.FromType(UIButtonType.RoundedRect);
            submitButton.Frame = new CGRect(10, 200, w - 20, 44);
            submitButton.SetTitle("Submit", UIControlState.Normal);
            submitButton.BackgroundColor = UIColor.White;
            submitButton.TouchUpInside += (sender, e) => SubmitUser();

            //loads onto page
            View.AddSubview(usernameField);
            View.AddSubview(passwordField);
            View.AddSubview(submitButton);

        }

        private void SubmitUser()
        {
            User newUser;
             if (Submit_Validate())
            {
                //checks if the credentials are valid first
                newUser = new User()
                {
                    Username = usernameField.Text,
                    Password = passwordField.Text
                };
                view.users.Add(newUser);
           //finds the file again where the users are stored
            string filename =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "CharterApp.UserList.json");
            //makes sure the file exists
            bool doesExist = File.Exists(filename);
            if (doesExist)
            {
                //reads the file
                string json = File.ReadAllText(filename);
                //converts them Json objects
                JArray Jitems = JArray.Parse(json);
                //adds the new user
                Jitems.Add(JObject.FromObject(newUser));
                //writes the entire file back to the original location with the new user included
                File.WriteAllText(filename, Jitems.ToString());
            }
            else
            {
                //makes brand new file if there isn't one made
                JArray Jitems = new JArray();
                //adds the first user
                Jitems.Add(JObject.FromObject(newUser));
                //writes from the file
                File.WriteAllText(filename, Jitems.ToString());
            }
            //navigates to the original root screen to ensure table reload
            var controller = new HomeScreenViewController();
            var navController = new UINavigationController(controller);
            UIApplication.SharedApplication.KeyWindow.RootViewController = navController;
                this.NavigationController.PopToRootViewController(true);

            }
        }

        private bool Submit_Validate()
        {
            //validation broken into individual conditionals to make testing easier
            string matchingSequence = "";
            List<string> redText = new List<string> { };
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(usernameField.Text))
            {
                redText.Add("Please Enter a Username");
                isValid = false;
            }
            else if (passwordField.Text.Length <5 || passwordField.Text.Length >12)
            {
                redText.Add("Password must be between 5 and 12 characters");
                isValid = false;
            }

            else if (!Regex.IsMatch(passwordField.Text, @"^[a-zA-Z0-9]+$") )
            {
                redText.Add("Password must consist of only numbers and letters. No special characters!");
                isValid = false;
            }

            else if (!(passwordField.Text.Any(Char.IsLetter)) )
            {
                redText.Add("Password must have at least one letter ");
                isValid = false;
            }
            else if ( !passwordField.Text.Any(Char.IsNumber))
            {
                redText.Add("Password must have at least one number");
                isValid = false;
            }
            else if (ContainsDuplicateSequence(passwordField.Text, 2, false, out matchingSequence))
            {
                redText.Add("Password cannot contain repeating phrases");
                isValid = false;
            }
           
            if (isValid == false)
            {
                UIAlertController okAlertController = UIAlertController.Create("Something went wrong",string.Join("\n",redText),
                    UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                this.PresentViewController(okAlertController, true, null);
            }
            return isValid;
        }

        static bool ContainsDuplicateSequence(string text, int maxCharSequenceToAllow, bool ignoreCaps, out string matchingSequence)
        {
            //method allows for text to be broken down no matter how many characters you define as a "sequence". I chose 2
            matchingSequence = "";
            if (ignoreCaps)
            {
                text = text.ToLower();
            }

            List<string> substrings = new List<string>();

            int Length = text.Length;

            for (int i = 0; i < Length; i++)
            {
                if (i + maxCharSequenceToAllow >= Length)
                {
                    return false;
                }
                string sub = text.Substring(i, maxCharSequenceToAllow + 1);
                if (substrings.Any(s => s.Equals(sub)))
                {
                    matchingSequence = sub;
                    return true;
                }
                substrings.Add(sub);
            }

            return false;
        }
    }
}