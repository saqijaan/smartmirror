using SmartMirror.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartMirror
{
    public partial class NewUser : Form
    {
        public NewUser()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var list = new List<Object>();
            list.Add(txt_name);
            list.Add(txt_email);
            list.Add(txt_phone);
            list.Add(txt_password);
            if (!this.ValidateForm(
                list
            )) return;
            User user       = new User();
            user.Name       = txt_name.Text;
            user.Phone      = txt_phone.Text;
            user.Email      = txt_email.Text;
            user.Password   = txt_password.Text;
            user.Status     = "active";
            user.Type       = "user";
            user.Save();
            MessageBox.Show("User Saved");
        }

        private bool ValidateForm(List<object> _params)
        {
            foreach(Object p in _params)
            {
                if  ( p.GetType() == typeof(TextBox))
                {
                    TextBox t = ((TextBox)p);
                    if (t.Text.Length == 0)
                    {
                        MessageBox.Show("The Feild "+t.Name+" is Required");
                        return false;
                    }
                }
                else if(p.GetType() == typeof(ComboBox)) 
                {
                    ComboBox t = ((ComboBox)p);
                    if (t.SelectedIndex < 0)
                    {
                        MessageBox.Show("The Feild " + t.Name + " is Required");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
