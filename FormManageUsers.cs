using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;

namespace StockSyncManageUsers
{
    public partial class FormManageUsers : Form
    {
        private class User
        {
            public string Name { get; set; }
            public string Role { get; set; }
            public List<string> Permissions { get; set; }
        }

        private List<User> users = new List<User>();

        private readonly string[] allPermissions = {
            "View Inventory",
            "Edit Inventory",
            "View Sales",
            "Edit Sales",
            "Generate Reports",
            "Manage Users"
        };

        public FormManageUsers()
        {
            InitializeComponent();

            cmbRole.Items.AddRange(new[] { "Owner", "User" });
            cmbRole.SelectedIndex = 1; // default to User
            clbPermissions.Items.AddRange(allPermissions);

            UpdatePermissionVisibility();
        }

        private void UpdatePermissionVisibility()
        {
            clbPermissions.Enabled = cmbRole.SelectedItem?.ToString() == "User";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string role = cmbRole.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a name.");
                return;
            }

            var newUser = new User
            {
                Name = name,
                Role = role,
                Permissions = new List<string>()
            };

            if (role == "User")
            {
                foreach (string permission in clbPermissions.CheckedItems)
                    newUser.Permissions.Add(permission);
            }
            else
            {
                newUser.Permissions.Add("All Permissions");
            }

            users.Add(newUser);
            RefreshUserList();
            ClearForm();
        }

        private void RefreshUserList()
        {
            lstUsers.Items.Clear();
            foreach (var user in users)
                lstUsers.Items.Add($"{user.Name} ({user.Role})");
        }

        private void ClearForm()
        {
            txtName.Clear();
            cmbRole.SelectedIndex = 1;
            for (int i = 0; i < clbPermissions.Items.Count; i++)
                clbPermissions.SetItemChecked(i, false);
        }

        private void lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = lstUsers.SelectedIndex;
            if (index < 0) return;

            var user = users[index];
            txtName.Text = user.Name;
            cmbRole.SelectedItem = user.Role;
            UpdatePermissionVisibility();

            for (int i = 0; i < clbPermissions.Items.Count; i++)
                clbPermissions.SetItemChecked(i, user.Permissions.Contains(clbPermissions.Items[i].ToString()));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int index = lstUsers.SelectedIndex;
            if (index < 0) return;

            string name = txtName.Text.Trim();
            string role = cmbRole.SelectedItem.ToString();
            if (string.IsNullOrWhiteSpace(name)) return;

            var updatedUser = new User
            {
                Name = name,
                Role = role,
                Permissions = new List<string>()
            };

            if (role == "User")
            {
                foreach (string permission in clbPermissions.CheckedItems)
                    updatedUser.Permissions.Add(permission);
            }
            else
            {
                updatedUser.Permissions.Add("All Permissions");
            }

            users[index] = updatedUser;
            RefreshUserList();
            ClearForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = lstUsers.SelectedIndex;
            if (index < 0) return;

            users.RemoveAt(index);
            RefreshUserList();
            ClearForm();
        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePermissionVisibility();
        }
    }
}