using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace CharterResumeApp
{
    public class TableSource : UITableViewSource
    {
        protected List<User> tableItems;
        protected string cellIdentifier = "TableCell";
        HomeScreenViewController page;
        //this page serves as the data collector
        public TableSource(List<User> users, HomeScreenViewController page)
        {
            tableItems = users;
            this.page = page;
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
            string item = (tableItems[indexPath.Row] as User).Username;

            if (cell == null)
            { cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier); }

            cell.TextLabel.Text = item;

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return tableItems.Count;
        }
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //creates an alert that gives user information, loaded from file
            UIAlertController okAlertController = UIAlertController.Create("User Selected", 
                ReturnMessage(tableItems[indexPath.Row] as User), 
                UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            page.PresentViewController(okAlertController, true, null);

            tableView.DeselectRow(indexPath, true);
        }

        public string ReturnMessage(User user)
        {
            return $"Username: {user.Username} \nPassword: {user.Password}" ;
        }
    }
}